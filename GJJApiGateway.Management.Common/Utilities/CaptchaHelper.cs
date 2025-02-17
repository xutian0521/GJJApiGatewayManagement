using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;

namespace GJJApiGateway.Management.Common.Utilities
{
    public static class CaptchaHelper
    {
        /// <summary>
        /// 根据文本生成图形验证码（使用 ImageSharp ）
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            int width = validateCode.Length * 25;
            int height = 32;

            using (Image<Rgba32> image = new Image<Rgba32>(width, height))
            {
                var random = new Random();
                var font = SystemFonts.CreateFont("Arial", 18, FontStyle.Bold);

                // 设置绘制文本的选项
                var textOptions = new RichTextOptions(font)
                {
                    Origin = new PointF(5, 5),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                // 设置背景色
                image.Mutate(ctx =>
                {
                    ctx.Fill(Color.White);

                    // 画干扰线（用 DrawPolygon 代替 DrawLines）
                    for (int i = 0; i < 25; i++)
                    {
                        float x1 = random.Next(width);
                        float y1 = random.Next(height);
                        float x2 = random.Next(width);
                        float y2 = random.Next(height);
                        ctx.DrawPolygon(Color.Silver, 1, new PointF[] { new PointF(x1, y1), new PointF(x2, y2) });
                    }

                    // 画验证码文字
                    ctx.DrawText(textOptions, validateCode, Color.Black);

                    // 画干扰点
                    for (int i = 0; i < 100; i++)
                    {
                        int x = random.Next(width);
                        int y = random.Next(height);
                        image[x, y] = new Rgba32((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                    }
                });

                using (MemoryStream ms = new MemoryStream())
                {
                    image.SaveAsJpeg(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
