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
        public void ReplacePlaceholders(List<PlaceholderReplacer> placeholderReplacerList, string templateDocxFullNameAndPath, string SaveAsDocxFullNameAndPath)
        {
           string worddocFullNameAndPath = string.Empty;

            try
            {

                //make a copy if the source and save paths are different

                if (templateDocxFullNameAndPath.ToLower() != SaveAsDocxFullNameAndPath.ToLower())
                {
                    File.Copy(templateDocxFullNameAndPath, SaveAsDocxFullNameAndPath,true);
                    if (File.Exists(SaveAsDocxFullNameAndPath) == false)
                    {
                        //raise exception if save as not created
                        throw new Exception("Exception creating a copy of template in WordTemplateTextReplace cause unknown");
                    }
                    else
                    {
                        worddocFullNameAndPath = SaveAsDocxFullNameAndPath;
                    }
                }
                else
                {
                    //in this case template itself will change
                    worddocFullNameAndPath = templateDocxFullNameAndPath;
                }

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(worddocFullNameAndPath, true))
                {
                                 
                    foreach (PlaceholderReplacer placeholderReplacer in placeholderReplacerList)
                    {
                        if (placeholderReplacer.IsHtml)
                        {
                            //locate placeholder or key in doc
                            foreach (var paragraph in wordDoc.MainDocumentPart.RootElement.Descendants<Paragraph>())
                            {
                                if (paragraph.InnerText.Contains(placeholderReplacer.Placeholder) != true) continue;

                                HtmlNodeUtilities htmlNodeUtilities = new HtmlNodeUtilities();
                                string innerHtml = htmlNodeUtilities.GetInnerHtml(placeholderReplacer.Replacer);

                                string replacer = string.Empty;
                                //incase placeholder matches check if prefix is to be added
                                if (placeholderReplacer.InlinePrefixHtmlText.Trim() != string.Empty)
                                {
                                   replacer = placeholderReplacer.InlinePrefixHtmlText + placeholderReplacer.Replacer;
                                }
                                else
                                {
                                   replacer = innerHtml;
                                }
                                //extract inner html from html replacer
                                   
                                //var htmlNode = HtmlNode.CreateNode(placeholderReplacer.Replacer);
                                    
                                //appy para tag
                                innerHtml = "<p>" + replacer + "</p>";
                                
                                //init Html to openxml converter
                                HtmlToOpenXml htmlToOpenXml = new HtmlToOpenXml();
                                //replace para inner open xml with the one returned from Html to openXml convert function
                                paragraph.InnerXml = htmlToOpenXml.ConvertHtmlToOpenXml(innerHtml);
                            }
                        }
                        //else
                        //{
                        //    OPTools.TextReplacer.SearchAndReplace(wordDoc, placeholderReplacer.Placeholder, placeholderReplacer.Replacer, false);
                        //}
                    }
                    wordDoc.Save();
                    wordDoc.Close();
                }
                
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(worddocFullNameAndPath, true))
                {

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
                            OPTools.TextReplacer.SearchAndReplace(wordDoc, placeholderReplacer.Placeholder, placeholderReplacer.Replacer, false);
                        //}
                    }
                    wordDoc.Save();
                    wordDoc.Close();
                }

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(worddocFullNameAndPath, false))
                {
                    //if (wordDoc.MainDocumentPart.Document.InnerText.Contains("#PH#") == true)
                    //{
                    //    throw new Exception("template to doc creation not successfull, some placeholders are still not updated");
                    //}
                    wordDoc.Close();
                }

            }
            catch (Exception ex)
            {

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

