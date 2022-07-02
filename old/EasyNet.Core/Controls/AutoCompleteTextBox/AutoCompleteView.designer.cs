
namespace EasyNet.Controls
{
    partial class AutoCompleteView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxInput = new EasyNet.Controls.AutoCompleteTextBox();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.lblSplitor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.autoCompleteList = new EasyNet.Controls.AutoCompleteList();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlTop.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.label3);
            this.pnlTop.Controls.Add(this.tbxInput);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.pnlTop.Size = new System.Drawing.Size(220, 25);
            this.pnlTop.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(217, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1, 25);
            this.label3.TabIndex = 4;
            // 
            // tbxInput
            // 
            this.tbxInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxInput.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbxInput.Location = new System.Drawing.Point(0, 0);
            this.tbxInput.Name = "tbxInput";
            this.tbxInput.Size = new System.Drawing.Size(218, 22);
            this.tbxInput.TabIndex = 0;
            this.tbxInput.DoProcessDialogKey += new EasyNet.Controls.DialogKeyProcessor(this.tbxInput_DoProcessDialogKey);
            this.tbxInput.TextChanged += new System.EventHandler(this.tbxInput_TextChanged);
            // 
            // pnlCenter
            // 
            this.pnlCenter.Controls.Add(this.lblSplitor);
            this.pnlCenter.Controls.Add(this.label2);
            this.pnlCenter.Controls.Add(this.autoCompleteList);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(0, 25);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.pnlCenter.Size = new System.Drawing.Size(220, 142);
            this.pnlCenter.TabIndex = 3;
            // 
            // lblSplitor
            // 
            this.lblSplitor.BackColor = System.Drawing.Color.DodgerBlue;
            this.lblSplitor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSplitor.Location = new System.Drawing.Point(0, 0);
            this.lblSplitor.Margin = new System.Windows.Forms.Padding(0);
            this.lblSplitor.Name = "lblSplitor";
            this.lblSplitor.Size = new System.Drawing.Size(217, 2);
            this.lblSplitor.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(217, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1, 142);
            this.label2.TabIndex = 3;
            // 
            // autoCompleteList
            // 
            this.autoCompleteList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoCompleteList.BackColor = System.Drawing.Color.White;
            this.autoCompleteList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.autoCompleteList.ImageList = null;
            this.autoCompleteList.ItemHeight = 16;
            this.autoCompleteList.Location = new System.Drawing.Point(1, 2);
            this.autoCompleteList.Margin = new System.Windows.Forms.Padding(0);
            this.autoCompleteList.MaximumSize = new System.Drawing.Size(220, 180);
            this.autoCompleteList.Name = "autoCompleteList";
            this.autoCompleteList.SelectedItemIndex = -1;
            this.autoCompleteList.Size = new System.Drawing.Size(217, 84);
            this.autoCompleteList.TabIndex = 0;
            this.autoCompleteList.VisibleItems = null;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 167);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(220, 31);
            this.pnlBottom.TabIndex = 4;
            this.pnlBottom.Visible = false;
            // 
            // AutoCompleteView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AutoCompleteView";
            this.Size = new System.Drawing.Size(220, 198);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlCenter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel pnlBottom;
        private AutoCompleteTextBox tbxInput;
        private AutoCompleteList autoCompleteList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSplitor;
    }
}
