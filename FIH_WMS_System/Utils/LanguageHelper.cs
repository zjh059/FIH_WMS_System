using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace FIH_WMS_System.Utils
{
    /// <summary>
    /// 全局多语言翻译助手
    /// </summary>
    public static class LanguageHelper
    {
        // 存储当前语言的字典包
        private static Dictionary<string, string> _dict = new Dictionary<string, string>();

        // 当前语言标识 (默认简中)
        public static string CurrentLang { get; private set; } = "zh-CN";

        // 新增：是否开启自动捕捉未翻译词条功能（正式上线时可改为 false）
        public static bool EnableAutoRecord = true;
        // 新增：用来暂存捕捉到的未翻译词条
        private static Dictionary<string, string> _missingDict = new Dictionary<string, string>();

        /// <summary>
        /// 加载指定语言包 (如 "en-US", "zh-TW")
        /// </summary>
        public static void LoadLanguage(string langCode)
        {
            CurrentLang = langCode;

            // 若是简中，直接清空字典（因为此代码和UI本为简中）
            if (langCode == "zh-CN")
            {
                _dict.Clear();
                return;
            }

            // 读取对应的 JSON 文件 (程序根目录下 Langs 文件夹)
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Langs", $"{langCode}.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
        }

        /// <summary>
        /// 翻译单个字符串
        /// </summary>
        /// <param name="originalText">原始简中文字</param>
        /// <returns>翻译后的文字，如果字典里没有，则返回原文</returns>
        //public static string GetString(string originalText)
        //{
        //    if (string.IsNullOrWhiteSpace(originalText)) return originalText;

        //    // 如果字典里有这个中文的翻译，就返回翻译结果；否则保持原样
        //    return _dict.ContainsKey(originalText) ? _dict[originalText] : originalText;
        //}
        /// <summary>
        /// 翻译单个字符串，并自动捕捉未翻译的词条
        /// </summary>
        public static string GetString(string originalText)
        {
            // 如果为空、或者是纯空格，直接返回
            if (string.IsNullOrWhiteSpace(originalText)) return originalText;

            // 1. 如果字典里已经有翻译了，直接返回翻译结果
            if (_dict.ContainsKey(originalText))
            {
                return _dict[originalText];
            }

            // 2. 如果没找到翻译，且当前不是简中模式，且开启了捕捉模式
            if (EnableAutoRecord && CurrentLang != "zh-CN")
            {
                // 过滤一下纯数字（比如表格里的数量、ID等不需要翻译的）
                if (!decimal.TryParse(originalText, out _) && !_missingDict.ContainsKey(originalText))
                {
                    // 记录下来，默认翻译留空，等你来填！
                    _missingDict[originalText] = "";
                    SaveMissingTranslations(); // 自动保存到文件
                }
            }

            // 3. 返回原词
            return originalText;
        }



        /// <summary>
        /// 动态翻译整个窗体的所有控件
        /// </summary>
        public static void TranslateForm(Form form)
        {
            // 1. 翻译窗体标题
            form.Text = GetString(form.Text);

            // 2. 递归翻译内部控件
            TranslateControls(form.Controls);
        }

        /// <summary>
        /// 递归遍历并翻译控件
        /// </summary>
        private static void TranslateControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                // 翻译控件显示的文本 (按钮、Label、TextBox的提示等)
                if (!string.IsNullOrWhiteSpace(ctrl.Text))
                {
                    ctrl.Text = GetString(ctrl.Text);
                }

                // 如果是特殊控件：DataGridView 表头
                if (ctrl is DataGridView dgv)
                {
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        col.HeaderText = GetString(col.HeaderText);
                    }
                }

                // 如果是特殊控件：ComboBox 下拉框
                // (如果你的下拉框绑定了对象，这里可能需要根据实际情况调整，如果是纯文本Items则可以直接翻译)
                if (ctrl is ComboBox cmb && cmb.DataSource == null)
                {
                    for (int i = 0; i < cmb.Items.Count; i++)
                    {
                        if (cmb.Items[i] is string strItem)
                        {
                            cmb.Items[i] = GetString(strItem);
                        }
                    }
                }

                // 递归翻译子控件 (如 Panel, GroupBox 里面的控件)
                if (ctrl.HasChildren)
                {
                    TranslateControls(ctrl.Controls);
                }
            }
        }


        /// <summary>
        /// 将未翻译的词条自动写入 JSON 文件
        /// </summary>
        //private static void SaveMissingTranslations()
        //{
        //    try
        //    {
        //        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Langs");
        //        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        //        // 生成一个带 "待翻译" 后缀的文件，防止覆盖写好的正式版
        //        string filePath = Path.Combine(dir, $"{CurrentLang}_待翻译.json");

        //        // 使用 Newtonsoft.Json 格式化输出 (Formatting.Indented 可以让 JSON 换行，方便阅读)
        //        string json = JsonConvert.SerializeObject(_missingDict, Formatting.Indented);
        //        File.WriteAllText(filePath, json);
        //    }
        //    catch
        //    {
        //        // 忽略文件写入冲突
        //    }
        //}
        /// <summary>
        /// 将未翻译的词条自动写入 JSON 文件（防丢失升级，避免之前的翻译字段还未修改便已丢失）
        /// </summary>
        private static void SaveMissingTranslations()
        {
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Langs");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string filePath = Path.Combine(dir, $"{CurrentLang}_待翻译.json");

                // 1. 先准备一个大池子
                Dictionary<string, string> mergedDict = new Dictionary<string, string>();

                // 2. 如果硬盘上已经有之前生成的待翻译文件，先把它读出来装进池子里，防止丢失
                if (File.Exists(filePath))
                {
                    string oldJson = File.ReadAllText(filePath);
                    mergedDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(oldJson) ?? new Dictionary<string, string>();
                }

                // 3. 把本次软件运行新抓到的词条，也加进大池子里
                foreach (var kvp in _missingDict)
                {
                    if (!mergedDict.ContainsKey(kvp.Key))
                    {
                        mergedDict[kvp.Key] = kvp.Value;
                    }
                }

                // 4. 把合并后的所有词条重新写回硬盘
                string json = JsonConvert.SerializeObject(mergedDict, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch
            {
                // 忽略文件写入冲突
            }
        }



    }
}