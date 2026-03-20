using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Speech.Synthesis;

namespace FIH_WMS_System.Utils
{
    /// <summary>
    /// 全局语音播报助手
    /// </summary>
    public static class VoiceHelper
    {
        // 静态方法，随时随地直接调用
        public static void Speak(string text)
        {
            // 使用 Task.Run 把它丢到后台线程去读，这样就不会卡住 MessageBox 弹窗了
            Task.Run(() =>
            {
                try
                {
                    // 召唤微软内置的语音合成器
                    using (SpeechSynthesizer synth = new SpeechSynthesizer())
                    {
                        // 设置为系统的默认音频输出设备（你的音箱或耳机）
                        synth.SetOutputToDefaultAudioDevice();
                        // 语速稍微调快一点点（范围是 -10 到 10，0是正常）
                        synth.Rate = 1;
                        // 开始朗读
                        synth.Speak(text);
                    }
                }
                catch (Exception)
                {
                    // 如果电脑没插音箱或没有声卡驱动，直接忽略，不让程序崩溃
                }
            });
        }
    }
}