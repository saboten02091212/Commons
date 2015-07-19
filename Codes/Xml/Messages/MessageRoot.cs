// ******************************************************************
// MessageRoot.cs  ：メッセージリストXMLルートクラス
// 作成日　：2012/12/12
// 更新履歴：2012/12/12 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Mizuochi.Commons.Codes.Xml.Messages
{
    /// <summary>
    /// メッセージリストを保持するクラスです。
    /// </summary>
    [XmlRoot("Messages")]
    public class MessageRoot
    {
        #region フィールド
        /// <summary>
        /// メッセージリスト
        /// </summary>
        private Collection<MessageRecord> messageCol;
        #endregion

        #region プロパティ
        /// <summary>
        /// メッセージリスト
        /// </summary>
        [XmlElement("Message")]
        public Collection<MessageRecord> MessageCol
        {
            get { return messageCol; }
            set { messageCol = value; }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public MessageRoot()
        {
            this.messageCol = new Collection<MessageRecord>();
        }
        #endregion
    }
}
