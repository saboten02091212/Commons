// ******************************************************************
// 作成日　：2014/02/26
// 更新履歴：2014/10/02 水落　　 範囲丸め拡張メソッドを追加。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// ジェネリックスのクラスを拡張するクラスです。
    /// </summary>
    public static class GenericsExtension
    {
        /// <summary>
        /// 別型の値に変換します。
        /// </summary>
        /// <typeparam name="InstanceType">インスタンスの型です。</typeparam>
        /// <typeparam name="ReturnType">戻り値の型です。</typeparam>
        /// <param name="instance">変換対象です。</param>
        /// <param name="selector">変換対象から戻り値を取得するメソッドです。</param>
        /// <returns>変換後の型の値</returns>
        public static ReturnType Convert<InstanceType, ReturnType>(this InstanceType instance, Func<InstanceType, ReturnType> selector)
        {
            // 変換メソッドがnullか判定
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            // インスタンス自体がnullか判定
            if (instance == null)
            {
                return default(ReturnType);
            }

            // 変換メソッドを使用した結果をキャスト
            return selector(instance);
        }

        /// <summary>
        /// 指定した最小値と最大値の範囲外の場合、指定範囲内の値を返却します。
        /// </summary>
        /// <typeparam name="T">インスタンスの型です。</typeparam>
        /// <param name="instance">範囲を絞る対象です。</param>
        /// <param name="minValue">最小値</param>
        /// <param name="maxValue">最大値</param>
        /// <returns>指定範囲内の値</returns>
        public static T Clamp<T>(this T instance, T minValue, T maxValue)
            where T : IComparable
        {
            return
                (instance.CompareTo(minValue) < 0) ? minValue :
                (instance.CompareTo(maxValue) > 0) ? maxValue :
                instance;
        }
    }
}
