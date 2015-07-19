// ******************************************************************
// Sheets.cs  ：Excel.Sheetsラップクラス
// 作成日　：2013/08/13
// 更新履歴：2013/08/13 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Excel
{
    /// <summary>
    /// Excel.Sheetsクラスをラップするクラスです。
    /// </summary>
    public class Sheets : IEnumerable<Worksheet>, IDisposable
    {
        #region フィールド
        /// <summary>
        /// 実体シートコレクション
        /// </summary>
        private dynamic interopSheets;
        /// <summary>
        /// ワークシートのリスト
        /// </summary>
        private IList<Worksheet> worksheetList;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        /// <summary>
        /// 指定された番号のワークシートを取得します。
        /// </summary>
        /// <param name="index">番号</param>
        /// <returns>ワークシート</returns>
        public Worksheet this[int index]
        {
            get
            {
                return worksheetList[index];
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="interopSheets">実体ワークシート</param>
        public Sheets(object interopSheets)
        {
            // リソース解放フラグ設定
            this.disposed = false;

            // 実体ワークシートコレクションインスタンス設定
            this.interopSheets = interopSheets;

            // ワークシートリスト生成
            this.worksheetList = new List<Worksheet>();
            foreach (var interopWorkSheet in this.interopSheets)
            {
                Worksheet worksheet = new Worksheet(interopWorkSheet);
                this.worksheetList.Add(worksheet);
            }
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~Sheets()
        {
            this.Dispose(false);
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
            foreach (var worksheet in this.worksheetList)
            {
                ((IDisposable)worksheet).Dispose();
            }
            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            return;
        }
        #endregion
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// sheetの末尾に新しいsheetを追加します。
        /// </summary>
        public void Add()
        {
            this.interopSheets.Add(Type.Missing, this.interopSheets[this.interopSheets.Count]);

            // ワークシートリスト生成
            Worksheet worksheet = new Worksheet(this.interopSheets[this.interopSheets.Count]);
            this.worksheetList.Add(worksheet);

            return;
        }
        #endregion

        #region IEnumerable<Worksheet> メンバー
        /// <summary>
        /// Worksheet型のイテレータを取得します。
        /// </summary>
        /// <returns>イテレータ</returns>
        IEnumerator<Worksheet> IEnumerable<Worksheet>.GetEnumerator()
        {
            foreach (var worksheet in this.worksheetList)
            {
                yield return worksheet;
            }
        }
        #endregion

        #region IEnumerable メンバー
        /// <summary>
        /// イテレータを取得します。
        /// </summary>
        /// <returns>イテレータ</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var worksheet in this.worksheetList)
            {
                yield return worksheet;
            }
        }
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
