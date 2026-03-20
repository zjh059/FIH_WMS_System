using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Windows.Forms;
using ClosedXML.Excel; // 引入刚才安装的包

namespace FIH_WMS_System.Utils
{
    /// <summary>
    /// 全局 Excel 导出助手
    /// </summary>
    public static class ExcelHelper
    {
        // 传入一个 DataGridView（界面上的表格），和你想保存的默认文件名
        public static void ExportToExcel(DataGridView dgv, string defaultFileName)
        {
            // 1. 防呆校验：如果表格是空的，直接退出
            if (dgv.Rows.Count == 0 || dgv.Columns.Count == 0)
            {
                MessageBox.Show("当前没有数据可以导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. 召唤系统的“保存文件”对话框
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel 工作簿|*.xlsx"; // 限制只能存为最新的 xlsx 格式
            sfd.FileName = $"{defaultFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"; // 自动加上时间戳

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 3. 开始在内存里捏造一个 Excel 文件
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("导出数据");

                        // 4. 画表头：把 DataGridView 的列名抄到 Excel 的第一行
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = dgv.Columns[i].HeaderText;
                            // 把表头搞成粗体，加个背景色，显得专业
                            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }

                        // 5. 填数据：把 DataGridView 里的每一行数据抄进 Excel 里
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgv.Columns.Count; j++)
                            {
                                // 获取单元格的值，如果是 null 就转成空字符串
                                string cellValue = dgv.Rows[i].Cells[j].Value?.ToString() ?? "";
                                worksheet.Cell(i + 2, j + 1).Value = cellValue;
                            }
                        }

                        // 让 Excel 的列宽自动适应文字长度
                        worksheet.Columns().AdjustToContents();

                        // 6. 保存到用户选择的路径！
                        workbook.SaveAs(sfd.FileName);

                        // 语音播报 + 弹窗提示
                        Utils.VoiceHelper.Speak("数据已成功导出为 Excel 文件。");
                        MessageBox.Show("🎉 导出成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导出失败，文件可能被占用或没有权限。\n错误信息：{ex.Message}", "导出错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}