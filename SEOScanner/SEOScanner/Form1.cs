using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        /* Загрузка ссылок из карты сайта */
        private void loadSitemap()
        {
            try
            {
                string sitemapPath = toolStripComboBoxPath.Text;
                ArrayList targets;
                ArrayList sitemaps;
                sitemaps = new ArrayList();
                targets = new ArrayList();

                /* собираю все sitemap */
                sitemaps.Add(sitemapPath);
                for (int i = 0; i < sitemaps.Count; i++)
                {
                    string xmlLink = sitemaps[i].ToString();
                    addConsoleMessage("Чтение данных из сайтмап: " + xmlLink);
                    if (xmlLink.Contains(".xml") == true)
                    {
                        ArrayList listSitemaps;
                        if (checkBoxUserAgent.Checked == false) listSitemaps = Sitemap.readXML(xmlLink, textBoxUserAgent.Text);
                        else listSitemaps = Sitemap.readXML(xmlLink, "");

                        foreach (string urlSitemap in listSitemaps)
                        {
                            if (urlSitemap.Contains(".xml") == true)
                            {
                                sitemaps.Add(urlSitemap);
                            }
                            else
                            {
                                targets.Add(urlSitemap);
                            }
                        }
                    }
                }

                addConsoleMessage("Было прочитано: " + sitemaps.Count.ToString() + " xml файлов (sitemap)");
                addConsoleMessage("Было получено " + targets.Count.ToString() + " ссылок");

                int index = 1;
                int amount = targets.Count;
                foreach (string target in targets)
                {
                    richTextBoxListLinks.Text = target + Environment.NewLine + richTextBoxListLinks.Text;
                    richTextBoxListLinks.Update();
                    label4.Text = "Загружено " + index.ToString() + " / " + amount.ToString() + " ссылок";
                    index++;
                }

                addConsoleMessage("Выгрузка ссылок из sitemap - завершена");
            }
            catch (Exception error)
            {
                addConsoleMessage("Сообщение: " + error.Message);
            }
            finally
            {
                thread.Abort();
                addConsoleMessage("Процесс завершен");
                MessageBox.Show("Процесс завершен!");
            }

            thread.Abort();
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

            if(radioButton1.Checked) webBrowser1.DocumentText = page;
            else webBrowser1.Navigate(currentURL); // без user agent !!!!!

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
            }
            else
            {
                textBoxUserAgent.ReadOnly = false;
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                toolStripComboBoxPath.Text = openFileDialog1.FileName;
            }
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

        private void toolStripButton8_Click(object sender, EventArgs e)
        {

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

            webBrowser1.ScriptErrorsSuppressed = true;
            
            thread = new Thread(runScanner);
            thread.Start();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete) return;
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath) return;

            thread.Abort();  // можно использовать задержку Thread.Sleep(1000);

            webBrowser1.Update();
            HtmlDOM.document = webBrowser1.Document;

            string _description;                      // краткое описание
            string _search_by_tag_name;               // значение поиска - имя тега
            string _search_by_tag_id;                 // значение поиска - идентификатор
            string _search_by_tag_attribute;          // значение поиска - имя аттрибута
            string _search_by_tag_attribute_value;    // значение поиска - значение в аттрибуте
            string _type_get_value_from;              // получить значение из
            string _get_value_from_attribute_name;    // имя аттрибута

            int amountItems = listView2.Items.Count;
            for (int i = 0; i < amountItems; i++)
            {
                _description = listView2.Items[i].SubItems[1].Text;
                _search_by_tag_name = listView2.Items[i].SubItems[2].Text;
                _search_by_tag_id = listView2.Items[i].SubItems[3].Text;
                _search_by_tag_attribute = listView2.Items[i].SubItems[4].Text;
                _search_by_tag_attribute_value = listView2.Items[i].SubItems[5].Text;
                _type_get_value_from = listView2.Items[i].SubItems[6].Text;
                _get_value_from_attribute_name = listView2.Items[i].SubItems[7].Text;

                ArrayList values = new ArrayList();
                values = HtmlDOM.getValues(_search_by_tag_name, _search_by_tag_id, _search_by_tag_attribute,
                    _search_by_tag_attribute_value, _type_get_value_from, _get_value_from_attribute_name);
                
                if(values.Count > 0)
                {
                    foreach (string value in values)
                    {
                        //addReportMessage(_description + " | Тег: " + _search_by_tag_name + " " + _search_by_tag_id + " | Аттрибут: " + _search_by_tag_attribute + " " + _search_by_tag_attribute_value + " | Тип: " + _type_get_value_from + " " + _get_value_from_attribute_name + " | Значение: " + value);
                        addReportMessage(_description + ": " + value);
                        if(value != "" && value != null) addItemInTableReport(BLUE, "", _description, value);
                        else addItemInTableReport(YELLOW, "", _description, "");
                    }
                }
                else
                {
                    addReportMessage(_description + ":");
                    addItemInTableReport(YELLOW, "", _description, "");
                }
            }
            addReportMessage("");


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
    }
}
