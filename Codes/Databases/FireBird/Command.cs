// ******************************************************************
// Command.cs  ：FireBirdデータベースコマンド発行クラス
// 作成日　：2013/01/26
// 更新履歴：2014/03/10 水落　　 ICommandを実装するよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace Mizuochi.Commons.Codes.Databases.FireBird
{
    /// <summary>
    /// データベースに対してコマンドを発行する機能を持ったクラスです。
    /// </summary>
    public class Command : ICommand, IDisposable
    {
        #region フィールド
        /// <summary>
        /// FireBirdのコマンド発行オブジェクト
        /// </summary>
        private FbCommand fbCommand;
        /// <summary>
        /// コマンドパラメタコレクション
        /// </summary>
        private Dictionary<string, object> parameters;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        /// <summary>
        /// 発行するSQL文を取得、設定します。
        /// </summary>
        public string CommandText
        {
            get
            {
                return fbCommand.CommandText;
            }
            set
            {
                fbCommand.CommandText = value;
            }
        }

        /// <summary>
        /// コマンドのパラメタコレクションを取得します。
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get
            {
                return parameters;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="commandText">SQL文</param>
        /// <param name="connectionSubstance">コネクションオブジェクト</param>
        /// <param name="transactionSubstance">トランザクションオブジェクト</param>
        internal Command(string commandText, ConnectionSubstance connectionSubstance, TransactionSubstance transactionSubstance)
        {
            this.fbCommand = new FbCommand(commandText, connectionSubstance.FbConnection, transactionSubstance.FbTransaction);
            this.parameters = new Dictionary<string, object>();
            this.disposed = false;
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// SELECT文を発行します。
        /// </summary>
        /// <param name="dataSet">取得したレコードセットが設定されます。</param>
        /// <returns>取得したレコード数</returns>
        public int Fill(out DataSet dataSet)
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // パラメタをFireBird用に変換し、コマンドオブジェクトに設定
            this.SetFbParameter();

            // SQLを発行し、データセットを取得
            DataSet ds = new DataSet();
            FbDataAdapter dataAdapter = new FbDataAdapter(this.fbCommand);
            int rowCount = dataAdapter.Fill(ds);

            dataSet = ds;
            return rowCount;
        }

        /// <summary>
        /// INSERT、UPDATE、DELETE文のいずれかを発行します。
        /// </summary>
        /// <returns>影響を与えたレコード数</returns>
        public object Execute()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // パラメタをFireBird用に変換し、コマンドオブジェクトに設定
            this.SetFbParameter();

            // SQLを発行
            int rowCount = this.fbCommand.ExecuteNonQuery();
            return rowCount;
        }

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
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// パラメタコレクションをFireBird用に変換し、コマンドオブジェクトに設定します。。
        /// </summary>
        private void SetFbParameter()
        {
            // 既存のパラメタを削除
            this.fbCommand.Parameters.Clear();

            // パラメタコレクションを変換、登録
            foreach (string key in this.parameters.Keys)
            {
                this.fbCommand.Parameters.Add(key, this.parameters[key]);
                this.fbCommand.Parameters[key].IsNullable = true;
            }

            return;
        }
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
            this.fbCommand.Dispose();
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
    }
}
