using System.Diagnostics;
using System.Drawing;
using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Users;
using Application.Validation;
using AutoMapper;
using DevExpress.Export.Xl;
using DevExpress.Office.Utils;
using DevExpress.Pdf;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;
using Domain.Enums;
using OfficeOpenXml;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly UserValidator _userValidator;
    private readonly IEnergyFlowService _energyFlowService;

    public UserService(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserValidator userValidator, IEnergyFlowService energyFlowService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _userValidator = userValidator;
        _energyFlowService = energyFlowService;
    }

    public BaseResponse<IEnumerable<UserDto>> GetAll()
    {
        IQueryable<User> users = _repositoryWrapper.UserRepository.GetAll();
        List<UserDto> models = _mapper.Map<List<UserDto>>(users);

        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<UserDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Пользователи успешно получены" });
        }

        return new BaseResponse<IEnumerable<UserDto>>(
            Result: models,
            Success: true,
            Messages: new List<string>
                { "Данные не были получены, возможно пользователи еще не созданы или удалены" });
    }

    public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
    {
        var queryableUsers = _repositoryWrapper.UserRepository.GetAll();
        return await DataSourceLoader.LoadAsync(queryableUsers, loadOptions);
    }
    
    public async Task<bool> GetLoadXlsx()
    {
        IXlExporter exporter = XlExport.CreateExporter(XlDocumentFormat.Xlsx);

        using (FileStream stream = new FileStream("Document.xlsx", FileMode.Create, FileAccess.ReadWrite)) {
                using (IXlDocument document = exporter.CreateDocument(stream))
                {
                    using (IXlSheet sheet = document.CreateSheet())
                    {
                        sheet.Name = "360";
                        
                        using (IXlColumn column = sheet.CreateColumn()) {
                            column.WidthInPixels = 75;
                        }
                        
                        using (IXlColumn column = sheet.CreateColumn()) {
                            column.WidthInPixels = 120;
                        }
                        
                        using (IXlColumn column = sheet.CreateColumn()) {
                            column.WidthInPixels = 120;
                        }
                        
                        XlCellFormatting cellFormatting = new XlCellFormatting();
                        cellFormatting.Font = new XlFont();
                        cellFormatting.Font.Name = "Century Gothic";
                        cellFormatting.Font.SchemeStyle = XlFontSchemeStyles.None;
                        
                        XlCellFormatting headerRowFormatting = new XlCellFormatting();
                        headerRowFormatting.CopyFrom(cellFormatting);
                        headerRowFormatting.Font.Bold = true;
                        headerRowFormatting.Font.Color = XlColor.FromTheme(XlThemeColor.Light1, 0.0);
                        headerRowFormatting.Fill = XlFill.SolidFill(XlColor.FromTheme(XlThemeColor.Accent2, 0.0));
                        
                        using (IXlRow row = sheet.CreateRow()) {
                            using (IXlCell cell = row.CreateCell()) {
                                cell.Value = "Градус";
                                cell.ApplyFormatting(headerRowFormatting);
                            }
                            using (IXlCell cell = row.CreateCell()) {
                                cell.Value = "Значение";
                                cell.ApplyFormatting(headerRowFormatting);
                            }
                            using (IXlCell cell = row.CreateCell()) {
                                cell.Value = "Тип";
                                cell.ApplyFormatting(headerRowFormatting);
                            }
                        }
                        var radiations = _repositoryWrapper.RadiationZoneRepository.GetAll().OrderBy(x=> x.Degree);
                        foreach (var radiation in radiations)
                        {
                            using (IXlRow row = sheet.CreateRow()) {
                                using (IXlCell cell = row.CreateCell()) {
                                    cell.Value = radiation.Degree;
                                    cell.ApplyFormatting(cellFormatting);
                                }
                                using (IXlCell cell = row.CreateCell()) {
                                    cell.Value = radiation.Value.ToString();
                                    cell.ApplyFormatting(cellFormatting);
                                }
                                using (IXlCell cell = row.CreateCell()) {
                                    cell.Value = radiation.DirectionType.ToString();
                                    cell.ApplyFormatting(cellFormatting);
                                }
                            }
                            
                        }

                        sheet.AutoFilterRange = sheet.DataRange;
                        
                        XlCellFormatting totalRowFormatting = new XlCellFormatting();
                        var maxAbsoluteRadiationValue = radiations.Max(radiation => Math.Abs(radiation.Value));
                        var radiationWithMaxAbsoluteValue = radiations.First(radiation => Math.Abs(radiation.Value) == maxAbsoluteRadiationValue);
                        totalRowFormatting.CopyFrom(cellFormatting);
                        totalRowFormatting.Font.Bold = true;
                        totalRowFormatting.Fill = XlFill.SolidFill(XlColor.FromTheme(XlThemeColor.Accent5, 0.6));
                        
                        using (IXlRow row = sheet.CreateRow()) {
                            using (IXlCell cell = row.CreateCell()) {
                                cell.ApplyFormatting(totalRowFormatting);
                            }
                            using (IXlCell cell = row.CreateCell()) {
                                cell.Value = "Максимальное значение";
                                cell.ApplyFormatting(totalRowFormatting);
                                cell.ApplyFormatting(XlCellAlignment.FromHV(XlHorizontalAlignment.Right, XlVerticalAlignment.Bottom));
                            }
                            using (IXlCell cell = row.CreateCell()) {
                                cell.ApplyFormatting(totalRowFormatting);
                                cell.Value = radiationWithMaxAbsoluteValue.Value.ToString();
                            }
                        }
                    }
                }
        }
        Process.Start(new ProcessStartInfo("Document.xlsx"){UseShellExecute = true});
        return true;
    }

    public async Task<bool> ReadExcel()
    {
        //Запись в базу 360 из excel
        
        // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        // string filePath = "Document.xlsx";
        //
        // using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        // {
        //     using (ExcelPackage package = new ExcelPackage(stream))
        //     {
        //         ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //
        //         int rowCount = worksheet.Dimension.Rows;
        //         int colCount = worksheet.Dimension.Columns;
        //
        //         Antenna? antenna = await _repositoryWrapper.AntennaRepository.GetByCondition(x => x.Model.Equals("TBXLHA-6565B-VTM"));
        //         TranslatorSpecs? translatorSpecs = await _repositoryWrapper.TranslatorSpecsRepository
        //             .GetByCondition(x => x.AntennaId == antenna.Id && x.Frequency == 900);
        //         for (int row = 1; row <= rowCount-1; row++)
        //         {
        //             var degreeCellValue = worksheet.Cells[row, 1].Value;
        //             var valueCellValue = worksheet.Cells[row, 2].Value;
        //             if (int.TryParse(degreeCellValue?.ToString(), out int degree) && decimal.TryParse(valueCellValue?.ToString(), out decimal value))
        //             {
        //                 RadiationZone radiationZone = new RadiationZone()
        //                 {
        //                     Degree = degree,
        //                     Value = value,
        //                     DirectionType = DirectionType.Horizontal,
        //                     TranslatorSpecsId = translatorSpecs.Id
        //                 };
        //                 await _repositoryWrapper.RadiationZoneRepository.CreateAsync(radiationZone);
        //             }
        //         }
        //         await _repositoryWrapper.Save();
        //     }
        // }
        
        

        using (var wordProcessor = new RichEditDocumentServer()) {

            // Load a document
            wordProcessor.LoadDocument("Arthur.docx");
            
            Document document = wordProcessor.Document;
            var asd = document.AppendText("Владелец радиоэлектронных средств: ");
            var contrAgent = await _repositoryWrapper.ContrAgentRepository.GetByCondition(x => x.CompanyName == "Tele2");
            document.InsertText(asd.End,$"{contrAgent.CompanyName}");
            ParagraphProperties pp =
                document.BeginUpdateParagraphs(document.Paragraphs[0].Range);
            pp.Alignment = ParagraphAlignment.Center;
            document.EndUpdateParagraphs(pp);
            // DocumentPosition pos1 = document.CreatePosition(2);
            // document.Unit = DevExpress.Office.DocumentUnit.Inch;
            // Shape picture = document.Shapes.InsertPicture(document.Range.End, DocumentImageSource.FromFile("image.jpg"));
            // picture.Size = new SizeF(7f, 10f);
            // picture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
            // picture.VerticalAlignment = ShapeVerticalAlignment.Center;
            // picture.Line.Color = Color.Black;
            Table oldTable = document.Tables.Create(document.Range.End, 37, 3);

        // Add new rows to the table
        oldTable.Rows.InsertBefore(0);
        oldTable.Rows.InsertAfter(0);

        // Add a new column to the table
        oldTable.Rows[0].Cells.Append();
        Table table = document.Tables[0];
        table.BeginUpdate();

        

        // Set the third column width
        foreach (var t in table.Rows)
        {
            // Set the width of the first column
            
        }
        TableCell firstCell = table.Rows[0].FirstCell;
        firstCell.PreferredWidthType = WidthType.Fixed;
        firstCell.PreferredWidth = Units.InchesToDocumentsF(1.4f);
        firstCell.HeightType = HeightType.Exact;
        firstCell.Height = Units.InchesToDocumentsF(0.5f);
        TableCell firstColumnCell = table[0, 1];
        firstColumnCell.PreferredWidthType = WidthType.Fixed;
        firstColumnCell.PreferredWidth = Units.InchesToDocumentsF(1.4f);

        TableCell secondColumnCell = table[1, 2];
        secondColumnCell.PreferredWidthType = WidthType.Fixed;
        secondColumnCell.PreferredWidth = Units.InchesToDocumentsF(1.4f);

        TableCell lastCell = table.Rows[0].LastCell;
        lastCell.PreferredWidthType = WidthType.Fixed;
        lastCell.PreferredWidth = Units.InchesToDocumentsF(1.4f);
        lastCell.HeightType = HeightType.Exact;
        lastCell.Height = Units.InchesToDocumentsF(0.5f);
        
        table.BeginUpdate();

        // Insert header data
        // document.InsertSingleLineText(table.Rows[0].Cells[1].Range.Start, "Active Customers");
        document.InsertSingleLineText(table[0, 0].Range.Start, "v, град");
        document.InsertSingleLineText(table[0, 1].Range.Start, "f(v), dBi");
        document.InsertSingleLineText(table[0, 2].Range.Start, "f(v), раз");
        document.InsertSingleLineText(table[0, 3].Range.Start, "R, м");

        // Insert the customer's photo


        var radiations = _repositoryWrapper.RadiationZoneRepository
            .GetAllByCondition(x => x.TranslatorSpecsId.ToString() == "da9edb91-e301-4fc2-baff-2e1ec5f5570e")
            .OrderBy(x=> x.Degree).ToList();
        var translator = await _repositoryWrapper.TranslatorSpecsRepository
            .GetByCondition(x => x.Id.ToString() == "da9edb91-e301-4fc2-baff-2e1ec5f5570e");
        var antenna = await _repositoryWrapper.AntennaRepository
            .GetByCondition(x =>
                x.Id.ToString() == "a7cbbe4b-0eaf-4a0f-9418-531cdf36e75c");
        var y = 1;
        for (int i = 0; i < radiations.Count; i++)
        {
            var x = i % 10;
            if (i == 0 || i == 3)
            {
                document.InsertText(table[y, 0].Range.Start, radiations[i].Degree.ToString());
                document.InsertText(table[y, 1].Range.Start, radiations[i].Value.ToString());
                document.InsertText(table[y, 2].Range.Start, _energyFlowService.Multiplier(radiations[i].Value).ToString("F3"));
                document.InsertText(table[y, 3].Range.Start, _energyFlowService
                    .EuclideanDistanceDecimal(translator.Power,translator.Gain,0.71M,radiations[i].Value).ToString("F3"));
                y++;
            }
            else if (x == 0)
            {
                document.InsertText(table[y, 0].Range.Start, radiations[i].Degree.ToString());
                document.InsertText(table[y, 1].Range.Start, radiations[i].Value.ToString());
                document.InsertText(table[y, 2].Range.Start, _energyFlowService.Multiplier(radiations[i].Value).ToString("F3"));
                document.InsertText(table[y, 3].Range.Start, _energyFlowService
                    .EuclideanDistanceDecimal(translator.Power,translator.Gain,0.71M,radiations[i].Value).ToString("F3"));
                y++;
            }

            
        }
        // Insert customer info
        // document.InsertText(table[3, 1].Range.Start, "Ryan Anita W");
        // document.InsertText(table[3, 2].Range.Start, "Intermediate");
        // document.InsertText(table[4, 1].Range.Start, "3/28/1984");
        // document.InsertText(table[5, 1].Range.Start, "anita_ryan@dxvideorent.com");
        // document.InsertText(table[5, 2].Range.Start, "(555)421-0059");
        // document.InsertText(table[6, 1].Range.Start, "5119 Beryl Dr, San Antonio, TX 78212");

        // document.InsertSingleLineText(table[3, 3].Range.Start, "18");
        table.EndUpdate();
        wordProcessor.SaveDocument("Arthur2.docx", DocumentFormat.OpenXml);
        }
        return true;
    }
    


    public async Task<BaseResponse<string>> CreateAsync(UserDto model, string creator)
    {
        var mapUser = _mapper.Map<User>(model);
        var result = await _userValidator.ValidateAsync(mapUser);
        if (result.IsValid)
        {
            model.CreatedBy = creator;
            User user = _mapper.Map<User>(model);
            await _repositoryWrapper.UserRepository.CreateAsync(user);

            await AssignRolesToUser(model, user);
            await _repositoryWrapper.Save();

            return new BaseResponse<string>(
                Result: user.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Пользователь успешно создан" });
        }

        List<string> messages = _mapper.Map<List<string>>(result.Errors);
        return new BaseResponse<string>(
            Result: "",
            Messages: messages,
            Success: false);
    }

    public async Task<BaseResponse<UserDto>> GetByOid(string oid)
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == oid);
        UserDto model = _mapper.Map<UserDto>(user);

        model.Roles = new List<string>();
        model.Roles = _repositoryWrapper.UserRoleRepository
            .GetAll()
            .Where(userRole => userRole.UserId == user.Id)
            .Select(userRole => userRole.Role.RoleName)
            .ToList();

        if (user is null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: false);
        }

        return new BaseResponse<UserDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Пользователь успешно найден" });
    }

    public async Task<BaseResponse<UserDto>> UpdateUser(UpdateUserDto model)
    {
        BaseResponse<UserDto> getUserResponse = await GetByOid(model.Id);
        if (!getUserResponse.Success || getUserResponse.Result == null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: false);
        }

        UserDto existingUserDto = getUserResponse.Result;
        _mapper.Map(model, existingUserDto);

        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.Equals(existingUserDto.Id));
        if (user == null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: false);
        }

        _mapper.Map(existingUserDto, user);
        user.LastModifiedBy = "Admin";

        var result = await _userValidator.ValidateAsync(user);
        if (!result.IsValid)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: _mapper.Map<List<string>>(result.Errors),
                Success: false);
        }

        _repositoryWrapper.UserRepository.Update(user);
        await AssignRolesToUser(existingUserDto, user);
        await _repositoryWrapper.Save();

        return new BaseResponse<UserDto>(
            Result: existingUserDto,
            Success: true,
            Messages: new List<string> { "Пользователь успешно изменен" });
    }

    public BaseResponse<List<Role>> GetRoles()
    {
        var roles = _repositoryWrapper.RoleRepository.GetAll().ToList();    
        if (roles.Count > 0)
        {
            return new BaseResponse<List<Role>>(
                Result: roles,
                Success: true,
                Messages: new List<string> { "Роли успешно получены" });
        }

        return new BaseResponse<List<Role>>(
            Result: roles,
            Success: true,
            Messages: new List<string>
                { "Данные не были получены, возможно роли еще не созданы или удалены" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (user is not null)
        {
            user.IsDelete = true;
            _repositoryWrapper.UserRepository.Update(user);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Пользователь успешно удален" });
        }

        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Пользователя не существует" },
            Success: false);
    }

    private async Task AssignRolesToUser(UserDto model, User user)
    {
        var userRoles = _repositoryWrapper.UserRoleRepository.GetAll();
        foreach (var userRole in userRoles)
        {
            if (userRole.UserId == user.Id)
            {
                _repositoryWrapper.UserRoleRepository.Delete(userRole);
            }
        }

        foreach (var role in model.Roles
                     .Select(roleName => GetRoles()
                         .Result!
                         .FirstOrDefault(r => r.RoleName == roleName))
                     .Where(role => role is not null))
        {
            await _repositoryWrapper.UserRoleRepository.CreateAsync(new UserRole
            {
                Role = role,
                RoleId = role.Id,
                User = user,
                UserId = user.Id
            });
        }
    }
}