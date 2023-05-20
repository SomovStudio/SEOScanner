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
            this.label1 = new System.Windows.Forms.Label();
            this.PageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ObjectRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ValueRichTextBox = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(581, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Страница:";
            // 
            // PageRichTextBox
            // 
            this.PageRichTextBox.BackColor = System.Drawing.Color.White;
            this.PageRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PageRichTextBox.Location = new System.Drawing.Point(0, 13);
            this.PageRichTextBox.Name = "PageRichTextBox";
            this.PageRichTextBox.ReadOnly = true;
            this.PageRichTextBox.Size = new System.Drawing.Size(581, 96);
            this.PageRichTextBox.TabIndex = 1;
            this.PageRichTextBox.Text = "";
            this.PageRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.pageRichTextBox_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PageRichTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(581, 109);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 109);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ObjectRichTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ValueRichTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(581, 267);
            this.splitContainer1.SplitterDistance = 123;
            this.splitContainer1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(581, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Объект:";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(581, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Значение:";
            // 
            // ObjectRichTextBox
            // 
            this.ObjectRichTextBox.BackColor = System.Drawing.Color.White;
            this.ObjectRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ObjectRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectRichTextBox.Location = new System.Drawing.Point(0, 13);
            this.ObjectRichTextBox.Name = "ObjectRichTextBox";
            this.ObjectRichTextBox.ReadOnly = true;
            this.ObjectRichTextBox.Size = new System.Drawing.Size(581, 110);
            this.ObjectRichTextBox.TabIndex = 2;
            this.ObjectRichTextBox.Text = "";
            // 
            // ValueRichTextBox
            // 
            this.ValueRichTextBox.BackColor = System.Drawing.Color.White;
            this.ValueRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ValueRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ValueRichTextBox.Location = new System.Drawing.Point(0, 13);
            this.ValueRichTextBox.Name = "ValueRichTextBox";
            this.ValueRichTextBox.ReadOnly = true;
            this.ValueRichTextBox.Size = new System.Drawing.Size(581, 127);
            this.ValueRichTextBox.TabIndex = 2;
            this.ValueRichTextBox.Text = "";
            // 
            // FormResultItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 376);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormResultItem";
            this.Text = "Информация из строки в таблице результатов";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RichTextBox PageRichTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.RichTextBox ObjectRichTextBox;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RichTextBox ValueRichTextBox;
        private System.Windows.Forms.Label label3;
    }
}