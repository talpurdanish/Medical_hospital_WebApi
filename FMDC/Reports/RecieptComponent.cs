using Domain.Viewmodels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace FMDC.Reports
{
    public class RecieptComponent : IComponent
    {
        private readonly RecieptViewModel _reciept;
        private readonly float _lineWidth = 0.2f;
        //private readonly string _path;
        public RecieptComponent(RecieptViewModel reciept, float lineWidth)
        {
            //, string path
            _reciept = reciept;
            _lineWidth = lineWidth;
            //_path = path;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Component(new ReportHeaderComponent(_reciept.RecieptNumber, "Reciept", new DateTime().ToString("dd/MM/yyyy",System.Globalization.CultureInfo.InvariantCulture)));
                column.Item().LineHorizontal(_lineWidth);
                column.Item().Element(ComposeInfoTable);
                column.Item().LineHorizontal(_lineWidth);
                column.Item().Element(ComposeDetailsTable);
            });
        }

        private void ComposeInfoTable(IContainer container)
        {

            var patientName = string.Format(System.Globalization.CultureInfo.InvariantCulture,"{0} (Mr No: {1})", _reciept.PatientName, _reciept.PatientNumber);
            var date = DateTime.Now.ToString("dd MMM, yyyy",System.Globalization.CultureInfo.InvariantCulture);
            container.Table(table =>
           {
               table.ColumnsDefinition(columns =>
               {
                   columns.RelativeColumn(1.5f);
                   columns.RelativeColumn(3.5f);
                   columns.RelativeColumn(1.5f);
                   columns.RelativeColumn(3.5f);

               });

               table.Cell().Element(CellStyle).Text("Patient:").SemiBold();
               table.Cell().ColumnSpan(3).Element(CellStyle).Text($"{patientName}").Underline();

               table.Cell().Element(CellStyle).Text("Date:").SemiBold();
               table.Cell().Element(CellStyle).Text($"{_reciept.Date} {_reciept.Time}").Underline();
               table.Cell().Element(CellStyle).Text("Doctor:").SemiBold();
               table.Cell().Element(CellStyle).Text($"Dr. {_reciept.Doctor}").Underline();

               table.Cell().Element(CellStyle).Text("Grand Total:").SemiBold();
               table.Cell().Element(CellStyle).Text($"{FormatCurrency(_reciept.GrandTotal)}").Underline();
               table.Cell().Element(CellStyle).Text("Visit:").SemiBold();
               table.Cell().Element(CellStyle).Text($"{_reciept.Appointment}").Underline();


               static IContainer CellStyle(IContainer container) => container.AlignLeft().PaddingVertical(6);

           });
        }

        private void ComposeDetailsTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold();
            var authorizedBy = string.IsNullOrEmpty(_reciept.AuthorizedBy) ? "" : "Dr. " + _reciept.AuthorizedBy;
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2f);
                    columns.RelativeColumn(5f);
                    columns.RelativeColumn(3f);

                });
                table.Header(header =>
                {
                    header.Cell().Text("#");
                    header.Cell().Text("Procedure").Style(headerStyle);
                    header.Cell().AlignRight().Text("Cost").Style(headerStyle);

                    header.Cell().ColumnSpan(3).PaddingTop(5).BorderBottom(0.2f).BorderColor(Colors.Black);
                });

                foreach (var item in _reciept.Procedures)
                {
                    var index = _reciept.Procedures.IndexOf(item) + 1;

                    table.Cell().Element(CellStyle).Text($"{index}");
                    table.Cell().Element(CellStyle).Text(item.Name);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{FormatCurrency(item.Cost)}");

                    static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
                table.Footer(footer =>
                {
                    footer.Cell().ColumnSpan(3).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(30f);
                            columns.RelativeColumn(35f);
                            columns.RelativeColumn(15f);
                            columns.RelativeColumn(20f);

                        });



                        if (!_reciept.Paid)
                        {
                            table.Cell();
                            table.Cell();
                            table.Cell().Element(CellStyle).Text("Total.:").SemiBold();
                            table.Cell().Element(CellStyle).Text($"{FormatCurrency(_reciept.Total)}");
                            table.Cell();
                            table.Cell();
                            table.Cell().Element(CellStyle).Text("Discount:").SemiBold();
                            table.Cell().Element(CellStyle).AlignRight().Text($"{_reciept.Discount}%");
                        }
                        else
                        {
                            //table.Cell();
                            table.Cell().ColumnSpan(2).RowSpan(2).AlignCenter().AlignMiddle().Element(CellStyle).Border(1).BorderColor(Colors.Red.Lighten1).MinimalBox().Padding(5).AlignCenter().Text($"This reciept has been PAID in full").ExtraBold().FontColor(Colors.Red.Darken1);
                            table.Cell().Element(CellStyle).Text("Total.:").SemiBold();
                            table.Cell().Element(CellStyle).Text($"{FormatCurrency(_reciept.Total)}");
                            table.Cell().Element(CellStyle).Text("Discount:").SemiBold();
                            table.Cell().Element(CellStyle).AlignRight().Text($"{_reciept.Discount}%");
                        }

                        if (!string.IsNullOrEmpty(_reciept.AuthorizedBy))
                        {
                            table.Cell().Element(CellStyle).AlignRight().Text("Discount Authorized By").SemiBold();
                            table.Cell().Element(CellStyle).Border(1).MinimalBox().Padding(5).Text($"{authorizedBy}");
                        }
                        else
                        {
                            table.Cell().Element(CellStyle).Text("");
                            table.Cell().Element(CellStyle).Text("");
                        }
                        table.Cell().Element(CellStyle).Text("Grand Total:").SemiBold();
                        table.Cell().Element(CellStyle).Border(1).MinimalBox().Background(Colors.Grey.Lighten2).Padding(5).Text($"{FormatCurrency(_reciept.GrandTotal)}");


                        static IContainer CellStyle(IContainer container) => container.AlignLeft().AlignMiddle().Padding(5);

                    });
                });
            });
        }


        public string FormatCurrency(double value)
        {
            return "PKR " + value.ToString("F2",System.Globalization.CultureInfo.InvariantCulture);
        }


    }
}