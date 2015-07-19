// ******************************************************************
// Waiter.cs  ：待機状態クラス
// 作成　　：2013/04/17 水落
// 更新履歴：2013/04/17 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.Forms
{
    /// <summary>
    /// 待機状態であることを示すクラスです。
    /// </summary>
    public class Waiter : IDisposable
    {
        #region フィールド
        /// <summary>
        /// カーソルを変更したかを示します。
        /// </summary>
        private bool changesCursol;
        /// <summary>
        /// 待機状態とするコントロールを示します。
        /// </summary>
        private Control control;
        /// <summary>
        /// 変更前のカーソルを保持します。
        /// </summary>
        private Cursor preCursor;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたキーでインスタンスを生成します。
        /// </summary>
        /// <param name="control">待機状態とするコントロール</param>
        public Waiter(Control control)
            : this(control, Cursors.WaitCursor)
        {
        }

        /// <summary>
        /// 指定されたキーでインスタンスを生成します。
        /// </summary>
        /// <param name="control">待機状態とするコントロール</param>
        /// <param name="preCursor">変更しないカーソル状態</param>
        public Waiter(Control control, Cursor preCursor)
        {
            // リソース解放フラグ設定
            this.disposed = false;

            // メンバ初期化
            this.changesCursol = false;
            this.control = control;
            this.preCursor = preCursor;

            // カーソル状態を待機状態に変更
            if (this.control.Cursor != this.preCursor)
            {
                this.preCursor = this.control.Cursor;
                this.control.Cursor = Cursors.WaitCursor;
                this.changesCursol = true;
            }
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~Waiter()
        {
            ((IDisposable)this).Dispose();
        }
        #endregion

        #region IDisposable メンバー
        void IDisposable.Dispose()
        {
            // 既に解放されていた場合、再度 解放しない
            if (true == this.disposed)
            {
                return;
            }

            // カーソル状態を元に戻す
            if (true == this.changesCursol)
            {
                this.control.Cursor = this.preCursor;
            }

            // ガベージコレクターに解放通知
            GC.SuppressFinalize(this);
            return;
        }
        #endregion
    }
}
