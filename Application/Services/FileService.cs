using System.Diagnostics;
using System.Drawing;
using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using DevExpress.Export.Xl;
using DevExpress.Office.Services;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Domain.Entities;
using Domain.Enums;
using OfficeOpenXml;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using DevExpress.XtraSpreadsheet.Services;
using ChartType = DevExpress.XtraRichEdit.API.Native.ChartType;
using DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat;
using SearchOptions = DevExpress.XtraRichEdit.API.Native.SearchOptions;
using Table = DevExpress.XtraRichEdit.API.Native.Table;

namespace Application.Services;

public class FileService : IFileService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IEnergyFlowService _energyFlowService;
    private readonly IBiohazardRadiusService _biohazardRadiusService;


    public FileService(IRepositoryWrapper repositoryWrapper, IEnergyFlowService energyFlowService, IBiohazardRadiusService biohazardRadiusService)
    {
        _repositoryWrapper = repositoryWrapper;
        _energyFlowService = energyFlowService;
        _biohazardRadiusService = biohazardRadiusService;
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

    public async Task<BaseResponse<bool>> ReadExcel(string filePath,TranslatorSpecs translatorSpecs,DirectionType type)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                List<RadiationZone> list = new List<RadiationZone>();
                for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                {
                    var degreeCellValue = worksheet.Cells[row, 1].Value;
                    var valueCellValue = worksheet.Cells[row, 2].Value;
                    if (int.TryParse(degreeCellValue?.ToString(), out int degree) && decimal.TryParse(valueCellValue?.ToString(), out decimal value))
                    {
                        RadiationZone radiationZone = new RadiationZone()
                        {
                            Degree = degree,
                            Value = value,
                            DirectionType = type,
                            TranslatorSpecsId = translatorSpecs.Id
                        };
                        await _repositoryWrapper.RadiationZoneRepository.CreateAsync(radiationZone);
                        list.Add(radiationZone);
                    }
                }
                if (list.Count != 360)
                {
                    return new BaseResponse<bool>(
                        Result: false,
                        Messages: new List<string> { "Файл не корректный" },
                        Success: false);
                }
                await _repositoryWrapper.Save();
            }
        }
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Файл успешно считан" },
            Success: true);
    }
    
    

    public async Task<BaseResponse<bool>> ProjectWord(string oid)
    {
        OfficeCharts.Instance.ActivateCrossPlatformCharts();
        var project = await  _repositoryWrapper.ProjectRepository.GetByCondition(x =>
            x.Id.ToString() == oid);
        var contrAgent = project.ContrAgent;
        var year = (project.SanPinDock?.DateOfIssue.Year) ?? DateTime.Now.Year;
        var executor = project.Executor;
        var executiveCompany = project.ExecutiveCompany;
        var summaryBiohazardRadius = project.SummaryBiohazardRadius;
        var es = project.TotalFluxDensity;
        using (var wordProcessor = new RichEditDocumentServer()) 
        { 
            wordProcessor.LoadDocument("Шаблон.docx");
            Document document = wordProcessor.Document;
            document.ReplaceAll("ContrAgent", $"{contrAgent.CompanyName}", SearchOptions.WholeWord);
            document.ReplaceAll("ExecutiveCompanyName", $"{executiveCompany.CompanyName}", SearchOptions.WholeWord);
            document.ReplaceAll("ExecutiveCompanyBIN", $"{executiveCompany.BIN}", SearchOptions.WholeWord);
            document.ReplaceAll("ProjectAddress", $"{project.Address}", SearchOptions.WholeWord);
            document.ReplaceAll("ExecutiveCompanyAddress", $"{executiveCompany.Address}", SearchOptions.WholeWord);
            document.ReplaceAll("ExecutorFIO", $"{executor.Surname} {executor.Name}", SearchOptions.WholeWord);
            document.ReplaceAll("ExecutorEmail", $"{executor.Login}", SearchOptions.WholeWord);
            document.ReplaceAll("License", $"{executiveCompany.LicenseNumber} от {executiveCompany.LicenseDateOfIssue} г.", SearchOptions.WholeWord);
            document.ReplaceAll("ProjectNumber", $"{project.ProjectNumber}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentPhone", $"{contrAgent.PhoneNumber}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentBIN", $"{contrAgent.BIN}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentFIO", 
                    $"{contrAgent.DirectorSurname} {contrAgent.DirectorName} {contrAgent.DirectorPatronymic}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentAddress", $"{contrAgent.Address}", SearchOptions.WholeWord);
            document.ReplaceAll("DateYear", $"{DateTime.Now.Year}", SearchOptions.WholeWord);
            document.ReplaceAll("YearOfInitial", $"{year}", SearchOptions.WholeWord);
            
            var projectAntennae = _repositoryWrapper.ProjectAntennaRepository
                .GetAllByCondition(x=> x.ProjectId == project.Id).ToList();

            
            for (int l = 0; l < projectAntennae.Count; l++)
            {
                var textAntennae = "";
                var gain = "";
                var power = "";
                var height = "";
                var frequency = "";
                var type = "";
                var powerList = new List<string>();
                var gainList = new List<string>();
                var heightList = new List<string>();
                var frequencyList = new List<string>();
                var typeList = new List<string>();
                var antennaTranslators = _repositoryWrapper.AntennaTranslatorRepository
                    .GetAllByCondition(x => x.ProjectAntennaId == projectAntennae[l].Id).ToList();
                var number = 1;
                powerList.AddRange(antennaTranslators.Select(antennaTranslator => antennaTranslator.Power.ToString()));
                gainList.AddRange(antennaTranslators.Select(antennaTranslator => antennaTranslator.Gain.ToString()));
                heightList.AddRange(antennaTranslators.Select(antennaTranslator => antennaTranslator.ProjectAntenna.Height.ToString()));
                frequencyList.AddRange(antennaTranslators.Select(antennaTranslator => antennaTranslator.TranslatorSpecs.Frequency.ToString()));
                typeList.AddRange(antennaTranslators.Select(antennaTranslator => antennaTranslator.TranslatorType.Type));
                var antennaTranslatorId = Guid.Empty;//переделать
                foreach (var antennaTranslator in antennaTranslators)
                {
                    
                    document.ReplaceAll("ContrAgent", $"{contrAgent.CompanyName}", SearchOptions.WholeWord);
                    antennaTranslatorId = antennaTranslator.Id;
                    antennaTranslator.TranslatorType = await _repositoryWrapper.TranslatorTypeRepository
                        .GetByCondition(x => x.Id == antennaTranslator.TranslatorTypeId);
                    var bioHorizontal = _repositoryWrapper.BiohazardRadiusRepository.GetAllByCondition(x =>
                        x.AntennaTranslatorId == antennaTranslator.Id && x.DirectionType == DirectionType.Horizontal).OrderBy(x=>x.Degree).ToList();
                    
                    var maxHorizontalZ = bioHorizontal.Max(radiation => Math.Abs(radiation.BiohazardRadiusZ));
                    var radiationMaxHorizontalZ = bioHorizontal.First(radiation => radiation.BiohazardRadiusZ == maxHorizontalZ);
                    var horizontalX = radiationMaxHorizontalZ.BiohazardRadiusX;
                    var horizontalBack = await _repositoryWrapper.BiohazardRadiusRepository
                        .GetByCondition(x => x.Degree == 180 && x.DirectionType == DirectionType.Horizontal);
                    var maxMaximumHorizontal = bioHorizontal.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
                    var maxRadiationHorizontal = bioHorizontal
                        .First(radiation => radiation.MaximumBiohazardRadius == maxMaximumHorizontal);
                    
                    
                    
                    var bioVertical = _repositoryWrapper.BiohazardRadiusRepository.GetAllByCondition(x =>
                        x.AntennaTranslatorId == antennaTranslator.Id && x.DirectionType == DirectionType.Vertical).OrderBy(x=>x.Degree).ToList();
                    var minVerticalZ = bioVertical.Min(radiation => radiation.BiohazardRadiusZ);
                    var radiationMinVerticalZ = bioVertical
                        .FirstOrDefault(radiation => radiation.BiohazardRadiusZ == minVerticalZ);
                    var verticalX = radiationMinVerticalZ.BiohazardRadiusX;
                    var verticalBack = await _repositoryWrapper.BiohazardRadiusRepository
                        .GetByCondition(x => x.Degree == 180 && x.DirectionType == DirectionType.Vertical);
                    var maxMaximumVertical = bioVertical.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
                    var maxRadiationVertical = bioHorizontal
                        .First(radiation => radiation.MaximumBiohazardRadius == maxMaximumVertical);
                    var maxRadius = Math.Max(maxMaximumHorizontal,maxMaximumVertical);
                    
                    var countTable = CheckCountTable(bioVertical, maxRadiationHorizontal.Degree, radiationMaxHorizontalZ.Degree,DirectionType.Vertical) - 1;
                    
                    
                    




                    var keywords = document.FindAll("Table",SearchOptions.WholeWord);
                    DocumentPosition insertPosition = keywords[0].Start;
                    ParagraphProperties titleParagraphProperties = document.BeginUpdateParagraphs(keywords[0]);
                    titleParagraphProperties.Alignment = ParagraphAlignment.Center;
                    document.EndUpdateParagraphs(titleParagraphProperties);
                    document.InsertText(insertPosition, $"Владелец радиоэлектронных средств: {contrAgent.CompanyName}\n");
                    document.Delete(keywords[0]);
                    Paragraph newAppendedParagraph = document.Paragraphs.Insert(insertPosition);
                    Table oldTable = document.Tables.Create(newAppendedParagraph.Range.Start, countTable, 12);

                    oldTable.Rows.InsertBefore(0);
                    oldTable.Rows.InsertAfter(0);
                    
                    oldTable.Rows[0].Cells.Append();
                    Table table = document.Tables.Last;
                    table.TableAlignment = TableRowAlignment.Center;
                    table.MergeCells(table[0, 6], table[countTable+1, 6]);
                    table.BeginUpdate();
                    for (int i = 0; i <= 12; i++)
                    {
                        TableCell columnCell = table[i, i];
                        columnCell.PreferredWidthType = WidthType.Auto;
                        columnCell.PreferredWidth = Units.InchesToDocumentsF(0.66f);
                        for (int j = 0; j <= countTable+1; j++)
                        {
                            columnCell = table[j, i];
                            columnCell.HeightType = HeightType.Auto;
                            columnCell.Height = 0.131f;
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
                    
                    document.InsertSingleLineText(table[0, 7].Range.Start, "v, град");
                    document.InsertSingleLineText(table[0, 8].Range.Start, "f(v), dBi");
                    document.InsertSingleLineText(table[0, 9].Range.Start, "f(v), раз");
                    document.InsertSingleLineText(table[0, 10].Range.Start, "Rб, м");
                    document.InsertSingleLineText(table[0, 11].Range.Start, "Rz, м");
                    document.InsertSingleLineText(table[0, 12].Range.Start, "Rx, м");
                    
                    
                    CreateTable360(document, table, bioVertical,maxRadiationHorizontal.Degree,radiationMaxHorizontalZ.Degree,DirectionType.Vertical);
                    CreateTable360(document, table, bioHorizontal,maxRadiationVertical.Degree,radiationMinVerticalZ.Degree,DirectionType.Horizontal);
                    
                    Paragraph newMaxAppendedParagraphText = document.Paragraphs.Insert(table.Range.End);
                    ParagraphProperties paragraphTextProperties = document.BeginUpdateParagraphs(newMaxAppendedParagraphText.Range);
                    CharacterProperties cpFirst = document.BeginUpdateCharacters(newMaxAppendedParagraphText.Range);
                    cpFirst.FontSize = 10;
                    cpFirst.FontName = "Cambria Math";
                    document.EndUpdateCharacters(cpFirst);
                    document.InsertText(newMaxAppendedParagraphText.Range.Start,"Максимальный радиус биологически-опасной зоны от секторных " +
                                                                             $"антенн {antennaTranslator.ProjectAntenna.Antenna.Model} в направлении излучения равен " +
                                                                             $"{maxRadius.ToString("F3")} м" +
                                                                             $" (стандарт {antennaTranslator.TranslatorType.Type}; мощность передатчика {antennaTranslator.Power} Вт; " +
                                                                             $"частота на передачу {antennaTranslator.TranslatorSpecs.Frequency} МГц;" +
                                                                             $" коэффициент усиления антенн {antennaTranslator.Gain} дБ, " +
                                                                             $"направление антенны в вертикальной плоскости " +
                                                                             $"{antennaTranslator.ProjectAntenna.Tilt}°).\n " +
                                                                             "В вертикальном сечении БОЗ повторяет диаграмму направленности." +
                                                                             " Максимальное отклонение от оси в вертикальном сечении составляет " +
                                                                             $"{minVerticalZ.ToString("F3")} м." +
                                                                             $" на расстоянии {verticalX} м. от центра излучения. " +
                                                                             "Максимальный радиус биологически-опасного излучения " +
                                                                             "от заднего лепестка антенны составил " +
                                                                             $"{verticalBack.MaximumBiohazardRadius.ToString("F3")} м.\n " +
                                                                             "В горизонтальном сечении БОЗ повторяет диаграмму направленности. " +
                                                                             "Максимальное отклонение от оси в горизонтальном сечении составляет " +
                                                                             $"{maxHorizontalZ.ToString("F3")} м." +
                                                                             $" на расстоянии {horizontalX} м. от центра излучения. " +
                                                                             "Максимальный радиус биологически-опасного излучения от" +
                                                                             $" заднего лепестка антенны составил {horizontalBack.MaximumBiohazardRadius.ToString("F3")} м.");
                    table.EndUpdate();
                    var secondSection = document.AppendSection();
                    Table oldTableSecond = document.Tables.Create(secondSection.Range.Start, 5, 2);
                    
                    oldTableSecond.Rows[0].Cells.Append();
                    Table tableSecond = document.Tables.Last;
                    tableSecond.TableAlignment = TableRowAlignment.Center;
                    tableSecond.BeginUpdate();
                    for (int i = 0; i <= 2; i++)
                    {
                        TableCell columnCell = tableSecond[i, i];
                        columnCell.PreferredWidthType = WidthType.Auto;
                        for (int j = 0; j < 5; j++)
                        {
                            columnCell = tableSecond[j, i];
                            columnCell.HeightType = HeightType.Auto;
                            columnCell.Height = 0.250f;
                            DocumentRange cellRange = columnCell.Range;
                            CharacterProperties cp = document.BeginUpdateCharacters(cellRange);
                            cp.FontSize = 12;
                            cp.FontName = "Cambria Math";
                            document.EndUpdateCharacters(cp);
                        }
                    }

                    document.InsertSingleLineText(tableSecond[0, 0].Range.Start, "Расчет биологически опасной зоны от секторной антенны:");
                    document.InsertSingleLineText(tableSecond[1, 0].Range.Start, "Рабочая частота (диапазон частот) на передачу, МГц чу, Вт:");
                    document.InsertSingleLineText(tableSecond[2, 0].Range.Start, "Мощность на передачу, Вт:");
                    document.InsertSingleLineText(tableSecond[3, 0].Range.Start, "Коэффициент усиления антенн, дБ");
                    document.InsertSingleLineText(tableSecond[4, 0].Range.Start, "Стандарт:");
                    
                    document.InsertSingleLineText(tableSecond[0, 1].Range.Start, $"{antennaTranslator.ProjectAntenna.Antenna.Model}");
                    document.InsertSingleLineText(tableSecond[1, 1].Range.Start, $"{antennaTranslator.TranslatorSpecs.Frequency}");
                    document.InsertSingleLineText(tableSecond[2, 1].Range.Start, $"{antennaTranslator.Power}");
                    document.InsertSingleLineText(tableSecond[3, 1].Range.Start, $"{antennaTranslator.Gain}");
                    document.InsertSingleLineText(tableSecond[4, 1].Range.Start, $"{antennaTranslator.TranslatorType.Type}");
                        
                    document.InsertSingleLineText(tableSecond[0, 2].Range.Start, $"Владелец радиоэлектронных средств: {contrAgent.CompanyName}");
                    document.InsertSingleLineText(tableSecond[1, 2].Range.Start, $"Угол наклона антенны {antennaTranslator.ProjectAntenna.Tilt}°");
                    document.InsertSingleLineText(tableSecond[2, 2].Range.Start, $"Передатчик №{number}");

                    
                    tableSecond.MergeCells(tableSecond[0, 0], tableSecond[4, 0]);
                    tableSecond.MergeCells(tableSecond[0, 1], tableSecond[4, 1]);
                    tableSecond.MergeCells(tableSecond[0, 2], tableSecond[4, 2]);
                    tableSecond.EndUpdate();
                    ParagraphProperties paragraphProperties = document.BeginUpdateParagraphs(secondSection.Range);
                    paragraphProperties.Alignment = ParagraphAlignment.Center;
                    CharacterProperties cpSecond = document.BeginUpdateCharacters(secondSection.Range);
                    cpSecond.FontSize = 11;
                    cpSecond.FontName = "Cambria Math";
                    document.EndUpdateCharacters(cpSecond);
                    document.EndUpdateParagraphs(paragraphProperties);
                    document.InsertText(secondSection.Range.End,"\nРасчеты размеров БОЗ в вертикальной и горизонтальной плоскостях:\n" +
                                                                "Биологически-опасная зона антенны повторяет форму диаграммы направленности в горизонтальной и вертикальной плоскости.\n" +
                                                                "Максимальный радиус биологически опасной зоны, Rб, м, в направлении излучения определяется по формуле:\n" +
                                                                "Rб = [(8*P* G( 𝜽 )*K* η)/ Ппду]^0,5 * F( 𝜽 ) * F( 𝝋 )\n" +
                                                                "Для определения максимального радиуса БОЗ примем F(𝜃)=1 и F(𝜑)=1:\n" +
                                                                $"Максимальный радиус БОЗ составляет Rmax= {maxRadius} м.\n" +
                                                                "Форму поперечного сечения биологически опасной зоны рассчитаем при помощи формул:\n" +
                                                                "Rz=Rmax•sin 𝝋, Rx=Rmax•cos 𝝋.                                        Rz=Rmax•sin 𝜃, Rx=Rmax•cos 𝜃 \n" +
                                                                "для горизонтальной плоскости                                               " +
                                                                "для вертикальной плоскости \n" +
                                                                "Значение Rz указывает на отклонение БОЗ от оси излучения антенны," +
                                                                " перпендикулярно к ней на расстоянии Rx от центра излучения вдоль оси");
                    await CreateDiagram(document,secondSection.Range.End,bioHorizontal);
                    await CreateDiagram(document,secondSection.Range.End,bioVertical);
                    
                    var thirdSection = document.InsertSection(secondSection.Range.End);
                    document.InsertText(thirdSection.Range.End,"Table");
                    table.EndUpdate();
                }
                var summary = _repositoryWrapper.SummaryBiohazardRadiusRepository
                    .GetAllByCondition(x => x.AntennaTranslatorId == antennaTranslatorId);
                var horizontalSummary =  summary.Where(x => x.DirectionType == DirectionType.Horizontal)
                    .OrderBy(x=>x.Degree).ToList();
                var maxHorizontalSummaryZ = horizontalSummary.Max(x => Math.Abs(x.BiohazardRadiusZ));
                var radiationMaxHorizontalSummaryZ = horizontalSummary
                    .Where(x => Math.Abs(x.BiohazardRadiusZ) == maxHorizontalSummaryZ)
                    .FirstOrDefault();
                var horizontalSummaryX = radiationMaxHorizontalSummaryZ.BiohazardRadiusX;
                var horizontalSummaryBack = await _repositoryWrapper.SummaryBiohazardRadiusRepository
                    .GetByCondition(x => x.Degree == 180 && x.DirectionType == DirectionType.Horizontal);
                var maxMaximumHorizontalSummary = horizontalSummary.Max(x => Math.Abs(x.MaximumBiohazardRadius));
                var maxRadiationHorizontalSummary = horizontalSummary
                    .First(x => x.MaximumBiohazardRadius == maxMaximumHorizontalSummary);
                
                var verticalSummary = summary.Where(x => x.DirectionType == DirectionType.Vertical)
                    .OrderBy(x=>x.Degree).ToList();
                var minVerticalSummaryZ = verticalSummary.Min(x => x.BiohazardRadiusZ);
                var radiationMinVerticalSummaryZ = verticalSummary.First(x => x.BiohazardRadiusZ == minVerticalSummaryZ);
                var verticalSummaryX = radiationMinVerticalSummaryZ.BiohazardRadiusX;
                var verticalSummaryBack = await _repositoryWrapper.SummaryBiohazardRadiusRepository
                    .GetByCondition(x => x.Degree == 180 && x.DirectionType == DirectionType.Horizontal);
                var maxMaximumVerticalSummary = verticalSummary.Max(x => Math.Abs(x.MaximumBiohazardRadius));
                var maxRadiationVerticalSummary = verticalSummary
                    .First(x => x.MaximumBiohazardRadius == maxMaximumVerticalSummary);
                
                var maxSummaryRadius = Math.Max(maxMaximumHorizontalSummary,maxMaximumVerticalSummary);
                var keywordsMax = document.FindAll("Table",SearchOptions.WholeWord);
                ParagraphProperties maxParagraphProperties = document.BeginUpdateParagraphs(keywordsMax[0]);
                maxParagraphProperties.Alignment = ParagraphAlignment.Center;
                document.EndUpdateParagraphs(maxParagraphProperties);
                document.InsertText(keywordsMax[0].Start, $"Владелец радиоэлектронных средств: {contrAgent.CompanyName}\n");
                document.Delete(keywordsMax[0]);
                Paragraph maxAppendedParagraph = document.Paragraphs.Insert(keywordsMax[0].End);
                    
                    
                var maxCountTable = CheckMaxCountTable(verticalSummary, 
                    maxRadiationHorizontalSummary.Degree, radiationMaxHorizontalSummaryZ.Degree,DirectionType.Vertical) - 1;
                Table newMaxTable = document.Tables.Create(maxAppendedParagraph.Range.Start, maxCountTable, 8);

                newMaxTable.Rows.InsertBefore(0);
                newMaxTable.Rows.InsertAfter(0);
                
                newMaxTable.Rows[0].Cells.Append();
                Table maxTable = document.Tables.Last;
                maxTable.TableAlignment = TableRowAlignment.Center;
                maxTable.MergeCells(maxTable[0, 4], maxTable[maxCountTable+1, 4]);
                maxTable.BeginUpdate();
                for (int i = 0; i <= 8; i++)
                {
                    TableCell columnCellMax = maxTable[i, i];
                    columnCellMax.PreferredWidthType = WidthType.Auto;
                    columnCellMax.PreferredWidth = Units.InchesToDocumentsF(0.66f);
                    for (int j = 0; j <= maxCountTable+1; j++)
                    {
                        columnCellMax = maxTable[j, i];
                        columnCellMax.HeightType = HeightType.Auto;
                        columnCellMax.Height = 0.131f;
                        CharacterProperties cpMax = document.BeginUpdateCharacters(columnCellMax.Range);
                        cpMax.FontSize = 8;
                        document.EndUpdateCharacters(cpMax);
                    }
                }

                document.InsertSingleLineText(maxTable[0, 0].Range.Start, "v, град");
                document.InsertSingleLineText(maxTable[0, 1].Range.Start, "Rб, м");
                document.InsertSingleLineText(maxTable[0, 2].Range.Start, "Rz, м");
                document.InsertSingleLineText(maxTable[0, 3].Range.Start, "Rx, м");
                
                document.InsertSingleLineText(maxTable[0, 5].Range.Start, "v, град");
                document.InsertSingleLineText(maxTable[0, 6].Range.Start, "Rб, м");
                document.InsertSingleLineText(maxTable[0, 7].Range.Start, "Rz, м");
                document.InsertSingleLineText(maxTable[0, 8].Range.Start, "Rx, м");
                
                
                CreateTableMaximum360(document, maxTable, verticalSummary,maxRadiationHorizontalSummary.Degree,
                    radiationMaxHorizontalSummaryZ.Degree,DirectionType.Vertical);
                CreateTableMaximum360(document, maxTable, horizontalSummary,maxRadiationVerticalSummary.Degree,
                    radiationMinVerticalSummaryZ.Degree,DirectionType.Horizontal);
                
                Paragraph newAppendedParagraphText = document.Paragraphs.Insert(maxTable.Range.End);
                document.BeginUpdateParagraphs(newAppendedParagraphText.Range);
                CharacterProperties cpNew = document.BeginUpdateCharacters(newAppendedParagraphText.Range);
                cpNew.FontSize = 10;
                cpNew.FontName = "Cambria Math";
                document.EndUpdateCharacters(cpNew);
                document.InsertText(newAppendedParagraphText.Range.Start, "Максимальный радиус биологически-опасной зоны от секторных ");
                document.InsertText(newAppendedParagraphText.Range.Start,"Максимальный радиус биологически-опасной зоны от секторных " +
                                                                         $"антенн {projectAntennae[l].Antenna.Model} в направлении излучения равен " +
                                                                         $"{maxSummaryRadius.ToString("F3")} м" +
                                                                         $" (стандарт {type}; мощность передатчика {power} Вт; " +
                                                                         $"частота на передачу {frequency} МГц;" +
                                                                         $" коэффициент усиления антенн {gain} дБ, " +
                                                                         $"направление антенны в вертикальной плоскости " +
                                                                         $"{projectAntennae[l].Tilt}°).\n " +
                                                                         "В вертикальном сечении БОЗ повторяет диаграмму направленности." +
                                                                         " Максимальное отклонение от оси в вертикальном сечении составляет " +
                                                                         $"{minVerticalSummaryZ.ToString("F3")} м." +
                                                                         $" на расстоянии {verticalSummaryX} м. от центра излучения. " +
                                                                         "Максимальный радиус биологически-опасного излучения " +
                                                                         "от заднего лепестка антенны составил " +
                                                                         $"{verticalSummaryBack.MaximumBiohazardRadius.ToString("F3")} м.\n " +
                                                                         "В горизонтальном сечении БОЗ повторяет диаграмму направленности. " +
                                                                         "Максимальное отклонение от оси в горизонтальном сечении составляет " +
                                                                         $"{maxHorizontalSummaryZ.ToString("F3")} м." +
                                                                         $" на расстоянии {horizontalSummaryX} м. от центра излучения. " +
                                                                         "Максимальный радиус биологически-опасного излучения от" +
                                                                         $" заднего лепестка антенны составил {horizontalSummaryBack.MaximumBiohazardRadius.ToString("F3")} м.");
                maxTable.EndUpdate();
                var secondSectionMax = document.AppendSection();
                Table oldTableSecondMax = document.Tables.Create(secondSectionMax.Range.Start, 5, 2);
                
                oldTableSecondMax.Rows[0].Cells.Append();
                Table tableSecondMax = document.Tables.Last;
                tableSecondMax.TableAlignment = TableRowAlignment.Center;
                tableSecondMax.BeginUpdate();
                for (int i = 0; i <= 2; i++)
                {
                    TableCell columnCellMax = tableSecondMax[i, i];
                    columnCellMax.PreferredWidthType = WidthType.Auto;
                    for (int j = 0; j < 5; j++)
                    {
                        columnCellMax = tableSecondMax[j, i];
                        columnCellMax.HeightType = HeightType.Auto;
                        columnCellMax.Height = 0.250f;
                        CharacterProperties cpMax = document.BeginUpdateCharacters(columnCellMax.Range);
                        cpMax.FontSize = 12;
                        cpMax.FontName = "Cambria Math";
                        document.EndUpdateCharacters(cpMax);
                    }
                }

                gain = string.Join(";", gainList);
                type = string.Join(";", typeList);
                power = string.Join(";", powerList);
                frequency = string.Join(";", frequencyList);
                height = string.Join(";", heightList);
                var sector = string.Join(",", Enumerable.Range(1, number - 1));
                textAntennae += $"{projectAntennae[l].Antenna.Model} (сектор {sector} – {antennaTranslators.Count} шт.)";
                
                document.InsertSingleLineText(tableSecondMax[0, 0].Range.Start, "Расчет биологически опасной зоны от секторной антенны:");
                document.InsertSingleLineText(tableSecondMax[1, 0].Range.Start, "Рабочая частота (диапазон частот) на передачу, МГц чу, Вт:");
                document.InsertSingleLineText(tableSecondMax[2, 0].Range.Start, "Мощность на передачу, Вт:");
                document.InsertSingleLineText(tableSecondMax[3, 0].Range.Start, "Коэффициент усиления антенн, дБ");
                document.InsertSingleLineText(tableSecondMax[4, 0].Range.Start, "Стандарт:");
                
                document.InsertSingleLineText(tableSecondMax[0, 1].Range.Start, $"{projectAntennae[l].Antenna.Model}");
                document.InsertSingleLineText(tableSecondMax[1, 1].Range.Start, $"{frequency}");
                document.InsertSingleLineText(tableSecondMax[2, 1].Range.Start, $"{power}");
                document.InsertSingleLineText(tableSecondMax[3, 1].Range.Start, $"{gain}");
                document.InsertSingleLineText(tableSecondMax[4, 1].Range.Start, $"{type}");

                document.InsertSingleLineText(tableSecondMax[0, 2].Range.Start, $"Владелец радиоэлектронных средств: {contrAgent.CompanyName}");
                document.InsertSingleLineText(tableSecondMax[1, 2].Range.Start, $"Угол наклона антенны {projectAntennae[l].Tilt}°");
                document.InsertSingleLineText(tableSecondMax[1, 2].Range.Start, $"Угол наклона антенны °");
                document.InsertSingleLineText(tableSecondMax[2, 2].Range.Start, $"Передатчик №{number}");

                
                tableSecondMax.MergeCells(tableSecondMax[0, 0], tableSecondMax[4, 0]);
                tableSecondMax.MergeCells(tableSecondMax[0, 1], tableSecondMax[4, 1]);
                tableSecondMax.MergeCells(tableSecondMax[0, 2], tableSecondMax[4, 2]);
                tableSecondMax.EndUpdate();
                ParagraphProperties paragraphPropertiesMax = document.BeginUpdateParagraphs(secondSectionMax.Range);
                paragraphPropertiesMax.Alignment = ParagraphAlignment.Center;
                CharacterProperties cpSecondMax = document.BeginUpdateCharacters(secondSectionMax.Range);
                cpSecondMax.FontSize = 11;
                cpSecondMax.FontName = "Cambria Math";
                document.EndUpdateCharacters(cpSecondMax);
                document.EndUpdateParagraphs(paragraphPropertiesMax);
                var formula = string.Join(" + ", Enumerable.Range(1, antennaTranslators.Count).Select(i => $"𝑅𝑅б{i}²"));
                document.InsertText(secondSectionMax.Range.End,"\nСуммируем результаты радиуса биологически-опасной зоны от передатчиков" +
                                                               " секторной антенны в вертикальной и горизонтальной плоскостях для определения" +
                                                               $" максимального радиус биологически-опасной зоны от секторной антенны: \nRб =√{formula}");
                await CreateDiagramSummary(document,secondSectionMax.Range.End,horizontalSummary);
                await CreateDiagramSummary(document,secondSectionMax.Range.End,verticalSummary);
                
                var thirdSectionMax = document.InsertSection(secondSectionMax.Range.End);
                document.InsertText(thirdSectionMax.Range.End,"Table");
                tableSecondMax.EndUpdate();
                number++;
                
                var antennae = document.FindAll("Antennae",SearchOptions.WholeWord);
                document.InsertText(antennae[0].Start, $"Антенна {projectAntennae[l].Antenna.Model} (сектор {sector} – " +
                                                $"{antennaTranslators.Count} шт.) " +
                                                $"Антенны размещаются на трубостойке на крыше, на высоте {projectAntennae[l].Height} м. " +
                                                $"Частота передачи {frequency} МГц. Коэффициент усиления {gain} дБ. " +
                                                $"Мощность передатчиков {power} Вт. Максимальный радиус биологически-опасной зоны от секторных антенн" +
                                                $" {projectAntennae[l].Antenna.Model} в направлении излучения равен Maximum м " +
                                                $"(угол наклона антенны {projectAntennae[l].Tilt}°). " +
                                                $"В вертикальном сечении БОЗ повторяет диаграмму направленности." +
                                                $" Максимальное отклонение от оси в вертикальном сечении составляет 1,829 м. на расстоянии 26,157 м. от центра излучения." +
                                                $" Максимальный радиус биологически-опасного излучения от заднего лепестка антенны составил 0,074 м." +
                                                $" В горизонтальном сечении БОЗ повторяет диаграмму направленности." +
                                                $" Максимальное отклонение от оси в горизонтальном сечении составляет 13,253 м. на расстоянии 24,925 м. от центра излучения. " +
                                                $"Максимальный радиус биологически-опасного излучения от заднего лепестка антенны составил 0,034 м.");
                document.Delete(antennae[0]);
                var azimut = document.FindAll("Azimut",SearchOptions.WholeWord);
                var azimutText = $"в направлении {projectAntennae[l].Azimuth}°; на расстоянии 47,313 м от антенны.";
                if (l != projectAntennae.Count - 1)
                {
                    document.InsertText(antennae[0].End, $"\nAntennae");
                    azimutText = $"в направлении {projectAntennae[l].Azimuth}°; на расстоянии 47,313 м от антенны.\nAzimut";
                }
                document.InsertText(azimut[0].Start, azimutText);
                document.Delete(azimut[0]);
                
            }

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


    private async Task<BaseResponse<bool>> CreateDiagram(Document document,DocumentPosition position,List<BiohazardRadius> biohazardRadii)
    {
        document.Unit = DevExpress.Office.DocumentUnit.Inch;
        var chartShape = document.Shapes.InsertChart(position,ChartType.ScatterSmooth);
        chartShape.Name = "Scatter Line chart";
        chartShape.Size = new SizeF(4.5f, 3.7f);
        chartShape.RelativeHorizontalPosition = ShapeRelativeHorizontalPosition.Column;
        chartShape.RelativeVerticalPosition = ShapeRelativeVerticalPosition.Line;
        chartShape.Offset = new PointF(0.95f, 0.65f);
        ChartObject chart = (ChartObject)chartShape.ChartFormat.Chart;
        Worksheet worksheet = (Worksheet)chartShape.ChartFormat.Worksheet;
        
        await SpecifyChartData(worksheet,biohazardRadii);
        chart.SelectData(worksheet.Range.FromLTRB(0, 0, 1, 360));
        chart.Legend.Visible = false;
        chart.Title.Visible = true;
        chart.Title.Font.Size = 8;
        var text = "Ширина БОЗ в вертикальной плоскости на расстоянии Rx от \nантенны вдоль линии горизонта по направлению излучения";
        if (biohazardRadii.First().DirectionType == DirectionType.Horizontal)
        {
            text = "Ширина БОЗ в горизонтальной плоскости на расстоянии Rx от \nантенны вдоль линии горизонта по направлению излучения";
        }
        chart.Title.SetValue(text);
        Axis valueAxisX = chart.PrimaryAxes[1];
        Axis valueAxisY = chart.PrimaryAxes[0];
        valueAxisX.Scaling.AutoMax = false;
        valueAxisX.Scaling.Max = 15;
        valueAxisX.Scaling.AutoMin = false;
        valueAxisX.Scaling.Min = -15;
        valueAxisY.Scaling.AutoMin = false;
        valueAxisY.Scaling.Min = -1;
        
        chart.Series[0].Outline.SetSolidFill(Color.FromArgb(0x00, 0x00, 0x00));
        chart.Series[0].Outline.Width = 1.2;
        
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Файл успешно создан" },
            Success: true);
    }
    
    private async Task<BaseResponse<bool>> CreateDiagramSummary(Document document,DocumentPosition position,List<SummaryBiohazardRadius> summaryBiohazardRadii)
    {
        document.Unit = DevExpress.Office.DocumentUnit.Inch;
        var chartShape = document.Shapes.InsertChart(position,ChartType.ScatterSmooth);
        chartShape.Name = "Scatter Line chart";
        chartShape.Size = new SizeF(4.5f, 3.7f);
        chartShape.RelativeHorizontalPosition = ShapeRelativeHorizontalPosition.Column;
        chartShape.RelativeVerticalPosition = ShapeRelativeVerticalPosition.Line;
        chartShape.Offset = new PointF(0.95f, 0.65f);
        ChartObject chart = (ChartObject)chartShape.ChartFormat.Chart;
        Worksheet worksheet = (Worksheet)chartShape.ChartFormat.Worksheet;
        
        await SpecifyChartDataSummary(worksheet,summaryBiohazardRadii);
        chart.SelectData(worksheet.Range.FromLTRB(0, 0, 1, 360));
        chart.Legend.Visible = false;
        chart.Title.Visible = true;
        chart.Title.Font.Size = 8;
        var text = "Ширина БОЗ в вертикальной плоскости на расстоянии Rx от \nантенны вдоль линии горизонта по направлению излучения";
        if (summaryBiohazardRadii.First().DirectionType == DirectionType.Horizontal)
        {
            text = "Ширина БОЗ в горизонтальной плоскости на расстоянии Rx от \nантенны вдоль линии горизонта по направлению излучения";
        }
        chart.Title.SetValue(text);
        Axis valueAxisX = chart.PrimaryAxes[1];
        Axis valueAxisY = chart.PrimaryAxes[0];
        valueAxisX.Scaling.AutoMax = false;
        valueAxisX.Scaling.Max = 15;
        valueAxisX.Scaling.AutoMin = false;
        valueAxisX.Scaling.Min = -15;
        valueAxisY.Scaling.AutoMin = false;
        valueAxisY.Scaling.Min = -1;
        
        chart.Series[0].Outline.SetSolidFill(Color.FromArgb(0x00, 0x00, 0x00));
        chart.Series[0].Outline.Width = 1.2;
        
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Файл успешно создан" },
            Success: true);
    }
    
    private async Task<bool> SpecifyChartData(Worksheet sheet,List<BiohazardRadius> biohazard)
    {
        for (int i = 0; i < biohazard.Count; i++)
        {
            sheet[i, 0].Value = biohazard[i].BiohazardRadiusX;
            sheet[i, 1].Value = biohazard[i].BiohazardRadiusZ;
        }

        return true;
    }
    
    private async Task<bool> SpecifyChartDataSummary(Worksheet sheet,List<SummaryBiohazardRadius> summaryBiohazardRadii)
    {
        for (int i = 0; i < summaryBiohazardRadii.Count; i++)
        {
            sheet[i, 0].Value = summaryBiohazardRadii[i].BiohazardRadiusX;
            sheet[i, 1].Value = summaryBiohazardRadii[i].BiohazardRadiusZ;
        }

        return true;
    }

    private void CreateTable360(Document document,Table table,List<BiohazardRadius> biohazardRadii,int maxRadiusDegree, int minDegreeZ,DirectionType type)
    {
        var maxRadius = biohazardRadii.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
        var radiationMaxRadius = biohazardRadii
            .First(radiation => Math.Abs(radiation.MaximumBiohazardRadius) == maxRadius);
        var minRadiusValueZ = biohazardRadii.Min(radiation => radiation.BiohazardRadiusZ);
        var radiationZ = biohazardRadii
            .First(radiation => radiation.BiohazardRadiusZ == minRadiusValueZ);
        if (type == DirectionType.Horizontal)
        {
            var maxRadiusValueZ = biohazardRadii.Max(radiation => Math.Abs(radiation.BiohazardRadiusZ) );
            radiationZ = biohazardRadii
                .First(radiation => Math.Abs(radiation.BiohazardRadiusZ) == maxRadiusValueZ);
        }
        var y = 1;
        for (int i = 0; i < biohazardRadii.Count; i++) 
        {
            var x = i % 10;
            if (x == 0 || biohazardRadii[i] == radiationMaxRadius || biohazardRadii[i] == radiationZ 
                || biohazardRadii[i].Degree == maxRadiusDegree || biohazardRadii[i].Degree == minDegreeZ) 
            {
                if (biohazardRadii[i].DirectionType == DirectionType.Horizontal)
                {
                    document.InsertText(table[y, 0].Range.Start, biohazardRadii[i].Degree.ToString());
                    document.InsertText(table[y, 1].Range.Start, biohazardRadii[i].Db.ToString("F3"));
                    document.InsertText(table[y, 2].Range.Start, biohazardRadii[i].DbRaz.ToString("F3"));
                    document.InsertText(table[y, 3].Range.Start, biohazardRadii[i].MaximumBiohazardRadius.ToString("F3"));
                    document.InsertText(table[y, 4].Range.Start, biohazardRadii[i].BiohazardRadiusZ.ToString("F3"));
                    document.InsertText(table[y, 5].Range.Start, biohazardRadii[i].BiohazardRadiusX.ToString("F3"));
                    y++;
                }
                if (biohazardRadii[i].DirectionType == DirectionType.Vertical)
                {
                    document.InsertText(table[y, 7].Range.Start, biohazardRadii[i].Degree.ToString());
                    document.InsertText(table[y, 8].Range.Start, biohazardRadii[i].Db.ToString("F3"));
                    document.InsertText(table[y, 9].Range.Start, biohazardRadii[i].DbRaz.ToString("F3"));
                    document.InsertText(table[y, 10].Range.Start, biohazardRadii[i].MaximumBiohazardRadius.ToString("F3"));
                    document.InsertText(table[y, 11].Range.Start, biohazardRadii[i].BiohazardRadiusZ.ToString("F3"));
                    document.InsertText(table[y, 12].Range.Start, biohazardRadii[i].BiohazardRadiusX.ToString("F3"));
                    y++;
                }
                 
            }
            
        }
    }
    
    
    private void CreateTableMaximum360(Document document,Table table,List<SummaryBiohazardRadius> summaryBiohazardRadii,int maxRadiusDegree, int minDegreeZ,DirectionType type)
    {
        var maxRadius = summaryBiohazardRadii.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
        var radiationMaxRadius = summaryBiohazardRadii
            .First(radiation => Math.Abs(radiation.MaximumBiohazardRadius) == maxRadius);
        var minRadiusValueZ = summaryBiohazardRadii.Min(radiation => radiation.BiohazardRadiusZ);
        var radiationZ = summaryBiohazardRadii
            .First(radiation => radiation.BiohazardRadiusZ == minRadiusValueZ);
        if (type == DirectionType.Horizontal)
        {
            var maxRadiusValueZ = summaryBiohazardRadii.Max(radiation => Math.Abs(radiation.BiohazardRadiusZ) );
            radiationZ = summaryBiohazardRadii
                .First(radiation => Math.Abs(radiation.BiohazardRadiusZ) == maxRadiusValueZ);
        }
        var y = 1;
        for (int i = 0; i < summaryBiohazardRadii.Count; i++) 
        {
            var x = i % 10;
            if (x == 0 || summaryBiohazardRadii[i] == radiationMaxRadius || summaryBiohazardRadii[i] == radiationZ 
                || summaryBiohazardRadii[i].Degree == maxRadiusDegree || summaryBiohazardRadii[i].Degree == minDegreeZ) 
            {
                if (summaryBiohazardRadii[i].DirectionType == DirectionType.Horizontal)
                {
                    document.InsertText(table[y, 0].Range.Start, summaryBiohazardRadii[i].Degree.ToString());
                    document.InsertText(table[y, 1].Range.Start, summaryBiohazardRadii[i].MaximumBiohazardRadius.ToString("F3"));
                    document.InsertText(table[y, 2].Range.Start, summaryBiohazardRadii[i].BiohazardRadiusZ.ToString("F3"));
                    document.InsertText(table[y, 3].Range.Start, summaryBiohazardRadii[i].BiohazardRadiusX.ToString("F3"));
                    y++;
                }
                if (summaryBiohazardRadii[i].DirectionType == DirectionType.Vertical)
                {
                    document.InsertText(table[y, 5].Range.Start, summaryBiohazardRadii[i].Degree.ToString());
                    document.InsertText(table[y, 6].Range.Start, summaryBiohazardRadii[i].MaximumBiohazardRadius.ToString("F3"));
                    document.InsertText(table[y, 7].Range.Start, summaryBiohazardRadii[i].BiohazardRadiusZ.ToString("F3"));
                    document.InsertText(table[y, 8].Range.Start, summaryBiohazardRadii[i].BiohazardRadiusX.ToString("F3"));
                    y++;
                }
                 
            }
            
        }
    }

    private int CheckCountTable(List<BiohazardRadius> biohazardRadii, int maxRadiusDegree, int degreeZ,DirectionType type)
    {
        var y = 0;
        var maxRadius = biohazardRadii.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
        var radiationMaxRadius = biohazardRadii
            .First(radiation => Math.Abs(radiation.MaximumBiohazardRadius) == maxRadius);
        var minRadiusValueZ = biohazardRadii.Min(radiation => radiation.BiohazardRadiusZ);
        var radiationZ = biohazardRadii
            .First(radiation => radiation.BiohazardRadiusZ == minRadiusValueZ);
        if (type == DirectionType.Horizontal)
        {
            var maxRadiusValueZ = biohazardRadii.Max(radiation => Math.Abs(radiation.BiohazardRadiusZ) );
            radiationZ = biohazardRadii
                .First(radiation => Math.Abs(radiation.BiohazardRadiusZ) == maxRadiusValueZ);
        }
        
        for (int i = 0; i < biohazardRadii.Count; i++) 
        {
            var x = i % 10;
            if (x == 0 || biohazardRadii[i] == radiationMaxRadius || biohazardRadii[i] == radiationZ
                || biohazardRadii[i].Degree == maxRadiusDegree || biohazardRadii[i].Degree == degreeZ)
            {
                y++;
            }
            
        }

        return y;
    }
    
    private int CheckMaxCountTable(List<SummaryBiohazardRadius> summaryBiohazardRadii, int maxRadiusDegree, int degreeZ,DirectionType type)
    {
        var y = 0;
        var maxRadius = summaryBiohazardRadii.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
        var radiationMaxRadius = summaryBiohazardRadii
            .First(radiation => Math.Abs(radiation.MaximumBiohazardRadius) == maxRadius);
        var minRadiusValueZ = summaryBiohazardRadii.Min(radiation => radiation.BiohazardRadiusZ);
        var radiationZ = summaryBiohazardRadii
            .First(radiation => radiation.BiohazardRadiusZ == minRadiusValueZ);
        if (type == DirectionType.Horizontal)
        {
            var maxRadiusValueZ = summaryBiohazardRadii.Max(radiation => Math.Abs(radiation.BiohazardRadiusZ) );
            radiationZ = summaryBiohazardRadii
                .First(radiation => Math.Abs(radiation.BiohazardRadiusZ) == maxRadiusValueZ);
        }
        
        for (int i = 0; i < summaryBiohazardRadii.Count; i++) 
        {
            var x = i % 10;
            if (x == 0 || summaryBiohazardRadii[i] == radiationMaxRadius || summaryBiohazardRadii[i] == radiationZ
                || summaryBiohazardRadii[i].Degree == maxRadiusDegree || summaryBiohazardRadii[i].Degree == degreeZ)
            {
                y++;
            }
            
        }

        return y;
    }
}