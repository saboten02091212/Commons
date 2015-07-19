// ******************************************************************
// StringExtension.cs ： String拡張クラス
// 作成日　：2013/09/04
// 更新履歴：2014/09/11 水落　　 Right/Leftメソッドを追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// Stringクラスを拡張するクラスです。
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Null文字を示します。
        /// </summary>
        public const char NullCharacter = '\u0000';

        /// <summary>
        /// 空白文字を示します。
        /// </summary>
        public const char SpaceCharacter = '\u0020';

        /// <summary>
        /// 指定された文字列で上書きます。
        /// </summary>
        /// <param name="source">元になる文字列</param>
        /// <param name="overWriteString">上書きする文字列</param>
        /// <param name="transparentCharacter">透過する文字列</param>
        /// <returns>上書きされた文字列</returns>
        public static string OverWrite(this String source, string overWriteString, char transparentCharacter = StringExtension.NullCharacter)
        {
            int length = new[] {source.Length, overWriteString.Length}.Max();
            char[] destination = new char[length];

            for (int index = 0; index < length; index++)
            {
                char sourceChar = (source.Length > index) ? source[index] : StringExtension.NullCharacter;
                char overWriteChar = (overWriteString.Length > index) ? overWriteString[index] : StringExtension.NullCharacter;

                char c;
                if ((StringExtension.NullCharacter != overWriteChar) && (transparentCharacter != overWriteChar))
                {
                    c = overWriteChar;
                }
                else if (StringExtension.NullCharacter != sourceChar)
                {
                    c = sourceChar;
                }
                else
                {
                    c = StringExtension.SpaceCharacter;
                }

                destination[index] = c;
            }

            return new String(destination);
        }

        /// <summary>
        /// 指定された文字列のバイト数を取得します。
        /// </summary>
        /// <param name="source">元になる文字列</param>
        /// <param name="encoding">文字列のエンコーディング</param>
        /// <returns>バイト数</returns>
        public static int GetByteCount(this String source, Encoding encoding = null)
        {
            Encoding convertEncoding = (null != encoding) ? encoding : Encoding.Default;
            return convertEncoding.GetByteCount(source);
        }

        /// <summary>
        /// 文字列の整数部と小数部が、指定された文字数以内であるかを判定します。
        /// </summary>
        /// <param name="source">元になる文字列</param>
        /// <param name="integerPartDigit">整数部の文字数</param>
        /// <param name="decimalPartDigit">小数部の文字数</param>
        /// <returns>true:指定の文字数と一致 false:指定の文字数と一致しない</returns>
        public static bool IsSpecifiedLength(this String source, int integerPartDigit, int decimalPartDigit)
        {
            // 文字列からカンマと負符号を削除します。
            string afterString = source.Replace(",", "").Replace("-", "");

            // 文字列が数値に変換できるかを判定します。
            decimal dre;
            bool decimalResult = Decimal.TryParse(afterString, out dre);

            // 文字列に変換できない場合は処理を終了
            if (decimalResult == false)
            {
                return false;
            }

            // 文字列にカンマが存在するか判定します。
            string integerPart = null;
            string decimalPart = null;
            if (true == afterString.Contains("."))
            {
                // カンマの前後を分けて、整数部と小数部を取得
                string[] stlist = afterString.Split('.');
                integerPart = stlist[0];
                decimalPart = stlist[1];
            }
            else
            {
                integerPart = afterString;
            }

            // 整数部の桁数を判定
            if ((null != integerPart) && (0 <= integerPartDigit))
            {
                if (integerPart.Length > integerPartDigit)
                {
                    return false;
                }
            }

            // 小数部の桁数を判定
            if ((null != decimalPart) && (0 <= decimalPartDigit))
            {
                if (decimalPart.Length > decimalPartDigit)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 文字列の先頭から指定された長さの文字列を取得します。
        /// </summary>
        /// <param name="source">元になる文字列</param>
        /// <param name="length">取得する文字列長</param>
        /// <returns>取得した文字列</returns>
        public static string Left(this String source, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("lengthに0未満の値が設定されてます。", "length");
            }

            if (source == null)
            {
                return null;
            }

            int tmpLength = (source.Length < length) ? source.Length : length;
            return source.Substring(0, tmpLength);
        }

        /// <summary>
        /// 文字列の末尾から指定された長さの文字列を取得します。
        /// </summary>
        /// <param name="source">元になる文字列</param>
        /// <param name="length">取得する文字列長</param>
        /// <returns>取得した文字列</returns>
        public static string Right(this String source, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("lengthに0未満の値が設定されてます。", "length");
            }

            if (source == null)
            {
                return null;
            }

            int tmpLength = (source.Length < length) ? source.Length : length;
            return source.Substring(source.Length - tmpLength, tmpLength);
        }
    }
}
