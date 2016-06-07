using System;
using System.IO;
using System.Linq;
using System.Xml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlAgilityPack;
using NotesFor.HtmlToOpenXml;
using System.Collections.Generic;

namespace TransferDesk.UtilitiesHtml.ConvertHtmlToOpenXml
{
   public class HtmlToOpenXml
    {
        public string ConvertHtmlToOpenXml(string html, List<string> logListOfString)
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

                    logListOfString.Add("memory stream created");

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

                    //return returnableOpenXML;

                    Body body = mainPart.Document.Body;

                    var paragraphs = converter.Parse(html);

                    logListOfString.Add("Tag converter success");

                    string returnableOpenXML = string.Empty;

                    foreach (OpenXmlCompositeElement t in paragraphs)
                    {
                        //body.Append(t);
                        returnableOpenXML += t.InnerXml;
                    }
                  

                    //https://en.wikipedia.org/wiki/Control_character
                    //http://www.codetable.net/hex/3
                    //http://paulbourke.net/dataformats/ascii/
                    if (returnableOpenXML.Contains("&#"))
                    {
                        returnableOpenXML = returnableOpenXML.Replace("&#x0;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x2;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x3;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x4;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x5;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x6;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x7;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x8;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x9;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xa;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xb;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xc;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xd;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xe;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xf;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x10;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x11;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x12;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x13;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x14;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x15;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x16;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x17;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x18;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x19;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1a;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1b;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1c;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1d;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1f;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x7f;", "");
                        //&#xC; form feed escape sequence character
                        //returnableOpenXML = returnableOpenXML.Replace("&#xC;", "");
                        //http://www.codetable.net/hex/b
                        //repeating above small case charcters as above C
                        returnableOpenXML = returnableOpenXML.Replace("&#xA;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xB;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xC;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xD;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xE;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#xF;", "");

                        returnableOpenXML = returnableOpenXML.Replace("&#x1A;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1B;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1C;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1D;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1E;", "");
                        returnableOpenXML = returnableOpenXML.Replace("&#x1F;", "");

                        returnableOpenXML = returnableOpenXML.Replace("&#x7F;", "");

                    }

                    returnableOpenXML = returnableOpenXML.Replace("\f", "");
                    returnableOpenXML = returnableOpenXML.Replace("\n", "");
                    returnableOpenXML = returnableOpenXML.Replace("\r", "");
                    returnableOpenXML = returnableOpenXML.Replace("\a", "");
                    returnableOpenXML = returnableOpenXML.Replace("\b", "");
                    returnableOpenXML = returnableOpenXML.Replace("\t", "");
                    returnableOpenXML = returnableOpenXML.Replace("\v", "");

                    return returnableOpenXML;

                }


                //generatedDocument.WriteTo(new FileStream("d:\\test.docx",FileMode.Create) );
                
            }
            //return "";

        }

        ///// <summary>
        ///// Remove control charcters from a string.
        ///// </summary>
        //public string Sanitize(string xml)
        //{
        //    return xml;
        //}

    }
}
