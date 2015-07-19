// ******************************************************************
// EnumDisplayAttribute.cs  ：列挙型表示名称属性クラス
// 作成日　：2013/03/05
// 更新履歴：2013/03/05 水落　　 新規作成。
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
    /// 列挙型に文字列情報を定義するカスタム属性クラスです。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumDisplayAttribute : System.Attribute
    {
        /// <summary>
        /// 表示値
        /// </summary>
        private string display;

        /// <summary>
        /// サブ表示値
        /// </summary>
        private string subDisplay;

        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="display">表示値</param>
        /// <param name="subDisplay">サブ表示値</param>
        public EnumDisplayAttribute(string display = "", string subDisplay = "")
        {
            this.display = display;
            this.subDisplay = subDisplay;
        }

        /// <summary>
        /// 引数で渡された値の表示値を取得します。
        /// </summary>
        /// <param name="enumValue">列挙型の値</param>
        /// <returns>表示値</returns>
        /// <exception cref="System.NotSupportedException">列挙体でない型を指定した場合、発生します。</exception>
        /// <exception cref="System.ArgumentException">enumValueにDisplay属性が定義されていない場合、発生します。</exception>
        public static string GetDisplayValue<EnumType>(EnumType enumValue)
            where EnumType : IConvertible
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException("指定された型は、列挙体ではありません。");
            }

            // フィールドの属性取得
            FieldInfo fInfo = typeof(EnumType).GetField(enumValue.ToString());
            object[] attributes = fInfo.GetCustomAttributes(typeof(EnumDisplayAttribute), false);

            // 属性が無ければエラー
            if (1 != attributes.Length)
            {
                throw new ArgumentException("Display属性が定義されていません。", "enumValue");
            }

            // 表示値取得
            EnumDisplayAttribute displayAttribute = (EnumDisplayAttribute)attributes[0];
            return displayAttribute.display;
        }

        /// <summary>
        /// 引数で渡された値のサブ表示値を取得します。
        /// </summary>
        /// <param name="enumValue">列挙型の値</param>
        /// <returns>サブ表示値</returns>
        /// <exception cref="System.NotSupportedException">列挙体でない型を指定した場合、発生します。</exception>
        /// <exception cref="System.ArgumentException">enumValueにDisplay属性が定義されていない場合、発生します。</exception>
        public static string GetSubDisplayValue<EnumType>(EnumType enumValue)
            where EnumType : IConvertible
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException("指定された型は、列挙体ではありません。");
            }

            // フィールドの属性取得
            FieldInfo fInfo = typeof(EnumType).GetField(enumValue.ToString());
            object[] attributes = fInfo.GetCustomAttributes(typeof(EnumDisplayAttribute), false);

            // 属性が無ければエラー
            if (1 != attributes.Length)
            {
                throw new ArgumentException("Display属性が定義されていません。", "enumValue");
            }

            // 表示値取得
            EnumDisplayAttribute displayAttribute = (EnumDisplayAttribute)attributes[0];
            return displayAttribute.subDisplay;
        }
    }
}
