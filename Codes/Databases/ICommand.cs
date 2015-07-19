// ******************************************************************
// ICommand.cs ： DBコマンドインタフェース
// 作成日　：2015/03/20
// 更新履歴：2015/03/20 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;

namespace Mizuochi.Commons.Codes.Databases
{
    /// <summary>
    /// データベースのコマンドに関する機能を公開します。
    /// </summary>
    public interface ICommand : IDisposable
    {
        /// <summary>
        /// SQL文を設定、取得します。
        /// </summary>
        string CommandText
        {
            get;
            set;
        }

        /// <summary>
        /// パラメータのコネクションを取得します。
        /// </summary>
        System.Collections.Generic.Dictionary<string, object> Parameters
        {
            get;
        }

        /// <summary>
        /// SELECT文を発行します。
        /// </summary>
        /// <param name="dataSet">取得したレコードセットが設定されます。</param>
        /// <returns>取得したレコード数</returns>
        int Fill(out System.Data.DataSet dataSet);

        /// <summary>
        /// INSERT、UPDATE、DELETE文のいずれかを発行します。
        /// </summary>
        /// <returns>実行したSQLが返す結果セットの最初の行にある最初の列の内容</returns>
        object Execute();
    }
}
