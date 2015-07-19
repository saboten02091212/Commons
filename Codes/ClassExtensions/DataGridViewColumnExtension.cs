// ******************************************************************
// DataGridViewColumnExtension.cs ： DataGridViewColumn拡張クラス
// 作成日　：2014/05/14
// 更新履歴：2014/05/14 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// DataGridViewColumnクラスを拡張するクラスです。
    /// </summary>
    public static class DataGridViewColumnExtension
    {
        /// <summary>
        /// 列の値の型を取得します。
        /// </summary>
        /// <param name="gridColumn">DataGridViewColumnオブジェクト</param>
        /// <returns> セルの値の型</returns>
        public static Type GetValueType(this DataGridViewColumn gridColumn)
        {
            Type valueType = null;

            var grid = gridColumn.DataGridView;

            foreach (DataGridViewRow gridRow in grid.Rows)
            {
                valueType = gridRow.Cells[gridColumn.Index].GetValueType();
                if (null != valueType)
                {
                    break;
                }
            }

            return valueType;
        }
    }
}
