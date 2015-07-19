// ******************************************************************
// ConnectionSubstance.cs  ：FireBirdデータベース接続実体クラス
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
    /// 複数個所から同一接続オブジェクトを利用するためのクラスです。
    /// </summary>
    internal class ConnectionSubstance : IDisposable
    {
        #region フィールド
        /// <summary>
        /// この接続実体オブジェクトの参照数です。
        /// </summary>
        private int count;
        /// <summary>
        /// 接続文字列です。
        /// </summary>
        private string connectionString;
        /// <summary>
        /// FireBird用の接続オブジェクトです。
        /// </summary>
        private FbConnection connection;
        /// <summary>
        /// この接続オブジェクトで使用している、トランザクション実体オブジェクトです。
        /// </summary>
        private TransactionSubstance transactionSubstance;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        /// <summary>
        /// FireBird用の接続オブジェクトを取得します。
        /// </summary>
        internal FbConnection FbConnection
        {
            get
            {
                return connection;
            }
        }

        /// <summary>
        /// この接続オブジェクトで使用している、トランザクション実体オブジェクトのディクショナリを取得します。
        /// </summary>
        internal TransactionSubstance TransactionSubstance
        {
            get
            {
                return transactionSubstance;
            }
            set
            {
                transactionSubstance = value;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定された接続文字列でインスタンスを生成します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        internal ConnectionSubstance(string connectionString)
        {
            this.count = 0;
            this.connectionString = connectionString;
            this.connection = new FbConnection(this.connectionString);
            this.transactionSubstance = null;
            this.disposed = false;
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// コネクションの利用数が0の場合、データベース接続を開きます。
        /// </summary>
        internal void Open()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // 利用数をインクリメントし、利用していなければ接続
            if (0 == this.count)
            {
                this.connection.Open();
            }
            this.count += 1;

            return;
        }

        /// <summary>
        /// コネクションの利用数が0の場合、データベースへの接続を閉じます。
        /// </summary>
        internal void Close()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // 利用数をデクリメントし、利用していなければ切断
            this.count -= 1;
            if (0 == this.count)
            {
                this.connection.Close();
            }

            return;
        }

        /// <summary>
        /// トランザクションを始めるため、トランザクションインスタンスを生成します。
        /// </summary>
        /// <returns>トランザクションインスタンス</returns>
        internal Transaction BeginTransaction()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // 未だトランザクションが開始されていなければ、トランザクション実体インスタンスを生成
            if (null == this.transactionSubstance)
            {
                this.transactionSubstance = new TransactionSubstance(this);
            }

            return new Transaction(this);
        }

        /// <summary>
        /// トランザクション実体クラスを削除します。
        /// </summary>
        internal void ReleaseTransaction()
        {
            this.transactionSubstance = null;
            return;
        }

        #region IDisposable メンバ
        /// <summary>
        /// このインスタンスで管理しているリソースを解放します。
        /// </summary>
        void IDisposable.Dispose()
        {
            // 他にコネクションを使用している場合、解放しない
            if (0 < this.count)
            {
                return;
            }

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
            if (null != this.transactionSubstance)
            {
                ((IDisposable)this.transactionSubstance).Dispose();
            }
            this.connection.Dispose();

            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            Connection.ReleaseConnectionSubstance(this.connectionString);
            return;
        }
        #endregion
        #endregion
    }
}
