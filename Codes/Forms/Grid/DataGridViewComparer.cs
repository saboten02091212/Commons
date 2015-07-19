// ******************************************************************
// DataGridViewComparer.cs ： DataGridViewソート抽象クラス
// 作成日　：2014/04/23
// 更新履歴：2014/04/23 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mizuochi.Commons.Codes.ClassExtensions;

namespace Mizuochi.Commons.Codes.Forms.Grid
{
    /// <summary>
    /// DataGridViewをソートする為の抽象クラス
    /// </summary>
    public class DataGridViewComparer : GridComparer
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="sortOrder">並び替え方法</param>
        public DataGridViewComparer(SortOrder sortOrder = SortOrder.Ascending)
            : base(sortOrder)
        {
        }

        /// <summary>
        ///  2つのオブジェクトを比較し、一方が他方より小さいか、等しいか、大きいかを示す値を返します。
        /// </summary>
        /// <param name="x">比較する最初のオブジェクトです。</param>
        /// <param name="y">比較する 2 番目のオブジェクトです。</param>
        /// <returns>x が y より小さい場合は負値。x と y は等しい場合は0。x が y より大きい場合は正値を返します。</returns>
        public override int Compare(object x, object y)
        {
            // 並び替えするか判定
            if (SortOrder.None == this.sortOrder)
            {
                return 0;
            }

            // 比較処理
            DataGridViewRow gridRowX = (DataGridViewRow)x;
            DataGridViewRow gridRowY = (DataGridViewRow)y;
            int compareResult = 0;
            foreach (var compareColumn in compareColumnList)
            {
                if (typeof(int) == compareColumn.ColumnType)
                {
                    // 数値型の列の場合
                    decimal valueX = Decimal.MinValue;
                    decimal valueY = Decimal.MinValue;
                    Decimal.TryParse(gridRowX.Cells[compareColumn.ColumnName].Value.Convert(item => item.ToString()), out valueX);
                    Decimal.TryParse(gridRowY.Cells[compareColumn.ColumnName].Value.Convert(item => item.ToString()), out valueY);
                    compareResult = valueX.CompareTo(valueY);
                    if (0 != compareResult)
                    {
                        var sortOrderModifierColumn =
                            (SortOrder.Ascending == compareColumn.SortOrder) ? 1 :
                            (SortOrder.Descending == compareColumn.SortOrder) ? -1 :
                                                                        0;
                        compareResult *= sortOrderModifierColumn;
                        break;
                    }
                }
                else if (typeof(DateTime) == compareColumn.ColumnType)
                {
                    // 日付型の列の場合
                    DateTime valueX = default(DateTime);
                    DateTime valueY = default(DateTime);
                    DateTime.TryParse(gridRowX.Cells[compareColumn.ColumnName].Value.Convert(item => item.ToString()), out valueX);
                    DateTime.TryParse(gridRowY.Cells[compareColumn.ColumnName].Value.Convert(item => item.ToString()), out valueY);
                    compareResult = valueX.CompareTo(valueY);
                    if (0 != compareResult)
                    {
                        var sortOrderModifierColumn =
                            (SortOrder.Ascending == compareColumn.SortOrder) ? 1 :
                            (SortOrder.Descending == compareColumn.SortOrder) ? -1 :
                                                                        0;
                        compareResult *= sortOrderModifierColumn;
                        break;
                    }
                }
                else
                {
                    // 文字列型の列の場合
                    compareResult = System.String.Compare(
                        (gridRowX.Cells[compareColumn.ColumnName].Value as string) ?? String.Empty,
                        (gridRowY.Cells[compareColumn.ColumnName].Value as string) ?? String.Empty);
                    if (0 != compareResult)
                    {
                        var sortOrderModifierColumn =
                            (SortOrder.Ascending == compareColumn.SortOrder) ? 1 :
                            (SortOrder.Descending == compareColumn.SortOrder) ? -1 :
                                                                        0;
                        compareResult *= sortOrderModifierColumn;
                        break;
                    }
                }
            }

            // 昇順、降順によって、結果を反転
            var sortOrderModifier =
                (SortOrder.Ascending == this.sortOrder) ? 1 :
                (SortOrder.Descending == this.sortOrder) ? -1 :
                                                            0;
            return compareResult * sortOrderModifier;
        }
    }
}
