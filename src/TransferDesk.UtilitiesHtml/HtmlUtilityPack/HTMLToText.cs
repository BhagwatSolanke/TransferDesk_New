using HtmlAgilityPack;

namespace TransferDesk.UtilitiesHtml.HtmlUtilityPack
{
    public class HtmlToText
    {
        private HtmlDocument _htmlDocument;

        public HtmlToText()
        {
            _htmlDocument = new HtmlDocument();
            //_htmlDocument.ToString();
        }

        public string ConvertHtmlToPlainText(string htmlFragment)
        {
            _htmlDocument.DocumentNode.InnerHtml = htmlFragment;
            return HtmlEntity.DeEntitize(_htmlDocument.DocumentNode.InnerText);
        }
    }
}
