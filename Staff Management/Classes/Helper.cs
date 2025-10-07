using ClosedXML.Excel;
using Staff_Management.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Staff_Management.Classes
{
    public class Helper
    {
        public static byte[] StaffExportAsExcel(List<StaffModel> staffs)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Staffs");

            ws.Cell(1, 1).Value = "Staff ID";
            ws.Cell(1, 2).Value = "Full Name";
            ws.Cell(1, 3).Value = "Birthday";
            ws.Cell(1, 4).Value = "Gender";

            int row = 2;
            foreach (var s in staffs)
            {
                ws.Cell(row, 1).Value = s.StaffId;
                ws.Cell(row, 2).Value = s.FullName;
                ws.Cell(row, 3).Value = s.Birthday.ToString("yyyy-MM-dd");
                ws.Cell(row, 4).Value = s.Gender == 1 ? "Male" : "Female";
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        public static byte[] StaffExportAsPdf(List<StaffModel> staffs)
        {
            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Header
                    page.Header()
                        .AlignCenter()
                        .Text("Staff Report")
                        .FontSize(18)
                        .Bold();

                    // Content (table)
                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(80); 
                                columns.RelativeColumn(); 
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(80);
                            });

                            // Header row
                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(1).Text("Staff ID").Bold();
                                header.Cell().BorderBottom(1).Text("Full Name").Bold();
                                header.Cell().BorderBottom(1).Text("Birthday").Bold();
                                header.Cell().BorderBottom(1).Text("Gender").Bold();
                            });

                            // Data rows
                            foreach (var s in staffs)
                            {
                                table.Cell().Text(s.StaffId);
                                table.Cell().Text(s.FullName);
                                table.Cell().Text(s.Birthday.ToString("yyyy-MM-dd"));
                                table.Cell().Text(s.Gender == 1 ? "Male" : "Female");
                            }
                        });

                    // Footer
                    page.Footer()
                        .AlignRight()
                        .Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                        .FontSize(10)
                        .Italic();
                });
            });

            return doc.GeneratePdf();
        }
    }
}
