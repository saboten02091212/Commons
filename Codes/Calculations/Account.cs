// ******************************************************************
// Account.cs ： 金額のフォーマット相互変換クラス
// 作成日　：2012/12/12
// 更新履歴：2013/02/10 水落　　 文字列から数値に変換可能か判定メソッド追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Calculations
{
    /// <summary>
    /// 金額のフォーマットの文字列と数値とを相互変換するクラスです。
    /// </summary>
    public static class Account
    {
        #region 静的パブリックメソッド
        /// <summary>
        /// 指定された数値を金額フォーマットの文字列に変換します。
        /// </summary>
        /// <param name="numericValue">変換対象数値を指定して下さい。</param>
        /// <param name="currencyType">円記号の表示形式を指定して下さい。</param>
        /// <param name="addComma">３桁区切りを付与する場合、true。しない場合、falseを指定して下さい。</param>
        /// <param name="decimals">小数点以下の有効桁数を指定して下さい。</param>
        /// <param name="roundType">丸め区分を指定して下さい。</param>
        /// <exception cref="System.ArgumentNullException">numericValueがNullの場合、発生します。</exception>
        /// <exception cref="System.FormatException">numericValueが数値に変換できない場合、発生します。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">decimalsに0未満、28より大きい値が設定された場合、発生します。</exception>
        /// <exception cref="System.ArgumentException">
        /// roundTypeに想定しない値が設定された場合、発生します。
        /// または、円記号の表示形式に想定しない値が設定された場合、発生します。
        /// </exception>
        /// <returns>金額フォーマット文字列</returns>
        public static string ConvertString(object numericValue, CurrencyType currencyType = CurrencyType.None, bool addComma = false, int decimals = 0, RoundType roundType = RoundType.RoundOff)
        {
            // Null値チェック
            if (null == numericValue)
            {
                throw new ArgumentNullException("numericValue");
            }

            // 丸め処理
            decimal value = Convert.ToDecimal(numericValue);
            value = MizuochiMath.Round(value, decimals, roundType);

            // 3ケタ区切り付与
            string format = String.Empty;
            if (true == addComma)
            {
                format = String.Format("{0}{1}", "N", decimals);
            }
            else
            {
                format = String.Format("{0}{1}", "F", decimals);
            }

            // 円記号付与
            string stringValue = value.ToString(format);
            switch (currencyType)
            {
                case CurrencyType.None:
                    break;

                case CurrencyType.Mark:
                    stringValue = @"\" + stringValue;
                    break;


                case CurrencyType.Kanji:
                    stringValue = stringValue + @"円";
                    break;

                default:
                    throw new InvalidEnumArgumentException("円記号の表示形式の値が不正です。");
            }

            return stringValue;
        }

        /// <summary>
        /// 金額フォーマットの文字列からdecimal型の数値に変換します。
        /// </summary>
        /// <param name="stringValue">変換対象文字列を指定して下さい。</param>
        /// <exception cref="ArgumentNullException">stringValueがNullの場合、発生します。</exception>
        /// <exception cref="FormatException">stringValueが数値に変換できない場合、発生します。</exception>
        /// <returns>数値</returns>
        public static decimal ConvertDecimal(string stringValue)
        {
            stringValue = stringValue
                            .Replace(@"\", "")
                            .Replace(@"円", "")
                            .Replace(@",", "");

            return decimal.Parse(stringValue);
        }

        /// <summary>
        /// 金額フォーマットの文字列からdecimal型の数値に変換できるか判定します。
        /// </summary>
        /// <param name="stringValue">変換対象文字列を指定して下さい。</param>
        /// <returns>数値に変換できる場合、true。変換できない場合、falseを返します。</returns>
        public static bool CanConvertDecimal(string stringValue)
        {
            stringValue = stringValue
                            .Replace(@"\", "")
                            .Replace(@"円", "")
                            .Replace(@",", "");
            decimal tmpValue;
            return decimal.TryParse(stringValue, out tmpValue);
        }
        #endregion
    }

    /// <summary>
    /// 円記号の表示種別
    /// </summary>
    public enum CurrencyType
    {
        /// <summary>
        /// 無し
        /// </summary>
        None,
        /// <summary>
        /// \
        /// </summary>
        Mark,
        /// <summary>
        /// 円
        /// </summary>
        Kanji,
    }
}
