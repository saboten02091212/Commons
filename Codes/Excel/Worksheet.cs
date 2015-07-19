// ******************************************************************
// Worksheet.cs  ：Excel.Worksheetラップクラス
// 作成日　：2013/08/13
// 更新履歴：2014/07/24 水落　　 Column、Rowsプロパティを追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Excel
{
    /// <summary>
    /// Excel.Worksheetクラスをラップするクラスです。
    /// </summary>
    public class Worksheet : IDisposable
    {
        #region フィールド
        /// <summary>
        /// 実体ワークシート
        /// </summary>
        private dynamic interopWorkSheet;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        /// <summary>
        /// ワークシート上の 1 つ以上の列を取得します。
        /// </summary>
        public dynamic Columns
        {
            get
            {
                return this.interopWorkSheet.Columns;
            }
        }

        /// <summary>
        /// ワークシート上の 1 つ以上の行を取得します。
        /// </summary>
        public dynamic Rows
        {
            get
            {
                return this.interopWorkSheet.Rows;
            }
        }

        /// <summary>
        /// ワークシートの全てのセルを取得します。
        /// </summary>
        public dynamic Cells
        {
            get
            {
                return this.interopWorkSheet.Cells;
            }
        }

        /// <summary>
        /// ワークシートの全てのページ設定を取得します。
        /// </summary>
        public dynamic PageSetup
        {
            get
            {
                return this.interopWorkSheet.PageSetup;
            }
        }

        /// <summary>
        /// ワークシートの全てのページ設定を取得します。
        /// </summary>
        public dynamic Name
        {
            get
            {
                return this.interopWorkSheet.Name;
            }
            set
            {
                this.interopWorkSheet.Name = value;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="interopWorkSheet">実体ワークシート</param>
        internal Worksheet(object interopWorkSheet)
        {
            // リソース解放フラグ設定
            this.disposed = false;

            // 実体ワークシートインスタンス設定
            this.interopWorkSheet = interopWorkSheet;
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~Worksheet()
        {
            this.Dispose(false);
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// シートを選択状態にします。
        /// </summary>
        public void Select()
        {
            this.interopWorkSheet.Select();
            return;
        }

        /// <summary>
        /// セル範囲を取得します。
        /// </summary>
        /// <param name="Cell1">開始セル</param>
        /// <param name="Cell2">終了セル</param>
        /// <returns>セル範囲</returns>
        public dynamic GetRange(object Cell1, object Cell2 = null)
        {
            return this.interopWorkSheet.Range[Cell1, Cell2 ?? Type.Missing];
        }
        #endregion

        #region プライベートメソッド
        #region リソース解放
        /// <summary>
        /// 保有しているリソースを解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースを解放する場合、true。それ以外の場合、falseを設定します。</param>
        private void Dispose(bool disposing)
        {
            // 既に解放されていた場合、再度 解放しない
            if (true == this.disposed)
            {
                return;
            }

            // マネージリソースを解放
            if (true == disposing)
            {
                this.ReleaseManagedResource();
            }

            // アンマネージリソースを解放
            this.ReleaseUnmanagedResource();

            this.disposed = true;

            return;
        }

        /// <summary>
        /// マネージリソースを解放します。
        /// </summary>
        private void ReleaseManagedResource()
        {
            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(this.interopWorkSheet);

            return;
        }
        #endregion
        #endregion

        #region IDisposable メンバー
        /// <summary>
        /// このインスタンスで管理しているリソースを解放します。
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);

            return;
        }
        #endregion
    }
}
