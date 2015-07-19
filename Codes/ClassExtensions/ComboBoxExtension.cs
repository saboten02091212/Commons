// ******************************************************************
// ComboBoxExtension.cs ： ComboBox拡張クラス
// 作成日　：2013/05/27
// 更新履歴：2013/05/27 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// ComboBoxクラスを拡張するクラスです。
    /// </summary>
    public static class ComboBoxExtension
    {
        /// <summary>
        /// 指定した値を持つ項目を選択します。
        /// </summary>
        /// <param name="comboBox">コンボボックス</param>
        /// <param name="setValue">選択する値</param>
        /// <returns>コンボボックス</returns>
        public static ComboBox SetSelectedValue(this ComboBox comboBox, object setValue)
        {
            // 指定値がnullか判定
            if (null == setValue)
            {
                comboBox.SelectedItem = null;
                return comboBox;
            }

            // データが存在するか判定
            if (0 >= comboBox.Items.Count)
            {
                comboBox.SelectedItem = null;
                return comboBox;
            }

            // プロパティが存在するか判定
            Type itemType = comboBox.Items[0].GetType();
            PropertyInfo valueProperty = itemType.GetProperty(comboBox.ValueMember);
            if (null == valueProperty)
            {
                comboBox.SelectedItem = null;
                return comboBox;
            }

            // IComparableを実装しているか判定
            Type interfaceType = setValue.GetType().GetInterface("System.IComparable");

            // 指定した値の項目を取得
            object selectedItem = null;
            foreach (object item in comboBox.Items)
            {
                object value = valueProperty.GetValue(item, null);

                // CompareToメソッドが使用できれば使用する
                if (null != interfaceType)
                {
                    if (0 == ((IComparable)setValue).CompareTo(value))
                    {
                        selectedItem = item;
                        break;
                    }
                }
                else
                {
                    if (value == setValue)
                    {
                        selectedItem = item;
                        break;
                    }
                }
            }

            // 指定項目を選択状態にする
            comboBox.SelectedItem = selectedItem;
            return comboBox;
        }
    }
}
