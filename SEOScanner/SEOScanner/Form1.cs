using System;
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
        
        private void Form1_Load(object sender, EventArgs e)
        {
            thread = new Thread(loadSitemap);
        }

        private void addConsoleMessage(string message)
        {
            DateTime localDate = DateTime.Now;
            consoleRichTextBox.Text = "[" + localDate.ToString() + "]: " + message + Environment.NewLine + consoleRichTextBox.Text;
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
                int amount = richTextBoxListLinks.Lines.Length;
                for (int i = 0; i < amount; i++)
                {
                    string url = richTextBoxListLinks.Lines[i];
                    addConsoleMessage("Просканировано " + (i+1).ToString() + " из " + amount.ToString() + " страниц " + url);
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
        /* ------------------------------------------------------------------------------------------- */

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
            thread = new Thread(scanner);
            thread.Start();
        }
    }
}
