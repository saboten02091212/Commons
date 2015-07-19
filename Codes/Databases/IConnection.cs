// ******************************************************************
// IConnection.cs ： DBコネクションインタフェース
// 作成日　：2015/03/20
// 更新履歴：2015/03/20 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;

namespace Mizuochi.Commons.Codes.Databases
{
    /// <summary>
    /// データベースのコネクションに関する機能を公開します。
    /// </summary>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// 接続文字列を取得します。
        /// </summary>
        string ConnectionString
        {
            get;
        }

        /// <summary>
        /// 接続の状態を取得します。
        /// </summary>
        System.Data.ConnectionState State
        {
            get;
        }

        /// <summary>
        /// データベース接続を開きます。
        /// </summary>
        void Open();

        /// <summary>
        /// データベースへの接続を閉じます。
        /// </summary>
        void Close();

        /// <summary>
        /// コマンドインスタンスを生成します。
        /// </summary>
        /// <param name="commandText">コマンドテキスト</param>
        /// <returns>コマンドインスタンス</returns>
        ICommand CreateCommand(string commandText = "");

        /// <summary>
        /// 接続できるかどうか判定します。
        /// </summary>
        /// <returns>接続できる場合、true。接続できない場合、falseを返します。</returns>
        bool CanConnect();
    }
}
