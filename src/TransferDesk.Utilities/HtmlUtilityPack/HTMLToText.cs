using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace TransferDesk.Utilities.HtmlUtilityPack
{
    public class HTMLToText
    {
        HtmlDocument _htmlDocument;

        public HTMLToText()
        {
            _htmlDocument = new HtmlDocument();
            _htmlDocument.ToString();
        }

        public string ConvertHTMLToPlainText(string HTMLFragment)
        {
            _htmlDocument.DocumentNode.InnerHtml = HTMLFragment;
            return HtmlEntity.DeEntitize(_htmlDocument.DocumentNode.InnerText);
            
        }
    }
}
