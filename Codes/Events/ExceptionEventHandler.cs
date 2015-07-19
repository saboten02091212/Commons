// ******************************************************************
// ExceptionEventHandler.cs ： 例外イベントハンドラ
// 作成日　：2014/07/04
// 更新履歴：2014/07/04 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Events
{
    /// <summary>
    /// 例外情報を持ったイベントデータのイベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="sender">イベントのソースです。</param>
    /// <param name="e">例外情報を持ったイベントデータを格納しています。</param>
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

    /// <summary>
    /// ExceptionEventHandler 向けのデータを提供します。
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 発生した例外情報
        /// </summary>
        private Exception exception;
        /// <summary>
        /// キャンセルフラグ
        /// </summary>
        private bool cancelFlg;

        /// <summary>
        /// 発生した例外情報を取得します。
        /// </summary>
        public Exception Exception
        {
            get
            {
                return exception;
            }
        }

        /// <summary>
        /// キャンセルするかどうかを示す値を取得または設定します。
        /// </summary>
        public bool CancelFlg
        {
            get
            {
                return cancelFlg;
            }
            set
            {
                cancelFlg = value;
            }
        }

        /// <summary>
        /// 指定された例外情報を元にインスタンスを生成します。
        /// </summary>
        /// <param name="exception"></param>
        public ExceptionEventArgs(Exception exception)
        {
            this.exception = exception;
            this.cancelFlg = false;
        }
    }
}
