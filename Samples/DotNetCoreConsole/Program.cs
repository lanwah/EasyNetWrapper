using System;

namespace DotNetCoreConsole
{
    /// <summary>
    /// 解决 dotnet core 应用进程间引用找不到 runtimeconfig 依赖文件:https://cloud.tencent.com/developer/article/1693220
    /// 深入理解.NET Core的基元: deps.json, runtimeconfig.json, dll文件:https://www.cnblogs.com/lwqlun/p/9704702.html
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
