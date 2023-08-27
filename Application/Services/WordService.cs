using System.Diagnostics;
using System.Drawing;
using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using DevExpress.Drawing.Printing;
using DevExpress.Export.Xl;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Commands;
using Domain.Entities;
using Domain.Enums;
using OfficeOpenXml;

namespace Application.Services;

public class WordService : IWordService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IEnergyFlowService _energyFlowService;


    public WordService(IRepositoryWrapper repositoryWrapper, IEnergyFlowService energyFlowService)
    {
        _repositoryWrapper = repositoryWrapper;
        _energyFlowService = energyFlowService;
    }

    public async Task<BaseResponse<bool>> GetLoadXlsx()
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
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Файл успешно создан" },
            Success: true);
    }

    public async Task<BaseResponse<bool>> ReadExcel()
    {
        //Запись в базу 360 из excel
        
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        string filePath = "Document.xlsx";
        
        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
        
                Antenna? antenna = await _repositoryWrapper.AntennaRepository.GetByCondition(x => x.Model.Equals("TBXLHA-6565B-VTM"));
                TranslatorSpecs? translatorSpecs = await _repositoryWrapper.TranslatorSpecsRepository
                    .GetByCondition(x => x.AntennaId == antenna.Id && x.Frequency == 900);
                for (int row = 1; row <= rowCount-1; row++)
                {
                    var degreeCellValue = worksheet.Cells[row, 1].Value;
                    var valueCellValue = worksheet.Cells[row, 2].Value;
                    if (int.TryParse(degreeCellValue?.ToString(), out int degree) && decimal.TryParse(valueCellValue?.ToString(), out decimal value))
                    {
                        RadiationZone radiationZone = new RadiationZone()
                        {
                            Degree = degree,
                            Value = value,
                            DirectionType = DirectionType.Horizontal,
                            TranslatorSpecsId = translatorSpecs.Id
                        };
                        await _repositoryWrapper.RadiationZoneRepository.CreateAsync(radiationZone);
                    }
                }
                await _repositoryWrapper.Save();
            }
        }
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Файл успешно считан" },
            Success: true);
    }

    public async Task<BaseResponse<bool>> ProjectWord()
    {
        using (var wordProcessor = new RichEditDocumentServer()) 
        { 
            wordProcessor.LoadDocument("Шаблон.docx");

            Document document = wordProcessor.Document;
            var contrAgent = await _repositoryWrapper.ContrAgentRepository.GetByCondition(x => x.CompanyName == "Tele2");
            document.ReplaceAll("ContrAgent", $"{contrAgent.CompanyName}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentPhone", $"{contrAgent.PhoneNumber}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentBIN", $"{contrAgent.BIN}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentFIO", 
                    $"{contrAgent.DirectorSurname} {contrAgent.DirectorName} {contrAgent.DirectorPatronymic}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentAddress", $"{contrAgent.Address}", SearchOptions.WholeWord);
            document.ReplaceAll("DateYear", 
                $"{DateTime.Now.Year}", SearchOptions.WholeWord);
            // var asd = document.AppendText("Владелец радиоэлектронных средств: ");
            // var contrAgent = await _repositoryWrapper.ContrAgentRepository.GetByCondition(x => x.CompanyName == "Tele2");
            // document.InsertText(asd.End,$"{contrAgent.CompanyName}");
            
            var keywords = document.FindAll("Table",SearchOptions.WholeWord);
            DocumentPosition insertPosition = keywords[0].Start;
            document.InsertText(insertPosition, $"Владелец радиоэлектронных средств: {contrAgent.CompanyName}");
            ParagraphProperties titleParagraphProperties = document.BeginUpdateParagraphs(keywords[0]);
            titleParagraphProperties.Alignment = ParagraphAlignment.Center;
            document.EndUpdateParagraphs(titleParagraphProperties);
            document.Delete(keywords[0]);
            Table oldTable = document.Tables.Create(insertPosition, 37, 5);

            oldTable.Rows.InsertBefore(0);
            oldTable.Rows.InsertAfter(0);
            
            oldTable.Rows[0].Cells.Append();
            Table table = document.Tables.Last;
            table.BeginUpdate();
            for (int i = 0; i <= 5; i++)
            {
                TableCell columnCell = table[i, i];
                columnCell.PreferredWidthType = WidthType.Fixed;
                columnCell.PreferredWidth = Units.InchesToDocumentsF(0.66f);
                for (int j = 0; j < 39; j++)
                {
                    columnCell = table[j, i];
                    columnCell.HeightType = HeightType.Exact;
                    columnCell.Height = Units.InchesToDocumentsF(0.131f);
                    DocumentRange cellRange = columnCell.Range;
                    CharacterProperties cp = document.BeginUpdateCharacters(cellRange);
                    cp.FontSize = 8;
                    document.EndUpdateCharacters(cp);
                }
            }

            document.InsertSingleLineText(table[0, 0].Range.Start, "v, град");
            document.InsertSingleLineText(table[0, 1].Range.Start, "f(v), dBi");
            document.InsertSingleLineText(table[0, 2].Range.Start, "f(v), раз");
            document.InsertSingleLineText(table[0, 3].Range.Start, "Rб, м");
            document.InsertSingleLineText(table[0, 4].Range.Start, "Rz, м");
            document.InsertSingleLineText(table[0, 5].Range.Start, "Rx, м");
            
            var radiations = _repositoryWrapper.RadiationZoneRepository
                .GetAllByCondition(x => x.TranslatorSpecsId.ToString() == "da9edb91-e301-4fc2-baff-2e1ec5f5570e")!
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
                var rB = _energyFlowService.GetRB(translator.Power, translator.Gain, 0.71M, radiations[i].Value);
                var rZ = _energyFlowService.GetRZ(radiations[i].Degree, rB);
                var rX = _energyFlowService.GetRX(radiations[i].Degree, rB);
                if (i == 0 || i == 4 || x == 0) 
                {
                    document.InsertText(table[y, 0].Range.Start, radiations[i].Degree.ToString());
                    document.InsertText(table[y, 1].Range.Start, radiations[i].Value.ToString());
                    document.InsertText(table[y, 2].Range.Start, _energyFlowService.Multiplier(radiations[i].Value).ToString("F3"));
                    document.InsertText(table[y, 3].Range.Start, rB.ToString("F3"));
                    document.InsertText(table[y, 4].Range.Start, rZ.ToString("F3"));
                    document.InsertText(table[y, 5].Range.Start, rX.ToString("F3"));
                    y++; 
                } 
            }
            table.EndUpdate();
            // document.AppendSection();
            // document.Unit = DevExpress.Office.DocumentUnit.Inch;
            // Shape picture = document.Shapes.InsertPicture(document.Range.End, DocumentImageSource.FromFile("image.jpg"));
            // picture.Size = new SizeF(7f, 10f);
            // picture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
            // picture.VerticalAlignment = ShapeVerticalAlignment.Center;
            // picture.Line.Color = Color.Black;
            // wordProcessor.SaveDocument("Arthur2.docx", DocumentFormat.OpenXml);
            wordProcessor.SaveDocument("Project.docx", DocumentFormat.OpenXml);
        }
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Файл успешно создан" },
            Success: true);
    }
}