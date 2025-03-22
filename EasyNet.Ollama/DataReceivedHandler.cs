using EasyNet.Ollama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Ollama
{
    /// <summary>
    /// 数据接收委托
    /// </summary>
    /// <param name="args"></param>
    public delegate void DataReceivedHandler(ChatReceivedArgs args);
}
