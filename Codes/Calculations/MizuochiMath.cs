// ******************************************************************
// MizuochiMath.cs ：ギャラクシー算術クラス
// 作成日　：2012/12/12
// 更新履歴：2013/01/16 水落　　 .Net Framework4.0 対応。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Calculations
{
    /// <summary>
    /// 数学的なメソッドを擁するクラスです。
    /// </summary>
    public static class MizuochiMath
    {
        #region 静的パブリックメソッド
        /// <summary>
        /// 指定された数値に対して、端数処理を行います。
        /// </summary>
        /// <param name="value">処理対象数値を指定して下さい。</param>
        /// <param name="decimals">小数点以下の有効桁数を指定して下さい。</param>
        /// <param name="roundType">丸め区分を指定して下さい。</param>
        /// <exception cref="System.ArgumentOutOfRangeException">decimalsに0未満、28より大きい値が設定された場合、発生します。</exception>
        /// <exception cref="System.ArgumentException">roundTypeに想定しない値が設定された場合、発生します。</exception>
        /// <returns>端数処理された数値</returns>
        public static decimal Round(decimal value, int decimals = 0, RoundType roundType = RoundType.RoundOff)
        {
            // 丸め区分によって調整値を算出
            decimal adjuster;
            switch (roundType)
            {
                // 四捨五入：調整値なし
                case RoundType.RoundOff:
                    adjuster = 0;
                    break;

                // 切り上げ：丸め幅の半分 - 1を加算
                case RoundType.RoundUp:
                    adjuster = Convert.ToDecimal(0 + Math.Pow(0.1D, decimals)) * 0.4M;
                    adjuster *= Math.Sign(value);
                    break;

                // 切り下げ：丸め幅の半分を減算
                case RoundType.RoundDown:
                    adjuster = Convert.ToDecimal(0 - Math.Pow(0.1D, decimals)) * 0.5M;
                    adjuster *= Math.Sign(value);
                    break;

                default:
                    throw new InvalidEnumArgumentException("丸め区分の値が不正です。");
            }
            
            // 丸め処理
            value = Math.Round(value + adjuster, decimals, MidpointRounding.AwayFromZero);
            return value;
        }

        /// <summary>
        /// 指定された数値に対して、端数処理を行います。
        /// </summary>
        /// <param name="value">処理対象数値を指定して下さい。</param>
        /// <param name="decimals">小数点以下の有効桁数を指定して下さい。</param>
        /// <param name="roundType">丸め区分を指定して下さい。</param>
        /// <exception cref="System.ArgumentOutOfRangeException">decimalsに0未満、28より大きい値が設定された場合、発生します。</exception>
        /// <exception cref="System.ArgumentException">roundTypeに想定しない値が設定された場合、発生します。</exception>
        /// <returns>端数処理された数値</returns>
        public static double Round(double value, int decimals = 0, RoundType roundType = RoundType.RoundOff)
        {
            // 丸め区分によって調整値を算出
            double adjuster;
            switch (roundType)
            {
                // 四捨五入：調整値なし
                case RoundType.RoundOff:
                    adjuster = 0;
                    break;

                // 切り上げ：丸め幅の半分 - 1を加算
                case RoundType.RoundUp:
                    adjuster = 0 + Math.Pow(0.1D, decimals) * 0.4D;
                    adjuster *= Math.Sign(value);
                    break;

                // 切り下げ：丸め幅の半分を減算
                case RoundType.RoundDown:
                    adjuster = 0 - Math.Pow(0.1D, decimals) * 0.5D;
                    adjuster *= Math.Sign(value);
                    break;

                default:
                    throw new InvalidEnumArgumentException("丸め区分の値が不正です。");
            }

            // 丸め処理
            value = Math.Round(value + adjuster, decimals, MidpointRounding.AwayFromZero);
            return value;
        }
        #endregion
    }

    /// <summary>
    /// 丸め区分
    /// </summary>
    public enum RoundType
    {
        /// <summary>
        /// 四捨五入
        /// </summary>
        RoundOff,
        /// <summary>
        /// 切り上げ
        /// </summary>
        RoundUp,
        /// <summary>
        /// 切り下げ
        /// </summary>
        RoundDown,
    }
}
