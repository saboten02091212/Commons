// ******************************************************************
// DatabaseInfomation.cs ： DB情報保持クラス
// 作成日　：2014/03/26
// 更新履歴：2014/03/26 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Databases
{
    /// <summary>
    /// データベースに関連する情報を保持するクラスです。
    /// </summary>
    public class DatabaseInfomation
    {
        /// <summary>
        /// 接続文字列
        /// </summary>
        private string connectionString;

        /// <summary>
        /// 文字コード
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// 接続文字列を取得、設定します。
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        /// <summary>
        /// 文字コードを取得、設定します。
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return encoding;
            }
            set
            {
                encoding = value;
            }
        }
    }
}
