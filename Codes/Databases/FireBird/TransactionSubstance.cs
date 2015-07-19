// ******************************************************************
// TransactionSubstance.cs  ：FireBirdトランザクション実体クラス
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
    /// 複数個所から同一トランザクションオブジェクトを利用するためのクラスです。
    /// </summary>
    internal class TransactionSubstance : IDisposable
    {
        #region フィールド
        /// <summary>
        /// このトランザクション実体オブジェクトの参照数です。
        /// </summary>
        private int count;
        /// <summary>
        /// ロールバックされたかどうかを示します。
        /// </summary>
        private bool isRollback;
        /// <summary>
        /// このトランザクションの接続実体オブジェクトを保持します。
        /// </summary>
        private ConnectionSubstance connectionSubstance;
        /// <summary>
        /// FireBird用のトランザクションオブジェクトです。
        /// </summary>
        private FbTransaction transaction;
        /// <summary>
        /// このトランザクションを利用しているコマンドのリストです。
        /// </summary>
        private List<Command> commandList;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        /// <summary>
        /// FireBird用の接続オブジェクトを取得します。
        /// </summary>
        internal FbTransaction FbTransaction
        {
            get
            {
                return transaction;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="connectionSubstance">接続実体オブジェクト</param>
        internal TransactionSubstance(ConnectionSubstance connectionSubstance)
        {
            this.count = 0;
            this.isRollback = false;
            this.connectionSubstance = connectionSubstance;
            this.transaction = this.connectionSubstance.FbConnection.BeginTransaction(Transaction.FbTransactionOption);
            this.commandList = new List<Command>();
            this.disposed = false;
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// トランザクションを始めます。
        /// </summary>
        internal void BeginTransaction()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            this.count += 1;
        }

        /// <summary>
        /// トランザクションの利用数が0の場合、コミットします。
        /// </summary>
        internal void Commit()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // トランザクションの利用数(入れ子数)をデクリメント
            this.count -= 1;

            // ロールバックが発生したか判定
            if (true == this.isRollback)
            {
                return;
            }

            // トランザクションの利用数(入れ子数)が0ならコミット処理
            if (0 == this.count)
            {
                this.transaction.Commit();
            }

            return;
        }

        /// <summary>
        /// トランザクションの利用数が0の場合、ロールバックします。
        /// </summary>
        internal void Rollback()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // トランザクションの利用数(入れ子数)をデクリメント
            this.count -= 1;

            // 既にロールバックが発生したか判定
            if (true == this.isRollback)
            {
                return;
            }

            // ロールバック処理
            this.transaction.Rollback();
            this.isRollback = true;

            return;
        }

        /// <summary>
        /// このトランザクションに紐付いたコマンドオブジェクトを生成します。
        /// </summary>
        /// <param name="commandText">実行するSQL文</param>
        /// <returns>コマンドオブジェクト</returns>
        public Command CreateCommand(string commandText)
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // コマンド生成
            Command command = new Command(commandText, this.connectionSubstance, this);
            this.commandList.Add(command);

            return command;
        }

        #region IDisposable メンバ
        /// <summary>
        /// このインスタンスで管理しているリソースを解放します。
        /// </summary>
        void IDisposable.Dispose()
        {
            // 上位メソッドでトランザクションを使用している場合、解放しない
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
            this.transaction.Dispose();
            foreach (Command command in this.commandList)
            {
                ((IDisposable)command).Dispose();
            }

            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            this.connectionSubstance.ReleaseTransaction();
            return;
        }
        #endregion
        #endregion
    }
}
