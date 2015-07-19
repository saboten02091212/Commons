// ******************************************************************
// ExcelApplication.cs  ：Excel.Applicationラップクラス
// 作成日　：2013/08/12
// 更新履歴：2013/10/08 水落　　 型取得メソッドを追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Excel
{
    /// <summary>
    /// Excel.Applicationクラスをラップするクラスです。
    /// </summary>
    public class ExcelApplication : IDisposable
    {
        #region 定数
        /// <summary>
        /// ExcelプログラムID
        /// </summary>
        private const string ExcelApplicationProgramId = "Excel.Application";
        #endregion

        #region フィールド
        /// <summary>
        /// 実体Excelアプリケーション
        /// </summary>
        private dynamic interopExcelApplication;
        /// <summary>
        /// ワークブックコレクション
        /// </summary>
        private Workbooks workbooks;
        /// <summary>
        /// 終了フラグ
        /// </summary>
        private bool quitFlg;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        /// <summary>
        /// Excelアプリケーションを取得します。
        /// </summary>
        internal dynamic InteropExcelApplication
        {
            get
            {
                return interopExcelApplication;
            }
        }

        /// <summary>
        /// アプリケーションで管理しているワークブックのコレクションを取得します。
        /// </summary>
        public Workbooks Workbooks
        {
            get
            {
                return workbooks;
            }
        }

        /// <summary>
        /// バージョンを取得します。
        /// </summary>
        public string Version
        {
            get
            {
                return this.interopExcelApplication.Version;
            }
        }

        /// <summary>
        /// セル範囲を取得します。
        /// </summary>
        public dynamic Range
        {
            get
            {
                return this.interopExcelApplication.Range;
            }
        }

        /// <summary>
        /// Excelを表示させるかどうかを取得、設定します。
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.interopExcelApplication.Visible;
            }
            set
            {
                this.interopExcelApplication.Visible = value;
            }
        }

        /// <summary>
        /// Excelを表示させるかどうかを取得、設定します。
        /// </summary>
        public bool DisplayAlerts
        {
            get
            {
                return this.interopExcelApplication.DisplayAlerts;
            }
            set
            {
                this.interopExcelApplication.DisplayAlerts = value;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタを元にインスタンスを生成します。
        /// </summary>
        /// <param name="quitFlg">終了フラグ</param>
        public ExcelApplication(bool quitFlg = true)
        {
            // リソース解放フラグ設定
            this.disposed = false;

            // Excelアプリケーションインスタンス生成
            Type excelAppType = Type.GetTypeFromProgID(ExcelApplication.ExcelApplicationProgramId, true);
            interopExcelApplication = Activator.CreateInstance(excelAppType);

            // ワークブックコレクション生成
            this.workbooks = new Workbooks(this.interopExcelApplication.Workbooks);

            // 終了フラグ設定
            this.quitFlg = quitFlg;
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~ExcelApplication()
        {
            this.Dispose(false);
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// セル範囲を取得します。
        /// </summary>
        /// <param name="Cell1">開始セル</param>
        /// <param name="Cell2">終了セル</param>
        /// <returns>セル範囲</returns>
        public dynamic GetRange(object Cell1, object Cell2 = null)
        {
            return this.interopExcelApplication.get_Range(Cell1, Cell2 ?? Type.Missing);
        }
        #endregion

        #region インターナルメソッド
        /// <summary>
        /// 指定された名前空間のTypeオブジェクトを取得します。
        /// </summary>
        /// <param name="name">名前空間</param>
        /// <returns>Typeオブジェクト</returns>
        internal static Type GetType(string name)
        {
            Type excelAppType = Type.GetTypeFromProgID(ExcelApplication.ExcelApplicationProgramId, true);
            Type type = excelAppType.Assembly.GetType(name);
            return type;
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
            ((IDisposable)this.workbooks).Dispose();
            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            if (true == this.quitFlg)
            {
                this.interopExcelApplication.Quit();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(this.interopExcelApplication);

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
