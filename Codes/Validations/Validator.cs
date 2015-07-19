// ******************************************************************
// Validator.cs  ：バリデーションクラス
// 作成日　：2012/12/12
// 更新履歴：2013/03/05 水落　　 最小文字列長の追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mizuochi.Commons.Codes.Calculations;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Mizuochi.Commons.Codes.Validations
{
    /// <summary>
    /// バリデーション機能を保持するクラスです。
    /// </summary>
    public class Validator
    {
        #region 定数
        /// <summary>
        /// 正規表現において、半角数字を表す文字列です。
        /// </summary>
        private const string RegexNumber = "[0-9]";
        /// <summary>
        /// 正規表現において、半角英字を表す文字列です。
        /// </summary>
        private const string RegexAlphabet = "[a-zA-Z]";
        /// <summary>
        /// 正規表現において、半角カナを表す文字列です。
        /// </summary>
        private const string RegexHarfWidthKana = "[\\uff66-\\uff9f]";
        /// <summary>
        /// 正規表現において、半角文字を表す文字列です。
        /// </summary>
        private const string RegexHarfWidth = "[\\u0021-\\u007d\\u203e\\uff61-\\uff9f]";
        /// <summary>
        /// 正規表現において、ホワイトスペースを表す文字列です。
        /// </summary>
        private const string RegexWhiteSpace = "[\\s]";
        #endregion

        #region 静的パブリックメソッド
        /// <summary>
        /// 指定された文字列が空かどうかを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <returns>空でなければ、true。空であれば、falseを返します。</returns>
        public static bool IsNotEmpty(string value)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 空文字チェック
            bool result = true;
            if (true == String.IsNullOrEmpty(value.ToString()))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が指定した文字列長以下かを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <param name="maxLength">最大文字列長</param>
        /// <param name="minLength">最小文字列長</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <exception cref="System.ArgumentException">minLengthとmaxLengthの大小関係が不正な場合、発生します。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// maxLengthが0以下の場合、発生します。
        /// または、minLengthが0以下の場合、発生します。
        /// </exception>
        /// <returns>指定した文字列長内であれば、true。指定した文字列長外であれば、falseを返します。</returns>
        public static bool IsExceed(string value, int maxLength, int minLength)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 最大文字列長チェック
            if (0 > maxLength)
            {
                throw new ArgumentOutOfRangeException("最大文字列長に0未満が指定されています。", "maxLength");
            }

            // 最小文字列長チェック
            if (0 > minLength)
            {
                throw new ArgumentOutOfRangeException("最小文字列長に0未満が指定されています。", "minLength");
            }

            // 範囲チェック
            if (minLength > maxLength)
            {
                throw new ArgumentException("最小数が最大数より大きい数です", "minLength");
            }

            // 文字列長チェック
            bool result = true;
            if (maxLength < value.ToString().Length)
            {
                result = false;
            }

            if (minLength > value.ToString().Length)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が指定したバイト長以下かを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <param name="maxByteLength">最大バイト長</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <exception cref="System.ArgumentException">valueのToStringメソッドでクラス名を取得した場合、発生します。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">maxByteLengthが0以下の場合、発生します。</exception>
        /// <returns>指定したバイト長以下であれば、true。以上であれば、falseを返します。</returns>
        public static bool IsExceedByte(string value, int maxByteLength)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 最大バイト長チェック
            if (0 > maxByteLength)
            {
                throw new ArgumentOutOfRangeException("最大文字列長に0未満が指定されています。", "maxLength");
            }

            // バイト長チェック
            bool result = true;
            if (maxByteLength < Encoding.GetEncoding("utf-8").GetByteCount(value.ToString()))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が数値かを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <returns>数値であれば、true。数値でなければ、falseを返します。</returns>
        public static bool IsNumeric(string value)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 数値に変換できるかチェック
            bool result = true;
            try
            {
                decimal number = Account.ConvertDecimal(value.ToString());
            }
            catch (FormatException ex)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が数字のみで構成されているかを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <returns>数字のみで構成されていれば、true。数値以外の文字があれば、falseを返します。</returns>
        public static bool IsNumber(string value)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 数値のみで構成されているかチェック
            bool result = true;
            string[] patternList =
            {
                Validator.RegexNumber,
            };

            string pattern = String.Format("^({0})+$", String.Join("|", patternList));
            if (false == Regex.IsMatch(value.ToString(), pattern))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が英数字のみで構成されているかを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <returns>英数字のみで構成されていれば、true。英数値以外の文字があれば、falseを返します。</returns>
        public static bool IsAlphanumeric(string value)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 英数字のみで構成されているかチェック
            bool result = true;
            string[] patternList =
            {
                Validator.RegexNumber,
                Validator.RegexAlphabet,
            };

            string pattern = String.Format("^({0})+$", String.Join("|", patternList));
            if (false == Regex.IsMatch(value.ToString(), pattern))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が英数字と半角カナのみで構成されているかを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <returns>英数字と半角カナのみで構成されていれば、true。英数値と半角カナ以外の文字があれば、falseを返します。</returns>
        public static bool IsAlphanumericAndKana(string value)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 英数字と半角カナのみで構成されているかチェック
            bool result = true;
            string[] patternList =
            {
                Validator.RegexNumber,
                Validator.RegexAlphabet,
                Validator.RegexHarfWidthKana,
            };

            string pattern = String.Format("^({0})+$", String.Join("|", patternList));
            if (false == Regex.IsMatch(value.ToString(), pattern))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が半角文字のみで構成されているかを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <returns>半角文字のみで構成されていれば、true。半角文字以外の文字があれば、falseを返します。</returns>
        public static bool IsHarfWidth(string value)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 半角文字のみで構成されているかチェック
            bool result = true;
            string[] patternList =
            {
                Validator.RegexHarfWidth
            };

            string pattern = String.Format("^({0})+$", String.Join("|", patternList));
            if (false == Regex.IsMatch(value.ToString(), pattern))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が半角文字とホワイトスペース以外で構成されているかを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <returns>半角文字とホワイトスペース以外で構成されていれば、true。半角文字とホワイトスペースの文字があれば、falseを返します。</returns>
        public static bool IsFullWidth(string value)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 半角文字、ホワイトスペースが入っていないかチェック
            bool result = true;
            string[] patternList =
            {
                Validator.RegexHarfWidth,
                Validator.RegexWhiteSpace,
            };

            string pattern = String.Format("{0}", String.Join("|", patternList));
            if (true == Regex.IsMatch(value.ToString(), pattern))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定されたオブジェクトの内容が指定された範囲内かを判定します。
        /// </summary>
        /// <param name="value">判定対象オブジェクト</param>
        /// <param name="minValue">最小数</param>
        /// <param name="maxValue">最大数</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <exception cref="System.ArgumentException">
        /// valueのToStringメソッドでクラス名を取得した場合、発生します。
        /// または、minValueとmaxValueの大小関係が不正な場合、発生します。
        /// </exception>
        /// <exception cref="System.FormatException">valueが数値に変換できない場合、発生します。</exception>
        /// <returns>指定された範囲内であれば、true。指定された範囲外であれば、falseを返します。</returns>
        public static bool IsRange(object value, decimal minValue, decimal maxValue)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 型チェック
            if (value.GetType().FullName == value.ToString())
            {
                throw new ArgumentException("文字列が取得できません。", "value");
            }

            // 範囲チェック
            if (minValue > maxValue)
            {
                throw new ArgumentException("最大数が最小数より小さい数です", "maxValue");
            }

            // 数値型に変換
            decimal number = Account.ConvertDecimal(value.ToString());

            // 最小数より小さい、または、最大数より大きいかチェック
            bool result = true;
            if ((minValue > number) || (maxValue < number))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字列が指定されたパターンかを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <param name="regular">判定パターンを正規表現で表した文字列</param>
        /// <exception cref="System.ArgumentNullException">
        /// valueがNullの場合、発生します。
        /// または、regularがNullの場合、発生します。
        /// </exception>
        /// <returns>指定されたパターンであれば、true。指定されたパターンでなければ、falseを返します。</returns>
        public static bool IsRegular(string value, string regular)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // Null値チェック
            if (null == regular)
            {
                throw new ArgumentNullException("regular");
            }

            // 正規表現チェック
            bool result = Regex.IsMatch(value, regular);
            return result;
        }

        /// <summary>
        /// 指定された文字列が日付かを判定します。
        /// </summary>
        /// <param name="value">判定対象文字列</param>
        /// <exception cref="System.ArgumentNullException">valueがNullの場合、発生します。</exception>
        /// <returns>日付であれば、true。日付でなければ、falseを返します。</returns>
        public static bool IsDate(string value)
        {
            // Null値チェック
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            // 正規表現チェック
            DateTime dummy;
            bool result = DateTime.TryParse(value, out dummy); 

            return result;
        }
        #endregion

        #region イベント
        /// <summary>
        /// 検証を行う際に発生します。
        /// </summary>
        public event CancelEventHandler Validating;
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 検証を行います。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <returns>全ての検証が成功したら、true。１つでも失敗していたら、falseを返します。</returns>
        public bool Validate(object sender)
        {
            bool result = true;

            CancelEventArgs e = new CancelEventArgs();
            this.Validating(sender, e);

            if (true == e.Cancel)
            {
                result = false;
            }

            return result;
        }
        #endregion
    }
}
