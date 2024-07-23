using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Extensions;
using EasyNet.Core;

namespace Comparer
{
    /// <summary>
    /// 图片类型识别
    /// </summary>
    [MemoryDiagnoser]
    public class ImageType
    {
        public const string FILEPATH = @"C:\Users\Administrator\Desktop\A-2.png";
        private readonly string FilePath = FILEPATH;
        public byte[] ImageBuffer;
        public string ImageBase64;
        public Stream ImageSteam;
        public Image Image;

        public ImageType()
        {
            this.ImageBuffer = File.ReadAllBytes(this.FilePath);
            this.ImageBase64 = Convert.ToBase64String(this.ImageBuffer);
            this.ImageSteam = new MemoryStream(this.ImageBuffer);
            this.Image = System.Drawing.Image.FromFile(this.FilePath);
        }

        public static void Run()
        {
            var filePath = FILEPATH;
            _ = ImageFormatter.Png.FromFile(filePath).IsMatch;

            var image = Image.FromFile(filePath);
            _ = ImageFormatter.Png.FromImage(image).IsMatch;

            var buffer = File.ReadAllBytes(filePath);
            var memoryStream = new MemoryStream(buffer);
            _ = ImageFormatter.Png.FromStream(memoryStream).IsMatch;

            var base64 = Convert.ToBase64String(buffer);
            _ = ImageFormatter.Png.FromBase64String(base64).IsMatch;
        }

        [Benchmark(Baseline = true)]
        public bool FromFile()
        {
            return ImageFormatter.Png.FromFile(this.FilePath).IsMatch;
        }
        [Benchmark]
        public bool FromBytes()
        {
            return ImageFormatter.Png.FromBytes(this.ImageBuffer).IsMatch;
        }
        [Benchmark]
        public bool FromBase64String()
        {
            return ImageFormatter.Png.FromBase64String(this.ImageBase64).IsMatch;
        }
        [Benchmark]
        public bool FromStream()
        {
            return ImageFormatter.Png.FromStream(this.ImageSteam).IsMatch;
        }
        [Benchmark]
        public bool FromImage()
        {
            return ImageFormatter.Png.FromImage(this.Image).IsMatch;
        }
    }
}
