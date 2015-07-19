// ******************************************************************
// ClientInfo.cs ： クライアント情報クラス
// 作成日　：2013/04/18
// 更新履歴：2013/04/18 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.Informations
{
    /// <summary>
    /// クライアントの情報を取得する機能を保持するクラスです。
    /// </summary>
    public static class ClientInfo
    {
        #region 静的プロパティ
        /// <summary>
        /// コンピューター名を取得します。
        /// </summary>
        public static string ComputerName
        {
            get
            {
                return SystemInformation.ComputerName;
            }
        }

        /// <summary>
        /// ネットワークに接続されているかどうかを示す値を取得します。
        /// </summary>
        public static bool ConnectsNetwork
        {
            get
            {
                return SystemInformation.Network;
            }
        }

        /// <summary>
        /// ユーザーが属するドメインの名前を取得します。
        /// </summary>
        public static string UserDomainName
        {
            get
            {
                return SystemInformation.UserDomainName;
            }
        }

        /// <summary>
        /// ユーザー名を取得します。
        /// </summary>
        public static string UserName
        {
            get
            {
                return SystemInformation.UserName;
            }
        }
        #endregion
    }
}
