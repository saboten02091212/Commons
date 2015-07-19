// ******************************************************************
// Workbook.cs  ：Excel.Workbookラップクラス
// 作成日　：2013/08/13
// 更新履歴：2014/05/15 水落　　 コンストラクタのスコープをinternalに変更。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Excel
{
    /// <summary>
    /// Excel.Workbookクラスをラップするクラスです。
    /// </summary>
    public class Workbook : IDisposable
    {
        #region フィールド
        /// <summary>
        /// 実体ワークブック
        /// </summary>
        private dynamic interopWorkbook;
        /// <summary>
        /// シートコレクション
        /// </summary>
        private Sheets sheets;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion
        
        #region プロパティ
        /// <summary>
        /// シートコレクションを取得します。
        /// </summary>
        public Sheets Sheets
        {
            get
            {
                return sheets;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="interopWorkbook">実体ワークブック</param>
        internal Workbook(object interopWorkbook)
        {
            // リソース解放フラグ設定
            this.disposed = false;

            // 実体ワークブックインスタンス設定
            this.interopWorkbook = interopWorkbook;

            // シートコレクション生成
            this.sheets = new Sheets(this.interopWorkbook.Sheets);
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~Workbook()
        {
            this.Dispose(false);
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// ワークブックを保存します。
        /// </summary>
        public void SaveAs(object Filename = null)
        {
            this.interopWorkbook.SaveAs(Filename: Filename);
            return;
        }
        /// <summary>
        /// ワークブックを閉じます。
        /// </summary>
        public void Close(object SaveChanges = null, object Filename = null, object RouteWorkbook = null)
        {
            this.interopWorkbook.Close(
                SaveChanges: SaveChanges ?? Type.Missing,
                Filename: Filename ?? Type.Missing,
                RouteWorkbook: RouteWorkbook ?? Type.Missing);
            return;
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
            ((IDisposable)this.sheets).Dispose();
            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(this.interopWorkbook);

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
