using System.IO;
using System.Xml;
using System.Xml.XPath;
using com.phoenixconsulting.common.mail;
using euthenias_delight.Properties;
using euthenias_delight.XML;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using MigraDocBorderStyle = MigraDoc.DocumentObjectModel.BorderStyle;
using MigraDocColor = MigraDoc.DocumentObjectModel.Color;
using MigraDocImage = MigraDoc.DocumentObjectModel.Shapes.Image;

namespace euthenias_delight {
    public class InvoiceEngine {
        private Document _document;
        private Table _table;
        private TextFrame _addressFrame;
        
        public InvoiceEngine(XmlDocument xmlDoc)
        {
            XmlDoc.Document = xmlDoc;
            XmlDoc.Navigator = XmlDoc.Document.CreateNavigator();
            XmlDoc.RootNavItem = XmlDoc.SelectItem(XmlDoc.ROOT);
            XmlDoc.ToNavItem = XmlDoc.SelectItem(XmlDoc.INVOICE_TO);
        }

        public bool ThereAreItemsToInvoice() {
            return XmlDoc.InvoiceItems.Count < 0;
        }

        public bool EmailInvoice() {
            EmailInvoice(CreatePDFInvoice());
            return true;
        }

        private void EmailInvoice(Document invoice) {
            MemoryStream stream = ConvertToStream(invoice);
            MailMessageBuilder.SendEmailToClient(XmlDoc.CustomerEmail, GetBody(), stream);
        }

        private MemoryStream ConvertToStream(Document invoice) {
            MemoryStream stream = new MemoryStream();
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always) {Document = invoice};
            renderer.RenderDocument();
            renderer.PdfDocument.Save(stream, false);
            return stream;
        }

        private string GetBody() {
            return XmlDoc.EmailBodyGreeting + XmlDoc.CustomerNickname + ", " + 
                Constants.TWO_LINES +
                XmlDoc.EmailBodyMessage + 
                Constants.TWO_LINES +
                XmlDoc.EmailBodySalutation;
        }

        private Document CreatePDFInvoice() {
            //Create a new MigraDoc document
            _document = new Document {Info = {Title = "Phoenix Consulting Services Invoice", Subject = "Sterling Equity", Author = "Andrew Gray"}};
            DefineStyles();
            CreatePage();
            FillContent();
            return _document;
        }

        private void DefineStyles() {
            //Get the predefined style Normal.
            Style style = _document.Styles["Normal"];
            //Because all styles are derived from Normal, the next line changes the 
            //font of the whole document. Or, more exactly, it changes the font of
            //all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = _document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = _document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            //Create a new style called Table based on style Normal
            style = _document.Styles.AddStyle("Table", "Normal");
            //style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            //Create a new style called Reference based on style Normal
            style = _document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        private void CreatePage() {
            //Each MigraDoc document needs at least one section.
            Section section = _document.AddSection();

            //Put a logo in the header
            MigraDocImage image = section.Headers.Primary.AddImage(Constants.PC_IMAGE);
            image.Height = "2.5cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Left;
            image.WrapFormat.Style = WrapStyle.Through;

            //Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Phoenix Consulting · Unit 1/6 Robert Street · Elwood 3184");
            paragraph.Format.Font.Name = "Times New Roman";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            //Create the text frame for the address
            _addressFrame = section.AddTextFrame();
            _addressFrame.Height = "3.0cm";
            _addressFrame.Width = "7.0cm";
            _addressFrame.Left = ShapePosition.Left;
            _addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            _addressFrame.Top = "5.0cm";
            _addressFrame.RelativeVertical = RelativeVertical.Page;

            //Add the print date field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "8cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("TAX INVOICE", TextFormat.Bold);
            AddTabs(paragraph, 1);
            paragraph.AddDateField("dd/MM/yyyy");

            //Create the item table
            _table = section.AddTable();
            _table.Style = "Table";
            _table.Borders.Color = Constants.TableBorder;
            _table.Borders.Width = 0.25;
            _table.Borders.Left.Width = 0.5;
            _table.Borders.Right.Width = 0.5;
            _table.Rows.LeftIndent = 0;

            //Before you can add a row, you must define the columns
            AddColumn("1cm", ParagraphAlignment.Center);
            AddColumn("2.5cm", ParagraphAlignment.Right);
            AddColumn("3cm", ParagraphAlignment.Right);
            AddColumn("3.5cm", ParagraphAlignment.Right);
            AddColumn("2cm", ParagraphAlignment.Center);
            AddColumn("4cm", ParagraphAlignment.Right);

            //Create the header of the table
            Row row = AddRow(true, ParagraphAlignment.Center, true, Constants.TableOrange);
            row.Cells[0].AddParagraph("Item");
            row.Cells[0].Format.Font.Bold = false;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            row.Cells[0].MergeDown = 1;
            row.Cells[1].AddParagraph("Details");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].MergeRight = 3;
            row.Cells[5].AddParagraph("Price");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
            row.Cells[5].MergeDown = 1;

            row = AddRow(true, ParagraphAlignment.Center, true, Constants.TableOrange);
            row.Cells[1].AddParagraph("Quantity");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].AddParagraph("Unit Price");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].AddParagraph("Discount (%)");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[4].AddParagraph("Taxable");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Left;

            _table.SetEdge(0, 0, 6, 2, Edge.Box, MigraDocBorderStyle.Single, 0.75, MigraDocColor.Empty);
        }

        private void AddColumn(string colWidth, ParagraphAlignment alignment) {
            Column column = _table.AddColumn(colWidth);
            column.Format.Alignment = alignment;
        }

        private Row AddRow(bool headingFormat, ParagraphAlignment alignment, bool fontBold, MigraDocColor color) {
            Row row = _table.AddRow();
            row.HeadingFormat = headingFormat;
            row.Format.Alignment = alignment;
            row.Format.Font.Bold = fontBold;
            row.Shading.Color = color;
            return row;
        }

        private void FillContent() {
            //Fill address in address text frame
            Paragraph paragraph = _addressFrame.AddParagraph();
            paragraph.AddText(XmlDoc.CustomerName);
            paragraph.AddLineBreak();
            paragraph.AddText(XmlDoc.CustomerAddressLine1);
            paragraph.AddLineBreak();
            paragraph.AddText(XmlDoc.CustomerAddressCity + " " + XmlDoc.CustomerAddressPostcode);

            paragraph = _addressFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddText("ATTENTION: " + XmlDoc.CustomerAttention);

            //Iterate the invoice items
            double totalExtendedPrice = 0;
            XPathNodeIterator iter = XmlDoc.InvoiceItems;

            while(iter.MoveNext()) {
                XPathNavigator item = iter.Current;

                double quantity = XmlDoc.ItemQuantity(item);
                double price = XmlDoc.ItemPrice(item);
                double discount = XmlDoc.ItemDiscount(item);

                //Each item fills two rows
                Row row1 = _table.AddRow();
                Row row2 = _table.AddRow();
                row1.TopPadding = 1.5;
                row1.Cells[0].Shading.Color = Constants.TableGray;
                row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                row1.Cells[0].MergeDown = 1;
                row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                row1.Cells[1].MergeRight = 3;
                row1.Cells[5].Shading.Color = Constants.TableGray;
                row1.Cells[5].MergeDown = 1;

                row1.Cells[0].AddParagraph(XmlDoc.ItemNumber(item));
                paragraph = row1.Cells[1].AddParagraph();
                paragraph.AddFormattedText(XmlDoc.ItemDescription(item), TextFormat.Bold);
                row2.Cells[1].AddParagraph(quantity.ToString());
                row2.Cells[2].AddParagraph(Constants.DOLLAR_SIGN + price.ToString("0.00"));
                row2.Cells[3].AddParagraph(discount.ToString("0.0"));
                row2.Cells[4].AddParagraph();
                row2.Cells[5].AddParagraph(price.ToString("0.00"));
                double extendedPrice = quantity * price;
                extendedPrice = extendedPrice * (100 - discount) / 100;
                //row1.Cells[5].AddParagraph(extendedPrice.ToString("0.00") + " €");
                row1.Cells[5].AddParagraph(Constants.DOLLAR_SIGN + extendedPrice.ToString("0.00"));
                row1.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
                totalExtendedPrice += extendedPrice;

                _table.SetEdge(0, _table.Rows.Count - 2, 6, 2, Edge.Box, MigraDocBorderStyle.Single, 0.75);
            }

            //Add an invisible row as a space line to the table
            Row row = _table.AddRow();
            row.Borders.Visible = false;

            //Add the total price row
            //row = _table.AddRow();
            //row.Cells[0].Borders.Visible = false;
            //row.Cells[0].AddParagraph("Total Price (Ex GST)");
            //row.Cells[0].Format.Font.Bold = true;
            //row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //row.Cells[0].MergeRight = 4;
            ////row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");
            //row.Cells[5].AddParagraph(DOLLAR_SIGN + totalExtendedPrice.ToString("0.00"));

            //Add the VAT row
            //row = _table.AddRow();
            //row.Cells[0].Borders.Visible = false;
            //row.Cells[0].AddParagraph("VAT (19%)");
            //row.Cells[0].Format.Font.Bold = true;
            //row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //row.Cells[0].MergeRight = 4;
            //row.Cells[5].AddParagraph((0.19 * totalExtendedPrice).ToString("0.00") + " €");
            //row.Cells[5].AddParagraph((0.19 * totalExtendedPrice).ToString("0.00") + " $");

            //Add the additional fee row
            //row = _table.AddRow();
            //row.Cells[0].Borders.Visible = false;
            //row.Cells[0].AddParagraph("Shipping and Handling");
            //row.Cells[5].AddParagraph(0.ToString("0.00") + " €");
            //row.Cells[5].AddParagraph(0.ToString("0.00") + " $");
            //row.Cells[0].Format.Font.Bold = true;
            //row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //row.Cells[0].MergeRight = 4;

            //Add the total due row
            row = _table.AddRow();
            row.Cells[0].AddParagraph("Total Due (Ex GST)");
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;

            totalExtendedPrice = GetExtendedPrice(totalExtendedPrice);

            //row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");
            row.Cells[5].AddParagraph(Constants.DOLLAR_SIGN + totalExtendedPrice.ToString("0.00"));
            row.Cells[5].Format.Font.Bold = true;

            //Set the borders of the specified cell range
            _table.SetEdge(5, _table.Rows.Count - 4, 1, 4, Edge.Box, MigraDocBorderStyle.Single, 0.75);

            //Add the terms paragraph
            paragraph = _document.LastSection.AddParagraph();

            FormatParagraph(paragraph, "1cm", 0.75, 3, Constants.TableBorder, Constants.TableGray);
            paragraph.AddText("TERMS: " + XmlDoc.InvoiceTerms);

            //Add the notes paragraph
            paragraph = _document.LastSection.AddParagraph();
            FormatParagraph(paragraph, "1cm", 0.75, 3, Constants.TableBorder, Constants.TableGray);
            iter = XmlDoc.InvoiceNotes;
            while(iter.MoveNext()) {
                string note = iter.Current.InnerXml;
                paragraph.AddText(note);
                if(iter.CurrentPosition != iter.Count) {
                    //not last element
                    paragraph.AddLineBreak();
                }
            }
        }

        private double GetExtendedPrice(double price) {
            if(WeAreCollectingGST()) {
                double gstpercent = XmlDoc.GSTPercent;
                price += gstpercent * price;
            }
            return price;
        }

        private bool WeAreCollectingGST() {
            string areCollecting = XmlDoc.GSTCollecting;
            return bool.Parse(areCollecting);
        }

        private void FormatParagraph(Paragraph p, string spaceBefore, double width, double distance, MigraDocColor borderColor, MigraDocColor shadingColor) {
            p.Format.SpaceBefore = spaceBefore;
            p.Format.Borders.Width = width;
            p.Format.Borders.Distance = distance;
            p.Format.Borders.Color = borderColor;
            p.Format.Shading.Color = shadingColor;
        }

        private void AddTabs(Paragraph p, int num) {
            for(int i = 0; i < num; i++) {
                p.AddTab();
            }
        }

        private string GetSequence(string seqName) {
            string sequence;
            if(seqName.Equals("JobNumber")) {
                sequence = Settings.Default.NextJobNumber;
                Settings.Default.NextJobNumber = (int.Parse(sequence) + 1).ToString();
            } else {
                sequence = Settings.Default.NextInvoiceNumber;
                Settings.Default.NextInvoiceNumber = (int.Parse(sequence) + 1).ToString();
            }
            Settings.Default.Save();
            return sequence;
        }      
    }
}