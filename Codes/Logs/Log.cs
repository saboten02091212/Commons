// ******************************************************************
// Log.cs ： ログクラス 
// 作成日　：2013/01/29
// 更新履歴：2013/08/30 水落　　 ログ出力フラグを追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mizuochi.Commons.Codes.Types;

namespace Mizuochi.Commons.Codes.Logs
{
    /// <summary>
    /// ログ出力するイベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="logLevel">ログレベル</param>
    /// <param name="logType">ログ区分</param>
    /// <param name="message">メッセージを表すオブジェクト</param>
    /// <param name="args">個別情報</param>
    public delegate void WriteLogEventHandler(LogLevel logLevel, LogType logType, object message, params object[] args);

    /// <summary>
    /// ログを出力する機能を保有するクラスです。
    /// </summary>
    public class Log
    {
        /// <summary>
        /// データベースログを出力する際に使用するログインスタンスです。
        /// </summary>
        public readonly static Log Database = new Log();
        /// <summary>
        /// アプリケーションログを出力する際に使用するログインスタンスです。
        /// </summary>
        public readonly static Log Application = new Log();
        /// <summary>
        /// システムログを出力する際に使用するログインスタンスです。
        /// </summary>
        public readonly static Log System = new Log();

        /// <summary>
        /// ログ出力時に発生します。
        /// </summary>
        public event WriteLogEventHandler Writing;

        /// <summary>
        /// ログ出力レベル
        /// </summary>
        private LogLevel writeLogLevel;

        /// <summary>
        /// ログ出力有無
        /// </summary>
        private bool enabled;

        /// <summary>
        /// ログの出力レベルを取得、設定します。
        /// </summary>
        public LogLevel WriteLogLevel
        {
            get
            {
                return writeLogLevel;
            }
            set
            {
                writeLogLevel = value;
            }
        }

        /// <summary>
        /// ログ出力有無を取得、設定します。
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        /// <summary>
        /// インタスタンスを生成します。
        /// </summary>
        private Log()
        {
            this.writeLogLevel = LogLevel.Verbose;
            this.enabled = true;
        }

        /// <summary>
        /// 指定されたパラメタでログを書き込みます。
        /// </summary>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="logType">ログ区分</param>
        /// <param name="message">メッセージ</param>
        /// <param name="args">個別情報</param>
        public void Write(LogLevel logLevel, LogType logType, string message, params object[] args)
        {
            Debug.Assert((LogLevel.Error != logLevel) && (LogLevel.Critical != logLevel));

            // ログ出力の有無を確認
            if (false == this.enabled)
            {
                return;
            }

            // ログレベルが足りなければ出力しない
            if (this.writeLogLevel > logLevel)
            {
                return;
            }

            // ログを出力
            if (null != this.Writing)
            {
                this.Writing(logLevel, logType, message, args);
            }

            return;
        }

        /// <summary>
        /// 指定されたパラメタでログを書き込みます。
        /// </summary>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="logType">ログ区分</param>
        /// <param name="exception">例外</param>
        /// <param name="args">個別情報</param>
        public void Write(LogLevel logLevel, LogType logType, Exception exception, params object[] args)
        {
            Debug.Assert((LogLevel.Error != logLevel) && (LogLevel.Critical != logLevel));

            // ログ出力の有無を確認
            if (false == this.enabled)
            {
                return;
            }

            // ログレベルが足りなければ出力しない
            if (this.writeLogLevel > logLevel)
            {
                return;
            }

            // ログを出力
            if (null != this.Writing)
            {
                this.Writing(logLevel, logType, exception, args);
            }

            return;
        }
    }

    /// <summary>
    /// ログレベル
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 解析
        /// </summary>
        [EnumDisplay("解析")]
        Verbose,
        /// <summary>
        /// 情報
        /// </summary>
        [EnumDisplay("情報")]
        Information,
        /// <summary>
        /// 警告
        /// </summary>
        [EnumDisplay("警告")]
        Warning,
        /// <summary>
        /// エラー
        /// </summary>
        [EnumDisplay("エラー")]
        Error,
        /// <summary>
        /// 例外
        /// </summary>
        [EnumDisplay("例外")]
        Critical,
        /// <summary>
        /// 無し
        /// </summary>
        [EnumDisplay("無し")]
        None,
    }

    /// <summary>
    /// ログ区分
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 情報
        /// </summary>
        Info = 10000,
        /// <summary>
        /// 情報 アプリケーション
        /// </summary>
        InfoApplication = 11000,
        /// <summary>
        /// 情報 アプリケーション ログイン
        /// </summary>
        InfoLogin = 11001,
        /// <summary>
        /// 情報 アプリケーション ログアウト
        /// </summary>
        InfoLogout = 11002,
        /// <summary>
        /// 情報 アプリケーション 画面In
        /// </summary>
        InfoFormLoad = 11003,
        /// <summary>
        /// 情報 アプリケーション 画面Out
        /// </summary>
        InfoFormUnload = 11004,
        /// <summary>
        /// 情報 アプリケーション メソッド
        /// </summary>
        InfoMethod = 11100,
        /// <summary>
        /// 情報 アプリケーション メソッド 開始
        /// </summary>
        InfoMethodIn = 11101,
        /// <summary>
        /// 情報 アプリケーション メソッド 終了
        /// </summary>
        InfoMethodOut = 11102,
        /// <summary>
        /// 情報 アプリケーション イベント
        /// </summary>
        InfoEvent = 11200,
        /// <summary>
        /// 情報 アプリケーション イベント 開始
        /// </summary>
        InfoEventIn = 11201,
        /// <summary>
        /// 情報 アプリケーション イベント 終了
        /// </summary>
        InfoEventOut = 11201,
        /// <summary>
        /// 情報 データベース
        /// </summary>
        InfoDatabase = 12000,
        /// <summary>
        /// 情報 データベース 選択
        /// </summary>
        InfoSelect = 12100,
        /// <summary>
        /// 情報 データベース 挿入
        /// </summary>
        InfoInsert = 12200,
        /// <summary>
        /// 情報 データベース 更新
        /// </summary>
        InfoUpdate = 12300,
        /// <summary>
        /// 情報 データベース 論理削除
        /// </summary>
        InfoLogicalDelete = 12400,
        /// <summary>
        /// 情報 データベース 物理削除
        /// </summary>
        InfoDelete = 12500,
        /// <summary>
        /// 情報 データベース トランザクション
        /// </summary>
        InfoTransaction = 12600,
        /// <summary>
        /// 情報 データベース トランザクション 開始
        /// </summary>
        InfoTranIn = 12601,
        /// <summary>
        /// 情報 データベース トランザクション 終了
        /// </summary>
        InfoTranOut = 12602,
        /// <summary>
        /// 情報 データベース トランザクション コミット
        /// </summary>
        InfoCommit = 12603,
        /// <summary>
        /// 情報 データベース ストアド
        /// </summary>
        InfoStored = 12700,
        /// <summary>
        /// 警告
        /// </summary>
        Warning = 20000,
        /// <summary>
        /// エラー
        /// </summary>
        Error = 30000,
        /// <summary>
        /// エラー アプリケーション
        /// </summary>
        ErrorApplication = 31000,
        /// <summary>
        /// エラー データベース
        /// </summary>
        ErrorDatabase = 32000,
        /// <summary>
        /// エラー データベース タイムアウト
        /// </summary>
        ErrorTimeout = 32001,
        /// <summary>
        /// エラー データベース トランザクション
        /// </summary>
        ErrorTran = 32100,
        /// <summary>
        /// エラー データベース ストアド
        /// </summary>
        ErrorStored = 32200,
        /// <summary>
        /// 例外
        /// </summary>
        Critical = 40000,
    }
}
