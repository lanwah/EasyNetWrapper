#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace EasyNet.Core
{
    /// <summary>
    /// Png图片识别器
    /// </summary>
    public class PngImage : ImageFormatter
    {
        /// <summary>
        /// PNG文件标识符
        /// </summary>
        public static byte[] PNG_IDENTIFIER = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        /// <summary>
        /// PNG文件头
        /// </summary>
        public static List<byte> PngHead = new List<byte>(PNG_IDENTIFIER);

        /// <summary>
        /// 构造函数
        /// <see langword="PNG文件格式详解" href="https://www.cnblogs.com/senior-engineer/p/9548347.html"/>
        /// </summary>
        public PngImage()
        {
            // 十进制数 十六进制数
            //   137       89
            //   80        50
            //   78        4e
            //   71        47
            //   13        0d
            //   10        0a
            //   26        1a
            //   10        0a
            this.HeadTypeData.Clear();
            this.HeadTypeData.AddRange(PNG_IDENTIFIER);
            this.ImageTypeName = PNG;
        }
    }
    /// <summary>
    /// Bmp图片识别器
    /// </summary>
    public class BmpImage : ImageFormatter
    {
        /// <summary>
        /// BMP文件标识符
        /// </summary>
        public static byte[] BMP_IDENTIFIER = { 0x42, 0x4D };
        /// <summary>
        /// BMP文件头
        /// </summary>
        public static List<byte> BmpHead = new List<byte>(BMP_IDENTIFIER);

        /// <summary>
        /// 构造函数
        /// </summary>
        public BmpImage()
        {
            // 十进制数 十六进制数
            //    66      42
            //    77      4D
            this.HeadTypeData.Clear();
            this.HeadTypeData.AddRange(BMP_IDENTIFIER);
            this.ImageTypeName = BMP;
        }
    }
    /// <summary>
    /// Jpg图片识别器
    /// </summary>
    public class JpgImage : ImageFormatter
    {
        /// <summary>
        /// JPG文件标识符
        /// </summary>
        public static byte[] JPG_IDENTIFIER = { 0xFF, 0xD8 };
        /// <summary>
        /// JPG文件头
        /// </summary>
        public static List<byte> JpgHead = new List<byte>(JPG_IDENTIFIER);

        /// <summary>
        /// 构造函数
        /// </summary>
        public JpgImage()
        {
            // 十进制数 十六进制数
            //    255      FF
            //    216      D8
            this.HeadTypeData.Clear();
            this.HeadTypeData.AddRange(JPG_IDENTIFIER);
            this.ImageTypeName = JPEG;
        }
    }
    /// <summary>
    /// Gif图片识别器
    /// </summary>
    public class GifImage : ImageFormatter
    {
        /// <summary>
        /// GIF文件标识符，47 49 46 38 39(37) 61 字符即：   GIF89(7)a
        /// </summary>
        public static byte[] GIF_IDENTIFIER = { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };
        /// <summary>
        /// GIF文件标识符2 
        /// </summary>
        public static byte[] GIF_IDENTIFIER2 = { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };
        /// <summary>
        /// GIF文件头
        /// </summary>
        public static List<byte> GifHead = new List<byte>(GIF_IDENTIFIER);
        /// <summary>
        /// GIF文件头2
        /// </summary>
        public static List<byte> GifHead2 = new List<byte>(GIF_IDENTIFIER2);

        /// <summary>
        /// 构造函数
        /// </summary>
        public GifImage()
        {
            // 十进制数 十六进制数
            //    71      47
            //    73      49
            //    70      46
            //    56      38
            //    57/55   39/37
            //    97      61
            this.HeadTypeData.Clear();
            this.HeadTypeData.AddRange(GIF_IDENTIFIER);
            this.ImageTypeName = GIF;
        }

        /// <summary>
        /// 更新图片匹配标识
        /// </summary>
        /// <param name="imageBuffer"></param>
        /// <returns></returns>
        protected override IImageFormat UpdateMatchFlag(byte[] imageBuffer)
        {
            this.IsMatch = false;
            if (imageBuffer.Length < this.HeadTypeData.Count)
            {
                return this;
            }

            this.ImageBuffer = imageBuffer;
            var imgHeadData = new byte[this.HeadTypeData.Count];
            imageBuffer.CopyTo(imgHeadData, 0);
            this.IsMatch = (this.HeadTypeData.SequenceEqual(imgHeadData) || GIF_IDENTIFIER2.SequenceEqual(imgHeadData));
            return this;
        }
    }
    /// <summary>
    /// Tiff图片识别器
    /// </summary>
    public class TiffImage : ImageFormatter
    {
        /// <summary>
        /// TIFF文件标识符
        /// </summary>
        public static byte[] TIFF_IDENTIFIER = { 0x49, 0x49, 0x2A, 0x00 };
        /// <summary>
        /// TIFF文件头
        /// </summary>
        public static List<byte> TiffHead = new List<byte>(TIFF_IDENTIFIER);

        /// <summary>
        /// 构造函数
        /// </summary>
        public TiffImage()
        {
            // 十进制数 十六进制数
            //    73      49
            //    73      49
            //    42      2A
            //    0       00
            this.HeadTypeData.Clear();
            this.HeadTypeData.AddRange(TIFF_IDENTIFIER);
            this.ImageTypeName = TIFF;
        }
    }
    /// <summary>
    /// Icon图片识别器
    /// </summary>
    public class IconImage : ImageFormatter
    {
        /// <summary>
        /// ICON文件标识符，00 00 01 00 01 00 20 20
        /// </summary>
        public static byte[] ICON_IDENTIFIER = { 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x20, 0x20 };
        /// <summary>
        /// ICON文件头
        /// </summary>
        public static List<byte> IconHead = new List<byte>(ICON_IDENTIFIER);

        /// <summary>
        /// 构造函数
        /// </summary>
        public IconImage()
        {
            // 十进制数 十六进制数
            //    0       00
            //    0       00
            //    1       01
            //    0       00
            //    1       01
            //    0       00
            //    32      20
            //    32      20
            this.HeadTypeData.Clear();
            this.HeadTypeData.AddRange(ICON_IDENTIFIER);
            this.ImageTypeName = ICON;
        }
    }
}
#endif