using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Services.Manuscript.ViewModel;
using TransferDesk.UtilitiesOpenXml.CreateDocXFromTemplate;
using TransferDesk.UtilitiesHtml.HtmlUtilityPack;
using TransferDesk.DAL.Manuscript.Repositories;
using System.Configuration;
using HtmlAgilityPack;
using System.IO;

namespace TransferDesk.Services.Manuscript.ReportOutputs
{
    public class TransferReportDocX
    {
        private ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;

        public TransferReportDocX()
        {
            string conString = string.Empty;
            conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
        }

        public string CreateReport(ManuscripScreeningVM manuscriptScreeningVM, string templatePath, string outputPath, List<string> logListOfString )
        {

            //make a copy if the source and save paths are different

            if (templatePath.ToLower() != outputPath.ToLower())
            {
                logListOfString.Add("copying template to output");

                File.Copy(templatePath, outputPath, true);
                if (File.Exists(outputPath) == false)
                {
                    //raise exception if save as not created
                    throw new Exception("Exception creating a copy of template");
                }
                else
                {
                    logListOfString.Add("copied to path " + outputPath);
                    //worddocFullNameAndPath = SaveAsDocxFullNameAndPath;
                }
            }
            else
            {
                //in this case template itself will change
                //= templatePath;
            }


            try
            {
                //HtmlToText htmlToText = new HtmlToText();

                logListOfString.Add("HTML to Text instantiated");

                WordTemplateTextReplace wordTextReplace = new WordTemplateTextReplace();

                logListOfString.Add("word Text Replace instantiated");

                List<PlaceholderReplacer> placeholderReplacerList = new List<PlaceholderReplacer>();

                logListOfString.Add("placeholderList instantiated");

                PlaceholderReplacer manuscriptTitle = new PlaceholderReplacer();
                logListOfString.Add("manuscriptTitle instantiated as first placeholderreplacer");

                manuscriptTitle.Placeholder = "#PH#Title#";
                manuscriptTitle.Replacer = manuscriptScreeningVM.ArticleTitle;
                placeholderReplacerList.Add(manuscriptTitle);

                logListOfString.Add("Manuscript title added to list : " + manuscriptScreeningVM.ArticleTitle);

                PlaceholderReplacer placeholderReplacer1 = new PlaceholderReplacer();
                placeholderReplacer1.Placeholder = "#PH#Authors#";
                string otherAuthors = manuscriptScreeningVM.CorrespondingAuthor;
                for (int count = 0; count < manuscriptScreeningVM.OtherAuthors.Count(); count++)
                {
                    otherAuthors += ", " + manuscriptScreeningVM.OtherAuthors[count].AuthorName;
                }
                placeholderReplacer1.Replacer = otherAuthors;
                //placeholderReplacer1.IsHtml = true;
                placeholderReplacerList.Add(placeholderReplacer1);

                PlaceholderReplacer affiliation = new PlaceholderReplacer();
                affiliation.Placeholder = "#PH#Affiliation#";
                string otherAuthorAffs = "<p>" + HtmlEntity.DeEntitize(manuscriptScreeningVM.CorrespondingAuthorAff) + "</p>";
                for (int count = 0; count < manuscriptScreeningVM.OtherAuthors.Count(); count++)
                {
                    string authorAffiliation = HtmlEntity.DeEntitize(manuscriptScreeningVM.OtherAuthors[count].Affillation);
                    //string authorAffiliation = System.Web.HttpUtility.HtmlEncode(manuscriptScreeningVM.OtherAuthors[count].Affillation);
                    //affiliation = affiliation.Replace(";","")
                    otherAuthorAffs += "<p>" + authorAffiliation + "</p>";
                }
                //affiliation.Replacer = HtmlEntity.Entitize(otherAuthorAffs.Replace(";", "</p><p>"));
                affiliation.Replacer = otherAuthorAffs.Replace(";", "</p><p>");
                affiliation.IsHtml = true;
                placeholderReplacerList.Add(affiliation);

                PlaceholderReplacer articletype = new PlaceholderReplacer();
                articletype.Placeholder = "#PH#Articletype#";
                articletype.Replacer =
                    _manuscriptDBRepositoryReadSide.GetArticleType(Convert.ToInt32(manuscriptScreeningVM.ArticleTypeID));
                placeholderReplacerList.Add(articletype);

                PlaceholderReplacer startDate = new PlaceholderReplacer();
                startDate.Placeholder = "#PH#StartDate#";
                startDate.Replacer = manuscriptScreeningVM.StartDate.ToShortDateString();
                placeholderReplacerList.Add(startDate);

                PlaceholderReplacer abstarct = new PlaceholderReplacer();
                abstarct.Placeholder = "#PH#Abstract#";
                string htmlFragment = manuscriptScreeningVM.Abstarct;
                if (!string.IsNullOrEmpty(htmlFragment))
                {
                    abstarct.Replacer = htmlFragment;
                    abstarct.InlinePrefixHtmlText = "<Strong>Abstract : </Strong>";
                    abstarct.IsHtml = true;
                }
                else
                    abstarct.Replacer = "-";
                placeholderReplacerList.Add(abstarct);

                PlaceholderReplacer conclusions = new PlaceholderReplacer();
                conclusions.Placeholder = "#PH#Conclusions#";
                string htmlFragment3 = manuscriptScreeningVM.Conclusion;
                if (!string.IsNullOrEmpty(htmlFragment3))
                {
                    conclusions.Replacer = htmlFragment3;
                    conclusions.InlinePrefixHtmlText = "<Strong>Last Paragraph/ Conclusions : </Strong>";
                    conclusions.IsHtml = true;
                }
                else
                    conclusions.Replacer = "-";

                placeholderReplacerList.Add(conclusions);

                PlaceholderReplacer iThenticateScore = new PlaceholderReplacer();
                iThenticateScore.Placeholder = "#PH#iThenticateScore#";
                if (!string.IsNullOrEmpty(manuscriptScreeningVM.iThenticatePercentage.ToString()))
                    iThenticateScore.Replacer = manuscriptScreeningVM.iThenticatePercentage.ToString();
                else
                    iThenticateScore.Replacer = "-";

                placeholderReplacerList.Add(iThenticateScore);

                PlaceholderReplacer HighestiThenticate = new PlaceholderReplacer();
                HighestiThenticate.Placeholder = "#PH#HighestiThenticate#";
                HighestiThenticate.Replacer = manuscriptScreeningVM.Highest_iThenticateFromSingleSrc.ToString();
                placeholderReplacerList.Add(HighestiThenticate);

                PlaceholderReplacer iThenticateAdvice = new PlaceholderReplacer();
                iThenticateAdvice.Placeholder = "#PH#iThenticateAdvice#";
                string iThenticateAdviceText =
                    _manuscriptDBRepositoryReadSide.GetMetrixLegendTitle(
                        Convert.ToInt32(manuscriptScreeningVM.Crosscheck_iThenticateResultID));

                if (!string.IsNullOrEmpty(iThenticateAdviceText))
                    iThenticateAdvice.Replacer = iThenticateAdviceText;
                else
                    iThenticateAdvice.Replacer = "-";

                placeholderReplacerList.Add(iThenticateAdvice);

                PlaceholderReplacer Comment_iThenticate = new PlaceholderReplacer();
                Comment_iThenticate.Placeholder = "#PH#Comment_iThenticate#";
                if (!string.IsNullOrEmpty(manuscriptScreeningVM.Comments_Crosscheck_iThenticateResult))
                    Comment_iThenticate.Replacer = manuscriptScreeningVM.Comments_Crosscheck_iThenticateResult;
                else
                    Comment_iThenticate.Replacer = "-";

                placeholderReplacerList.Add(Comment_iThenticate);

                PlaceholderReplacer EnglishAdvice = new PlaceholderReplacer();
                EnglishAdvice.Placeholder = "#PH#EnglishAdvice#";
                EnglishAdvice.Replacer =
                    _manuscriptDBRepositoryReadSide.GetMetrixLegendTitle(
                        Convert.ToInt32(manuscriptScreeningVM.English_Lang_QualityID));
                placeholderReplacerList.Add(EnglishAdvice);

                PlaceholderReplacer Comment_English = new PlaceholderReplacer();
                Comment_English.Placeholder = "#PH#Comment_English#";
                Comment_English.Replacer = manuscriptScreeningVM.Comments_English_Lang_Quality;

                if (!string.IsNullOrEmpty(manuscriptScreeningVM.Comments_English_Lang_Quality))
                    Comment_English.Replacer = manuscriptScreeningVM.Comments_English_Lang_Quality;
                else
                    Comment_English.Replacer = "-";

                placeholderReplacerList.Add(Comment_English);

                PlaceholderReplacer EthicsAdvice = new PlaceholderReplacer();
                EthicsAdvice.Placeholder = "#PH#EthicsAdvice#";
                EthicsAdvice.Replacer =
                    _manuscriptDBRepositoryReadSide.GetMetrixLegendTitle(
                        Convert.ToInt32(manuscriptScreeningVM.Ethics_ComplianceID));
                placeholderReplacerList.Add(EthicsAdvice);

                PlaceholderReplacer Comment_Ethics = new PlaceholderReplacer();
                Comment_Ethics.Placeholder = "#PH#Comment_Ethics#";
                if (!string.IsNullOrEmpty(manuscriptScreeningVM.Comments_Ethics_Compliance))
                    Comment_Ethics.Replacer = manuscriptScreeningVM.Comments_Ethics_Compliance;
                else
                    Comment_Ethics.Replacer = "-";

                placeholderReplacerList.Add(Comment_Ethics);

                PlaceholderReplacer GeneralAdvice = new PlaceholderReplacer();
                GeneralAdvice.Placeholder = "#PH#GeneralAdvice#";

                string OverallAnalysis =
                    _manuscriptDBRepositoryReadSide.GetMetrixLegendTitle(
                        Convert.ToInt32(manuscriptScreeningVM.OverallAnalysisID));
                if (!string.IsNullOrEmpty(OverallAnalysis))
                    GeneralAdvice.Replacer = OverallAnalysis;
                else
                    GeneralAdvice.Replacer = "-";
                placeholderReplacerList.Add(GeneralAdvice);

                PlaceholderReplacer TransferredFrom = new PlaceholderReplacer();
                TransferredFrom.Placeholder = "#PH#TransferredFrom#";
                if (!string.IsNullOrEmpty(manuscriptScreeningVM.TransferFrom))
                    TransferredFrom.Replacer = manuscriptScreeningVM.TransferFrom;
                else
                    TransferredFrom.Replacer = "-";
                placeholderReplacerList.Add(TransferredFrom);

                PlaceholderReplacer Results = new PlaceholderReplacer();
                //#PH#ReviewerComment#
                Results.Placeholder = "#PH#ReviewerComment#";
                string htmlFragmentR = manuscriptScreeningVM.ReviewerComments;
                //if (!string.IsNullOrEmpty(htmlFragment2))
                //    Results.Replacer = htmlToText.ConvertHtmlToPlainText(htmlFragment2);
                //else
                if (!string.IsNullOrEmpty(htmlFragmentR))
                {
                    Results.Replacer = htmlFragmentR;
                    Results.InlinePrefixHtmlText = "<Strong>Reviewer's Comments : </Strong>";
                    Results.IsHtml = true;
                }
                else
                    Results.Replacer = "None";

                placeholderReplacerList.Add(Results);

                logListOfString.Add("Sending Placeholder List to replacer");

                wordTextReplace.ReplacePlaceholders(placeholderReplacerList, outputPath, logListOfString);
            }
            catch (Exception ex)
            {
               
               // throw;
            }

            return outputPath; //return path
        }

       

    }

}
