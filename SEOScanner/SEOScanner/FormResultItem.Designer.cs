namespace SEOScanner
{
    partial class FormResultItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResultItem));
            this.PageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ObjectTextBox = new System.Windows.Forms.TextBox();
            this.ValueTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PageRichTextBox
            // 
            this.PageRichTextBox.BackColor = System.Drawing.Color.White;
            this.PageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PageRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PageRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.PageRichTextBox.Name = "PageRichTextBox";
            this.PageRichTextBox.ReadOnly = true;
            this.PageRichTextBox.Size = new System.Drawing.Size(571, 107);
            this.PageRichTextBox.TabIndex = 1;
            this.PageRichTextBox.Text = "";
            this.PageRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.pageRichTextBox_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.PageRichTextBox);
            this.panel1.Location = new System.Drawing.Point(4, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(573, 109);
            this.panel1.TabIndex = 2;
            // 
            // panel10
            // 
            this.panel10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Controls.Add(this.label5);
            this.panel10.Location = new System.Drawing.Point(4, 1);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(573, 18);
            this.panel10.TabIndex = 6;
            // 
            // panel11
            // 
            this.panel11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel11.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel11.Location = new System.Drawing.Point(0, 16);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(573, 2);
            this.panel11.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.RoyalBlue;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "Страница:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(4, 133);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(573, 240);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ObjectTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(565, 214);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Объект";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ValueTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(562, 214);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Значение";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ObjectTextBox
            // 
            this.ObjectTextBox.BackColor = System.Drawing.Color.White;
            this.ObjectTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectTextBox.Location = new System.Drawing.Point(3, 3);
            this.ObjectTextBox.Multiline = true;
            this.ObjectTextBox.Name = "ObjectTextBox";
            this.ObjectTextBox.ReadOnly = true;
            this.ObjectTextBox.Size = new System.Drawing.Size(559, 208);
            this.ObjectTextBox.TabIndex = 0;
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.BackColor = System.Drawing.Color.White;
            this.ValueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ValueTextBox.Location = new System.Drawing.Point(3, 3);
            this.ValueTextBox.Multiline = true;
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.ReadOnly = true;
            this.ValueTextBox.Size = new System.Drawing.Size(556, 208);
            this.ValueTextBox.TabIndex = 1;
            // 
            // FormResultItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 376);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormResultItem";
            this.Text = "Запись";
            this.panel1.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.RichTextBox PageRichTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.TextBox ObjectTextBox;
        public System.Windows.Forms.TextBox ValueTextBox;
    }
}