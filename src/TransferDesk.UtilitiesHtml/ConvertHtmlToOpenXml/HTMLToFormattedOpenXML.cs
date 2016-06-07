using System.IO;
using System.Linq;
using System.Xml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlAgilityPack;
using NotesFor.HtmlToOpenXml;
using System.Collections.Generic;
using A = DocumentFormat.OpenXml.Drawing;

namespace TransferDesk.UtilitiesHtml.ConvertHtmlToOpenXml
{
    public class ParagraphImageData
    {
        public Stream ImageStream { get; set; }
        public string ImageContentType { get; set; }
        public string ImageEmbedId { get; set; }
        public bool IsAddedToDocument { get; set; } 
        public string ImageNewID { get; set; }

    }
   public class HtmlToOpenXml
    {
        public string ConvertHtmlToOpenXml(string html, List<ParagraphImageData> listOfParagraphImageData)
        {
            ////create a outer html as htmldoc from html fragment
            //var htmlDocument = new HtmlDocument();
           
            //var node = HtmlNode.CreateNode("<html><head></head><body>" + html + "</body></html>");

            //htmlDocument.DocumentNode.AppendChild(node);

            string docOuterHtml = "<html><head></head><body>" + html + "</body></html>";
                //htmlDocument.DocumentNode.OuterHtml;

            //htmlDocument = null;
          
            //using struct will help dispose off memory stream, as auto disposed
            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (
                    WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument,
                        WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new Document(new Body()).Save(mainPart);

                    }

                    // Assign a reference to the existing document body.
                    //Body body = wordprocessingDocument.MainDocumentPart.Document.Body;
                    // Add new text.
                    //Paragraph para = body.AppendChild(new Paragraph());
                    //Run run = para.AppendChild(new Run());
                    //run.AppendChild(new Text(txt));

                    //< w:document xmlns:w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main" >

                    //< w:body >

                    //< w:p >

                    //< w:r >

                    //< w:t > Example text.</ w:t >

                    //</ w:r >

                    //</ w:p >

                    //</ w:body >
                    //</ w:document >

                    if (mainPart.Document.Elements<Paragraph>().Count() != 0)
                    {
                    }

                    HtmlConverter converter = new HtmlConverter(mainPart);
                    //string html = "<HTML><Body>Test</Body></HTML>";
                    //var paralist = converter.Parse(docOuterHtml);

                    //return entitizedTextFromXml;

                    Body body = mainPart.Document.Body;

                    var paragraphs = converter.Parse(html);
                    
                    string entitizedTextFromXml = string.Empty;

                    foreach (OpenXmlCompositeElement t in paragraphs)
                    {
                        //body.Append(t);
                        entitizedTextFromXml += t.InnerXml;

                        //// get all images in paragraph
                        //List<A.Blip> imagesInParagraph = t.Descendants<A.Blip>().ToList();
                        ////R7f520e88ead74f8d for check
                        //if (imagesInParagraph.Any())
                        //{
                        //    foreach (var imageInParagraph in imagesInParagraph)
                        //    {
                        //        string embed = imageInParagraph.Embed;
                        //    }
                        //}
                    }
                   
                    //clean control characters(invisible characters)
                    entitizedTextFromXml = CleanupControlCharactersAsEntities(entitizedTextFromXml);

                    entitizedTextFromXml = CleanupEscapedLineFeedCharacters(entitizedTextFromXml);

                    mainPart.Document.Save();
                    
                    var imageParts = mainPart.ImageParts;
                    foreach (var imagePart in imageParts)
                    {
                        var imageStream = imagePart.GetStream();
                        //imageStream.Position = 0;

                        ParagraphImageData paragraphImageData = new ParagraphImageData();
                        paragraphImageData.IsAddedToDocument = false;
                        paragraphImageData.ImageStream = new  MemoryStream();
                        imageStream.CopyTo(paragraphImageData.ImageStream);
                        paragraphImageData.ImageContentType = imagePart.ContentType;
                        paragraphImageData.ImageEmbedId = mainPart.GetIdOfPart(imagePart);
                      
                        listOfParagraphImageData.Add(paragraphImageData);
                    }
                    //generatedDocument.WriteTo(new FileStream("d:\\test05.docx", FileMode.Create));
                    return entitizedTextFromXml;
                }


                //generatedDocument.WriteTo(new FileStream("d:\\test05.docx",FileMode.Create) );
                
            }
            //return "";

        }

       public string CleanupControlCharactersAsEntities(string entitizedTextFromXml)
       {

            //https://en.wikipedia.org/wiki/Control_character
            //http://www.codetable.net/hex/3
            //http://paulbourke.net/dataformats/ascii/
            if (entitizedTextFromXml.Contains("&#"))
            {
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x0;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x2;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x3;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x4;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x5;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x6;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x7;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x8;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x9;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xa;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xb;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xc;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xd;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xe;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xf;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x10;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x11;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x12;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x13;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x14;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x15;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x16;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x17;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x18;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x19;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1a;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1b;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1c;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1d;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1f;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x7f;", "");
                //&#xC; form feed escape sequence character
                //entitizedTextFromXml = entitizedTextFromXml.Replace("&#xC;", "");
                //http://www.codetable.net/hex/b
                //repeating above small case charcters as above C
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xA;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xB;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xC;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xD;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xE;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#xF;", "");

                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1A;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1B;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1C;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1D;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1E;", "");
                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x1F;", "");

                entitizedTextFromXml = entitizedTextFromXml.Replace("&#x7F;", "");

            }
           return entitizedTextFromXml;

       }

       public string CleanupEscapedLineFeedCharacters(string textToClean)
       {
            textToClean = textToClean.Replace("\f", "");
            textToClean = textToClean.Replace("\n", "");
            textToClean = textToClean.Replace("\r", "");
            textToClean = textToClean.Replace("\a", "");
            textToClean = textToClean.Replace("\b", "");
            textToClean = textToClean.Replace("\t", "");
            textToClean = textToClean.Replace("\v", "");

            return textToClean;
       }


    }
}
