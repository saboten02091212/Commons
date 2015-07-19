// ******************************************************************
// IEnumerableGenericsExtension.cs ： IEnumerable<T>拡張クラス
// 作成日　：2014/09/10
// 更新履歴：2014/10/02 水落　　 ForEachメソッドを追加。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// IEnumerable&lt;T&gt;クラスを拡張するクラスです。
    /// </summary>
    public static class IEnumerableGenericsExtension
    {
        /// <summary>
        /// コレクションから重複した要素を削除して返します。
        /// </summary>
        /// <typeparam name="T">コレクションの内部型</typeparam>
        /// <param name="enumerable">コレクション</param>
        /// <param name="comparerFunctionArray">重複判定要素の配列</param>
        /// <returns>重複した要素を削除したコレクションを返します。</returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumerable, params Func<T, object>[] comparerFunctionArray)
        {
            // 重複削除リストを生成
            var distinctList = new List<T>();

            foreach (var source in enumerable)
            {
                // 重複削除リストに既に登録済みか判定
                bool isDistinct = distinctList.Any(destination => 
                {
                    // 全ての重複判定要素が同一であるか判定
                    return comparerFunctionArray.All(comparerFunction =>
                    {
                        bool result = true;
                        if ((source != null) && (destination != null))
                        {
                            object sourceValue = comparerFunction(source);
                            object destinationValue = comparerFunction(destination);
                            result = (sourceValue != null) ?
                                (sourceValue.Equals(destinationValue)) :
                                (sourceValue == destinationValue);
                        }
                        else
                        {
                            result = ((source == null) && (destination == null));
                        }

                        return result;
                    });
                });

                // 重複削除リストに入ってなければ追加
                if (isDistinct == false)
                {
                    distinctList.Add(source);
                }
            }

            return distinctList;
        }

        /// <summary>
        /// 各要素に対して、指定された処理を実行します。
        /// </summary>
        /// <typeparam name="T">コレクションの内部型</typeparam>
        /// <param name="enumerable">コレクション</param>
        /// <param name="action">処理</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }

            return;
        }
    }
}
