// ******************************************************************
// IEnumerableExtension.cs ： IEnumerable拡張クラス
// 作成日　：2013/03/12
// 更新履歴：2013/03/12 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// IEnumerableクラスを拡張するクラスです。
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// コレクション内の型を取得します。
        /// </summary>
        /// <param name="enumerable">取得するコレクション</param>
        /// <returns>コレクションが空の場合は、Null値を返します。それ以外の場合は、コレクション内の型を返します。</returns>
        public static Type GetInnerType(this IEnumerable enumerable)
        {
            Type type = null;
            foreach (object obj in enumerable)
            {
                type = obj.GetType();
                break;
            }

            return type;
        }
    }
}
