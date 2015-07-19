// ******************************************************************
// GridToExcelCreater.cs ： DataGridViewからExcelワークシート生成クラス
// 作成日　：2014/04/23
// 更新履歴：2014/10/17 水落　　 出力時の列幅を広げた。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mizuochi.Commons.Codes.ClassExtensions;
using Mizuochi.Commons.Codes.Excel;
using Mizuochi.Commons.Codes.Excel.ConstWrappers;

namespace Mizuochi.Commons.Codes.Forms.Grid
{
    /// <summary>
    /// DataGridViewからExcelワークシートを生成するクラスです。
    /// </summary>
    public class GridToExcelConverter
    {
        /// <summary>
        /// DataGridViewをExcelワークシートに変換します。
        /// </summary>
        /// <param name="grid">変換元のDataGridView</param>
        /// <param name="worksheet">変換先のExcelワークシート</param>
        public void Convert(DataGridView grid, ref Worksheet worksheet)
        {
            this.SetColumn(grid, ref worksheet);
            this.SetValue(grid, ref worksheet);

            return;
        }

        /// <summary>
        /// DataGridViewから、Excelの列に関する設定を行います。
        /// </summary>
        /// <param name="grid">変換元のDataGridViewオブジェクト</param>
        /// <param name="worksheet">変換先のExcelワークシート</param>
        private void SetColumn(DataGridView grid, ref Worksheet worksheet)
        {
            int excelColumnIndex = 1;
            foreach (DataGridViewColumn gridColumn in grid.Columns)
            {
                // 非表示なら処理しない
                if (false == gridColumn.Visible)
                {
                    continue;
                }

                dynamic cell = worksheet.Cells[1, excelColumnIndex];
                dynamic columnRange = cell.EntireColumn;

                // 列全体の設定
                columnRange.ColumnWidth = gridColumn.Width / columnRange.Font.Size * 2;
                this.SetFormat(gridColumn, ref columnRange);
                this.SetCellBackColor(gridColumn.DefaultCellStyle.BackColor, ref columnRange);
                this.SetHorizontalAlignment(gridColumn.DefaultCellStyle.Alignment, ref columnRange);

                // 列見出しの設定
                cell.Value = gridColumn.HeaderText;
                this.SetCellBackColor(gridColumn.HeaderCell.Style.BackColor, ref cell);
                this.SetHorizontalAlignment(gridColumn.HeaderCell.Style.Alignment, ref columnRange);

                ++excelColumnIndex;
            }

            return;
        }

        /// <summary>
        /// DataGridViewから、Excelの値に関する設定を行います。
        /// </summary>
        /// <param name="grid">変換元のDataGridViewオブジェクト</param>
        /// <param name="worksheet">変換先のExcelワークシート</param>
        private void SetValue(DataGridView grid, ref Worksheet worksheet)
        {
            // グリッド行でループ処理
            int excelRowIndex = 2;
            foreach (DataGridViewRow gridRow in grid.Rows)
            {
                // 非表示なら処理しない
                if (false == gridRow.Visible)
                {
                    continue;
                }

                // グリッド列でループ処理
                int excelColumnIndex = 1;
                foreach (DataGridViewColumn gridColumn in grid.Columns)
                {
                    // 非表示なら処理しない
                    if (false == gridColumn.Visible)
                    {
                        continue;
                    }

                    // セル設定
                    dynamic cell = worksheet.Cells[excelRowIndex, excelColumnIndex];
                    DataGridViewCell gridCell = gridRow.Cells[gridColumn.Index];
                    cell.Value = gridCell.Value;
                    this.SetFormat(gridCell, ref cell);
                    this.SetCellBackColor(gridCell.Style.BackColor, ref cell);

                    ++excelColumnIndex;
                }

                ++excelRowIndex;
            }

            return;
        }

        /// <summary>
        /// セルの背景色を設定します。
        /// </summary>
        /// <param name="color">設定する背景色</param>
        /// <param name="range">設定対象セル</param>
        private void SetCellBackColor(Color color, ref dynamic range)
        {
            if (false == color.IsEmpty)
            {
                range.Interior.Color = color.ToArgb();
            }

            return;
        }

        /// <summary>
        /// DataGridViewのセル位置から、Excelのセル横位置を取得します。
        /// </summary>
        /// <param name="dataGridViewContentAlignment">DataGridViewのセル位置</param>
        /// <param name="range">設定対象セル</param>
        /// <returns>Excelのセル横位置</returns>
        private void SetHorizontalAlignment(DataGridViewContentAlignment dataGridViewContentAlignment, ref dynamic range)
        {
            XlHAlign? xlHAlign = null;

            switch (dataGridViewContentAlignment)
            {
                case DataGridViewContentAlignment.BottomCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.TopCenter:
                    xlHAlign = XlHAlign.xlHAlignCenter;
                    break;
                case DataGridViewContentAlignment.BottomLeft:
                case DataGridViewContentAlignment.MiddleLeft:
                case DataGridViewContentAlignment.TopLeft:
                    xlHAlign = XlHAlign.xlHAlignLeft;
                    break;
                case DataGridViewContentAlignment.BottomRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.TopRight:
                    xlHAlign = XlHAlign.xlHAlignRight;
                    break;
            }

            if (null != xlHAlign)
            {
                range.HorizontalAlignment = xlHAlign;
            }

            return;
        }

        /// <summary>
        /// DataGridViewの列オブジェクトから、Excelの表示形式を取得します。
        /// </summary>
        /// <param name="gridColumn">DataGridViewの列オブジェクト</param>
        /// <param name="range">設定対象セル</param>
        /// <returns>Excelの表示形式</returns>
        private void SetFormat(DataGridViewColumn gridColumn, ref dynamic range)
        {
            var valueType = gridColumn.GetValueType();
            string gridFormat = gridColumn.DefaultCellStyle.Format.Convert(format => format.Replace(@"\\", @"\"));
            string numberFormat =
                (true == valueType.IsNumeric()) ? this.GetNumericExcelFormt(gridFormat) :
                (typeof(DateTime) == valueType) ? this.GetDateTimeExcelFormt(gridFormat) :
                (typeof(string) == valueType) ? this.GetStringExcelFormt(gridFormat) :
                                                this.GetOtherExcelFormt(gridFormat);

            if (null != numberFormat)
            {
                range.NumberFormat = numberFormat;
            }

            return;
        }

        /// <summary>
        /// DataGridViewのセルオブジェクトから、Excelの表示形式を取得します。
        /// </summary>
        /// <param name="gridCell">DataGridViewのセルオブジェクト</param>
        /// <param name="range">設定対象セル</param>
        /// <returns>Excelの表示形式</returns>
        private void SetFormat(DataGridViewCell gridCell, ref dynamic range)
        {
            if (true == String.IsNullOrWhiteSpace(gridCell.Style.Format))
            {
                return;
            }

            var valueType = gridCell.GetValueType();
            string gridFormat = gridCell.Style.Format.Convert(format => format.Replace(@"\\", @"\"));
            string numberFormat =
                (true == valueType.IsNumeric()) ? this.GetNumericExcelFormt(gridFormat) :
                (typeof(DateTime) == valueType) ? this.GetDateTimeExcelFormt(gridFormat) :
                (typeof(string) == valueType) ? this.GetStringExcelFormt(gridFormat) :
                                                this.GetOtherExcelFormt(gridFormat);

            if (null != numberFormat)
            {
                range.NumberFormat = numberFormat;
            }

            return;
        }

        /// <summary>
        /// .Net Frameworkの数値書式指定子からExcelの書式形式を生成します。
        /// </summary>
        /// <param name="format">.Net Frameworkの数値書式指定子</param>
        /// <returns>Excelの書式形式</returns>
        private string GetNumericExcelFormt(string format)
        {
            if (true == String.IsNullOrWhiteSpace(format))
            {
                return null;
            }

            int precision = 0;
            string numberFormat = null;

            switch (format.FirstOrDefault())
            {
                // 金額
                case 'c':
                case 'C':
                    precision = (1 < format.Count()) ? Int32.Parse(format.Substring(1)) : 0;
                    numberFormat = (0 != precision) ?
                        String.Format("¥#,##0.{0};¥-#,##0.{0}", new String('0', precision)) :
                        String.Format("¥#,##0;¥-#,##0");
                    break;
                // 指数系
                case 'd':
                case 'D':
                case 'e':
                case 'E':
                case 'f':
                case 'F':
                case 'g':
                case 'G':
                    precision = (1 < format.Count()) ? Int32.Parse(format.Substring(1)) : 0;
                    numberFormat = (0 != precision) ?
                        String.Format("0.{0}_ ", new String('0', precision)) :
                        String.Format("0_ ");
                    break;
                // 数値
                case 'n':
                case 'N':
                    precision = (1 < format.Count()) ? Int32.Parse(format.Substring(1)) : 0;
                    numberFormat = (0 != precision) ?
                        String.Format("#,##0.{0}_ ", new String('0', precision)) :
                        String.Format("#,##0_ ");
                    break;
                // パーセント
                case 'p':
                case 'P':
                    precision = (1 < format.Count()) ? Int32.Parse(format.Substring(1)) : 0;
                    numberFormat = (0 != precision) ?
                        String.Format("0.{0}%", new String('0', precision)) :
                        String.Format("0%");
                    break;
                // 上記以外の標準書式
                case 'r':
                case 'R':
                case 'x':
                case 'X':
                    numberFormat = String.Empty;
                    break;
                // カスタム書式
                default:
                    numberFormat = format;
                    break;
            }

            return numberFormat;
        }

        /// <summary>
        /// .Net Frameworkの日付書式指定子からExcelの書式形式を生成します。
        /// </summary>
        /// <param name="format">.Net Frameworkの数値書式指定子</param>
        /// <returns>Excelの書式形式</returns>
        private string GetDateTimeExcelFormt(string format)
        {
            if (true == String.IsNullOrWhiteSpace(format))
            {
                return null;
            }

            string numberFormat = null;

            // 標準書式指定子か判定
            if (1 == format.Count())
            {
                // 標準書式指定子
                numberFormat = 
                    ('d' == format.First()) ? @"yyyy/mm/dd;@" :
                    ('D' == format.First()) ? @"yyyy""年""m""月""d""日"";@" :
                    ('f' == format.First()) ? @"yyyy""年""m""月""d""日"" h:mm;@" :
                    ('F' == format.First()) ? @"yyyy""年""m""月""d""日"" h:mm:ss;@" :
                    ('g' == format.First()) ? @"yyyy/mm/dd h:mm;@" :
                    ('G' == format.First()) ? @"yyyy/mm/dd h:mm:ss;@" :
                    ('m' == format.First()) ? @"""m""月""d""日"";@" :
                    ('M' == format.First()) ? @"""m""月""d""日"";@" :
                    ('o' == format.First()) ? @"" :
                    ('O' == format.First()) ? @"" :
                    ('r' == format.First()) ? @"" :
                    ('R' == format.First()) ? @"" :
                    ('s' == format.First()) ? @"" :
                    ('t' == format.First()) ? @"h:mm;@" :
                    ('T' == format.First()) ? @"h:mm:ss;@" :
                    ('u' == format.First()) ? @"" :
                    ('U' == format.First()) ? @"" :
                    ('Y' == format.First()) ? @"yyyy""年""m""月"";@" :
                    ('y' == format.First()) ? @"yyyy""年""m""月"";@" :
                                              @"";
            }
            else
            {
                // カスタム書式指定子
                numberFormat = format;
            }

            return numberFormat;
        }

        /// <summary>
        /// 文字列のExcelの書式形式を生成します。
        /// </summary>
        /// <param name="format">.Net Frameworkの書式指定子</param>
        /// <returns>Excelの書式形式</returns>
        private string GetStringExcelFormt(string format)
        {
            return "@";
        }

        /// <summary>
        /// 汎用的なExcelの書式形式を生成します。
        /// </summary>
        /// <param name="format">.Net Frameworkの書式指定子</param>
        /// <returns>Excelの書式形式</returns>
        private string GetOtherExcelFormt(string format)
        {
            return (false == String.IsNullOrWhiteSpace(format)) ?
                String.Empty:
                null;
        }
    }
}
