namespace EasyNet.Controls
{
    partial class AutoCompleteIMEControl
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
            this.autoCompleteDataItem = new EasyNet.Controls.AutoCompleteDataItem();
            this.SuspendLayout();
            // 
            // autoCompleteDataItem
            // 
            this.autoCompleteDataItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoCompleteDataItem.Location = new System.Drawing.Point(0, 0);
            this.autoCompleteDataItem.Name = "autoCompleteDataItem";
            this.autoCompleteDataItem.Size = new System.Drawing.Size(277, 30);
            this.autoCompleteDataItem.TabIndex = 2;
            // 
            // AutoCompleteIMEControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoCompleteDataItem);
            this.Name = "AutoCompleteIMEControl";
            this.Size = new System.Drawing.Size(277, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoCompleteDataItem autoCompleteDataItem;
    }
}
