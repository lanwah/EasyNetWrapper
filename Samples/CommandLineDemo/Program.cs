using CommandLine;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineDemo
{
    /// <summary>
    /// commandlineparser/commandline：https://github.com/commandlineparser/commandline
    /// C#/.NET 使用 CommandLineParser 来标准化地解析命令行：https://www.cnblogs.com/walterlv/p/10236378.html
    /// System.CommandLine:https://github.com/dotnet/command-line-api
    /// 教程：System.CommandLine 入门:https://learn.microsoft.com/zh-cn/dotnet/standard/commandline/get-started-tutorial
    /// 
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CommandLineDemo");
            Console.WriteLine($"OSVersion = {Environment.OSVersion}");
            Console.WriteLine($"Version = {Environment.Version}");
            Console.WriteLine($"Is64BitOperatingSystem = {Environment.Is64BitOperatingSystem}");
            Console.WriteLine($"Is64BitProcess = {Environment.Is64BitProcess}");

            //args=new string[] {"-n","John","-a","30"};
            //var arg = "-n xulh -a 30 -v true";
            //args = arg.Split();
            var line = string.Empty;

            Console.WriteLine("请输入命令行参数，输入exit退出程序：");
            while ((line = Console.ReadLine()) != "exit")
            {
                args = line.Split();
                //// 方式一：
                //Parser.Default.ParseArguments<Options>(args)
                //   .WithParsed(RunOptions)
                //   .WithNotParsed(HandleParseError);

                //// 方式二：
                //var result = Parser.Default.ParseArguments<AddOptions, CommitOptions, CloneOptions>(args)
                //.MapResult(
                //(AddOptions opts) => RunAddAndReturnExitCode(opts),
                //(CommitOptions opts) => RunCommitAndReturnExitCode(opts),
                //(CloneOptions opts) => RunCloneAndReturnExitCode(opts),
                //errs => -1);
                //Console.WriteLine($"result = {result}");

                //// 方式三：
                //Parser.Default.ParseArguments<EnumerableOptions>(args)
                //.WithParsed(opts =>
                // {
                //     Console.WriteLine("Success");
                //     var props = opts.Props;
                //     //foreach (var prop in props)
                //     Console.WriteLine("props= {0}", string.Join(",", props));
                // })
                //.WithNotParsed(HandleParseError);



                //// System.CommandLine
                //var fileOption = new System.CommandLine.Option<FileInfo>(name: "--file", description: "The file to read and display on the console.");
                //fileOption.AddAlias("-f");
                //var rootCommand = new System.CommandLine.RootCommand("Sample app for System.CommandLine");
                //rootCommand.AddOption(fileOption);
                //rootCommand.SetHandler((file) =>
                //{
                //    Console.WriteLine($"File: {file.FullName}");
                //    //ReadFile(file);
                //}, fileOption);
                //// 调用
                //rootCommand.Invoke(args);

                // 多参数
                var option1 = new System.CommandLine.Option<string>("--name", "The name of the person to greet.");
                option1.IsRequired = true;
                option1.AddAlias("-n");
                var option2 = new System.CommandLine.Option<int>("--age", "The age of the person to greet.");
                option2.AddAlias("-a");
                var option3 = new System.CommandLine.Option<bool>("--verbose", "Prints all messages to standard output.");
                option3.AddAlias("-v");
                var rootCommand = new System.CommandLine.RootCommand("Sample app for System.CommandLine");
                rootCommand.Add(option1);
                rootCommand.Add(option2);
                rootCommand.Add(option3);
                rootCommand.SetHandler((name, age, verbose) =>
                {
                    Console.WriteLine($"Hello, {name}, verbose={verbose}, age={age}!");
                }, option1, option2, option3);
                rootCommand.Invoke(args);   


                Console.WriteLine("请输入命令行参数，输入exit退出程序：");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
        static void ReadFile(FileInfo file)
        {
            File.ReadLines(file.FullName).ToList().ForEach(line => Console.WriteLine(line));
        }

        private static int RunCloneAndReturnExitCode(CloneOptions opts)
        {
            Console.WriteLine("RunCloneAndReturnExitCode");
            return 0;
        }

        private static int RunCommitAndReturnExitCode(CommitOptions opts)
        {
            Console.WriteLine("RunCommitAndReturnExitCode");
            return 2;
        }

        private static int RunAddAndReturnExitCode(AddOptions opts)
        {
            Console.WriteLine("RunAddAndReturnExitCode");
            return 1;
        }

        private static void HandleParseError(IEnumerable<Error> enumerable)
        {
            Console.WriteLine($"Command line arguments error!!!{Environment.NewLine}");
        }

        static void RunOptions(Options options)
        {
            //if (options.Verbose)
            //{
            //    Console.WriteLine($"Hello, {options.Name}!");
            //}
            //else
            //{
            //    Console.WriteLine($"Hello, {options.Name}!");
            //}


            Console.WriteLine($"Hello, {options.Name}, verbose={options.Verbose}, age={options.Age}!{Environment.NewLine}");
        }

        public class Options
        {
            [Option('v', "verbose", Required = false, HelpText = "Prints all messages to standard output.")]
            public bool Verbose { get; set; }

            [Option('n', "name", Required = true, HelpText = "The name of the person to greet.")]
            public string Name { get; set; }

            [Option('a', "age", Required = false, HelpText = "The age of the person to greet.")]
            public int Age { get; set; }
        }

        [Verb("add", HelpText = "Add file contents to the index.")]
        class AddOptions
        {
            //normal options here
        }
        [Verb("commit", HelpText = "Record changes to the repository.")]
        class CommitOptions
        {
            //commit options here
        }
        [Verb("clone", HelpText = "Clone a repository into a new directory.")]
        class CloneOptions
        {
            //clone options here
        }

        class EnumerableOptions
        {
            [Value(0)]
            public IEnumerable<string> Props
            {
                get;
                set;
            }
        }
    }
}
