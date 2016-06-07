using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Services.Manuscript.ViewModel;
using HtmlAgilityPack;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Utilities.HtmlUtilityPack;


namespace TransferDesk.Services.Manuscript.Preview
{
    public class HtmlRowData
    {
        public string InnerHtml { get; set; }
        public string LabelText { get; set; }
        public string StyleClass { get; set; }
    }
    public class ManuscriptScreeningHtmlPreview
    {
        private ManuscripScreeningVM _manuscriptScreeningVM;
        private HTMLToText _htmlToText;

        public List<ArticleType> ArticleTypeList { get; set; }
        public List<Section> SectionList { get; set; }
        public List<Journal> JournalList { get; set; }
        public List<JournalStatus> JournalStatusList { get; set; }

        public ManuscriptScreeningHtmlPreview(ManuscripScreeningVM manuscriptScreeningVM)
        {
            _manuscriptScreeningVM = manuscriptScreeningVM;
            _htmlToText = new HTMLToText();

        }

        public string CreateHtmlPreview(List<HtmlRowData> htmlNodeDataList, string parentNodename, bool asHtmlPage,string tableClassName)
        {
            var doc = new HtmlDocument();

            if (asHtmlPage == true)
            {
                parentNodename = "Body";
            }

            var parentNode = doc.CreateElement(parentNodename);

            var tableNode = doc.CreateElement("Table class='"+ tableClassName+"'");

            parentNode.AppendChild(tableNode);

            foreach (HtmlRowData htmlNodeData in htmlNodeDataList)
            {
                CreateAppendRow(doc, tableNode, htmlNodeData);
            }


            if (asHtmlPage == true)
            {
                string testHtml = "<HTML><Head><meta charset='UTF-8'><Head><Body>" + tableNode.OuterHtml + "</Body></HTML>";
                return testHtml;
            }
            else
            {
                return doc.DocumentNode.OuterHtml;
            }
        }

        public string GetArticleType(int? articleTypeID)
        {
            if (articleTypeID == null) return string.Empty;

            foreach (ArticleType articleType in ArticleTypeList)
            {
                if (articleType.ID == _manuscriptScreeningVM.ArticleTypeID)
                {
                    return articleType.ArticleTypeName;

                }
            }

            return string.Empty;
        }

        public string GetSection(int? sectionID)
        {
            if (sectionID == null) return string.Empty;

            foreach (Section section in SectionList)
            {
                if (section.ID == _manuscriptScreeningVM.SectionID)
                {
                    return section.SectionName;

                }
            }

            return string.Empty;
        }

        public string GetJournalStatus(int? journalStatusID)
        {
            if (journalStatusID == null) return string.Empty;

            foreach (JournalStatus journalStatus in JournalStatusList)
            {
                if (journalStatus.ID == _manuscriptScreeningVM.JournalStatusID)
                {
                    return journalStatus.Status;
                }
            }

            return string.Empty;
        }

        public string GetJournal(int? journalID)
        {
            if (journalID == null) return string.Empty;

            foreach (Journal journal in JournalList)
            {
                if (journal.ID == _manuscriptScreeningVM.JournalID)
                {
                    return journal.JournalTitle;

                }
            }

            return string.Empty;
        }

        private void CreateAppendRow(HtmlDocument doc, HtmlNode appendToTableNode, HtmlRowData htmlRowData)
        {
            if (htmlRowData.InnerHtml == null)
            {
                htmlRowData.InnerHtml = string.Empty;
            }

            //Table Row Starts
            var rowNode = doc.CreateElement("TR class ='" + htmlRowData.StyleClass + "'");

            //Table First Column Div Starts
            var rowDivLabelNode = doc.CreateElement("TD");

            //First Column Cell Label Starts
            var rowLabelNode = doc.CreateElement("Div");

            //Label caption text
            rowLabelNode.InnerHtml = htmlRowData.LabelText;

            //Label Cell node appended to column node
            rowDivLabelNode.AppendChild(rowLabelNode);

            //Column node append to Row
            rowNode.AppendChild(rowDivLabelNode);

            //Table Second Column Text Div Starts
            var rowDivTextNode = doc.CreateElement("TD");

            //SecondColumn Column Cell Label Starts
            var textColumnNode = doc.CreateElement("Div");

            //Text column text
            //textColumnNode.InnerHtml = _htmlToText.ConvertHTMLToPlainText(htmlRowData.InnerHtml);
            textColumnNode.InnerHtml = htmlRowData.InnerHtml;

            //text Cell node appended to column node
            rowDivTextNode.AppendChild(textColumnNode);

            //Column node append to Row
            rowNode.AppendChild(rowDivTextNode);

            //Table Row append to Table Node
            appendToTableNode.AppendChild(rowNode);

        }

    }
}