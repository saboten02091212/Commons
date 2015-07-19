// ******************************************************************
// DataGridViewCellExtension.cs ： DataGridViewCell拡張クラス
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
    /// DataGridViewCellクラスを拡張するクラスです。
    /// </summary>
    public static class DataGridViewCellExtension
    {
        /// <summary>
        /// セルの値の型を取得します。
        /// </summary>
        /// <param name="gridCell">DataGridViewCellオブジェクト</param>
        /// <returns> セルの値の型</returns>
        public static Type GetValueType(this DataGridViewCell gridCell)
        {
            Type valueType = null;

            if (null != gridCell.Value)
            {
                valueType = gridCell.Value.GetType();
            }

            return valueType;
        }
    }
}
