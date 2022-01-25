using System;
using System.Collections;
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

        /*       
        public static ArrayList getValuesFormElements(string search_by_tag_name, string search_by_tag_id, string type_get_value_from, string get_value_from_attribute_name)
        {
            ArrayList values = new ArrayList();
            if (search_by_tag_name != "")
            {
                HtmlElementCollection elements = HtmlDOM.getElementsByTagName(search_by_tag_name);
                
                foreach (HtmlElement element in elements)
                {
                    if (type_get_value_from == FileJSON.FROM_TAG) values.Add(element.InnerText);
                    else if(type_get_value_from == FileJSON.FROM_ATTRIBUTE) values.Add(element.GetAttribute(get_value_from_attribute_name));
                }

            }else if (search_by_tag_id != "")
            {
                HtmlElement element = HtmlDOM.getElementById(search_by_tag_id);
                if (type_get_value_from == FileJSON.FROM_TAG) values.Add(element.InnerText);
                else if (type_get_value_from == FileJSON.FROM_ATTRIBUTE) values.Add(element.GetAttribute(get_value_from_attribute_name));
            }
            return values;
        }
        */

        public static ArrayList getValues(string search_by_tag_name, string search_by_tag_id, string search_by_tag_attribute, string search_by_tag_attribute_value, string type_get_value_from, string get_value_from_attribute_name)
        {
            ArrayList values = new ArrayList();
            if (search_by_tag_name != "") // имя тега ----------------------------------------------------------------------------------------------
            {
                HtmlElementCollection elements = HtmlDOM.getElementsByTagName(search_by_tag_name);
                foreach (HtmlElement element in elements)
                {
                    if (search_by_tag_attribute != "") // фильтр по атрибуту
                    {
                        if (search_by_tag_attribute_value == element.GetAttribute(search_by_tag_attribute))
                        {
                            if (type_get_value_from == FileJSON.FROM_TAG) values.Add(element.InnerText);
                            else if (type_get_value_from == FileJSON.FROM_ATTRIBUTE) values.Add(element.GetAttribute(get_value_from_attribute_name));
                        }
                    }
                    else // фильтр только по тегу (без атрибута)
                    {
                        if (type_get_value_from == FileJSON.FROM_TAG) values.Add(element.InnerText);
                        else if (type_get_value_from == FileJSON.FROM_ATTRIBUTE) values.Add(element.GetAttribute(get_value_from_attribute_name));
                    }
                }
            }
            else if (search_by_tag_id != "") // идентификатор тега --------------------------------------------------------------------------------
            {
                HtmlElement element = HtmlDOM.getElementById(search_by_tag_id);
                if (search_by_tag_attribute != "") // фильтр по атрибуту
                {
                    if (search_by_tag_attribute_value == element.GetAttribute(search_by_tag_attribute))
                    {
                        if (type_get_value_from == FileJSON.FROM_TAG) values.Add(element.InnerText);
                        else if (type_get_value_from == FileJSON.FROM_ATTRIBUTE) values.Add(element.GetAttribute(get_value_from_attribute_name));
                    }
                }
                else // фильтр только по тегу (без атрибута)
                {
                    if (type_get_value_from == FileJSON.FROM_TAG) values.Add(element.InnerText);
                    else if (type_get_value_from == FileJSON.FROM_ATTRIBUTE) values.Add(element.GetAttribute(get_value_from_attribute_name));
                }
            }
            return values;
        }

    }
}
