﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Windows.Forms;

namespace SEOScanner
{
    /*class TestJson
    {
        public string description;
        public int port;
        public string[] arguments;
        public int timeout;
    }*/

    class FileJsonConfig
    {
        public FileJsonConfigArguments[] arguments;
    }

    class FileJsonConfigArguments
    {
        public string description;                      // краткое описание
        public string search_by_tag_name;               // значение поиска - имя тега
        public string search_by_tag_id;                 // значение поиска - идентификатор
        public string search_by_tag_attribute;          // значение поиска - имя аттрибута
        public string search_by_tag_attribute_value;    // значение поиска - значение в аттрибуте
        public string type_get_value_from;              // получить значение из
        public string get_value_from_attribute_name;    // имя аттрибута
    }


    static class FileJSON
    {
        public const string DEFAULT = "DEFAULT";
        public const string UTF_8 = "UTF-8";
        public const string UTF_8_BOM = "UTF-8-BOM";
        public const string WINDOWS_1251 = "WINDOWS-1251";

        public const string FROM_TAG = "tag";
        public const string FROM_ATTRIBUTE = "attribute";

        public const string PASSED = "PASSED";
        public const string FAILED = "FAILED";

        public static string[] validatorJson(string encoding, string filename)
        {
            string[] result = new string[2];
            result[0] = "";
            result[1] = "";
            try
            {
                StreamReader sr;
                if (encoding == DEFAULT)
                {
                    sr = new StreamReader(filename, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    sr = new StreamReader(filename, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    sr = new StreamReader(filename, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    sr = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    sr = new StreamReader(filename, Encoding.Default);
                }
                string jsonText = sr.ReadToEnd();
                sr.Close();
                //TestJsonConfig test = JsonConvert.DeserializeObject<TestJsonConfig>(jsonText);
                result[0] = PASSED;
                result[1] = "Проверка: синтаксис JSON файла - корректный";
            }
            catch (Exception ex)
            {
                result[0] = FAILED;
                result[1] = ex.Message;
            }

            return result;
        }

        public static string[] initConfigFile(string encoding, string filename)
        {
            string[] result = new string[2];
            result[0] = "";
            result[1] = "";

            if (File.Exists(filename))
            {
                result[0] = PASSED;
                result[1] = "Файл конфигурации config.json";
            }
            else
            {
                string json = "{";
                json += System.Environment.NewLine + "\"arguments\":[";

                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Ключевые слова (keywords)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "meta" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "name" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + "keywords" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_ATTRIBUTE + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "content" + "\"";
                json += System.Environment.NewLine + "},";
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Описание страницы (description)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "meta" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "name" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"description" + "" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_ATTRIBUTE + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "content" + "\"";
                json += System.Environment.NewLine + "},";
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Заголовок документа (title)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "title" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_TAG + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "" + "\"";
                json += System.Environment.NewLine + "},";
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Заголовок на странице (h1)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "h1" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_TAG + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "" + "\"";
                json += System.Environment.NewLine + "},";
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Заголовок на странице (h2)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "h2" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_TAG + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "" + "\"";
                json += System.Environment.NewLine + "},";
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Заголовок на странице (h3)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "h3" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_TAG + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "" + "\"";
                json += System.Environment.NewLine + "},";
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Заголовок на странице (h4)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "h4" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_TAG + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "" + "\"";
                json += System.Environment.NewLine + "},";
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Заголовок на странице (h5)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "h5" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_TAG + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "" + "\"";
                json += System.Environment.NewLine + "},";
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + "Заголовок на странице (h6)" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + "h6" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + "" + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + FROM_TAG + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + "" + "\"";
                json += System.Environment.NewLine + "}";

                json += System.Environment.NewLine + "]";
                json += System.Environment.NewLine + "}";

                try
                {
                    StreamWriter writer;
                    if (encoding == DEFAULT)
                    {
                        writer = new StreamWriter(filename, false, Encoding.Default);
                    }
                    else if (encoding == UTF_8)
                    {
                        writer = new StreamWriter(filename, false, new UTF8Encoding(false));
                    }
                    else if (encoding == UTF_8_BOM)
                    {
                        writer = new StreamWriter(filename, false, new UTF8Encoding(true));
                    }
                    else if (encoding == WINDOWS_1251)
                    {
                        writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                    }
                    else
                    {
                        writer = new StreamWriter(filename, false, Encoding.Default);
                    }
                    writer.Write(json);
                    writer.Close();

                    result[0] = PASSED;
                    result[1] = "Файл конфигурации config.json - успешно создан!";
                }
                catch (Exception ex)
                {
                    result[0] = FAILED;
                    result[1] = ex.Message;
                }
            }
            
            return result;
        }

        public static FileJsonConfig readConfigFile(string encoding, string filename)
        {
            FileJsonConfig file = null;
            try
            {
                StreamReader sr;
                if (encoding == DEFAULT)
                {
                    sr = new StreamReader(filename, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    sr = new StreamReader(filename, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    sr = new StreamReader(filename, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    sr = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    sr = new StreamReader(filename, Encoding.Default);
                }
                string jsonText = sr.ReadToEnd();
                sr.Close();

                file = JsonConvert.DeserializeObject<FileJsonConfig>(jsonText);
                
            }
            catch (Exception ex)
            {

            }

            return file;
        }

        public static string[] updateConfigFile(string encoding, string filename, ListView listView)
        {
            string[] result = new string[2];
            result[0] = "";
            result[1] = "";

            string json = "{";
            json += System.Environment.NewLine + "\"arguments\":[";

            int count = listView.Items.Count;
            for (int i = 0; i < count; i++)
            {
                ListViewItem item = listView.Items[i];
                json += System.Environment.NewLine + "{";
                json += System.Environment.NewLine + "\"description\":\"" + item.SubItems[1].Text + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_name\":\"" + item.SubItems[2].Text + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_id\":\"" + item.SubItems[3].Text + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute\":\"" + item.SubItems[4].Text + "\",";
                json += System.Environment.NewLine + "\"search_by_tag_attribute_value\":\"" + item.SubItems[5].Text + "\",";
                json += System.Environment.NewLine + "\"type_get_value_from\":\"" + item.SubItems[6].Text + "\",";
                json += System.Environment.NewLine + "\"get_value_from_attribute_name\":\"" + item.SubItems[7].Text + "\"";
                if (i < count - 1) json += System.Environment.NewLine + "},";
                else json += System.Environment.NewLine + "}";
            }

            json += System.Environment.NewLine + "]";
            json += System.Environment.NewLine + "}";

            try
            {
                StreamWriter writer;
                if (encoding == DEFAULT)
                {
                    writer = new StreamWriter(filename, false, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    writer = new StreamWriter(filename, false, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    writer = new StreamWriter(filename, false, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    writer = new StreamWriter(filename, false, Encoding.Default);
                }
                writer.Write(json);
                writer.Close();

                result[0] = PASSED;
                result[1] = "Файл конфигурации config.json - обновлен";
            }
            catch (Exception ex)
            {
                result[0] = FAILED;
                result[1] = ex.Message;
            }

            return result;
        }



    }
}
