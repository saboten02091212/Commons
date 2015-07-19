// ******************************************************************
// DecimalExtension.cs ： Decimal拡張クラス
// 作成日　：2014/06/18
// 更新履歴：2014/06/18 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// DateTimeクラスを拡張するクラスです。
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// 整数部ないしは小数部が、指定された桁数以内であるかどうかを判定します。
        /// </summary>
        /// <param name="source">判定対象数値</param>
        /// <param name="integerPartDigit">整数部の桁数</param>
        /// <param name="decimalPartDigit">小数部の桁数</param>
        /// <returns>指定された桁数内の場合、true。指定された桁数以上の場合、falseを返します。</returns>
        public static bool IsSpecifiedLength(this decimal source, int integerPartDigit, int decimalPartDigit = 0)
        {
            // 整数部、小数部を取得
            decimal integerPart = Math.Truncate(source);
            decimal decimalPart = source - integerPart;

            // 桁数範囲内か判定
            bool result = 
                (Math.Abs(integerPart).ToString().Length > integerPartDigit) ? false :
                (Math.Abs(decimalPart).ToString().Length > (decimalPartDigit + 2)) ? false :
                true;

            return result;
        }
    }
}
