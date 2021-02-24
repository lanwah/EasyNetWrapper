namespace EasyNet.Core.Test
{
    partial class Form1
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
            this.autoCompleteIMEControl1 = new EasyNet.Controls.AutoCompleteIMEControl();
            this.SuspendLayout();
            // 
            // autoCompleteIMEControl1
            // 
            this.autoCompleteIMEControl1.Location = new System.Drawing.Point(29, 36);
            this.autoCompleteIMEControl1.Name = "autoCompleteIMEControl1";
            this.autoCompleteIMEControl1.SearchCallback = null;
            this.autoCompleteIMEControl1.SelectedItem = null;
            this.autoCompleteIMEControl1.Size = new System.Drawing.Size(277, 30);
            this.autoCompleteIMEControl1.SourceItems = null;
            this.autoCompleteIMEControl1.TabIndex = 0;
            this.autoCompleteIMEControl1.DataChanged += new System.EventHandler(this.autoCompleteIMEControl1_DataChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.autoCompleteIMEControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.AutoCompleteIMEControl autoCompleteIMEControl1;
    }
}