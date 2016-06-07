using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Services.Manuscript.ViewModel;
using TransferDesk.Utilities.CreateDocXFromTemplate;
using TransferDesk.Utilities.HtmlUtilityPack;
using TransferDesk.DAL.Manuscript.Repositories;
using System.Configuration;
namespace TransferDesk.Services.Manuscript.ReportOutputs
{
    public class EXCLUDETransferReportDocX
    {
        private ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;

        public EXCLUDETransferReportDocX()
        {
            string conString = string.Empty;
            conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
        }

        public string CreateReport(ManuscripScreeningVM manuscriptScreeningVM, string templatePath, string outputPath)
        {
            HTMLToText htmlToText = new HTMLToText();

            WordTemplateTextReplace wordTextReplace = new WordTemplateTextReplace();

            List<PlaceholderReplacer> placeholderReplacerList = new List<PlaceholderReplacer>();

            PlaceholderReplacer manuscriptTitle = new PlaceholderReplacer();

            manuscriptTitle._placeholder = "#PH#Title#";
            manuscriptTitle._replacer = manuscriptScreeningVM.ArticleTitle;
            placeholderReplacerList.Add(manuscriptTitle);

            PlaceholderReplacer placeholderReplacer1 = new PlaceholderReplacer();
            placeholderReplacer1._placeholder = "#PH#Authors#";  
            string otherAuthors=string.Empty;
            for(int count=0; count<manuscriptScreeningVM.OtherAuthors.Count();count++){
                otherAuthors+=","+ manuscriptScreeningVM.OtherAuthors[count].AuthorName;
            }
            placeholderReplacer1._replacer = manuscriptScreeningVM.CorrespondingAuthor+otherAuthors;
            placeholderReplacerList.Add(placeholderReplacer1);

            PlaceholderReplacer affiliation = new PlaceholderReplacer();
            affiliation._placeholder = "#PH#Affiliation#";
            affiliation._replacer = manuscriptScreeningVM.CorrespondingAuthorAff;
            placeholderReplacerList.Add(affiliation);

            PlaceholderReplacer articletype = new PlaceholderReplacer();
            articletype._placeholder = "#PH#Articletype#";
            articletype._replacer = _manuscriptDBRepositoryReadSide.GetArticleType(Convert.ToInt32(manuscriptScreeningVM.ArticleTypeID));
            placeholderReplacerList.Add(articletype);

            PlaceholderReplacer startDate = new PlaceholderReplacer();
            startDate._placeholder = "#PH#StartDate#";
            startDate._replacer = manuscriptScreeningVM.StartDate.ToShortDateString();
            placeholderReplacerList.Add(startDate);

            PlaceholderReplacer Abstarct = new PlaceholderReplacer();
            Abstarct._placeholder = "#PH#Abstract#";
            string htmlFragment = manuscriptScreeningVM.Abstarct;
            if (!string.IsNullOrEmpty(htmlFragment))
                Abstarct._replacer = htmlToText.ConvertHTMLToPlainText(htmlFragment);
            else
                Abstarct._replacer = "-";
            placeholderReplacerList.Add(Abstarct);

            PlaceholderReplacer Conclusions = new PlaceholderReplacer();
            Conclusions._placeholder = "#PH#Conclusions#";
            string htmlFragment3 = manuscriptScreeningVM.Conclusion;
            if (!string.IsNullOrEmpty(htmlFragment3))
                Conclusions._replacer = htmlToText.ConvertHTMLToPlainText(htmlFragment3);
            else
                Conclusions._replacer = "-";
            
            placeholderReplacerList.Add(Conclusions);

            PlaceholderReplacer iThenticateScore = new PlaceholderReplacer();
            iThenticateScore._placeholder = "#PH#iThenticateScore#";
            if (!string.IsNullOrEmpty(manuscriptScreeningVM.iThenticatePercentage.ToString()))
                iThenticateScore._replacer = manuscriptScreeningVM.iThenticatePercentage.ToString();
            else
                iThenticateScore._replacer = "-";

            placeholderReplacerList.Add(iThenticateScore);

            PlaceholderReplacer HighestiThenticate = new PlaceholderReplacer();
            HighestiThenticate._placeholder = "#PH#HighestiThenticate#";
            HighestiThenticate._replacer = manuscriptScreeningVM.Highest_iThenticateFromSingleSrc.ToString();
            placeholderReplacerList.Add(HighestiThenticate);

            PlaceholderReplacer iThenticateAdvice = new PlaceholderReplacer();
            iThenticateAdvice._placeholder = "#PH#iThenticateAdvice#";
            string iThenticateAdviceText=_manuscriptDBRepositoryReadSide.GetMetrixLegendTitle(Convert.ToInt32(manuscriptScreeningVM.Crosscheck_iThenticateResultID));

            if (!string.IsNullOrEmpty(iThenticateAdviceText))
                iThenticateAdvice._replacer = iThenticateAdviceText;
            else
                iThenticateAdvice._replacer = "-";

            placeholderReplacerList.Add(iThenticateAdvice);

            PlaceholderReplacer Comment_iThenticate= new PlaceholderReplacer();
            Comment_iThenticate._placeholder = "#PH#Comment_iThenticate#";
            if (!string.IsNullOrEmpty(manuscriptScreeningVM.Comments_Crosscheck_iThenticateResult))
                Comment_iThenticate._replacer = manuscriptScreeningVM.Comments_Crosscheck_iThenticateResult;
            else
                Comment_iThenticate._replacer = "-";

            placeholderReplacerList.Add(Comment_iThenticate);

            PlaceholderReplacer EnglishAdvice = new PlaceholderReplacer();
            EnglishAdvice._placeholder = "#PH#EnglishAdvice#";
            EnglishAdvice._replacer = _manuscriptDBRepositoryReadSide.GetMetrixLegendTitle(Convert.ToInt32(manuscriptScreeningVM.English_Lang_QualityID));
            placeholderReplacerList.Add(EnglishAdvice);

            PlaceholderReplacer Comment_English = new PlaceholderReplacer();
            Comment_English._placeholder = "#PH#Comment_English#";
            Comment_English._replacer = manuscriptScreeningVM.Comments_English_Lang_Quality;

            if (!string.IsNullOrEmpty(manuscriptScreeningVM.Comments_English_Lang_Quality))
                Comment_English._replacer = manuscriptScreeningVM.Comments_English_Lang_Quality;
            else
                Comment_English._replacer = "-";

            placeholderReplacerList.Add(Comment_English);

            PlaceholderReplacer EthicsAdvice = new PlaceholderReplacer();
            EthicsAdvice._placeholder = "#PH#EthicsAdvice#";
            EthicsAdvice._replacer = _manuscriptDBRepositoryReadSide.GetMetrixLegendTitle(Convert.ToInt32(manuscriptScreeningVM.Ethics_ComplianceID));
            placeholderReplacerList.Add(EthicsAdvice);

            PlaceholderReplacer Comment_Ethics = new PlaceholderReplacer();
            Comment_Ethics._placeholder = "#PH#Comment_Ethics#";
            if (!string.IsNullOrEmpty(manuscriptScreeningVM.Comments_Ethics_Compliance))
                Comment_Ethics._replacer = manuscriptScreeningVM.Comments_Ethics_Compliance;
            else
                Comment_Ethics._replacer = "-";

            placeholderReplacerList.Add(Comment_Ethics);

            PlaceholderReplacer GeneralAdvice = new PlaceholderReplacer();
            GeneralAdvice._placeholder = "#PH#GeneralAdvice#";

           string OverallAnalysis= _manuscriptDBRepositoryReadSide.GetMetrixLegendTitle(Convert.ToInt32(manuscriptScreeningVM.OverallAnalysisID));
           if (!string.IsNullOrEmpty(OverallAnalysis))
               GeneralAdvice._replacer = OverallAnalysis;
            else
                GeneralAdvice._replacer = "-";
            placeholderReplacerList.Add(GeneralAdvice);

            PlaceholderReplacer TransferredFrom = new PlaceholderReplacer();
            TransferredFrom._placeholder = "#PH#TransferredFrom#";
            if (!string.IsNullOrEmpty(manuscriptScreeningVM.TransferFrom))
                TransferredFrom._replacer = manuscriptScreeningVM.TransferFrom;
            else
                TransferredFrom._replacer = "-";
            placeholderReplacerList.Add(TransferredFrom);

            PlaceholderReplacer Results = new PlaceholderReplacer();
            Results._placeholder = "#PH#ReviewerComment#";
            string htmlFragment2 = manuscriptScreeningVM.ReviewerComments;
            if (!string.IsNullOrEmpty(htmlFragment2))
                Results._replacer = htmlToText.ConvertHTMLToPlainText(htmlFragment2);
            else
                Results._replacer = "None";

            placeholderReplacerList.Add(Results);

            wordTextReplace.ReplacePlaceholders(placeholderReplacerList, templatePath, outputPath);

            return outputPath; //return path
        }

    }
}
