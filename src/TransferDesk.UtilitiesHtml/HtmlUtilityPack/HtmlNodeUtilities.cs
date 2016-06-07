using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace TransferDesk.UtilitiesHtml.HtmlUtilityPack
{
    public class HtmlNodeUtilities
    {
        public HtmlNodeUtilities()
        {
           
        }

        public string GetInnerHtml(string html)
        {
            var htmlNode = HtmlNode.CreateNode(html);
            return htmlNode.InnerHtml;
        }
    }
}
