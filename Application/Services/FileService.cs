using System.Diagnostics;
using System.Drawing;
using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Projects;
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


    public FileService(IRepositoryWrapper repositoryWrapper, IEnergyFlowService energyFlowService)
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

    public async Task<BaseResponse<bool>> ReadExcel(string filePath,TranslatorSpecs translatorSpecs,DirectionType type)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                List<RadiationZone> list = new List<RadiationZone>();
                for (int row = 1; row <= worksheet.Dimension.Rows-1; row++)
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
            x.Id.ToString() ==oid);
        var contrAgent = project.ContrAgent;
        var executor = project.Executor;
        var executiveCompany = project.ExecutiveCompany;
        var summaryBiohazardRadius = project.SummaryBiohazardRadius;
        var es = project.TotalFluxDensity;
        using (var wordProcessor = new RichEditDocumentServer()) 
        { 
            wordProcessor.LoadDocument("Шаблон.docx");

            Document document = wordProcessor.Document;
            // var contrAgent = await _repositoryWrapper.ContrAgentRepository.GetByCondition(x => x.CompanyName == "Tele2");
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

            var projectAntennae = project.ProjectAntennae;
            foreach (var value in projectAntennae)
            {
                var ant = _repositoryWrapper
                    .AntennaTranslatorRepository.GetAllByCondition(x => x.ProjectAntennaId == value.Id).ToList();
                foreach (var item in ant)
                {
                    var bio = _repositoryWrapper.BiohazardRadiusRepository.GetAllByCondition(x =>
                        x.AntennaTranslatorId == item.Id).ToList();
                    var y = 1;
                    for (int i = 0; i < bio.Count; i++) 
                    {
                        var x = i % 10;
                        // var rB = _energyFlowService.GetRB(translator.Power, translator.Gain, 0.71M, radiations[i].Value);
                        // var rZ = _energyFlowService.GetRZ(radiations[i].Degree, rB);
                        // var rX = _energyFlowService.GetRX(radiations[i].Degree, rB);
                        if (i == 0 || i == 4 || x == 0) 
                        {
                            document.InsertText(table[y, 0].Range.Start, bio[i].Degree.ToString());
                            document.InsertText(table[y, 1].Range.Start, bio[i].Db.ToString("F3"));
                            document.InsertText(table[y, 2].Range.Start, bio[i].DbRaz.ToString("F3"));
                            document.InsertText(table[y, 3].Range.Start, bio[i].MaximumBiohazardRadius.ToString("F3"));
                            document.InsertText(table[y, 4].Range.Start, bio[i].BiohazardRadiusZ.ToString("F3"));
                            document.InsertText(table[y, 5].Range.Start, bio[i].BiohazardRadiusX.ToString("F3"));
                            y++; 
                        }
                    }
                    document.InsertText(table.Range.End,"Максимальный радиус биологически-опасной зоны от секторных " +
                                                        "антенн TBXLHA-6565B-VTM в направлении излучения равен 27,5432381892065 м" +
                                                        " (стандарт GSM/UMTS; мощность передатчика 25 Вт; частота на передачу 900 МГц;" +
                                                        " коэффициент усиления антенн 16,5 дБ, направление антенны в вертикальной плоскости 0°). " +
                                                        "В вертикальном сечении БОЗ повторяет диаграмму направленности. Максимальное отклонение от оси в вертикальном сечении составляет 0,966 м." +
                                                        " на расстоянии 18,426 м. от центра излучения. Максимальный радиус биологически-опасного излучения от заднего лепестка антенны составил 0,060 м. " +
                                                        "В горизонтальном сечении БОЗ повторяет диаграмму направленности. Максимальное отклонение от оси в горизонтальном сечении составляет 8,662 м." +
                                                        " на расстоянии 16,291 м. от центра излучения. Максимальный радиус биологически-опасного излучения от заднего лепестка антенны составил 0,015 м.");
                    table.EndUpdate();
                }
                
                // var qwe = value.
                // var asd = value.;
                // asd.TranslatorSpecsList
            }
            // var radiations = _repositoryWrapper.RadiationZoneRepository
            //     .GetAllByCondition(x => x.TranslatorSpecsId.ToString() == "f8edf3ec-733a-4d82-bb13-d88548616368" && x.DirectionType == DirectionType.Vertical)!
            //     .OrderBy(x=> x.Degree).ToList();
            // var translator = await _repositoryWrapper.TranslatorSpecsRepository
            //     .GetByCondition(x => x.Id.ToString() == "f8edf3ec-733a-4d82-bb13-d88548616368");
            // var antenna = await _repositoryWrapper.AntennaRepository
            //     .GetByCondition(x => 
            //         x.Id.ToString() == "eeccb820-4828-488c-9eb8-21a479c73c30");
            // var y = 1;
            // for (int i = 0; i < radiations.Count; i++) 
            // {
            //     var x = i % 10;
            //     var rB = _energyFlowService.GetRB(translator.Power, translator.Gain, 0.71M, radiations[i].Value);
            //     var rZ = _energyFlowService.GetRZ(radiations[i].Degree, rB);
            //     var rX = _energyFlowService.GetRX(radiations[i].Degree, rB);
            //     if (i == 0 || i == 4 || x == 0) 
            //     {
            //         document.InsertText(table[y, 0].Range.Start, radiations[i].Degree.ToString());
            //         document.InsertText(table[y, 1].Range.Start, radiations[i].Value.ToString());
            //         document.InsertText(table[y, 2].Range.Start, _energyFlowService.Multiplier(radiations[i].Value).ToString("F3"));
            //         document.InsertText(table[y, 3].Range.Start, rB.ToString("F3"));
            //         document.InsertText(table[y, 4].Range.Start, rZ.ToString("F3"));
            //         document.InsertText(table[y, 5].Range.Start, rX.ToString("F3"));
            //         y++; 
            //     }
            // }
            // document.InsertText(table.Range.End,"Максимальный радиус биологически-опасной зоны от секторных " +
            //                     "антенн TBXLHA-6565B-VTM в направлении излучения равен 27,5432381892065 м" +
            //                     " (стандарт GSM/UMTS; мощность передатчика 25 Вт; частота на передачу 900 МГц;" +
            //                     " коэффициент усиления антенн 16,5 дБ, направление антенны в вертикальной плоскости 0°). " +
            //                     "В вертикальном сечении БОЗ повторяет диаграмму направленности. Максимальное отклонение от оси в вертикальном сечении составляет 0,966 м." +
            //                     " на расстоянии 18,426 м. от центра излучения. Максимальный радиус биологически-опасного излучения от заднего лепестка антенны составил 0,060 м. " +
            //                     "В горизонтальном сечении БОЗ повторяет диаграмму направленности. Максимальное отклонение от оси в горизонтальном сечении составляет 8,662 м." +
            //                     " на расстоянии 16,291 м. от центра излучения. Максимальный радиус биологически-опасного излучения от заднего лепестка антенны составил 0,015 м.");
            // table.EndUpdate();
            var diagram = document.FindAll("Diagram",SearchOptions.WholeWord);
            ParagraphProperties titleParagraphPropertiesSecond = document.BeginUpdateParagraphs(diagram[0]);
            titleParagraphPropertiesSecond.Alignment = ParagraphAlignment.Center;
            document.EndUpdateParagraphs(titleParagraphPropertiesSecond);
            document.Delete(diagram[0]);
            DocumentPosition insertPositionSecond = diagram[0].Start;
            // DocumentPosition insertPositionThird = diagram[0].End;
            // if (radiations.Find(x=>x.DirectionType == DirectionType.Horizontal) is null)
            //     document.InsertText(insertPositionThird, $"Ширина БОЗ в вертикальной плоскости на расстоянии Rx от " +
            //                                               $"антенны вдоль линии горизонта по направлению излучения");
            // else
            //     document.InsertText(insertPositionThird, $"Ширина БОЗ в горизонтальной плоскости на расстоянии Rx от " +
            //                                               $"антенны вдоль линии горизонта по направлению излучения");
            
            await CreateGrafic(document,insertPositionSecond,project);
            document.Delete(diagram[0]);
            table.EndUpdate();
            // if (radiations.Find(x=>x.DirectionType == DirectionType.Horizontal) is null)
            //     document.InsertText(table.Range.End, $"Ширина БОЗ в вертикальной плоскости на расстоянии Rx от " +
            //                                          $"антенны вдоль линии горизонта по направлению излучения");
            // else
            //     document.InsertText(table.Range.End, $"Ширина БОЗ в горизонтальной плоскости на расстоянии Rx от " +
            //                                          $"антенны вдоль линии горизонта по направлению излучения");
            
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


    public async Task<BaseResponse<bool>> CreateGrafic(Document document,DocumentPosition position,Project project)
    {
        // using (var wordProcessor = new RichEditDocumentServer())
        // {
            // Document document = wordProcessor.Document;
            document.Unit = DevExpress.Office.DocumentUnit.Inch;
            var chartShape = document.Shapes.InsertChart(position,ChartType.ScatterSmooth);
            chartShape.Name = "Scatter Line chart";
            chartShape.Size = new SizeF(4f, 3.2f);
            chartShape.RelativeHorizontalPosition = ShapeRelativeHorizontalPosition.Margin;
            chartShape.RelativeVerticalPosition = ShapeRelativeVerticalPosition.Paragraph;
            chartShape.Offset = new PointF(0, 0);
            ChartObject chart = (ChartObject)chartShape.ChartFormat.Chart;
            Worksheet worksheet = (Worksheet)chartShape.ChartFormat.Worksheet;
            await SpecifyChartData(worksheet);
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
            
            // wordProcessor.SaveDocument("Ar222.docx", DocumentFormat.OpenXml);
        // }
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Файл успешно создан" },
            Success: true);
    }
    
    private async Task<bool> SpecifyChartData(Worksheet sheet)
    {
        var radiations = _repositoryWrapper.RadiationZoneRepository
            .GetAllByCondition(x => x.TranslatorSpecsId.ToString() == "f8edf3ec-733a-4d82-bb13-d88548616368" && x.DirectionType == DirectionType.Vertical)!
            .OrderBy(x=> x.Degree).ToList();
        var translator = await _repositoryWrapper.TranslatorSpecsRepository
            .GetByCondition(x => x.Id.ToString() == "f8edf3ec-733a-4d82-bb13-d88548616368");
        // var radiations = _repositoryWrapper.RadiationZoneRepository
        //     .GetAllByCondition(x => x.TranslatorSpecsId == antennaTranslator.TranslatorSpecsId && x.DirectionType == DirectionType.Vertical)!
        //     .OrderBy(x=> x.Degree).ToList();  
        // var translator = await _repositoryWrapper.TranslatorSpecsRepository
        //     .GetByCondition(x => x.Id == antennaTranslator.TranslatorSpecsId);  Как будет
        
        for (int i = 0; i < radiations.Count; i++)
        {
            // var rB = _energyFlowService.GetRB(antennaTranslator.Power, antennaTranslator.Gain, antennaTranslator.TransmitLossFactor, radiations[i].Value);  Как будет
            var rB = _energyFlowService.GetRB(translator.Power, translator.Gain, 0.71M, radiations[i].Value);
            var rZ = _energyFlowService.GetRZ(radiations[i].Degree, rB);
            var rX = _energyFlowService.GetRX(radiations[i].Degree, rB);
    
            sheet[i, 0].Value = rX;
            sheet[i, 1].Value = rZ;
        }

        return true;
    }
}