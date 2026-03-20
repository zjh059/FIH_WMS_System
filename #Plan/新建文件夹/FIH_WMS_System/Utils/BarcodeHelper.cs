using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing; // 添加引用以支持 Bitmap
using ZXing;
using ZXing.QrCode;

using ZXing.Rendering;
using ZXing.Windows.Compatibility;

namespace FIH_WMS_System.Utils
{
    /// <summary>
    /// 全局条码/二维码生成助手
    /// </summary>
    public static class BarcodeHelper
    {
        // 生成标准二维码图片
        public static Bitmap GenerateQRCode(string content)
        {
            // 明确指定输出类型为 Bitmap，并使用优雅的对象初始化语法
            var writer = new BarcodeWriter<Bitmap>
            {
                Format = BarcodeFormat.QR_CODE, // 指定格式为二维码
                Options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8", // 支持中文
                    Width = 250,            // 宽度
                    Height = 250,           // 高度
                    Margin = 1              // 白边设为最小
                },
                Renderer = new ZXing.Windows.Compatibility.BitmapRenderer()
            };

            return writer.Write(content); // 生成并返回图片
        }
    }
}