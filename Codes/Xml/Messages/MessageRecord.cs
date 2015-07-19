// ******************************************************************
// MessageRecord.cs  ：メッセージ情報XMLノードクラス
// 作成日　：2012/12/12
// 更新履歴：2013/04/04 水落　　 エスケープ文字列を変換するよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Mizuochi.Commons.Codes.Xml.Messages
{
    /// <summary>
    /// メッセージ情報を保持するクラスです。
    /// </summary>
    public class MessageRecord
    {
        #region フィールド
        /// <summary>
        /// メッセージID
        /// </summary>
        private string id;

        /// <summary>
        /// メッセージテキスト
        /// </summary>
        private string text;
        #endregion

        #region プロパティ
        /// <summary>
        /// メッセージID
        /// </summary>
        [XmlAttribute]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// メッセージテキスト
        /// </summary>
        [XmlText]
        public string Text
        {
            get { return text; }
            set
            {
                text = MessageRecord.EscapeDecord(value);
            }
        }
        #endregion

        #region 静的プライベートメソッド
        /// <summary>
        /// 指定された文字列のエスケープ文字を変換します。
        /// </summary>
        /// <param name="source">変換元文字列</param>
        /// <returns>変換後文字列</returns>
        private static string EscapeDecord(string source)
        {
            // 変換パターン生成
            var convertPattern = new[]
            {
                new {encord = @"\n", decord = "\n" },
                new {encord = @"\t", decord = "\t" },
                new {encord = @"\\", decord = "\\" },
            };

            // 文字列変換
            string destination = source;
            foreach (var convret in convertPattern)
            {
                destination = destination.Replace(convret.encord, convret.decord);
            }

            return destination;
        }
        #endregion
    }
}
