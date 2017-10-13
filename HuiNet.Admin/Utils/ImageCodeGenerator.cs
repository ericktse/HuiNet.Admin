using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace HuiNet.Admin.Utils
{
    public class ImageCodeGenerator
    {
        #region 验证码长度(默认4个验证码的长度)
        int _length = 4;
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
        #endregion

        #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
        int _fontSize = 13;
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
        #endregion

        #region 边框补(默认1像素)
        int _padding = 2;
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }
        #endregion

        #region 是否输出燥点(默认不输出)
        bool _chaos = false;
        public bool Chaos
        {
            get { return _chaos; }
            set { _chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        Color _chaosColor = Color.Gray;
        public Color ChaosColor
        {
            get { return _chaosColor; }
            set { _chaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        Color _backgroundColor = Color.Bisque;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        Color[] _colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.BlueViolet, Color.Brown, Color.DarkCyan, Color.Purple };
        public Color[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }
        #endregion

        #region 自定义字体数组
        string[] _fonts = { "Arial", "Georgia" };
        public string[] Fonts
        {
            get { return _fonts; }
            set { _fonts = value; }
        }
        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)
        //string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        //string codeSerial = "0,1,2,3,4,5,6,7,8,9";
        string _codeSerial = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        public string CodeSerial
        {
            get { return _codeSerial; }
            set { _codeSerial = value; }
        }
        #endregion

        #region 产生波形滤镜效果
        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary> 
        /// 正弦曲线Wave扭曲图片 
        /// </summary> 
        /// <param name="srcBmp">图片路径</param> 
        /// <param name="bXDir">如果扭曲则选择为True</param> 
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param> 
        /// <returns></returns> 
        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            // 将位图背景填充为白色 
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    // 取得当前点的颜色 
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                    && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }
        #endregion

        #region 生成校验码图片

        /// <summary>
        /// 生成校验码图片
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;
            int imageWidth = (int)(code.Length * fWidth) + 5 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;
            Bitmap image = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(BackgroundColor);
            Random rand = new Random();
            //给背景添加随机生成的燥点 
            if (this.Chaos)
            {
                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 10;
                for (int i = 0; i < c; i++)
                {
                    int caoIdx = rand.Next(Colors.Length - 1);
                    Pen caoPen = new Pen(_colors[caoIdx], 0);
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);
                    g.DrawRectangle(caoPen, x, y, 1, 1);
                }
                // 3. 绘制干扰线条，采用比背景略深一些的颜色 
                int nLines = 10;
                Random rd = new Random((int)DateTime.Now.Ticks);
                for (int a = 0; a < nLines; a++)
                {
                    int x1 = rd.Next() % imageWidth;
                    int y1 = rd.Next() % imageHeight;
                    int x2 = rd.Next() % imageWidth;
                    int y2 = rd.Next() % imageHeight;
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
            int left = 0, top = 0, top1 = 1, top2 = 1;
            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;
            Font f;
            Brush b;
            int cindex, findex;
            //随机字体和颜色的验证码字符 
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);
                f = new Font(Fonts[findex], fSize, FontStyle.Bold);
                b = new SolidBrush(Colors[cindex]);
                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }
                left = i * fWidth;
                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }
            //画一个边框 边框颜色为Color.Gainsboro 
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();
            //产生波形（Add By 51aspx.com） 
            image = TwistImage(image, true, 2, 2);
            return image;
        }
        #endregion

        #region 生成随机字符码
        /// <summary>
        /// 生成随机字符吗
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        public string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }
            string[] arr = CodeSerial.Split(',');
            string code = "";
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }

        /// <summary>
        /// 生成随机字符吗
        /// </summary>
        /// <returns></returns>
        public string CreateVerifyCode()
        {
            return CreateVerifyCode(0);
        }
        #endregion

        #region 【无刷新仿google波形扭曲彩色】验证码样式
        /// <summary>
        /// 无刷新仿google波形扭曲彩色
        /// </summary>
        /// <param name="code">随机码</param>
        /// <returns></returns>
        public byte[] GenerateValidateCode(string code)
        {
            this.Length = this._length;
            this.FontSize = this._fontSize;
            this.Chaos = false;
            this.BackgroundColor = this._backgroundColor;
            this.ChaosColor = this._chaosColor;
            this.CodeSerial = this._codeSerial;
            this.Colors = this._colors;
            this.Fonts = this._fonts;
            this.Padding = this._padding;

            // 输出图片 
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap image = this.CreateImageCode(code);
                image.Save(ms, ImageFormat.Jpeg);
                image.Dispose();

                return ms.ToArray();
            }
        }
        #endregion

        #region 另一种验证码样式
        /// <summary>
        /// 另一种验证码样式 
        /// </summary>
        /// <param name="nLen">声称验证码的个数</param>
        public byte[] GenerateImageCode(int nLen)
        {
            int nBmpWidth = GetImagewidth(nLen);//得到图片宽度
            int nBmpHeight = GetImageHeight();//得到图片高度
            Bitmap bmp = new Bitmap(nBmpWidth, nBmpHeight);
            //对图像进行弯曲 
            TwistImage(bmp, true, 12, 2);

            // 1. 生成随机背景颜色 
            int nRed, nGreen, nBlue; // 背景的三元色 
            Random rd = new Random((int)DateTime.Now.Ticks);
            nRed = rd.Next(255) % 128 + 128;
            nGreen = rd.Next(255) % 128 + 128;
            nBlue = rd.Next(255) % 128 + 128;
            //nRed = 255;
            //nGreen = 225;
            //nBlue = 196;
            // 2. 填充位图背景 
            Graphics graph = Graphics.FromImage(bmp);
            graph.FillRectangle(new SolidBrush(Color.FromArgb(nRed, nGreen, nBlue))
            , 0
            , 0
            , nBmpWidth
            , nBmpHeight);

            // 3. 绘制干扰线条，采用比背景略深一些的颜色 
            int nLines = 4;
            //System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(nRed - 17, nGreen - 17, nBlue - 17), 2);
            Pen pen = new Pen(Color.FromArgb(192, 192, 192), 2);
            for (int a = 0; a < nLines; a++)
            {
                int x1 = rd.Next() % nBmpWidth;
                int y1 = rd.Next() % nBmpHeight;
                int x2 = rd.Next() % nBmpWidth;
                int y2 = rd.Next() % nBmpHeight;
                graph.DrawLine(pen, x1, y1, x2, y2);
            }
            // 采用的字符集，可以随即拓展，并可以控制字符出现的几率 
            string strCode = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // 4. 循环取得字符，并绘制 
            for (int i = 0; i < nLen; i++)
            {
                int x = (i * 13 + rd.Next(3));
                int y = rd.Next(4) + 1;
                // 确定字体 
                var font = new Font("Courier New",//文字字体类型 
                12 + rd.Next() % 4,//文字字体大小 
                FontStyle.Bold);//文字字体样式 
                char c = strCode[rd.Next(strCode.Length)]; // 随机获取字符 
                // 绘制字符 
                graph.DrawString(c.ToString(),
                font,
                new SolidBrush(Color.FromArgb(nRed - 60 + y * 5, nGreen - 60 + y * 5, nBlue - 40 + y * 5)),
                x,
                y);
            }

            //对图像进行弯曲 
            TwistImage(bmp, true, 4, 4);


            // 5. 输出字节流
            using (MemoryStream bstream = new MemoryStream())
            {
                bmp.Save(bstream, ImageFormat.Jpeg);

                bmp.Dispose();
                graph.Dispose();


                return bstream.ToArray();
            }
        }

        ///<summary> 
        ///得到验证码图片的宽度 
        ///</summary> 
        ///<param name="validateNumLength">验证码的长度</param> 
        ///<returns></returns> 
        public static int GetImagewidth(int validateNumLength)
        {
            return (int)(15 * validateNumLength + 5);
        }
        ///<summary> 
        ///得到验证码的高度 
        ///</summary> 
        ///<returns></returns> 
        public static int GetImageHeight()
        {
            return 25;
        }
        #endregion
    }
}