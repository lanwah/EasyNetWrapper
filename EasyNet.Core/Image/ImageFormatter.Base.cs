#if NETFRAMEWORK
using System;
using System.Collections.Generic;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using EasyNet.Extensions;

//跨平台支持：https://learn.microsoft.com/zh-cn/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only

namespace EasyNet.Core
{

    /// <summary>
    /// 图片类型识别基类
    /// <see langword="C++中图片类型的识别与转换详解方法" href="http://www.zzvips.com/article/226289.html"/>
    /// <see langword="图片的文件头标识" href="https://zhidao.baidu.com/question/1827773615729430788.html"/>
    /// <see langword="各类文件的文件头标志" href="https://www.renrendoc.com/paper/223241037.html"/>
    /// 在能拿到对应数据的情况下，接口效率 FromBytes > FromImage > FromStream > FromFile > FromBase64String
    /// <benchmark>\Comparer\ImageType.cs</benchmark>
    /// </summary>
    public class ImageFormatter : IImageFormat, IDisposable
    {
        #region IImageFormat 接口实现
        private bool _isMatch = false;
        /// <summary>
        /// 图片类型是否匹配
        /// </summary>
        public bool IsMatch
        {
            get => this._isMatch;
            protected set => this._isMatch = value;
        }
        private List<byte> _headTypeData = new List<byte>();
        /// <summary>
        /// 文件头标识
        /// </summary>
        public List<byte> HeadTypeData
        {
            get => this._headTypeData;
            protected set => this._headTypeData = value;
        }
        private string _imageTypeName;
        /// <summary>
        /// 图片类型名称
        /// </summary>
        public string ImageTypeName
        {
            get => _imageTypeName;
            protected set => this._imageTypeName = value;
        }
        /// <summary>
        /// 图片内容
        /// </summary>
        protected byte[] ImageBuffer
        {
            get; set;
        }
        /// <summary>
        /// 图片对象
        /// </summary>
        public Image Image
        {
            get; set;
        }


        /// <summary>
        /// 从文件加载图片并识别图片类型
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IImageFormat FromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return this;
            }

            // 会释放图片资源
            var data = File.ReadAllBytes(filePath);
            if (data.IsNull())
            {
                return this;
            }

            return this.UpdateMatchFlag(data);
        }
        /// <summary>
        /// 从图片流加载图片并识别图片类型
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public IImageFormat FromStream(Stream stream)
        {
            // 外部流，不进行释放处理
            var buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            if (buffer.IsNull())
            {
                return this;
            }
            // 读取完数据后把当前位置重置为开始
            stream.Position = 0;

            return this.UpdateMatchFlag(buffer);
        }
        /// <summary>
        /// 从图片识别图片类型
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public IImageFormat FromImage(Image image)
        {
            if (image.IsNull())
            {
                return this;
            }

            return this.UpdateMatchFlag(image);
        }
        /// <summary>
        /// 从图片二进制数据中识别图片类型
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public IImageFormat FromBytes(byte[] bytes)
        {
            if (bytes.IsNull())
            {
                return this;
            }

            return this.UpdateMatchFlag(bytes);
        }
        /// <summary>
        /// 从图片的Base64字符串识别图片类型
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public IImageFormat FromBase64String(string base64String)
        {
            if (base64String.IsNullOrEmpty())
            {
                return this;
            }

            var buffer = Convert.FromBase64String(base64String);
            return this.UpdateMatchFlag(buffer);
        }

        /// <summary>
        /// 识别图片类型并更新<see cref="IsMatch"/>字段
        /// </summary>
        /// <param name="imageBuffer"></param>
        /// <returns></returns>
        protected virtual IImageFormat UpdateMatchFlag(byte[] imageBuffer)
        {
            this.IsMatch = false;
            if (imageBuffer.Length < this.HeadTypeData.Count)
            {
                return this;
            }

            this.ImageBuffer = imageBuffer;
            var imgHeadData = new byte[this.HeadTypeData.Count];
            Array.Copy(imageBuffer, imgHeadData, imgHeadData.Length);
            this.IsMatch = this.HeadTypeData.SequenceEqual(imgHeadData);
            return this;
        }
        /// <summary>
        /// 识别图片类型并更新<see cref="IsMatch"/>字段
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        protected virtual IImageFormat UpdateMatchFlag(System.Drawing.Image image)
        {
            this.IsMatch = false;

            this.Image = image;
            var imgType = GetImageFormat(image);
            this.IsMatch = (imgType == this.ImageTypeName);
            return this;
        }
        #endregion

        #region IDisposable 接口实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.Image.IsNotNull())
            {
                // 只是断开引用而不释放资源，因为图片为外部传入，外部可能需要继续使用
                this.Image = null;
            }

            if (this.ImageBuffer.IsNotNull())
            {
                this.ImageBuffer = null;
            }
        }
        #endregion

        /// <summary>
        /// Png图片格式
        /// </summary>
        public const string PNG = "Png";
        /// <summary>
        /// Bmp图片格式
        /// </summary>
        public const string BMP = "Bmp";
        /// <summary>
        /// Gif图片格式
        /// </summary>
        public const string GIF = "Gif";
        /// <summary>
        /// Jpeg图片格式
        /// </summary>
        public const string JPEG = "Jpeg";
        /// <summary>
        /// Tiff图片格式
        /// </summary>
        public const string TIFF = "Tiff";
        /// <summary>
        /// Icon图片格式
        /// </summary>
        public const string ICON = "Icon";
        /// <summary>
        /// 获取图像的文件格式
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string GetImageFormat(System.Drawing.Image image)
        {
            // 要用 Equals 不能用 ==
            if (image.RawFormat.Equals(ImageFormat.MemoryBmp))
            {
                return "MemoryBmp";
            }
            else if (image.RawFormat.Equals(ImageFormat.Bmp))
            {
                return BMP;
            }
            else if (image.RawFormat.Equals(ImageFormat.Emf))
            {
                return "Emf";
            }
            else if (image.RawFormat.Equals(ImageFormat.Wmf))
            {
                return "Wmf";
            }
            else if (image.RawFormat.Equals(ImageFormat.Gif))
            {
                return GIF;
            }
            else if (image.RawFormat.Equals(ImageFormat.Jpeg))
            {
                return JPEG;
            }
            else if (image.RawFormat.Equals(ImageFormat.Png))
            {
                return PNG;
            }
            else if (image.RawFormat.Equals(ImageFormat.Tiff))
            {
                return TIFF;
            }
            else if (image.RawFormat.Equals(ImageFormat.Exif))
            {
                return "Exif";
            }
            else if (image.RawFormat.Equals(ImageFormat.Icon))
            {
                return ICON;
            }

            var guid = image.RawFormat.Guid;
            return "[ImageFormat: " + guid.ToString() + "]";
        }


        private static readonly IImageFormat _png = new PngImage();
        private static readonly IImageFormat _bmp = new BmpImage();
        private static readonly IImageFormat _gif = new GifImage();
        private static readonly IImageFormat _jpeg = new JpgImage();
        private static readonly IImageFormat _tiff = new TiffImage();
        private static readonly IImageFormat _icon = new IconImage();
        /// <summary>
        /// Png图片识别器
        /// </summary>
        public static IImageFormat Png => _png;
        /// <summary>
        /// Bmp图片识别器
        /// </summary>
        public static IImageFormat Bmp => _bmp;
        /// <summary>
        /// Gif图片识别器
        /// </summary>
        public static IImageFormat Gif => _gif;
        /// <summary>
        /// Jpeg图片识别器
        /// </summary>
        public static IImageFormat Jpeg => _jpeg;
        /// <summary>
        /// Tiff图片识别器
        /// </summary>
        public static IImageFormat Tiff => _tiff;
        /// <summary>
        /// Icon图片识别器
        /// </summary>
        public static IImageFormat Icon => _icon;
    }

    /// <summary>
    /// 图片类型接口
    /// </summary>
    public interface IImageFormat
    {
        /// <summary>
        /// 是否匹配
        /// </summary>
        bool IsMatch
        {
            get;
        }
        /// <summary>
        /// 文件头标识
        /// </summary>
        List<byte> HeadTypeData
        {
            get;
        }
        /// <summary>
        /// 图片类型名称
        /// </summary>
        string ImageTypeName
        {
            get;
        }


        /// <summary>
        /// 从文件加载图片并识别图片类型
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IImageFormat FromFile(string filePath);
        /// <summary>
        /// 从图片流加载图片并识别图片类型
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        IImageFormat FromStream(Stream stream);
        /// <summary>
        /// 从图片识别图片类型
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        IImageFormat FromImage(Image image);
        /// <summary>
        /// 从图片二进制数据中识别图片类型
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        IImageFormat FromBytes(byte[] bytes);
        /// <summary>
        /// 从图片的Base64字符串识别图片类型
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        IImageFormat FromBase64String(string base64String);
    }
}
#endif