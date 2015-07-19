// ******************************************************************
// ValueChangeEventArgs.cs ： Value変更イベントデータクラス
// 作成日　：2013/06/24
// 更新履歴：2013/06/24 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Controls.Events
{
    /// <summary>
    /// Value変更イベントハンドラ
    /// </summary>
    /// <param name="sender">イベントのソースです。</param>
    /// <param name="e">イベントデータを格納しています。</param>
    public delegate void ValueChangeEventhandler(object sender, ValueChangeEventArgs e);

    /// <summary>
    /// Value変更イベントのデータを提供します。
    /// </summary>
    public class ValueChangeEventArgs : EventArgs
    {
        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="preValue">変更前の値</param>
        /// <param name="currentValue">変更後の値</param>
        public ValueChangeEventArgs(decimal preValue, decimal currentValue)
        {
            this.preValue = preValue;
            this.currentValue = currentValue;
        }
        #endregion

        #region フィールド
        /// <summary>
        /// 変更前の値
        /// </summary>
        private decimal preValue;
        /// <summary>
        /// 変更後の値
        /// </summary>
        private decimal currentValue;
        #endregion

        #region プロパティ
        /// <summary>
        /// 変更前の値
        /// </summary>
        public decimal PreValue
        {
            get
            {
                return preValue;
            }
        }

        /// <summary>
        /// 変更後の値
        /// </summary>
        public decimal CurrentValue
        {
            get
            {
                return currentValue;
            }
        }
        #endregion
    }
}
