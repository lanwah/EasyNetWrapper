using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：AutoCompleteEnum
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:23:01
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
    internal enum CompareResult
    {
        /// <summary>
        /// Item do not appears
        /// </summary>
        Hidden,
        /// <summary>
        /// Item appears
        /// </summary>
        Visible,
        /// <summary>
        /// Item appears and will selected
        /// </summary>
        VisibleAndSelected
    }
}
