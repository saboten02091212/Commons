// ******************************************************************
// Enumeration.cs  ：列挙型操作クラス
// 作成日　：2012/12/12
// 更新履歴：2013/02/25 水落　　 列挙型かの判定処理を修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;

namespace Mizuochi.Commons.Codes.Types
{
    /// <summary>
    /// 列挙型を扱うクラスです。
    /// </summary>
    public static class Enumeration
    {
        #region 静的パブリックメソッド
        /// <summary>
        /// 指定された列挙体の値を返します。
        /// </summary>
        /// <param name="enumValue">列挙体</param>
        /// <returns>列挙体の値</returns>
        public static int Value(Enum enumValue)
        {
            return Int32.Parse(enumValue.ToString("D"));
        }

        /// <summary>
        /// 指定された列挙体の値を文字列型で返します。
        /// </summary>
        /// <param name="enumValue">列挙体</param>
        /// <returns>列挙体の値</returns>
        public static string Text(Enum enumValue)
        {
            return enumValue.ToString("D");
        }

        /// <summary>
        /// 指定された列挙体の名前を返します。
        /// </summary>
        /// <param name="enumValue">列挙体</param>
        /// <returns>列挙体の名前</returns>
        public static string Name(Enum enumValue)
        {
            return enumValue.ToString("G");
        }

        /// <summary>
        /// 指定された列挙体が定義されているかどうかを示す値を返します。
        /// </summary>
        /// <param name="enumValue">列挙体</param>
        /// <returns>定義されていれば、true。それ以外の場合は、falseを返します。</returns>
        public static bool IsDefined(Enum enumValue)
        {
            return Enum.IsDefined(enumValue.GetType(), enumValue);
        }

        /// <summary>
        /// 指定した値を持つ、指定した列挙体に変換します。
        /// </summary>
        /// <typeparam name="EnumType">返却する列挙体の型</typeparam>
        /// <param name="obj">返却する列挙体の持つ値</param>
        /// <exception cref="System.FormatException">objが数値に変換できない場合、発生します。</exception>
        /// <returns>列挙体</returns>
        public static EnumType Parse<EnumType>(object obj)
            where EnumType : new()
        {
            EnumType et;
            bool result = Enumeration.TryParse<EnumType>(obj, out et);
            if (false == result)
            {
                throw new FormatException("指定された値は、数値型に変換できません。");
            }

            return et;
        }

        /// <summary>
        /// 指定した値を持つ、指定した列挙体に変換します。
        /// </summary>
        /// <typeparam name="EnumType">返却する列挙体の型</typeparam>
        /// <param name="obj">返却する列挙体の持つ値</param>
        /// <param name="errorValue">列挙体にキャストできない場合の返却値</param>
        /// <returns>列挙体</returns>
        public static EnumType Parse<EnumType>(object obj, EnumType errorValue)
            where EnumType : new()
        {
            EnumType et;
            bool result = Enumeration.TryParse<EnumType>(obj, out et);
            if (false == result)
            {
                et = errorValue;
            }

            return et;
        }

        /// <summary>
        /// 指定した値を持つ、指定した列挙体に変換します。戻り値は、変換が成功したかどうかを示します。
        /// </summary>
        /// <typeparam name="EnumType">返却する列挙体の型</typeparam>
        /// <param name="obj">返却する列挙体の持つ値</param>
        /// <param name="et">変換した列挙体。変換に失敗した場合、-1の値を持つ列挙体が設定されます。</param>
        /// <exception cref="System.NotSupportedException">列挙体でない型を指定した場合、発生します。</exception>
        /// <returns>正常に変換できれば、true。それ以外では、falseを返します。</returns>
        public static bool TryParse<EnumType>(object obj, out EnumType et)
            where EnumType : new()
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException("指定された型は、列挙体ではありません。");
            }

            bool result = false;
            et = (EnumType)Enum.ToObject(typeof(EnumType), -1);

            int value;
            if (null != obj)
            {
                if (true == Int32.TryParse(obj.ToString(), out value))
                {
                    result = true;
                    et = (EnumType)Enum.ToObject(typeof(EnumType), value);
                }
            }

            return result;
        }
        #endregion
    }
}
