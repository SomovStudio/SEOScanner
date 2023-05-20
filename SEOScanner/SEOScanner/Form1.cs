using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SEOScanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private Thread thread;
        private int steps, step, percent;
        private string currentURL;
        private string programPath;
        private string configFile;


        private void Form1_Load(object sender, EventArgs e)
        {
            thread = new Thread(loadSitemap);
            programPath = Environment.CurrentDirectory + "\\";
            configFile = programPath + "config.json";

            string[] result = FileJSON.initConfigFile(FileJSON.UTF_8_BOM, configFile);
            addConsoleMessage(result[1]);
            if(result[0] == FileJSON.PASSED)
            {
                FileJsonConfig configData = FileJSON.readConfigFile(FileJSON.UTF_8_BOM, configFile);
                if(configData != null)
                {
                    listView2.Items.Clear();
                    ListViewItem item;
                    ListViewItem.ListViewSubItem subitem;
                    foreach (FileJsonConfigArguments argument in configData.arguments)
                    {
                        item = new ListViewItem();
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = argument.description;
                        item.SubItems.Add(subitem);
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = argument.search_by_tag_name;
                        item.SubItems.Add(subitem);
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = argument.search_by_tag_id;
                        item.SubItems.Add(subitem);
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = argument.search_by_tag_attribute;
                        item.SubItems.Add(subitem);
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = argument.search_by_tag_attribute_value;
                        item.SubItems.Add(subitem);
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = argument.type_get_value_from;
                        item.SubItems.Add(subitem);
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = argument.get_value_from_attribute_name;
                        item.SubItems.Add(subitem);
                        listView2.Items.Add(item);
                    }
                }
                else
                {
                    addConsoleMessage("Ошибка чтения конфигурационного файла config.json");
                }
            }

            if (checkBoxUserAgent.Checked) addConsoleMessage("Настройки User-Agent - по умолчанию (" + textBoxUserAgent.Text + ")");
            if (radioButton1.Checked) addConsoleMessage("Способ отображения страниц - только HTML (по умолчанию)");
            else addConsoleMessage("Способ отображения страниц - полностью.");

        }

        private void addConsoleMessage(string message)
        {
            DateTime localDate = DateTime.Now;
            consoleRichTextBox.Text = "[" + localDate.ToString() + "]: " + message + Environment.NewLine + consoleRichTextBox.Text;
            consoleRichTextBox.Update();
        }

        private void addReportMessage(string message)
        {
            reportRichTextBox.Text = reportRichTextBox.Text + Environment.NewLine + message;
            reportRichTextBox.Update();
        }

        private void addReportMessageFailed(string message)
        {
            reportFailedRichTextBox.Text = reportFailedRichTextBox.Text + Environment.NewLine + message;
            reportFailedRichTextBox.Update();
        }

        private const string BLUE = "blue";
        private const string YELLOW = "yellow";
        private const string RED = "red";

        private void addItemInTableReport(string color, string page, string obj, string value)
        {
            ListViewItem item;
            ListViewItem.ListViewSubItem subitem;
            item = new ListViewItem();
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = page;
            item.SubItems.Add(subitem);
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = obj;
            item.SubItems.Add(subitem);
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = value;
            item.SubItems.Add(subitem);
            if (color == BLUE) item.ImageIndex = 0;
            if (color == YELLOW) item.ImageIndex = 1;
            if (color == RED) item.ImageIndex = 2;
            listView1.Items.Add(item);
        }

        /* Остановить процесс */
        private void stopProcess()
        {
            if (thread.ThreadState.ToString() == "Unstarted")
            {
                MessageBox.Show("Процесс еще не запущен");
                return;
            }

            addConsoleMessage("Процесс прерван пользователем");
            try
            {
                thread.Abort();
                addConsoleMessage("Процесс завершен");
                MessageBox.Show("Процесс завершен!");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                addConsoleMessage("Сообщение: " + error.Message);
            }
        }

        private ArrayList readXML(string filename)
        {
            ArrayList list = new ArrayList();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            XmlDocument xDoc;
            if (checkBoxUserAgent.Checked == false)
            {
                WebClient client = new WebClient();
                client.Headers["User-Agent"] = textBoxUserAgent.Text;
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                string data = client.DownloadString(filename);

                xDoc = new XmlDocument();
                xDoc.LoadXml(data);
            }
            else
            {
                xDoc = new XmlDocument();
                xDoc.Load(filename);
            }

            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                for (int j = 0; j <= xnode.ChildNodes.Count; j++)
                {
                    if (xnode.ChildNodes[j].Name == "loc")
                    {
                        string xmlLink = xnode.ChildNodes[j].InnerText;
                        list.Add(xmlLink);
                        break;
                    }
                }
            }

            return list;
        }

        private void loadSitemap()
        {
            try
            {
                ArrayList sitemaps = new ArrayList();
                string urlSitemap = "";

                /* собираю все sitemap */
                sitemaps.Add(toolStripTextBoxPath.Text);
                for (int i = 0; i < sitemaps.Count; i++)
                {
                    string xmlLink = sitemaps[i].ToString();
                    if (xmlLink.Contains(".xml") == true)
                    {
                        ArrayList listSitemaps = readXML(xmlLink);
                        for (int j = 0; j < listSitemaps.Count; j++)
                        {
                            urlSitemap = "";
                            urlSitemap = (string)listSitemaps[j];
                            if (urlSitemap.Contains(".xml") == true)
                            {
                                sitemaps.Add(urlSitemap);
                            }
                            else
                            {
                                richTextBoxListLinks.Text += urlSitemap;
                                if (j != (listSitemaps.Count - 1)) richTextBoxListLinks.Text += Environment.NewLine;
                                richTextBoxListLinks.Update();
                            }
                        }
                    }
                }
                addConsoleMessage("Загрузка ссылок завершена");
                MessageBox.Show("Загрузка ссылок завершена", "Сообщение");
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка");
            }
            panelMessageLoadLinks.Visible = false;
        }

        private void runScanner()
        {
            currentURL = richTextBoxListLinks.Lines[step];
            step++;

            addReportMessage("Страница:" + currentURL);

            string page = "";
            try
            {
                if (checkBoxUserAgent.Checked == false) page = Sitemap.getPageHtmlDOM(currentURL, textBoxUserAgent.Text);
                else page = Sitemap.getPageHtmlDOM(currentURL, "");
                addItemInTableReport(BLUE, currentURL, "", "");
            }
            catch (Exception error)
            {
                addConsoleMessage("Ошибка: " + error.Message);
                addConsoleMessage("Ошибка чтения странирцы " + currentURL);
                addItemInTableReport(RED, currentURL, "", error.Message);
            }

            if (radioButton1.Checked) webBrowser1.DocumentText = page;
            else
            {
                if (checkBoxUserAgent.Checked == true) webBrowser1.Navigate(currentURL, "", null, string.Format("User-Agent: {0}\r\n", textBoxUserAgent.Text));
                else webBrowser1.Navigate(currentURL); // без user agent !!!!!
            }

            thread.Abort();
        }

        public void updateConfig()
        {
            programPath = Environment.CurrentDirectory + "\\";
            configFile = programPath + "config.json";
            string[] result = FileJSON.updateConfigFile(FileJSON.UTF_8_BOM, configFile, listView2);
            addConsoleMessage(result[1]);
        }

        /* Поиск по тексту */
        int _findIndex = 0;
        int _findLast = 0;
        String _findText = "";
        private void findText(ToolStripComboBox _cbox)
        {
            try
            {
                bool resolution = true;
                for (int k = 0; k < _cbox.Items.Count; k++)
                    if (_cbox.Items[k].ToString() == _cbox.Text) resolution = false;
                if (resolution) _cbox.Items.Add(_cbox.Text);
                if (_findText != _cbox.Text)
                {
                    _findIndex = 0;
                    _findLast = 0;
                    _findText = _cbox.Text;
                }
                if (reportRichTextBox.Find(_cbox.Text, _findIndex, reportRichTextBox.TextLength - 1, RichTextBoxFinds.None) >= 0)
                {
                    reportRichTextBox.Select();
                    _findIndex = reportRichTextBox.SelectionStart + reportRichTextBox.SelectionLength;
                    if (_findLast == reportRichTextBox.SelectionStart)
                    {
                        addConsoleMessage("Поиск в отчете - завершен");
                        MessageBox.Show("Поиск в отчете - завершен");
                        _findIndex = 0;
                        _findLast = 0;
                        _findText = _cbox.Text;
                    }
                    else
                    {
                        _findLast = reportRichTextBox.SelectionStart;
                    }
                }
                else
                {
                    addConsoleMessage("Поиск в отчете - завершен");
                    MessageBox.Show("Поиск в отчете - завершен");
                    _findIndex = 0;
                    _findLast = 0;
                    _findText = _cbox.Text;
                }

            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        /* Поиск по таблице */
        int _tabFindIndex = 0;
        int _tabFindLast = 0;
        String _tabFindText = "";
        private void tabFindValue(ToolStripComboBox _cbox)
        {
            try
            {
                bool resolution = true;
                for (int k = 0; k < _cbox.Items.Count; k++)
                {
                    if (_cbox.Items[k].ToString() == _cbox.Text) resolution = false;
                }
                if (resolution) _cbox.Items.Add(_cbox.Text);
                if (_tabFindText != _cbox.Text)
                {
                    _tabFindIndex = 0;
                    _tabFindLast = 0;
                    _tabFindText = _cbox.Text;
                }

                string _page;
                string _object;
                string _value;
                int count = listView1.Items.Count;
                for (_tabFindIndex = _tabFindLast; _tabFindIndex < count; _tabFindIndex++)
                {
                    ListViewItem item = listView1.Items[_tabFindIndex];
                    _page = item.SubItems[1].Text;
                    _object = item.SubItems[2].Text;
                    _value = item.SubItems[3].Text;

                    if(_page.IndexOf(_tabFindText) > 0 || _object.IndexOf(_tabFindText) > 0 || _value.IndexOf(_tabFindText) > 0)
                    {
                        listView1.Focus();
                        listView1.Items[_tabFindIndex].Selected = true;
                        listView1.EnsureVisible(_tabFindIndex);
                        _tabFindLast = _tabFindIndex + 1;
                        break;
                    }
                }

                if(_tabFindIndex >= count)
                {
                    addConsoleMessage("Поиск в таблице результатов - завершен");
                    MessageBox.Show("Поиск в таблице результатов - завершен");
                    _tabFindIndex = 0;
                    _tabFindLast = 0;
                    _tabFindText = _cbox.Text;
                }
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        /* ------------------------------------------------------------------------------------------- */

        /*
        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            HtmlDocument document = this.webBrowser1.Document;
            if (document != null && document.All["userName"] != null && String.IsNullOrEmpty(document.All["userName"].GetAttribute("value")))
            {
                e.Cancel = true;
                addConsoleMessage(e.Url.ToString());
                MessageBox.Show("You must enter your name before you can navigate to " + e.Url.ToString());
            }
        }
        */

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUserAgent.Checked == true)
            {
                textBoxUserAgent.ReadOnly = true;
                textBoxUserAgent.Text = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1";
                addConsoleMessage("Настройки User-Agent - по умолчанию (" + textBoxUserAgent.Text + ")");
            }
            else
            {
                textBoxUserAgent.ReadOnly = false;
                addConsoleMessage("Пользовательские настройки User-Agent");
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            stopProcess();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Text = "Новая запись";
            form.parentForm = this;
            form.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (listView2.FocusedItem != null)
            {
                string _description;                      // краткое описание
                string _search_by_tag_name;               // значение поиска - имя тега
                string _search_by_tag_id;                 // значение поиска - идентификатор
                string _search_by_tag_attribute;          // значение поиска - имя аттрибута
                string _search_by_tag_attribute_value;    // значение поиска - значение в аттрибуте
                string _type_get_value_from;              // получить значение из
                string _get_value_from_attribute_name;    // имя аттрибута

                int index = listView2.FocusedItem.Index;

                _description = listView2.Items[index].SubItems[1].Text;
                _search_by_tag_name = listView2.Items[index].SubItems[2].Text;
                _search_by_tag_id = listView2.Items[index].SubItems[3].Text;
                _search_by_tag_attribute = listView2.Items[index].SubItems[4].Text;
                _search_by_tag_attribute_value = listView2.Items[index].SubItems[5].Text;
                _type_get_value_from = listView2.Items[index].SubItems[6].Text;
                _get_value_from_attribute_name = listView2.Items[index].SubItems[7].Text;

                Form2 form = new Form2();
                form.Text = "Редактировать запись";
                form.parentForm = this;
                form.index = index;
                form.textBox1.Text = _description;
                form.textBox2.Text = _search_by_tag_name;
                form.textBox3.Text = _search_by_tag_id;
                form.textBox4.Text = _search_by_tag_attribute;
                form.textBox5.Text = _search_by_tag_attribute_value;
                form.comboBox1.Text = _type_get_value_from;
                form.textBox6.Text = _get_value_from_attribute_name;

                form.Show();
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (listView2.FocusedItem != null)
            {
                if (MessageBox.Show("Вы точно хотите удалить запись?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int index = listView2.FocusedItem.Index;
                    listView2.Items[index].Remove();
                    updateConfig();
                }
            }
        }

        private void toolStripComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar.GetHashCode().ToString() == "851981")
                {
                    tabFindValue(toolStripComboBox1);
                }
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            try
            {
                tabFindValue(toolStripComboBox1);
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                findText(toolStripComboBox2);
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void toolStripComboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar.GetHashCode().ToString() == "851981")
                {
                    findText(toolStripComboBox2);
                }
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    reportRichTextBox.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                    if(File.Exists(saveFileDialog1.FileName)) addConsoleMessage("Отчет успешно сохранён в файл " + saveFileDialog1.FileName);
                    else addConsoleMessage("Ошибка: Отчет не удалось сохранить в файл " + saveFileDialog1.FileName);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                addConsoleMessage("Ошибка: " + error.Message);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (thread.ThreadState.ToString() == "Running")
            {
                MessageBox.Show("Процесс занят! Дождитесь завершения или прекратите текущий процесс вручную.");
                return;
            }
            if(richTextBoxListLinks.Lines.Length <= 0)
            {
                MessageBox.Show("Список ссылок пустой, невозможно запустить сканирование");
                return;
            }

            steps = richTextBoxListLinks.Lines.Length;
            step = 0;
            percent = 0;

            toolStripStatusLabelProcessPercent.Text = "...";
            toolStripProgressBar1.Value = step;
            toolStripProgressBar1.Maximum = steps;
            toolStripStatusLabelProcessPercent.Text = Convert.ToString(percent) + "%";
            listView1.Items.Clear();
            reportRichTextBox.Text = "";
            reportFailedRichTextBox.Text = "";

            webBrowser1.ScriptErrorsSuppressed = true;
            
            thread = new Thread(runScanner);
            thread.Start();
        }

        string _description;                      // краткое описание
        string _search_by_tag_name;               // значение поиска - имя тега
        string _search_by_tag_id;                 // значение поиска - идентификатор
        string _search_by_tag_attribute;          // значение поиска - имя аттрибута
        string _search_by_tag_attribute_value;    // значение поиска - значение в аттрибуте
        string _type_get_value_from;              // получить значение из
        string _get_value_from_attribute_name;    // имя аттрибута
        bool failed;                              // статус
        int amountItems;                          // количество строк
        ArrayList values;                         // список значений

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete) return;
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath) return;

            thread.Abort();  // можно использовать задержку Thread.Sleep(1000);

            webBrowser1.Update();
            HtmlDOM.document = webBrowser1.Document;

            failed = false;
            amountItems = listView2.Items.Count;
            for (int i = 0; i < amountItems; i++)
            {
                _description = listView2.Items[i].SubItems[1].Text;
                _search_by_tag_name = listView2.Items[i].SubItems[2].Text;
                _search_by_tag_id = listView2.Items[i].SubItems[3].Text;
                _search_by_tag_attribute = listView2.Items[i].SubItems[4].Text;
                _search_by_tag_attribute_value = listView2.Items[i].SubItems[5].Text;
                _type_get_value_from = listView2.Items[i].SubItems[6].Text;
                _get_value_from_attribute_name = listView2.Items[i].SubItems[7].Text;

                values = new ArrayList();
                values = HtmlDOM.getValues(_search_by_tag_name, _search_by_tag_id, _search_by_tag_attribute,
                    _search_by_tag_attribute_value, _type_get_value_from, _get_value_from_attribute_name);
                
                if(values.Count > 0)
                {
                    foreach (string value in values)
                    {
                        //addReportMessage(_description + " | Тег: " + _search_by_tag_name + " " + _search_by_tag_id + " | Аттрибут: " + _search_by_tag_attribute + " " + _search_by_tag_attribute_value + " | Тип: " + _type_get_value_from + " " + _get_value_from_attribute_name + " | Значение: " + value);
                        addReportMessage(_description + ": " + value);
                        if (value != "" && value != null) addItemInTableReport(BLUE, "", _description, value);
                        else
                        {
                            failed = true;
                            addItemInTableReport(YELLOW, "", _description, "");
                        }
                    }
                }
                else
                {
                    failed = true;
                    addReportMessage(_description + ":");
                    addItemInTableReport(YELLOW, "", _description, "");
                }
            }
            addReportMessage("");
            if (failed == true) addReportMessageFailed(currentURL);

            toolStripProgressBar1.Value = step;
            percent = (int)(((double)toolStripProgressBar1.Value / (double)toolStripProgressBar1.Maximum) * 100);
            addConsoleMessage("Просканировано " + step.ToString() + " из " + steps.ToString() + " страниц " + currentURL);
            toolStripStatusLabelProcessPercent.Text = Convert.ToString(percent) + "%";
            
            if (step == steps)
            {
                addConsoleMessage("Сканирование страниц - завершено");
                MessageBox.Show("Сканирование страниц - завершено");
            }
            else
            {
                thread = new Thread(runScanner);
                thread.Start();
            }
            
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxListLinks.Copy();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxListLinks.Paste();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxListLinks.Clear();
        }

        private void выделитьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxListLinks.SelectAll();
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxListLinks.Clear();
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxListLinks.Undo();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxListLinks.Cut();
        }

        private void очиститьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            consoleRichTextBox.Clear();
        }

        
        private void очиститьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            reportRichTextBox.Clear();
        }

        private void открытьSitemapФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        
        private void сохранитьОтчетКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    reportRichTextBox.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                    if (File.Exists(saveFileDialog1.FileName)) addConsoleMessage("Отчет успешно сохранён в файл " + saveFileDialog1.FileName);
                    else addConsoleMessage("Ошибка: Отчет не удалось сохранить в файл " + saveFileDialog1.FileName);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                addConsoleMessage("Ошибка: " + error.Message);
            }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void загрузитьСсылкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (thread.ThreadState.ToString() == "Running")
            {
                MessageBox.Show("Процесс занят! Дождитесь завершения или прекратите текущий процесс вручную.");
                return;
            }
            richTextBoxListLinks.Clear();
            thread = new Thread(loadSitemap);
            thread.Start();
        }

        
        private void запуститьСканерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (thread.ThreadState.ToString() == "Running")
            {
                MessageBox.Show("Процесс занят! Дождитесь завершения или прекратите текущий процесс вручную.");
                return;
            }
            if (richTextBoxListLinks.Lines.Length <= 0)
            {
                MessageBox.Show("Список ссылок пустой, невозможно запустить сканирование");
                return;
            }

            steps = richTextBoxListLinks.Lines.Length;
            step = 0;
            percent = 0;

            toolStripStatusLabelProcessPercent.Text = "...";
            toolStripProgressBar1.Value = step;
            toolStripProgressBar1.Maximum = steps;
            toolStripStatusLabelProcessPercent.Text = Convert.ToString(percent) + "%";
            listView1.Items.Clear();
            reportRichTextBox.Text = "";

            webBrowser1.ScriptErrorsSuppressed = true;

            thread = new Thread(runScanner);
            thread.Start();
        }

        private void остановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopProcess();
        }

        
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 about = new Form3();
            about.ShowDialog();
        }

        private void richTextBoxListLinks_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@e.LinkText);
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void consoleRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@e.LinkText);
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void reportRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@e.LinkText);
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void openSitemapFile()
        {
            if (thread.ThreadState.ToString() == "Running")
            {
                MessageBox.Show("Загрузка ссылок в процессе, пожалуйста подождите.", "Сообщение");
                addConsoleMessage("Загрузка ссылок в процессе, пожалуйста подождите.");
                return;
            }

            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBoxListLinks.Clear();
                toolStripTextBoxPath.Text = openFileDialog1.FileName;
                thread = new Thread(loadSitemap);
                thread.Start();
                panelMessageLoadLinks.Visible = true;
            }
        }

        private void loadSitemapUrl()
        {
            if (thread.ThreadState.ToString() == "Running")
            {
                MessageBox.Show("Загрузка ссылок в процессе, пожалуйста подождите.", "Сообщение");
                addConsoleMessage("Загрузка ссылок в процессе, пожалуйста подождите.");
                return;
            }

            FormInputBox inputBox = new FormInputBox();
            inputBox.FormClosed += InputBox_FormClosed;
            inputBox.Parent = this;
            inputBox.ShowDialog();
        }

        private void InputBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (toolStripTextBoxPath.Text != "")
            {
                richTextBoxListLinks.Clear();
                thread = new Thread(loadSitemap);
                thread.Start();
                panelMessageLoadLinks.Visible = true;
            }
            else
            {
                MessageBox.Show("Вы не ввели URL ссылку к карте сайта.", "Сообщение");
                addConsoleMessage("Вы не ввели URL ссылку к карте сайта.");
            }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            loadSitemapUrl();
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            openSitemapFile();
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            Form3 about = new Form3();
            about.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            reportFailedRichTextBox.Clear();
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    reportFailedRichTextBox.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                    if (File.Exists(saveFileDialog1.FileName)) addConsoleMessage("Отчет успешно сохранён в файл " + saveFileDialog1.FileName);
                    else addConsoleMessage("Ошибка: Отчет не удалось сохранить в файл " + saveFileDialog1.FileName);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                addConsoleMessage("Ошибка: " + error.Message);
            }
        }

        private void открытьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormResultItem item = new FormResultItem();
                item.PageRichTextBox.Text = listView1.SelectedItems[0].SubItems[1].Text;
                item.ObjectRichTextBox.Text = listView1.SelectedItems[0].SubItems[2].Text;
                item.ValueRichTextBox.Text = listView1.SelectedItems[0].SubItems[3].Text;
                item.Show();
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void reportFailedRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@e.LinkText);
            }
            catch (Exception ex)
            {
                addConsoleMessage("Ошибка: " + ex.Message);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) addConsoleMessage("Способ отображения страниц - только HTML (по умолчанию)");
            else addConsoleMessage("Способ отображения страниц - полностью.");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}
