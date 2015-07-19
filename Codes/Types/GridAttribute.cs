// ******************************************************************
// GridAttribute.cs  ：グリッド属性クラス
// 作成日　：2013/07/11
// 更新履歴：2013/07/11 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mizuochi.Commons.Codes.Types
{
    /// <summary>
    /// グリッド列に対して見出しや幅などの情報を付与するカスタム属性クラスです。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class GridAttribute : System.Attribute
    {
        /// <summary>
        /// 見出し
        /// </summary>
        private string title;

        /// <summary>
        /// 幅
        /// </summary>
        private int width;

        /// <summary>
        /// 説明文
        /// </summary>
        private string explainText;

        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="title">見出し</param>
        /// <param name="width">幅</param>
        /// <param name="explainText">説明文</param>
        public GridAttribute(string title = "", int width = 100, string explainText = "")
        {
            this.title = title;
            this.width = width;
            this.explainText = explainText;
        }

        /// <summary>
        /// 指定されたメンバの見出しを取得します。
        /// </summary>
        /// <param name="memberInfo">メンバ情報</param>
        /// <returns>見出し</returns>
        public static string GetTitle(MemberInfo memberInfo)
        {
            // フィールドの属性取得
            IEnumerable<GridAttribute> attributes = memberInfo.GetCustomAttributes(typeof(GridAttribute), true).Cast<GridAttribute>();

            // 表示値取得
            return attributes.Single().title;
        }

        /// <summary>
        /// 指定されたメンバの幅を取得します。
        /// </summary>
        /// <param name="memberInfo">メンバ情報</param>
        /// <returns>幅</returns>
        public static int GetWidth(MemberInfo memberInfo)
        {
            // フィールドの属性取得
            IEnumerable<GridAttribute> attributes = memberInfo.GetCustomAttributes(typeof(GridAttribute), true).Cast<GridAttribute>();

            // 表示値取得
            return attributes.Single().width;
        }

        /// <summary>
        /// 指定されたメンバの説明文を取得します。
        /// </summary>
        /// <param name="memberInfo">メンバ情報</param>
        /// <returns>説明文のリスト</returns>
        public static IEnumerable<string> GetExplainText(MemberInfo memberInfo)
        {
            // フィールドの属性取得
            IEnumerable<GridAttribute> attributes = memberInfo.GetCustomAttributes(typeof(GridAttribute), true).Cast<GridAttribute>();

            // 表示値取得
            return attributes.Select<GridAttribute, string>(item => item.explainText);
        }
    }
}
