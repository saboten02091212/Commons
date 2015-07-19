// ******************************************************************
// Command.cs  ：SqlServerデータベースコマンド発行クラス
// 作成日　：2013/02/20
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
using Mizuochi.Commons.Codes.ClassExtensions;
using Mizuochi.Commons.Codes.Logs;

namespace Mizuochi.Commons.Codes.Databases.SqlServer
{
    /// <summary>
    /// データベースに対してコマンドを発行する機能を持ったクラスです。
    /// </summary>
    public class Command : ICommand, IDisposable
    {
        #region 静的フィールド
        /// <summary>
        /// コマンドタイムアウト時間です。
        /// </summary>
        private static int defaultCommandTimeout;
        /// <summary>
        /// ストアドコマンドタイムアウト時間です。
        /// </summary>
        private static int defaultStoredTimeout;
        /// <summary>
        /// リトライ設定情報です。
        /// </summary>
        private static IList<TimeSpan> retryTable;
        #endregion

        #region 静的プロパティ
        /// <summary>
        ///コマンドタイムアウト時間を取得、設定します。
        /// </summary>
        public static int DefaultCommandTimeout
        {
            get
            {
                return Command.defaultCommandTimeout;
            }
            set
            {
                Command.defaultCommandTimeout = value;
            }
        }

        /// <summary>
        /// ストアドコマンドタイムアウト時間を取得、設定します。
        /// </summary>
        public static int DefaultStoredTimeout
        {
            get
            {
                return Command.defaultStoredTimeout;
            }
            set
            {
                Command.defaultStoredTimeout = value;
            }
        }

        /// <summary>
        /// リトライ設定情報です。
        /// </summary>
        public static IList<TimeSpan> RetryTable
        {
            get
            {
                return Command.retryTable;
            }
        }
        #endregion

        #region 静的コンストラクタ
        /// <summary>
        /// 静的フィールドの初期化を行います。
        /// </summary>
        static Command()
        {
            Command.retryTable = new[]
            {
                new TimeSpan(0, 0, 0, 0, 100),
                new TimeSpan(0, 0, 0, 1, 0),
                new TimeSpan(0, 0, 0, 3, 0),
                new TimeSpan(0, 0, 0, 15, 0),
            };
        }
        #endregion

        #region フィールド
        /// <summary>
        /// SqlServerのコマンド発行オブジェクト
        /// </summary>
        private SqlCommand sqlCommand;
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
                return sqlCommand.CommandText;
            }
            set
            {
                sqlCommand.CommandText = value;
            }
        }

        /// <summary>
        /// コマンド文字列の解釈方法を取得、設定します。
        /// </summary>
        public CommandType CommandType
        {
            get
            {
                return sqlCommand.CommandType;
            }
            set
            {
                sqlCommand.CommandType = value;
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
        /// <param name="connection">コネクションオブジェクト</param>
        /// <param name="commandType">コマンド文字列解釈方法</param>
        internal Command(string commandText, SqlConnection connection, CommandType commandType = CommandType.Text)
        {
            this.sqlCommand = new SqlCommand(commandText, connection);
            this.sqlCommand.CommandType = commandType;
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

            // パラメタ設定
            this.SetCommandParameter(this.sqlCommand);

            // SQLを発行し、データセットを取得
            var retryTableWorker = Command.retryTable.GetEnumerator();
            retryTableWorker.MoveNext();
            DataSet ds = new DataSet();
            int rowCount = default(int);
            while (true)
            {
                try
                {
                    // SQL発行
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(this.sqlCommand);
                    rowCount = dataAdapter.Fill(ds);
                }
                catch (InvalidOperationException ex)
                {
#if !DEBUG
                    // リトライ処理
                    if (null != retryTableWorker)
                    {
                        Log.System.Write(LogLevel.Error, LogType.ErrorTimeout, String.Format("Retry Command\n{0}", ex));

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
                        Log.System.Write(LogLevel.Error, LogType.ErrorTimeout, String.Format("Retry Command\n{0}", ex));

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

                break;
            }

            dataSet = ds;
            return rowCount;
        }

        /// <summary>
        /// INSERT、UPDATE、DELETE文のいずれかを発行します。
        /// </summary>
        /// <returns>実行したSQLが返す結果セットの最初の行にある最初の列の内容</returns>
        public object Execute()
        {
            // リソース解放済みか判定
            if (true == this.disposed)
            {
                throw new TypeUnloadedException("リソースが解放されています。");
            }

            // パラメタをFireBird用に変換し、コマンドオブジェクトに設定
            this.SetCommandParameter(this.sqlCommand);

            // タイムアウト時間設定
            this.sqlCommand.CommandTimeout =
                (CommandType.Text == this.sqlCommand.CommandType) ? Command.defaultCommandTimeout :
                (CommandType.StoredProcedure == this.sqlCommand.CommandType) ? Command.defaultStoredTimeout :
                this.sqlCommand.CommandTimeout;

            // SQLを発行
            var retryTableWorker = Command.retryTable.GetEnumerator();
            retryTableWorker.MoveNext();
            object obj = null;
            while (true)
            {
                try
                {
                    obj = this.sqlCommand.ExecuteScalar();
                }
                catch (InvalidOperationException ex)
                {
                    // リトライ処理
                    if (null != retryTableWorker)
                    {
                        Log.System.Write(LogLevel.Error, LogType.ErrorTimeout, String.Format("Retry Command\n{0}", ex));

                        // 既定の時間待つ
                        TimeSpan waitTime = retryTableWorker.Current;
                        Thread.Sleep(waitTime);

                        if (false == retryTableWorker.MoveNext())
                        {
                            retryTableWorker = null;
                        }

                        continue;
                    }

                    throw;
                }
                catch (SqlException ex)
                {
                    // リトライ処理
                    if (null != retryTableWorker)
                    {
                        Log.System.Write(LogLevel.Error, LogType.ErrorTimeout, String.Format("Retry Command\n{0}", ex));

                        // 既定の時間待つ
                        TimeSpan waitTime = retryTableWorker.Current;
                        Thread.Sleep(waitTime);

                        if (false == retryTableWorker.MoveNext())
                        {
                            retryTableWorker = null;
                        }

                        continue;
                    }

                    throw;
                }

                break;
            }

            return obj;
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
        /// パラメタコレクションをSqlServer用に変換し、コマンドオブジェクトに設定します。
        /// </summary>
        /// <param name="sqlCommand">パラメタを設定するコマンドオブジェクト</param>
        private void SetCommandParameter(SqlCommand sqlCommand)
        {
            // 既存のパラメタを削除
            sqlCommand.Parameters.Clear();

            // パラメタコレクションを変換、登録
            foreach (string key in this.parameters.Keys)
            {
                sqlCommand.Parameters.Add(new SqlParameter(key, this.parameters[key] ?? DBNull.Value));
                sqlCommand.Parameters[key].IsNullable = true;

                // C#とSqlServerでDateTimeの範囲が違う為、変換
                if (DbType.DateTime == sqlCommand.Parameters[key].DbType)
                {
                    sqlCommand.Parameters[key].DbType = DbType.DateTime2;
                }
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
            this.sqlCommand.Dispose();
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
