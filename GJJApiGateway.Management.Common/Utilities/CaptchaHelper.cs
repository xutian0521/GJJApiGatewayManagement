using System;
using System.IO;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.Drawing.Processing;
//using SixLabors.Fonts;
//using System.Linq;
//using System.Runtime.InteropServices;
//using SixLabors.ImageSharp.Formats;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GJJApiGateway.Management.Common.Utilities
{
    public static class CaptchaHelper
    {
        ///// <summary>
        ///// 根据文本生成图形验证码（使用 ImageSharp ）
        ///// </summary>
        ///// <param name="validateCode"></param>
        ///// <returns></returns>
        //public static byte[] CreateValidateGraphic(string validateCode)
        //{
        //    int width = validateCode.Length * 25;
        //    int height = 32;

        //    using (Image<Rgba32> image = new Image<Rgba32>(width, height))
        //    {
        //        var random = new Random();
        //        var font = GetFont();

        //        // 设置绘制文本的选项
        //        var textOptions = new RichTextOptions(font)
        //        {
        //            Origin = new PointF(5, 5),
        //            HorizontalAlignment = HorizontalAlignment.Left
        //        };

        //        // 设置背景色
        //        image.Mutate(ctx =>
        //        {
        //            ctx.Fill(Color.White);

        //            // 画干扰线（用 DrawPolygon 代替 DrawLines）
        //            for (int i = 0; i < 25; i++)
        //            {
        //                float x1 = random.Next(width);
        //                float y1 = random.Next(height);
        //                float x2 = random.Next(width);
        //                float y2 = random.Next(height);
        //                ctx.DrawPolygon(Color.Silver, 1, new PointF[] { new PointF(x1, y1), new PointF(x2, y2) });
        //            }

        //            // 画验证码文字
        //            ctx.DrawText(textOptions, validateCode, Color.Black);

        //            // 画干扰点
        //            for (int i = 0; i < 100; i++)
        //            {
        //                int x = random.Next(width);
        //                int y = random.Next(height);
        //                image[x, y] = new Rgba32((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
        //            }
        //        });

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            image.SaveAsJpeg(ms);
        //            return ms.ToArray();
        //        }
        //    }
        //}

        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        ///// <summary>
        ///// 获取合适的字体，Windows和Linux平台适用
        ///// </summary>
        ///// <returns></returns>
        //public static Font GetFont()
        //{
        //    // 获取当前系统字体集合
        //    var fontCollection = new FontCollection();

        //    // 尝试查找 DejaVu Sans 字体系列
        //    var fontFamily = fontCollection.Families.FirstOrDefault(f => f.Name.Contains("DejaVu Sans"));

        //    if (fontFamily == null)
        //    {
        //        // 如果没有找到 DejaVu Sans 字体，尝试加载 Liberation Sans
        //        fontFamily = fontCollection.Families.FirstOrDefault(f => f.Name.Contains("Liberation Sans"));
        //    }

        //    // 如果找到字体系列，创建字体
        //    if (fontFamily != null)
        //    {
        //        // 返回找到的字体系列，大小为18，样式为粗体
        //        return fontFamily.CreateFont(18, FontStyle.Bold);
        //    }
        //    else
        //    {
        //        // 如果没有找到任何字体，使用默认字体
        //        return SystemFonts.CreateFont("DejaVu Sans", 18, FontStyle.Bold);
        //    }
        //}
    }
}
