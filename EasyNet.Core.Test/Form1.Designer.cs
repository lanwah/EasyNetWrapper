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
            this.closeLabel1 = new EasyNet.Controls.CloseLabel();
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
            // closeLabel1
            // 
            this.closeLabel1.AutoSize = true;
            this.closeLabel1.BackColor = System.Drawing.Color.LightGray;
            this.closeLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.closeLabel1.CloseWidth = 22;
            this.closeLabel1.Location = new System.Drawing.Point(593, 54);
            this.closeLabel1.Name = "closeLabel1";
            this.closeLabel1.Size = new System.Drawing.Size(119, 24);
            this.closeLabel1.TabIndex = 1;
            this.closeLabel1.Text = "closeLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.closeLabel1);
            this.Controls.Add(this.autoCompleteIMEControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.AutoCompleteIMEControl autoCompleteIMEControl1;
        private Controls.CloseLabel closeLabel1;
    }
}