using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace MeetingSystem
{
    public class ExcelUtil
    {
        Microsoft.Office.Interop.Excel.Application _Excel = null;

        /// <summary>
        /// 引用活頁簿類別
        /// </summary>
        _Workbook book = null;
        
        /// <summary>
        /// 引用工作表類別
        /// </summary>
        _Worksheet sheetWork = null;

        /// <summary>
        /// 引用Range類別
        /// </summary>
        Range range = null;
        
        bool isOpenView;

        public ExcelUtil(string filePath, bool isOpenView, int sheetIndex)
        {
            this.isOpenView = isOpenView;
            _Excel = new Microsoft.Office.Interop.Excel.Application();
            book = _Excel.Workbooks.Open(filePath);
            _Excel.Visible = isOpenView;
            _Excel.DisplayAlerts = false;
            sheetWork = book.Sheets[sheetIndex];
        }

        public void setSheet(int sheetIndex)
        {
            // test it
            if (sheetIndex > book.Sheets.Count)
            {
                book.Sheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                sheetWork = book.Sheets[1]; // 產生在最前面
                sheetWork.Move(After: book.Sheets[book.Sheets.Count]); // 移到最後面
            }
            sheetWork = book.Sheets[sheetIndex];
        }

        public void write(int row, int col, string data)
        {
            try
            {
                sheetWork.Cells[row, col] = data;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public string read(int row, int col)
        {
            return sheetWork.Cells[row, col].Text;
        }

        public void save()
        {
            book.Save();
        }

        public void close()
        {
            if (!isOpenView)
            {
                book.Close();
                _Excel.Quit();
                releaseObject(sheetWork);
                releaseObject(book);
                releaseObject(_Excel);
            }
        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        internal void reNameSheet(string sheetName)
        {
            sheetWork.Name = sheetName;
        }

        internal void clone(int totalPage)
        {
            for (int i = 1; i < totalPage; i++)
            {
                sheetWork.Copy(book.Sheets[i]);
            }
        }

        /// <summary>
        /// 設定列寬 (參數 range 範例 "A6:E5")
        /// </summary>
        /// <param name="range"></param>
        /// <param name="width"></param>
        internal void setColumnWidth(string range, int width)
        {
            sheetWork.get_Range(range).EntireColumn.ColumnWidth = width;
        }

        /// <summary>
        /// 設定行高 (參數 range 範例 "A6:E5")
        /// </summary>
        /// <param name="range"></param>
        /// <param name="height"></param>
        internal void setRowHeight(string range, int height)
        {
            sheetWork.get_Range(range).EntireRow.RowHeight = height;
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字大小
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="size"></param>
        public void fontSize(int row, int col, int size)
        {
            sheetWork.Cells[row, col].Font.Size = size;
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字對齊為置中
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setCellAlignCenter(int row, int col)
        {
            sheetWork.Cells[row, col].Style.HorizontalAlignment = XlHAlign.xlHAlignCenter;
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字對齊為靠左
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setCellAlignLeft(int row, int col)
        {
            sheetWork.Cells[row, col].HorizontalAlignment = XlHAlign.xlHAlignLeft;
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字對齊為分散
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setCellAlignDistributed(int row, int col)
        {
            sheetWork.Cells[row, col].HorizontalAlignment = XlHAlign.xlHAlignDistributed;
        }

        /// <summary>
        /// 設定 Cell[row, col] 的邊框寬度為 2d
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setCellBorder(int row, int col)
        {
            sheetWork.Cells[row, col].Borders.Weight = 2d;
        }

        /// <summary>
        /// 設定 Range 內的 Cell 文字是否換行 (參數 range 範例 "A6:E5")
        /// </summary>
        /// <param name="range"></param>
        /// <param name="isWrapText"></param>
        internal void setWrapText(string range, bool isWrapText)
        {
            sheetWork.Range[range].Style.WrapText = isWrapText;
        }

        /// <summary>
        /// 設定 Range 內的 Cell 合併且文字換行 (參數 range 範例 "A6:E5")
        /// </summary>
        /// <param name="range"></param>
        internal void mergeColumn(string range)
        {
            sheetWork.Range[range].Merge();
            sheetWork.Range[range].Style.WrapText = true;
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字字型為標楷體
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void font(int row, int col)
        {
            sheetWork.Cells[row, col].Font.Name = "標楷體";
        }

        /// <summary>
        /// 設定 Cell[row, col] 背景顏色為黃色
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setInteriorColorYellow(int row, int col)
        {
            sheetWork.Cells[row, col].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
        }

        /// <summary>
        /// 設定 Range 內的 Cell 背景顏色為珊瑚色 (參數 range 範例 "A6:E5")
        /// </summary>
        /// <param name="range"></param>
        internal void setInteriorColorCoral(string range)
        {
            sheetWork.Range[range].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Coral);
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字顏色為黑色
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setFontColorBlack(int row, int col)
        {
            sheetWork.Cells[row, col].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字顏色為藍色
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setFontColorBule(int row, int col)
        {
            sheetWork.Cells[row, col].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字顏色為紅色
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setFontColorRed(int row, int col)
        {
            sheetWork.Cells[row, col].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        }

        /// <summary>
        /// 設定 Cell[row, col] 內的文字顏色為灰色
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void setFontColorGray(int row, int col)
        {
            sheetWork.Cells[row, col].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray);
            sheetWork.Cells[row, col].Font.Strikethrough = true;
        }

        internal void deleteRows(int startRow, int endRow)
        {
            for (int i = startRow; i <= endRow; i++)
            {
                sheetWork.Rows[startRow].Delete();
            }
        }

        internal void setPrintArea(string range)
        {
            sheetWork.PageSetup.PrintArea = range;
        }
    }
}
