// ******************************************************************
// Transaction.cs  ：FireBirdトランザクションクラス
// 作成日　：2012/12/12
// 更新履歴：2013/02/15 水落　　 名前空間を変更。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace Mizuochi.Commons.Codes.Databases.FireBird
{
    /// <summary>
    /// トランザクションを開始するためのクラスです。
    /// </summary>
    public class Transaction : IDisposable
    {
        #region 静的
        #region 静的フィールド
        /// <summary>
        /// トランザクションのオプションを取得、設定します。
        /// </summary>
        private static FbTransactionOptions fbTransactionOption;
        #endregion

        #region 静的プロパティ
        /// <summary>
        /// トランザクションのオプションを取得、設定します。
        /// </summary>
        public static FbTransactionOptions FbTransactionOption
        {
            get { return Transaction.fbTransactionOption; }
            set { Transaction.fbTransactionOption = value; }
        }
        #endregion

        #region 静的コンストラクタ
        /// <summary>
        /// 静的フィールドの初期化を行います。
        /// </summary>
        static Transaction()
        {
            Transaction.fbTransactionOption = new FbTransactionOptions();
        }
        #endregion
        #endregion

        #region フィールド
        /// <summary>
        /// コミット、ロールオーバーが発生したかどうかを示します。
        /// </summary>
        private bool isOver;
        /// <summary>
        /// このトランザクションの接続実体オブジェクトを保持します。
        /// </summary>
        private ConnectionSubstance connectionSubstance;
        /// <summary>
        /// トランザクション名に対応したトランザクション実体オブジェクトを保持します。
        /// </summary>
        private TransactionSubstance transactionSubstance;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="connectionSubstance">接続実体オブジェクト</param>
        internal Transaction(ConnectionSubstance connectionSubstance)
        {
            this.isOver = false;
            this.connectionSubstance = connectionSubstance;
            this.transactionSubstance = connectionSubstance.TransactionSubstance;
            this.disposed = false;

            this.transactionSubstance.BeginTransaction();
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~Transaction()
        {
            this.Dispose(false);
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// このトランザクションに紐付いたコマンドオブジェクトを生成します。
        /// </summary>
        /// <param name="commandText">実行するSQL文</param>
        /// <returns>コマンドオブジェクト</returns>
        public Command CreateCommand(string commandText = "")
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            return this.transactionSubstance.CreateCommand(commandText);
        }

        /// <summary>
        /// トランザクションをコミットします。
        /// </summary>
        public void Commit()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // コミット、ロールオーバーが発生したか判定
            if (true == this.isOver)
            {
                throw new InvalidOperationException("既にコミット。もしくは、ロールオーバー済みです。");
            }

            // コミット処理
            this.transactionSubstance.Commit();

            this.isOver = true;

            return;
        }

        /// <summary>
        /// トランザクションをロールバックします。
        /// </summary>
        public void Rollback()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // コミット、ロールオーバーが発生したか判定
            if (true == this.isOver)
            {
                throw new InvalidOperationException("既にコミット。もしくは、ロールオーバー済みです。");
            }

            // ロールオーバー処理
            this.transactionSubstance.Rollback();

            this.isOver = true;

            return;
        }

        #region IDisposable メンバ
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
            ((IDisposable)this.transactionSubstance).Dispose();
            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            // コミット、ロールオーバーが発生していなければ、ロールバック処理を行う
            if (false == this.isOver)
            {
                this.transactionSubstance.Rollback();
            }

            return;
        }
        #endregion
        #endregion
    }
}
