using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using SimplePDF.Services;
using System.Threading.Tasks;

namespace SimplePDF.Controllers
{
    public class HomeController : Controller
    {
        EmailService sv;

        public HomeController()
        {
            sv = new EmailService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ProcessMethod(string fileName, string title, string paragraph, string email,string download,string send)
        {
            if (!string.IsNullOrEmpty(download))
                DownloadPDF(fileName, title, paragraph);
            else
               await SendEmail(fileName, title, paragraph, email);

            return View("Index");
        }

        private async Task SendEmail(string fileName, string title, string paragraph, string email)
        {
            EmailService sv = new EmailService();
            sv.FileName = fileName;
            sv.FromEmail = "support@simplepdf.com";
            sv.FromName = "Simple PDF";
            sv.Message = "<p>Thank you for using Simple PDF application!</p>";
            sv.Subject = "Your generated PDF file";
            sv.ToEmail = email;
            sv.FileStream = CreatePDFStream(fileName, title, paragraph);
            await sv.sendEmailAsync();
        }

        private byte[] CreatePDFStream(string fileName, string title, string paragraph)
        {
            Document document = new Document(PageSize.A4, 10, 10, 10, 10);
            var filestream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, filestream);

            document.Open();

            iTextSharp.text.Font fn = FontFactory.GetFont("Verdana", 11,Font.BOLD);
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
            return filestream.ToArray();
        }


        private void DownloadPDF(string fileName,string title,string paragraph)
        {
            var fileStream = CreatePDFStream(fileName, title, paragraph);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileStream);
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