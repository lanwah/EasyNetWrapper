namespace EasyNet.Demo
{
    partial class FormEasyNetWinForm
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
            this.closeLabel1 = new EasyNet.WinForm.Controls.CloseLabel();
            this.closeLabel2 = new EasyNet.WinForm.Controls.CloseLabel();
            this.closeLabel3 = new EasyNet.WinForm.Controls.CloseLabel();
            this.closeLabel4 = new EasyNet.WinForm.Controls.CloseLabel();
            this.closeLabel5 = new EasyNet.WinForm.Controls.CloseLabel();
            this.closeLabel6 = new EasyNet.WinForm.Controls.CloseLabel();
            this.historyTextBox1 = new EasyNet.WinForm.Controls.HistoryTextBox();
            this.historyTextBox2 = new EasyNet.WinForm.Controls.HistoryTextBox();
            this.SuspendLayout();
            // 
            // closeLabel1
            // 
            this.closeLabel1.BackColor = System.Drawing.Color.LightGray;
            this.closeLabel1.BorderColor = System.Drawing.Color.DarkGray;
            this.closeLabel1.Location = new System.Drawing.Point(33, 37);
            this.closeLabel1.Name = "closeLabel1";
            this.closeLabel1.Size = new System.Drawing.Size(105, 24);
            this.closeLabel1.TabIndex = 0;
            this.closeLabel1.Text = "closeLabel1";
            // 
            // closeLabel2
            // 
            this.closeLabel2.BackColor = System.Drawing.Color.LightGray;
            this.closeLabel2.BorderColor = System.Drawing.Color.DarkGray;
            this.closeLabel2.Location = new System.Drawing.Point(33, 77);
            this.closeLabel2.Name = "closeLabel2";
            this.closeLabel2.Size = new System.Drawing.Size(105, 24);
            this.closeLabel2.TabIndex = 1;
            this.closeLabel2.Text = "closeLabel2";
            // 
            // closeLabel3
            // 
            this.closeLabel3.BackColor = System.Drawing.Color.LightGray;
            this.closeLabel3.BorderColor = System.Drawing.Color.DarkGray;
            this.closeLabel3.Location = new System.Drawing.Point(33, 120);
            this.closeLabel3.Name = "closeLabel3";
            this.closeLabel3.Size = new System.Drawing.Size(105, 24);
            this.closeLabel3.TabIndex = 2;
            this.closeLabel3.Text = "closeLabel3";
            // 
            // closeLabel4
            // 
            this.closeLabel4.BackColor = System.Drawing.Color.LightGray;
            this.closeLabel4.BorderColor = System.Drawing.Color.DarkGray;
            this.closeLabel4.Location = new System.Drawing.Point(161, 37);
            this.closeLabel4.Name = "closeLabel4";
            this.closeLabel4.Size = new System.Drawing.Size(105, 24);
            this.closeLabel4.TabIndex = 3;
            this.closeLabel4.Text = "closeLabel4";
            // 
            // closeLabel5
            // 
            this.closeLabel5.BackColor = System.Drawing.Color.LightGray;
            this.closeLabel5.BorderColor = System.Drawing.Color.DarkGray;
            this.closeLabel5.Location = new System.Drawing.Point(161, 77);
            this.closeLabel5.Name = "closeLabel5";
            this.closeLabel5.Size = new System.Drawing.Size(105, 24);
            this.closeLabel5.TabIndex = 4;
            this.closeLabel5.Text = "closeLabel5";
            // 
            // closeLabel6
            // 
            this.closeLabel6.BackColor = System.Drawing.Color.LightGray;
            this.closeLabel6.BorderColor = System.Drawing.Color.DarkGray;
            this.closeLabel6.Location = new System.Drawing.Point(161, 120);
            this.closeLabel6.Name = "closeLabel6";
            this.closeLabel6.Size = new System.Drawing.Size(105, 24);
            this.closeLabel6.TabIndex = 5;
            this.closeLabel6.Text = "closeLabel6";
            // 
            // historyTextBox1
            // 
            this.historyTextBox1.IsCustomInput = true;
            this.historyTextBox1.IsFoundSelected = false;
            this.historyTextBox1.Location = new System.Drawing.Point(33, 179);
            this.historyTextBox1.Name = "historyTextBox1";
            this.historyTextBox1.SearchCallback = null;
            this.historyTextBox1.SelectedItem = null;
            this.historyTextBox1.Size = new System.Drawing.Size(233, 30);
            this.historyTextBox1.SourceItems = null;
            this.historyTextBox1.TabIndex = 6;
            this.historyTextBox1.DataChanged += new System.EventHandler(this.HistoryTextBox1_DataChanged);
            // 
            // historyTextBox2
            // 
            this.historyTextBox2.IsCustomInput = true;
            this.historyTextBox2.IsFoundSelected = true;
            this.historyTextBox2.Location = new System.Drawing.Point(33, 226);
            this.historyTextBox2.Name = "historyTextBox2";
            this.historyTextBox2.SearchCallback = null;
            this.historyTextBox2.SelectedItem = null;
            this.historyTextBox2.Size = new System.Drawing.Size(233, 30);
            this.historyTextBox2.SourceItems = null;
            this.historyTextBox2.TabIndex = 7;
            this.historyTextBox2.DataChanged += new System.EventHandler(this.HistoryTextBox2_DataChanged);
            // 
            // FormEasyNetWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 486);
            this.Controls.Add(this.historyTextBox2);
            this.Controls.Add(this.historyTextBox1);
            this.Controls.Add(this.closeLabel6);
            this.Controls.Add(this.closeLabel5);
            this.Controls.Add(this.closeLabel4);
            this.Controls.Add(this.closeLabel3);
            this.Controls.Add(this.closeLabel2);
            this.Controls.Add(this.closeLabel1);
            this.Name = "FormEasyNetWinForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EasyNet.WinForm 测试";
            this.Load += new System.EventHandler(this.FormEasyNetWinForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WinForm.Controls.CloseLabel closeLabel1;
        private WinForm.Controls.CloseLabel closeLabel2;
        private WinForm.Controls.CloseLabel closeLabel3;
        private WinForm.Controls.CloseLabel closeLabel4;
        private WinForm.Controls.CloseLabel closeLabel5;
        private WinForm.Controls.CloseLabel closeLabel6;
        private WinForm.Controls.HistoryTextBox historyTextBox1;
        private WinForm.Controls.HistoryTextBox historyTextBox2;
    }
}