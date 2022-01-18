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

        private void addConsoleMessage(string message)
        {
            consoleRichTextBox.Text = message + Environment.NewLine + consoleRichTextBox.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

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

        /* Загрузка ссылок из карты сайта */
        private void toolStripButton2_Click(object sender, EventArgs e)
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

                foreach (string target in targets)
                {
                    richTextBoxListLinks.Text = target + Environment.NewLine + richTextBoxListLinks.Text;
                }

            }
            catch(Exception error)
            {
                addConsoleMessage("Сообщение: " + error.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                toolStripComboBoxPath.Text = openFileDialog1.FileName;
            }
        }
    }
}
