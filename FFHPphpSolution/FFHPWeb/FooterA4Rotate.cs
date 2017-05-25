using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace FFHPWeb
{
    public partial class FooterA4Rotate : PdfPageEventHelper
    {

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            
            //string s = Server.MapPath("Images/Calibri.ttf");
            //BaseFont bf = BaseFont.CreateFont(s, BaseFont.IDENTITY_H, true);
            Paragraph footer = new Paragraph("www.FFHP.in "+DateTime.Now.ToString("d MMM yyyy"), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));

            footer.Alignment = Element.ALIGN_MIDDLE;

            PdfPTable footerTbl = new PdfPTable(1);

            footerTbl.TotalWidth = 300;

            footerTbl.HorizontalAlignment = Element.ALIGN_MIDDLE;

            PdfPCell cell = new PdfPCell(footer);

            cell.Border = 0;

            cell.PaddingLeft = 10;

            footerTbl.AddCell(cell);

            footerTbl.WriteSelectedRows(0, -1, 80, 30, writer.DirectContent);

        }

    }
}
