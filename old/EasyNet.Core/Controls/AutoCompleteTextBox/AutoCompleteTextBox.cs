using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：AutoCompleteTextBox
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:28:28
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Controls
{
    [ToolboxItem(false)]
    public class AutoCompleteTextBox : TextBox
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return ExecuteDialogKey(keyData) || base.ProcessDialogKey(keyData);
        }

        public bool ExecuteDialogKey(Keys keyData)
        {
            if (DoProcessDialogKey != null && DoProcessDialogKey(keyData))
            {
                return true;
            }

            return false;
        }

        public event DialogKeyProcessor DoProcessDialogKey;
    }
}
