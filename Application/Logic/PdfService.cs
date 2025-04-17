using System;
using System.IO;
using System.Linq;
using Application.Dto.OrderDTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Logic
{
    public class PdfService
    {
        public byte[] GenerateOrderReceiptPdf(OrderReceipt orderSummary)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);

                    page.Content().Column(column =>
                    {
                        // Header with padding
                        column.Item().PaddingBottom(10).Text("Order Receipt").FontSize(20).Bold().AlignCenter();
                        column.Item().PaddingBottom(5).Text("Café de Luxe").FontSize(16).SemiBold().AlignCenter();
                        column.Item().PaddingBottom(15).Text("INVOICE").FontSize(14).Italic().AlignCenter();

                        // Order Info with consistent padding
                        column.Item().PaddingBottom(5).Row(row =>
                        {
                            row.RelativeColumn().Text("Order ID:").Bold();
                            row.RelativeColumn().Text(orderSummary.Id);
                        });

                        column.Item().PaddingBottom(5).Row(row =>
                        {
                            row.RelativeColumn().Text("Order Date:").Bold();
                            row.RelativeColumn().Text(orderSummary.CreatedOn?.ToString("dd/MM/yyyy") ?? "");
                        });

                        column.Item().PaddingBottom(5).Row(row =>
                        {
                            row.RelativeColumn().Text("Order Type:").Bold();
                            row.RelativeColumn().Text(orderSummary.OrderType);
                        });

                        column.Item().PaddingBottom(15).Row(row =>
                        {
                            row.RelativeColumn().Text("Paid By:").Bold();
                            row.RelativeColumn().Text(orderSummary.PaymentType);
                        });

                        // Dashed Line
                        AddSolidLine(column);

                        // Products header with column titles
                        column.Item().PaddingBottom(5).Row(row =>
                        {
                            row.RelativeColumn().Text("Products").FontSize(14).Bold();
                            row.ConstantColumn(80).Text("Qty").FontSize(14).Bold().AlignCenter();
                            row.ConstantColumn(120).Text("Subtotal").FontSize(14).Bold().AlignRight();
                        });

                        // Product List with aligned columns
                        foreach (var item in orderSummary.CartItems)
                        {
                            column.Item().PaddingBottom(5).Row(row =>
                            {
                                row.RelativeColumn().Text(item.ProductVariantName).FontSize(12);
                                row.ConstantColumn(80).Text($"{item.Quantity}").FontSize(12).AlignCenter();
                                row.ConstantColumn(120).Text($"{item.SubTotal}").FontSize(12).AlignRight();
                            });
                        }

                        // Dashed line under items
                        AddSolidLine(column);

                        // Summary with right-aligned values
                        column.Item().PaddingBottom(5).Row(row =>
                        {
                            row.RelativeColumn().Text("Total Quantity:").FontSize(12).Bold();
                            row.RelativeColumn().Text(orderSummary.CartItems.Sum(x => x.Quantity).ToString()).FontSize(12).AlignRight();
                        });

                        column.Item().PaddingBottom(15).Row(row =>
                        {
                            row.RelativeColumn().Text("Total Amount:").FontSize(16).Bold();
                            row.RelativeColumn().Text(orderSummary.TotalAmount.ToString()).FontSize(16).Bold().AlignRight();
                        });

                        // Dashed line after total
                        AddSolidLine(column);

                        // Footer with padding
                        column.Item().PaddingTop(10).AlignCenter().Text("THANK YOU, PLEASE COME AGAIN").FontSize(12).Bold();
                        column.Item().AlignCenter().Text("Have a nice day!").FontSize(10).Italic();
                    });
                });
            });

            using var memoryStream = new MemoryStream();
            document.GeneratePdf(memoryStream);
            return memoryStream.ToArray();
        }

        private void AddSolidLine(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
        }


    }
}