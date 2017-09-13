using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace SimplePDF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void GeneratePDF(string fileName,string title,string paragraph)
        {
            Document document = new Document(PageSize.A4, 10, 10, 10, 10);
            var filestream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, filestream);

            document.Open();

            iTextSharp.text.Font fn = FontFactory.GetFont("Verdana", 11, Font.BOLD);
            iTextSharp.text.Font fn2 = FontFactory.GetFont("Verdana", 8);

            Chunk titleChunk = new Chunk(title, fn);
            Chunk paragraphChunk = new Chunk(paragraph, fn2);


            var cellTitle = new PdfPCell(new Phrase(titleChunk))
            {
                HorizontalAlignment = 1,
                VerticalAlignment = 1,
                Border = 0
            };

            var cellParagraph = new PdfPCell(new Phrase(paragraphChunk))
            {
                HorizontalAlignment = 0,
                VerticalAlignment = 1,
                Border = 0
            };

            var pdfTable = new PdfPTable(1);

            PdfPCell[] Tablecells = new PdfPCell[] { new PdfPCell(cellTitle) };
            PdfPRow Tablerow = new PdfPRow(Tablecells);
            pdfTable.Rows.Add(Tablerow);


            Tablecells = new PdfPCell[] { new PdfPCell(cellParagraph) };
            Tablerow = new PdfPRow(Tablecells);
            pdfTable.Rows.Add(Tablerow);

            document.Add(pdfTable);
            document.Close();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(filestream.ToArray());
            Response.End();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}