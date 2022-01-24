﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                ArrayList configData = FileJSON.readConfigFile(FileJSON.UTF_8_BOM, configFile);
                if(configData.Count > 0)
                {
                    listView2.Items.Clear();
                    ListViewItem item;
                    ListViewItem.ListViewSubItem subitem;
                    foreach (string[] data in configData)
                    {
                        item = new ListViewItem();
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = data[0];
                        item.SubItems.Add(subitem);
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = "ByTag";
                        item.SubItems.Add(subitem);
                        subitem = new ListViewItem.ListViewSubItem();
                        subitem.Text = data[1];
                        item.SubItems.Add(subitem);
                        listView2.Items.Add(item);

                        addConsoleMessage("Имя: " + data[0] + " | Значение: " + data[1]);
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

        private void scanner()
        {
            try
            {
                int percent = 0;
                int amount = richTextBoxListLinks.Lines.Length;
                toolStripStatusLabelProcessPercent.Text = Convert.ToString(percent) + "%";
                toolStripProgressBar1.Maximum = amount;

                for (int i = 0; i < amount; i++)
                {
                    string url = richTextBoxListLinks.Lines[i];
                    string page;
                    if (checkBoxUserAgent.Checked == false) page = Sitemap.getPageHtmlDOM(url, textBoxUserAgent.Text);
                    else page = Sitemap.getPageHtmlDOM(url, "");

                    //webBrowser1.Navigate(url);
                    webBrowser1.DocumentText = page;
                    



                    toolStripProgressBar1.Value = i+1;
                    percent = (int)(((double)toolStripProgressBar1.Value / (double)toolStripProgressBar1.Maximum) * 100);
                    toolStripStatusLabelProcessPercent.Text = Convert.ToString(percent) + "%";
                    addConsoleMessage("Просканировано " + (i + 1).ToString() + " из " + amount.ToString() + " страниц " + url);
                }


                addConsoleMessage("Сканирование страниц - завершено");
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

            string page = "";
            try
            {
                if (checkBoxUserAgent.Checked == false) page = Sitemap.getPageHtmlDOM(currentURL, textBoxUserAgent.Text);
                else page = Sitemap.getPageHtmlDOM(currentURL, "");
            }
            catch (Exception error)
            {
                addConsoleMessage("Ошибка: " + error.Message);
                addConsoleMessage("Ошибка чтения странирцы " + currentURL);
            }

            if(radioButton1.Checked) webBrowser1.DocumentText = page;
            else webBrowser1.Navigate(currentURL);

            thread.Abort();
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

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (thread.ThreadState.ToString() == "Running")
            {
                MessageBox.Show("Процесс занят! Дождитесь завершения или прекратите текущий процесс вручную.");
                return;
            }

            steps = richTextBoxListLinks.Lines.Length;
            step = 0;
            percent = 0;

            toolStripStatusLabelProcessPercent.Text = "...";
            toolStripProgressBar1.Value = step;
            toolStripProgressBar1.Maximum = steps;
            toolStripStatusLabelProcessPercent.Text = Convert.ToString(percent) + "%";

            webBrowser1.ScriptErrorsSuppressed = true;
            
            thread = new Thread(runScanner);
            thread.Start();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete) return;
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath) return;

            thread.Abort();  // если что можно использовать задержку Thread.Sleep(1000);

            webBrowser1.Update();
            HtmlDocument document = webBrowser1.Document;
            //HtmlElement hElement = document.GetElementsByTagName("h1")[0];
            addConsoleMessage("Количество тэгов H2 на странице = " + document.GetElementsByTagName("h2").Count.ToString());

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
