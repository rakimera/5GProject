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
        var executor = project.Executor;
        var executiveCompany = project.ExecutiveCompany;
        var summaryBiohazardRadius = project.SummaryBiohazardRadius;
        var es = project.TotalFluxDensity;
        using (var wordProcessor = new RichEditDocumentServer()) 
        { 
            wordProcessor.LoadDocument("Шаблон.docx");
            Document document = wordProcessor.Document;
            document.ReplaceAll("ContrAgent", $"{contrAgent.CompanyName}", SearchOptions.WholeWord);
            document.ReplaceAll("ExecutiveCompany", $"{executiveCompany.CompanyName}", SearchOptions.WholeWord);
            document.ReplaceAll("ProjectNumber", $"{project.ProjectNumber}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentPhone", $"{contrAgent.PhoneNumber}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentBIN", $"{contrAgent.BIN}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentFIO", 
                    $"{contrAgent.DirectorSurname} {contrAgent.DirectorName} {contrAgent.DirectorPatronymic}", SearchOptions.WholeWord);
            document.ReplaceAll("ContrAgentAddress", $"{contrAgent.Address}", SearchOptions.WholeWord);
            document.ReplaceAll("DateYear", 
                $"{DateTime.Now.Year}", SearchOptions.WholeWord);
            
            var projectAntennae = _repositoryWrapper.ProjectAntennaRepository
                .GetAllByCondition(x=> x.ProjectId == project.Id).ToList();
            
            for (int l = 0; l < projectAntennae.Count; l++)
            {
                var antennaTranslators = _repositoryWrapper.AntennaTranslatorRepository
                    .GetAllByCondition(x => x.ProjectAntennaId == projectAntennae[l].Id).ToList();
                foreach (var antennaTranslator in antennaTranslators)
                {
                    var bioHorizontal = _repositoryWrapper.BiohazardRadiusRepository.GetAllByCondition(x =>
                        x.AntennaTranslatorId == antennaTranslator.Id && x.DirectionType == DirectionType.Horizontal).OrderBy(x=>x.Degree).ToList();

                    var maxHorizontalZ = bioHorizontal.Max(radiation => Math.Abs(radiation.BiohazardRadiusZ));
                    var radiationMinHorizontalZ = bioHorizontal
                        .First(radiation => radiation.BiohazardRadiusZ == maxHorizontalZ);
                    var horizontalX = radiationMinHorizontalZ.BiohazardRadiusX;
                    var horizontalBack = await _repositoryWrapper.BiohazardRadiusRepository
                        .GetByCondition(x => x.Degree == 180 && x.DirectionType == DirectionType.Horizontal);
                    var maxMaximumHorizontal = bioHorizontal.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
                    var maxRadiationHorizontal = bioHorizontal
                        .First(radiation => radiation.MaximumBiohazardRadius == maxMaximumHorizontal);
                    
                    
                    
                    var bioVertical = _repositoryWrapper.BiohazardRadiusRepository.GetAllByCondition(x =>
                        x.AntennaTranslatorId == antennaTranslator.Id && x.DirectionType == DirectionType.Vertical).OrderBy(x=>x.Degree).ToList();
                    var minVerticalZ = bioVertical.Min(radiation => radiation.BiohazardRadiusZ);
                    var radiationMinVerticalZ = bioVertical
                        .First(radiation => radiation.BiohazardRadiusZ == minVerticalZ);
                    var verticalX = radiationMinVerticalZ.BiohazardRadiusX;
                    var verticalBack = await _repositoryWrapper.BiohazardRadiusRepository
                        .GetByCondition(x => x.Degree == 180 && x.DirectionType == DirectionType.Vertical);
                    var maxMaximumVertical = bioVertical.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
                    var maxRadiationVertical = bioHorizontal
                        .First(radiation => radiation.MaximumBiohazardRadius == maxMaximumVertical);
                    var maxRadius = Math.Max(maxMaximumHorizontal,maxMaximumVertical);

                    var countTable = CheckCountTable(bioVertical, maxRadiationHorizontal.Degree, radiationMinHorizontalZ.Degree,DirectionType.Vertical) - 1;




                    var keywords = document.FindAll("Table",SearchOptions.WholeWord);
                    DocumentPosition insertPosition = keywords[0].Start;
                    document.InsertText(insertPosition, $"Владелец радиоэлектронных средств: {contrAgent.CompanyName}\n");
                    ParagraphProperties titleParagraphProperties = document.BeginUpdateParagraphs(keywords[0]);
                    titleParagraphProperties.Alignment = ParagraphAlignment.Center;
                    document.EndUpdateParagraphs(titleParagraphProperties);
                    document.Delete(keywords[0]);
                    Paragraph newAppendedParagraph = document.Paragraphs.Insert(insertPosition);
                    Table oldTable = document.Tables.Create(newAppendedParagraph.Range.Start, countTable, 12);

                    oldTable.Rows.InsertBefore(0);
                    oldTable.Rows.InsertAfter(0);
                    
                    oldTable.Rows[0].Cells.Append();
                    Table table = document.Tables.Last;
                    table.TableAlignment = TableRowAlignment.Center;
                    table.MergeCells(table[0, 6], table[countTable, 6]);
                    table.BeginUpdate();
                    for (int i = 0; i <= 12; i++)
                    {
                        TableCell columnCell = table[i, i];
                        columnCell.PreferredWidthType = WidthType.Auto;
                        columnCell.PreferredWidth = Units.InchesToDocumentsF(0.66f);
                        for (int j = 0; j < countTable; j++)
                        {
                            columnCell = table[j, i];
                            columnCell.HeightType = HeightType.AtLeast;
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
                    
                    
                    CreateTable360(document, table, bioVertical,maxRadiationHorizontal.Degree,radiationMinHorizontalZ.Degree,DirectionType.Vertical);
                    CreateTable360(document, table, bioHorizontal,maxRadiationVertical.Degree,radiationMinVerticalZ.Degree,DirectionType.Horizontal);
                    document.InsertText(table.Range.End,"\nМаксимальный радиус биологически-опасной зоны от секторных " +
                                                        $"антенн {antennaTranslator.ProjectAntenna.Antenna.Model} в направлении излучения равен " +
                                                        $"{maxRadius.ToString("F3")} м" +
                                                        $" (стандарт {antennaTranslator.TranslatorType}; мощность передатчика {antennaTranslator.Power} Вт; " +
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
                    await CreateGrafic(document,secondSection.Range.Start,bioHorizontal);
                    await CreateGrafic(document,secondSection.Range.Start,bioVertical);
                    if (antennaTranslator != antennaTranslators.Last())
                    {
                        var thirdSection = document.InsertSection(secondSection.Range.End);
                        document.InsertText(thirdSection.Range.End,"Table");
                    }
                    table.EndUpdate();
                }

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


    private async Task<BaseResponse<bool>> CreateGrafic(Document document,DocumentPosition position,List<BiohazardRadius> biohazardRadii)
    {
        document.Unit = DevExpress.Office.DocumentUnit.Inch;
        var chartShape = document.Shapes.InsertChart(position,ChartType.ScatterSmooth);
        chartShape.Name = "Scatter Line chart";
        chartShape.Size = new SizeF(4f, 3.2f);
        chartShape.RelativeHorizontalPosition = ShapeRelativeHorizontalPosition.Column;
        chartShape.RelativeVerticalPosition = ShapeRelativeVerticalPosition.Line;
        chartShape.Offset = new PointF(0, 0);
        ChartObject chart = (ChartObject)chartShape.ChartFormat.Chart;
        Worksheet worksheet = (Worksheet)chartShape.ChartFormat.Worksheet;
        
        await SpecifyChartData(worksheet,biohazardRadii);
        chart.SelectData(worksheet.Range.FromLTRB(0, 0, 1, 360));
        chart.Legend.Visible = false;
        Axis valueAxisX = chart.PrimaryAxes[1];
        Axis valueAxisY = chart.PrimaryAxes[0];
        valueAxisX.Scaling.AutoMax = false;
        valueAxisX.Scaling.Max = 10;
        valueAxisX.Scaling.AutoMin = false;
        valueAxisX.Scaling.Min = -10;
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
}