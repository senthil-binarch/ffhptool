using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace FFHPWeb
{
    public partial class Footer : PdfPageEventHelper
    {
        public string _FontPath="";
        public float _Xpos = 0;
        public Footer()
        {
        }
        public Footer(string _Path,float _pos)
        {
            _FontPath = _Path;
            _Xpos=_pos;
        }
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            string s = _FontPath;//Server.MapPath("Images/Calibri.ttf");
            BaseFont bf = BaseFont.CreateFont(s, BaseFont.IDENTITY_H, true);
            
            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);

            Paragraph footer = new Paragraph("www.FFHP.in "+DateTime.Now.ToString("d MMM yyyy"), font);

            footer.Alignment = Element.ALIGN_MIDDLE;

            PdfPTable footerTbl = new PdfPTable(1);

            footerTbl.TotalWidth = 400;

            footerTbl.HorizontalAlignment = Element.ALIGN_MIDDLE;

            PdfPCell cell = new PdfPCell(footer);

            cell.Border = 0;

            cell.PaddingLeft = 10;

            footerTbl.AddCell(cell);

            footerTbl.WriteSelectedRows(0, -1, _Xpos, 30, writer.DirectContent);

        }

    }
}
