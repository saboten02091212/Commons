// ******************************************************************
// Workbooks.cs  ：Excel.Workbooksラップクラス
// 作成日　：2013/08/13
// 更新履歴：2013/10/09 水落　　 ブック種類を列挙型に修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mizuochi.Commons.Codes.Excel.ConstWrappers;

namespace Mizuochi.Commons.Codes.Excel
{
    /// <summary>
    /// Excel.Workbooksクラスをラップするクラスです。
    /// </summary>
    public class Workbooks : IEnumerable<Workbook>, IDisposable
    {
        #region フィールド
        /// <summary>
        /// 実体ワークブックコレクション
        /// </summary>
        private dynamic interopWorkbooks;
        /// <summary>
        /// ワークブックのリスト
        /// </summary>
        private IList<Workbook> workbookList;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        /// <summary>
        /// 指定された番号のワークブックを取得します。
        /// </summary>
        /// <param name="index">番号</param>
        /// <returns>ワークブック</returns>
        Workbook this[int index]
        {
            get
            {
                return workbookList[index];
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="interopWorkbooks">実体ワークブックコレクション</param>
        internal Workbooks(object interopWorkbooks)
        {
            // リソース解放フラグ設定
            this.disposed = false;

            // 実体ワークブックコレクションインスタンス設定
            this.interopWorkbooks = interopWorkbooks;

            // ワークブックリスト生成
            this.workbookList = new List<Workbook>();
            foreach (var interopWorkbook in this.interopWorkbooks)
            {
                Workbook workbook = new Workbook(interopWorkbook);
                this.workbookList.Add(workbook);
            }
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~Workbooks()
        {
            this.Dispose(false);
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// ワークブックを追加します。
        /// </summary>
        /// <param name="xlWBATemplate">ブックの種類</param>
        /// <returns>追加されたワークブックインスタンス</returns>
        public Workbook Add(XlWBATemplate? xlWBATemplate = null)
        {
            // 実体操作
            object paramXlWBATemplate = (null != xlWBATemplate) ? xlWBATemplate : Type.Missing;
            dynamic interopWorkbook = this.interopWorkbooks.Add(paramXlWBATemplate);

            // ワークブック追加
            Workbook workbook = new Workbook(interopWorkbook);
            this.workbookList.Add(workbook);

            return workbook;
        }

        /// <summary>
        /// ワークブックを開きます。
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <returns>開いたワークブックインスタンス</returns>
        public Workbook Open(string filename)
        {
            // 実体操作
            dynamic interopWorkbook = this.interopWorkbooks.Open(filename);

            // ワークブック追加
            Workbook workbook = new Workbook(interopWorkbook);
            this.workbookList.Add(workbook);

            return workbook;
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
            foreach (var workbook in this.workbookList)
            {
                ((IDisposable)workbook).Dispose();
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

        #region IEnumerable<Workbook> メンバー
        /// <summary>
        /// Workbook型のイテレータを取得します。
        /// </summary>
        /// <returns>イテレータ</returns>
        IEnumerator<Workbook> IEnumerable<Workbook>.GetEnumerator()
        {
            foreach (var workbook in this.workbookList)
            {
                yield return workbook;
            }
        }
        #endregion

        #region IEnumerable メンバー
        /// <summary>
        /// イテレータを取得します。
        /// </summary>
        /// <returns>イテレータ</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var workbook in this.workbookList)
            {
                yield return workbook;
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
