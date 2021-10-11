using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Web;

namespace System.Drawing
{
    public class ValidateImage
    {
        static int Validate_Code_Image_Line_Count = Convert.ToInt32("0" + System.Web.Configuration.WebConfigurationManager.AppSettings["Validate_Code_Image_Line_Count"]);
        static bool Validate_Code_Image_RandomSize = "true".Equals(System.Web.Configuration.WebConfigurationManager.AppSettings["Validate_Code_Image_RandomSize"], StringComparison.OrdinalIgnoreCase);
        private static Bitmap DrawAuthCode(string AuthCode)
        {
            //根据校验码的长度确定输出图片的长度
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(65, 25, PixelFormat.Format24bppRgb);//(int)Math.Ceiling(Convert.ToDouble(checkCode.Length * 15))
            //创建Graphics对象
            using (Graphics g = Graphics.FromImage(image))
            {

                //生成随机数种子
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);


                //---------------------------------------------------
                //Brush b = Brushes.Silver;
                //g.FillRectangle(b, 0, 0, image.Width, image.Height);
                //---------------------以上两种任选其一------------------------------
                //输出图片中校验码的字体: 12号Arial,粗斜体
                //LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Red, Color.Purple, 1.2f, true);
                HatchBrush brush = new HatchBrush(HatchStyle.DarkHorizontal, Color.Black);
                float last_size = 10;
                for (var i = 0; i < AuthCode.Length; i++)
                {
                    Font font = new Font("Arial", Validate_Code_Image_RandomSize ? random.Next(25, 32) : 12, FontStyle.Bold);


                    g.DrawString(AuthCode[i].ToString(), font, brush, last_size, 3 + random.Next(2));
                    last_size += font.Size - (Validate_Code_Image_RandomSize ? 5 : 2);
                    font.Dispose();
                }


                //画图片的背景噪音线
                //---------------------------------------------------
                for (int i = 0; i < Validate_Code_Image_Line_Count; i++)
                {
                    //噪音线起点坐标(x1,y1),终点坐标(x2,y2)
                    int x1 = random.Next(image.Width / 2);
                    int x2 = random.Next(image.Width / 2, image.Width);
                    int y1 = random.Next(image.Height / 4, image.Height / 2);
                    int y2 = random.Next(image.Height / 4, image.Height / 2);

                    //用银色画出噪音线
                    Color color = Color.Black;
                    if (i % 2 == 1) color = Color.White;
                    g.DrawLine(new Pen(color, 3), x1, y1, x2, y2);
                    //g.DrawBezier(new Pen(Color.Black, 3), x1, y1, x2, y2, random.Next(2), random.Next(3), random.Next(4), random.Next(5));
                }

                brush.Dispose();

                ////画图片的前景噪音点 100个
                //for (int i = 0; i < 1000; i++)
                //{
                //    int x = random.Next(image.Width);
                //    int y = random.Next(image.Height);
                //    image.SetPixel(x, y, Color.Black);
                //}

            }

            return image;
        }

        public static void NewCode(string code)
        {

            HttpContext.Current.Response.CacheControl = "no-cache";


            var image = DrawAuthCode(code);
            image = WaveDistortion(image);

            image.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            image.Dispose();

        }



        #region  KCAPTCHA 波纹扭曲

        /// <summary>

        /// # KCAPTCHA PROJECT VERSION 1.2.6

        /// www.captcha.ru, www.kruglov.ru

        /// 波形扭曲 FROM KCAPTCHA

        /// </summary>

        /// <param name="srcBmp">待扭曲的图像 必须为 PixelFormat.Format24bppRgb 格式图像</param>

        /// <returns></returns>

        private static Bitmap WaveDistortion(Bitmap srcBmp)
        {

            if (srcBmp == null)

                return null;

            if (srcBmp.PixelFormat != PixelFormat.Format24bppRgb)

                throw new ArgumentException("srcBmp PixelFormat.Format24bppRgb 格式图像", "srcBmp");



            var width = srcBmp.Width;

            var height = srcBmp.Height;



            Bitmap destBmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            {

                Random randx = new Random();
                //前景色

                Color foreground_color = Color.Black;// Color.FromArgb(randx.Next(10, 100), randx.Next(10, 100), randx.Next(10, 100));

                //背景色

                //Color background_color = Color.FromArgb(randx.Next(200, 250), randx.Next(200, 250), randx.Next(200, 250));
                Color background_color = Color.FromArgb(255, 255, 255);


                using (Graphics newG = Graphics.FromImage(destBmp))
                {

                    newG.Clear(background_color);

                    // periods 时间

                    double rand1 = randx.Next(710000, 1200000) / 10000000.0;

                    double rand2 = randx.Next(710000, 1200000) / 10000000.0;

                    double rand3 = randx.Next(710000, 1200000) / 10000000.0;

                    double rand4 = randx.Next(710000, 1200000) / 10000000.0;

                    // phases  相位

                    double rand5 = randx.Next(0, 31415926) / 10000000.0;

                    double rand6 = randx.Next(0, 31415926) / 10000000.0;

                    double rand7 = randx.Next(0, 31415926) / 10000000.0;

                    double rand8 = randx.Next(0, 31415926) / 10000000.0;

                    // amplitudes 振幅

                    double rand9 = randx.Next(330, 450) / 110.0;

                    double rand10 = randx.Next(330, 450) / 110.0;

                    double amplitudesFactor = randx.Next(5, 10) / 10.0;//振幅小点防止出界

                    double center = width / 2.0;



                    //wave distortion 波纹扭曲

                    BitmapData destData = destBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, destBmp.PixelFormat);

                    BitmapData srcData = srcBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, srcBmp.PixelFormat);

                    for (var x = 0; x < width; x++)
                    {

                        for (var y = 0; y < height; y++)
                        {

                            var sx = x + (Math.Sin(x * rand1 + rand5)

                                        + Math.Sin(y * rand3 + rand6)) * rand9 - width / 2 + center + 1;

                            var sy = y + (Math.Sin(x * rand2 + rand7)

                                        + Math.Sin(y * rand4 + rand8)) * rand10 * amplitudesFactor; //振幅小点防止出界



                            int color, color_x, color_y, color_xy;

                            Color overColor = Color.Empty;



                            if (sx < 0 || sy < 0 || sx >= width - 1 || sy >= height - 1)
                            {

                                continue;

                            }

                            else
                            {

                                color = BitmapDataColorAt(srcData, (int)sx, (int)sy).B;

                                color_x = BitmapDataColorAt(srcData, (int)(sx + 1), (int)sy).B;

                                color_y = BitmapDataColorAt(srcData, (int)sx, (int)(sy + 1)).B;

                                color_xy = BitmapDataColorAt(srcData, (int)(sx + 1), (int)(sy + 1)).B;

                            }



                            if (color == 255 && color_x == 255 && color_y == 255 && color_xy == 255)
                            {

                                continue;

                            }

                            else if (color == 0 && color_x == 0 && color_y == 0 && color_xy == 0)
                            {

                                overColor = Color.FromArgb(foreground_color.R, foreground_color.G, foreground_color.B);

                            }

                            else
                            {

                                double frsx = sx - Math.Floor(sx);

                                double frsy = sy - Math.Floor(sy);

                                double frsx1 = 1 - frsx;

                                double frsy1 = 1 - frsy;



                                double newColor =

                                     color * frsx1 * frsy1 +

                                     color_x * frsx * frsy1 +

                                     color_y * frsx1 * frsy +

                                     color_xy * frsx * frsy;



                                if (newColor > 255) newColor = 255;

                                newColor = newColor / 255;

                                double newcolor0 = 1 - newColor;



                                int newred = Math.Min((int)(newcolor0 * foreground_color.R + newColor * background_color.R), 255);

                                int newgreen = Math.Min((int)(newcolor0 * foreground_color.G + newColor * background_color.G), 255);

                                int newblue = Math.Min((int)(newcolor0 * foreground_color.B + newColor * background_color.B), 255);



                                overColor = Color.FromArgb(newred, newgreen, newblue);

                            }

                            BitmapDataColorSet(destData, x, y, overColor);

                        }

                    }

                    destBmp.UnlockBits(destData);

                    srcBmp.UnlockBits(srcData);
                    //边框
                    newG.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220), 2), 0, 0, width - 1, height - 1);
                    //画图片的边框线
                }

                if (srcBmp != null)

                    srcBmp.Dispose();

            }

            return destBmp;

        }

        /// <summary>

        /// 获得 BitmapData 指定坐标的颜色信息

        /// 实现 PHP imagecolorat

        /// </summary>

        /// <param name="srcData">从图像数据获得颜色 必须为 PixelFormat.Format24bppRgb 格式图像数据</param>

        /// <param name="x"></param>

        /// <param name="y"></param>

        /// <returns>x,y 坐标的颜色数据</returns>

        /// <remarks>

        /// Format24BppRgb 已知X，Y坐标，像素第一个元素的位置为Scan0+(Y*Stride)+(X*3)。

        /// 这是blue字节的位置，接下来的2个字节分别含有green、red数据。

        /// </remarks>

        static Color BitmapDataColorAt(BitmapData srcData, int x, int y)
        {

            if (srcData.PixelFormat != PixelFormat.Format24bppRgb)

                throw new ArgumentException("srcData PixelFormat.Format24bppRgb 格式图像数据", "srcData");



            byte[] rgbValues = new byte[3];

            Marshal.Copy((IntPtr)((int)srcData.Scan0 + ((y * srcData.Stride) + (x * 3))), rgbValues, 0, 3);

            return Color.FromArgb(rgbValues[2], rgbValues[1], rgbValues[0]);

        }

        /// <summary>

        /// 设置 BitmapData 指定坐标的颜色信息

        /// 实现 PHP ImageColorSet

        /// </summary>

        /// <param name="destData">设置图像数据的颜色 必须为 PixelFormat.Format24bppRgb 格式图像数据</param>

        /// <param name="x"></param>

        /// <param name="y"></param>

        /// <param name="color">待设置颜色</param>

        /// <remarks>

        /// Format24BppRgb 已知X，Y坐标，像素第一个元素的位置为Scan0+(Y*Stride)+(X*3)。

        /// 这是blue字节的位置，接下来的2个字节分别含有green、red数据。

        /// </remarks>

        static void BitmapDataColorSet(BitmapData destData, int x, int y, Color color)
        {

            if (destData.PixelFormat != PixelFormat.Format24bppRgb)

                throw new ArgumentException("destData PixelFormat.Format24bppRgb 格式图像数据", "destData");



            byte[] rgbValues = new byte[3] { color.B, color.G, color.R };

            Marshal.Copy(rgbValues, 0, (IntPtr)((int)destData.Scan0 + ((y * destData.Stride) + (x * 3))), 3);

        }

        #endregion

    }
}
