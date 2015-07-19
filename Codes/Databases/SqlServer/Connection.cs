// ******************************************************************
// Connection.cs  ：SqlServerデータベース接続クラス
// 作成日　：2013/02/15
// 更新履歴：2014/11/21 水落　　 デバック時にリトライしないよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using Mizuochi.Commons.Codes.Logs;

namespace Mizuochi.Commons.Codes.Databases.SqlServer
{
    /// <summary>
    /// SqlServerに接続するための機能を保有するクラスです。
    /// </summary>
    public class Connection : IConnection, IDisposable
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
        [ThreadStatic]
        private static Dictionary<string, ConnectionCounter> dictionarySqlConnection;
        /// <summary>
        /// リトライ設定情報です。
        /// </summary>
        private static IList<TimeSpan> retryTable;
        #endregion

        #region 静的プロパティ
        /// <summary>
        /// デフォルトの接続文字列を取得、設定します。
        /// </summary>
        public static string DefaultConnectionString
        {
            get
            {
                return Connection.defaultConnectionString;
            }
            set
            {
                Connection.defaultConnectionString = value;
            }
        }

        /// <summary>
        /// コネクションを行う接続実体クラスのディクショナリを取得します。
        /// </summary>
        private static Dictionary<string, ConnectionCounter> DictionarySqlConnection
        {
            get
            {
                // 生成されていなければ、生成
                if (null == Connection.dictionarySqlConnection)
                {
                    Connection.dictionarySqlConnection = new Dictionary<string, ConnectionCounter>();
                }

                return Connection.dictionarySqlConnection;
            }
        }

        /// <summary>
        /// リトライ設定情報です。
        /// </summary>
        public static IList<TimeSpan> RetryTable
        {
            get
            {
                return Connection.retryTable;
            }
        }
        #endregion

        #region 静的コンストラクタ
        /// <summary>
        /// 静的フィールドの初期化を行います。
        /// </summary>
        static Connection()
        {
            Connection.retryTable = new[]
            {
                new TimeSpan(0, 0, 0, 0, 100),
                new TimeSpan(0, 0, 0, 1, 0),
                new TimeSpan(0, 0, 0, 5, 0),
                new TimeSpan(0, 0, 0, 10, 0),
            };
        }
        #endregion

        #region 静的パブリックメソッド
        /// <summary>
        /// 指定された接続文字列を持つオブジェクトをコネクションを行う接続実体クラスのディクショナリから削除します。
        /// </summary>
        /// <param name="connectionString">削除するオブジェクトの接続文字列</param>
        internal static void ReleaseConnectionSubstance(string connectionString)
        {
            Connection.DictionarySqlConnection.Remove(connectionString);
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
        /// 接続文字列を保持します。
        /// </summary>
        private string connectionString;
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
        public ConnectionState State
        {
            get
            {
                return state;
            }
        }

        /// <summary>
        /// 接続文字列を取得します。
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
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
            this.state = ConnectionState.Closed;
            this.connectionString = connectionString;
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
        public SqlTransaction BeginTransaction()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // 接続しているかを判定します。
            if (ConnectionState.Closed == this.state)
            {
                throw new InvalidOperationException("接続されていません。");
            }

            return Connection.DictionarySqlConnection[this.connectionString].Connection.BeginTransaction();
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
            var retryTableWorker = Connection.retryTable.GetEnumerator();
            retryTableWorker.MoveNext();
            while (true)
            {
                lock (Connection.DictionarySqlConnection)
                {
                    // 接続されていなければ接続する
                    if (false == Connection.DictionarySqlConnection.ContainsKey(this.connectionString))
                    {
                        SqlConnection connection = null;
                        try
                        {
                            connection = new SqlConnection(this.connectionString);
                            connection.Open();
                            Connection.DictionarySqlConnection.Add(this.connectionString, new ConnectionCounter(0, connection));
                            //Connection.DictionarySqlConnection[this.connectionString].Connection.Open();
                        }
                        catch (InvalidOperationException ex)
                        {
#if !DEBUG
                            // リトライ処理
                            if (null != retryTableWorker)
                            {
                                Log.System.Write(LogLevel.Error, LogType.ErrorTimeout, String.Format("Retry Connection\n{0}", ex));

                                // 既定の時間待つ
                                TimeSpan waitTime = retryTableWorker.Current;
                                Thread.Sleep(waitTime);

                                if (false == retryTableWorker.MoveNext())
                                {
                                    retryTableWorker = null;
                                }

                                continue;
                            }
#endif

                            throw;
                        }
                        catch (SqlException ex)
                        {
#if !DEBUG
                            // リトライ処理
                            if (null != retryTableWorker)
                            {
                                Log.System.Write(LogLevel.Error, LogType.ErrorTimeout, String.Format("Retry Connection\n{0}", ex));

                                // 既定の時間待つ
                                TimeSpan waitTime = retryTableWorker.Current;
                                Thread.Sleep(waitTime);

                                if (false == retryTableWorker.MoveNext())
                                {
                                    retryTableWorker = null;
                                }

                                continue;
                            }
#endif

                            throw;
                        }
                    }

                    // 接続数をカウントアップ
                    Connection.DictionarySqlConnection[this.connectionString].Count++;
                }

                break;
            }

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

            lock (Connection.DictionarySqlConnection)
            {
                // 接続数をカウントダウン
                Connection.DictionarySqlConnection[this.connectionString].Count--;

                // 接続数が0なら切断
                if (0 >= Connection.DictionarySqlConnection[this.connectionString].Count)
                {
                    Connection.DictionarySqlConnection[this.connectionString].Connection.Close();
                    Connection.DictionarySqlConnection.Remove(this.connectionString);
                }
            }

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

            // 接続しているかを判定します。
            if (ConnectionState.Closed == this.state)
            {
                throw new InvalidOperationException("接続されていません。");
            }

            // コマンドインスタンス生成
            SqlConnection connection = Connection.DictionarySqlConnection[this.connectionString].Connection;
            Command command = new Command(commandText, connection);

            return command;
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
        private SqlConnection GetSqlConnection()
        {
            // 同じ接続文字列の接続実体オブジェクトがあれば、それを使用する
            if (false == Connection.DictionarySqlConnection.ContainsKey(this.connectionString))
            {
                // 新しい接続実体オブジェクトを作成し、一覧に追加
                SqlConnection connection = new SqlConnection(this.connectionString);
                Connection.DictionarySqlConnection.Add(connectionString, new ConnectionCounter(1, connection));
            }

            return Connection.DictionarySqlConnection[this.connectionString].Connection;
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
            if (ConnectionState.Open == this.state)
            {
                this.Close();
            }

            return;
        }
        #endregion
        #endregion

        #region 内部定義
        private class ConnectionCounter
        {
            public int Count
            {
                get;
                set;
            }
            public SqlConnection Connection
            {
                get;
                set;
            }

            public ConnectionCounter(int count, SqlConnection connection)
            {
                this.Count = count;
                this.Connection = connection;
            }
        }
        #endregion
    }
}
