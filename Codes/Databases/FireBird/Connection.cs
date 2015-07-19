// ******************************************************************
// Connection.cs  ：FireBirdデータベース接続クラス
// 作成日　：2012/12/12
// 更新履歴：2014/03/10 水落　　 IConnectionを実装するよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Databases.FireBird
{
    /// <summary>
    /// データベース接続を開くクラスです。
    /// </summary>
    public class Connection : IConnection
    {
        #region 静的
        #region 静的フィールド
        /// <summary>
        /// デフォルトの接続文字列です。
        /// </summary>
        private static string defaultConnectionString;
        /// <summary>
        /// コネクションを行う接続実体クラスのディクショナリです。
        /// </summary>
        private static Dictionary<string, ConnectionSubstance> dictConnectionSubstance;
        #endregion

        #region 静的プロパティ
        /// <summary>
        /// デフォルトの接続文字列を取得、設定します。
        /// </summary>
        public static string DefaultConnectionString
        {
            get { return Connection.defaultConnectionString; }
            set { Connection.defaultConnectionString = value; }
        }
        #endregion

        #region 静的コンストラクタ
        /// <summary>
        /// 静的フィールドの初期化を行います。
        /// </summary>
        static Connection()
        {
            Connection.dictConnectionSubstance = new Dictionary<string, ConnectionSubstance>();
        }
        #endregion

        #region 静的パブリックメソッド
        /// <summary>
        /// 指定された接続文字列を持つオブジェクトをコネクションを行う接続実体クラスのディクショナリから削除します。
        /// </summary>
        /// <param name="connectionString">削除するオブジェクトの接続文字列</param>
        internal static void ReleaseConnectionSubstance(string connectionString)
        {
            Connection.dictConnectionSubstance.Remove(connectionString);
            return;
        }
        #endregion
        #endregion

        #region フィールド
        /// <summary>
        /// コネクションが接続したかどうかを示します。
        /// </summary>
        private ConnectionState state;
        /// <summary>
        /// コネクションが接続したかどうかを示します。
        /// </summary>
        private bool isOpened;
        /// <summary>
        /// コネクションが切断したかどうかを示します。
        /// </summary>
        private bool isClosed;
        /// <summary>
        /// 接続文字列を保持します。
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 接続文字列に対応した接続実体オブジェクトを保持します。
        /// </summary>
        private ConnectionSubstance connectionSubstance;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        #region IConnection メンバー
        /// <summary>
        /// コネクションの状態を取得します。
        /// </summary>
        public System.Data.ConnectionState State
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 接続文字列を取得します。
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
        }
        #endregion
        #endregion

        #region コンストラクタ
        /// <summary>
        /// デフォルトの接続文字列でインスタンスを生成します。
        /// </summary>
        public Connection()
            : this(Connection.defaultConnectionString)
        {
        }

        /// <summary>
        /// 指定された接続文字列でインスタンスを生成します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        public Connection(string connectionString)
        {
            this.isOpened = false;
            this.isClosed = false;
            this.connectionString = connectionString;
            this.connectionSubstance = this.GetConnectionSubstance();
            this.disposed = false;
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~Connection()
        {
            this.Dispose(false);
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// トランザクションを始めるため、トランザクションインスタンスを生成します。
        /// </summary>
        /// <returns>トランザクションインスタンス</returns>
        public Transaction BeginTransaction()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            return this.connectionSubstance.BeginTransaction();
        }

        #region IConnection メンバー
        /// <summary>
        /// データベース接続を開きます。
        /// </summary>
        public void Open()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // 既に接続されているか判定
            if (ConnectionState.Open == this.state)
            {
                throw new InvalidOperationException("既に接続されています。");
            }

            // DB接続
            this.connectionSubstance.Open();

            this.state = ConnectionState.Open;

            return;
        }

        /// <summary>
        /// データベースへの接続を閉じます。
        /// </summary>
        public void Close()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // 接続されたか判定
            if (ConnectionState.Closed == this.state)
            {
                throw new InvalidOperationException("接続されていません。");
            }

            // DB切断
            this.connectionSubstance.Close();

            this.state = ConnectionState.Closed;

            return;
        }

        /// <summary>
        /// コマンドインスタンスを生成します。
        /// </summary>
        /// <param name="commandText">コマンドテキスト</param>
        /// <returns>コマンドインスタンス</returns>
        public ICommand CreateCommand(string commandText = "")
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // 接続しているか判定
            if (ConnectionState.Closed == this.state)
            {
                throw new InvalidOperationException("接続されていません。");
            }

            // トランザクション作成済みか判定
            if (null == this.connectionSubstance.TransactionSubstance)
            {
                throw new InvalidOperationException("トランザクションが作成されてません。");
            }

            return this.connectionSubstance.TransactionSubstance.CreateCommand(commandText);
        }

        /// <summary>
        /// 接続できるかどうか判定します。
        /// </summary>
        /// <returns>接続できる場合、true。接続できない場合、falseを返します。</returns>
        public bool CanConnect()
        {
            bool result = true;
            bool openFlg = false;
            try
            {
                // 接続が開かれてなければ開く
                if (ConnectionState.Open != this.state)
                {
                    this.Open();
                    openFlg = true;
                }

                // コマンドインスタンス生成
                string commandText = "SELECT CURRENT_TIMESTAMP";
                using (ICommand command = this.CreateCommand(commandText))
                {
                    // コマンド発行
                    DataSet tmpDataSet;
                    command.Fill(out tmpDataSet);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                // 開いたら閉じる
                if (true == openFlg)
                {
                    this.Close();
                }
            }

            return result;
        }
        #endregion

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
        /// <summary>
        /// 接続文字列に対応した、接続実体オブジェクトを取得します。
        /// </summary>
        /// <returns>接続実体オブジェクト</returns>
        private ConnectionSubstance GetConnectionSubstance()
        {
            // 同じ接続文字列の接続実体オブジェクトがあれば、それを使用する
            if (false == Connection.dictConnectionSubstance.ContainsKey(this.connectionString))
            {
                // 新しい接続実体オブジェクトを作成し、一覧に追加
                ConnectionSubstance connection = new ConnectionSubstance(this.connectionString);
                Connection.dictConnectionSubstance.Add(connectionString, connection);
            }

            return Connection.dictConnectionSubstance[this.connectionString];
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
            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            if ((true == this.isOpened) && (false == this.isClosed))
            {
                this.Close();
            }

            ((IDisposable)this.connectionSubstance).Dispose();
            
            return;
        }
        #endregion
        #endregion
    }
}
