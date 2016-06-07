using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using OPTools = OpenXmlPowerTools;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using TransferDesk.UtilitiesHtml.ConvertHtmlToOpenXml;
using HtmlAgilityPack;
using TransferDesk.UtilitiesHtml.HtmlUtilityPack;


namespace TransferDesk.UtilitiesOpenXml.CreateDocXFromTemplate
{
    public class PlaceholderReplacer
    {
        public string Placeholder { get; set; }

        public string Replacer { get; set; }

        public bool HasToCheckReplacement { get; set; }

        public bool IsHtml { get; set; }

        public string InlinePrefixHtmlText { get; set; }
    }

    public class WordTemplateTextReplace
    {
        //this text replacer will replace a list of values by searching all over the doc and replace first instance 
        public void ReplacePlaceholders(List<PlaceholderReplacer> placeholderReplacerList, string worddocFullNameAndPath, List<string> logListOfString )
        {
           //string worddocFullNameAndPath = string.Empty;

            try
            {
                logListOfString.Add("will Replace Placeholders");

                ////make a copy if the source and save paths are different

                //if (templateDocxFullNameAndPath.ToLower() != SaveAsDocxFullNameAndPath.ToLower())
                //{
                //    logListOfString.Add("copying template to output");

                //    File.Copy(templateDocxFullNameAndPath, SaveAsDocxFullNameAndPath,true);
                //    if (File.Exists(SaveAsDocxFullNameAndPath) == false)
                //    {
                //        //raise exception if save as not created
                //        throw new Exception("Exception creating a copy of template in WordTemplateTextReplace cause unknown");
                //    }
                //    else
                //    {
                //        logListOfString.Add("copied to path " + SaveAsDocxFullNameAndPath);
                //        worddocFullNameAndPath = SaveAsDocxFullNameAndPath;
                //    }
                //}
                //else
                //{
                //    //in this case template itself will change
                //    worddocFullNameAndPath = templateDocxFullNameAndPath;
                //}

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(worddocFullNameAndPath, true))
                {
                    logListOfString.Add("word Doc opened from location : " + worddocFullNameAndPath);
                    foreach (PlaceholderReplacer placeholderReplacer in placeholderReplacerList)
                    {
                        if (placeholderReplacer.IsHtml)
                        {
                            logListOfString.Add("locating placeholder for html data of : " + placeholderReplacer.Placeholder);
                            //locate placeholder or key in doc
                            foreach (var paragraph in wordDoc.MainDocumentPart.RootElement.Descendants<Paragraph>())
                            {
                                if (paragraph.InnerText.Contains(placeholderReplacer.Placeholder) != true) continue;

                                logListOfString.Add("located for: " + placeholderReplacer.Placeholder);

                                bool hasJunk = placeholderReplacer.Replacer.Contains("/xml");
                                //hasJunk = true;
                                if (hasJunk)
                                {
                                    HtmlDocument htmlDocument = new HtmlDocument();

                                    logListOfString.Add("html document instantiated to check junk data");

                                    htmlDocument.LoadHtml(placeholderReplacer.Replacer);
                                    
                                    while (hasJunk)
                                    {
                                        //use recursion if editor has further inner junk nodes
                                        foreach (var htmlNode in htmlDocument.DocumentNode.ChildNodes)
                                        {
                                            if( ((htmlNode.InnerHtml.Contains("<xml") == true) || (htmlNode.NodeType == HtmlNodeType.Text) || (htmlNode.NodeType == HtmlNodeType.Comment)) && (htmlNode.InnerHtml.Contains("<span") == false))
                                            {
                                                htmlNode.ParentNode.RemoveChild(htmlNode, false);

                                                break;
                                            }
                                            if (htmlNode.InnerHtml.Contains("<span") == true && (htmlNode.InnerHtml.Contains("<xml") == true))
                                            {
                                                string innerhtmlForSpan = string.Empty;
                                                //in this case only span to be extracted and placed in para as innertext
                                                foreach (var htmlNodeInner in htmlNode.ChildNodes)
                                                {
                                                    if (htmlNodeInner.NodeType != HtmlNodeType.Comment)
                                                    {
                                                        innerhtmlForSpan += htmlNodeInner.OuterHtml;
                                                    }
                                                }
                                                htmlNode.InnerHtml = innerhtmlForSpan;

                                                break;
                                            }

                                        }

                                        if ((htmlDocument.DocumentNode.InnerHtml.Contains("<xml")) || (htmlDocument.DocumentNode.InnerHtml.Contains("\r")))
                                        {
                                            hasJunk = true;
                                        }
                                        else
                                            break;
                                    }

                                    //foreach (var htmlNode in htmlDocument.DocumentNode.ChildNodes)
                                    //{
                                    //    if ((htmlNode.InnerHtml.Contains("/xml") == true) || (htmlNode.NodeType == HtmlNodeType.Text))
                                    //    {
                                    //        htmlNode.ParentNode.RemoveChild(htmlNode, false);

                                    //        break;
                                    //    }
                                    //}

                                    foreach (var htmlNode in htmlDocument.DocumentNode.ChildNodes)
                                    {
                                        if ((htmlNode.Name == "p") && (htmlNode.Attributes.Count > 0))
                                        {
                                            //htmlNode.ParentNode.RemoveChild(htmlNode, false);

                                            htmlNode.Attributes.RemoveAll();
                                        }
                                    }
                                    string backslashremoved = htmlDocument.DocumentNode.InnerHtml.Replace("\"", "'");

                                    placeholderReplacer.Replacer = backslashremoved;
                                }

                                string replacer = string.Empty;

                                //Convert para without breaks into para with breaks
                                replacer = placeholderReplacer.Replacer;

                                //addprefix to identify first para
                                string paraID = "#FirstPara#";
                                replacer = paraID + replacer.Trim();

                                //create breaks next to para
                                replacer = replacer.Replace("<p>", "<p><br/>");

                                //remove firstpara ID
                                replacer = replacer.Replace(paraID + "<p><br/>", "<p>");

                                replacer = replacer.Replace(paraID, "");


                                //incase placeholder matches check if prefix is to be added
                                if ((placeholderReplacer.InlinePrefixHtmlText != null) && (placeholderReplacer.InlinePrefixHtmlText.Trim() != string.Empty))
                                {
                                   replacer = placeholderReplacer.InlinePrefixHtmlText + replacer;
                                }
                                //else
                                //{
                                //   replacer = placeholderReplacer.Replacer;
                                //}
                                //extract inner html from html replacer

                                //var htmlNode = HtmlNode.CreateNode(placeholderReplacer.Replacer);

                                logListOfString.Add("cleanup and preformating  of data success");

                                //appy para tag

                                //string  innerHtml = "<p>" + replacer + "</p>";
                                string innerHtml =replacer;

                                //init Html to openxml converter
                                HtmlToOpenXml htmlToOpenXml = new HtmlToOpenXml();

                                logListOfString.Add("sending html for conversion");

                                //replace para inner open xml with the one returned from Html to openXml convert function
                                paragraph.InnerXml = htmlToOpenXml.ConvertHtmlToOpenXml(innerHtml,logListOfString);
                                wordDoc.Save();
                                logListOfString.Add("docx part save success");
                            }
                        }
                        //else
                        //{
                        //    OPTools.TextReplacer.SearchAndReplace(wordDoc, placeholderReplacer.Placeholder, placeholderReplacer.Replacer, false);
                        //}
                    }
                    wordDoc.Save();

                    logListOfString.Add("docX save success");

                    wordDoc.Close();

                    logListOfString.Add("docx phase 1 success and close");
                }
                
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(worddocFullNameAndPath, true))
                {
                    logListOfString.Add("docx opened for phase 2 plain text ");
                    foreach (PlaceholderReplacer placeholderReplacer in placeholderReplacerList)
                    {
                        //if (placeholderReplacer.IsHtml)
                        //{
                        //    //locate placeholder or key in doc
                        //    foreach (var paragraph in wordDoc.MainDocumentPart.RootElement.Descendants<Paragraph>())
                        //    {
                        //        if (paragraph.InnerText.Contains(placeholderReplacer.Placeholder) != true) continue;

                        //        HtmlNodeUtilities htmlNodeUtilities = new HtmlNodeUtilities();
                        //        string innerHtml = htmlNodeUtilities.GetInnerHtml(placeholderReplacer.Replacer);

                        //        string replacer = string.Empty;
                        //        //incase placeholder matches check if prefix is to be added
                        //        if (placeholderReplacer.InlinePrefixHtmlText.Trim() != string.Empty)
                        //        {
                        //            replacer = placeholderReplacer.InlinePrefixHtmlText + placeholderReplacer.Replacer;
                        //        }
                        //        else
                        //        {
                        //            replacer = innerHtml;
                        //        }
                        //        //extract inner html from html replacer

                        //        //var htmlNode = HtmlNode.CreateNode(placeholderReplacer.Replacer);

                        //        //appy para tag
                        //        innerHtml = "<p>" + replacer + "</p>";

                        //        //init Html to openxml converter
                        //        HtmlToOpenXml htmlToOpenXml = new HtmlToOpenXml();
                        //        //replace para inner open xml with the one returned from Html to openXml convert function
                        //        paragraph.InnerXml = htmlToOpenXml.ConvertHtmlToOpenXml(innerHtml);
                        //    }
                        //}
                        //else
                        //{

                        string replacer = placeholderReplacer.Replacer.Replace("\f", "");
                        replacer = replacer.Replace("\n", "");
                        replacer = replacer.Replace("\r", "");
                        replacer = replacer.Replace("\a", "");
                        replacer = replacer.Replace("\b", "");
                        replacer = replacer.Replace("\t", "");
                        replacer = replacer.Replace("\v", "");

                        OPTools.TextReplacer.SearchAndReplace(wordDoc, placeholderReplacer.Placeholder, replacer, false);
                        //logListOfString.Add("docx text change using PowerTools success");
                        //}
                    }
                    wordDoc.Save();
                    logListOfString.Add("docx re-save success");

                    wordDoc.Close();
                    logListOfString.Add("docx closed");
                }

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(worddocFullNameAndPath, false))
                {
                    //if (wordDoc.MainDocumentPart.Document.InnerText.Contains("#PH#") == true)
                    //{
                    //    throw new Exception("template to doc creation not successfull, some placeholders are still not updated");
                    //}
                    wordDoc.Close();
                    //static string RemoveInvalidXmlChars(string text) {
                    //    char[] validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
                    //    return new string(validXmlChars);
                    //}

                }

                logListOfString.Add("placeholder # success check code is run disabled");

            }
            catch (Exception ex)
            {
                logListOfString.Add("Exception: " + ex.Message);
                
            }

        }

        public void HTMLToText(string htmlString)
        {
            //HtmlDocument htmlDocument = new HtmlDocument();
            //htmlDocument.Load("<p><span style='font - size: 12.0pt; line - height: 115 %; font - family: 'Arial','sans-serif'; mso - fareast - font - family: Calibri; mso - fareast - theme - font: minor - latin; mso - ansi - language: EN - US; mso - fareast - language: EN - US; mso - bidi - language: AR - SA; '>Smartphones and tablets with phones and calendars are the necessities of a modern man. These are mobile nodes (MN) with wireless network capabilities. The necessities of the MNs are generally included in the cellular modules available in LTE/3G and Wi-Fi for high-speed Internet. Further, many people with Wi-Fi and LTE/3G use a cross-layer-based handoff as they move from one place to another. MN mobility management has been handled adequately; however, using the network-based mobility management described in this paper, carriers can manage and maintain a lower-cost network.</span></p>");
            //return htmlDocument.DocumentNode.InnerText;

            //HtmlConverter htmlConverter = new HtmlConverter();

        }
    }
}

