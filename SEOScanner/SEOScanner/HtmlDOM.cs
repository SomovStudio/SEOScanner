using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEOScanner
{
    static class HtmlDOM
    {
        public static HtmlDocument document;

        public static HtmlElementCollection getElementsByTagName(string tagName)
        {
            return document.GetElementsByTagName(tagName);
        }

        public static HtmlElement getElementById(string tagName)
        {
            return document.GetElementById(tagName);
        }

        public static string getAttribute(HtmlElement element, string attributeName)
        {
            return element.GetAttribute(attributeName);
        }

        public static void getElementsByXPath(string xPathQuery)
        {
            
        }

        

    }
}
