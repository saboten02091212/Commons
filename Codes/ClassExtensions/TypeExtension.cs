// ******************************************************************
// TypeExtension.cs ： Type拡張クラス
// 作成日　：2013/01/22
// 更新履歴：2013/01/22 水落　　 新規作成。
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
    /// Typeクラスを拡張するクラスです。
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// 現在の型からジェネリックのリストオブジェクトを生成します。
        /// </summary>
        /// <param name="type">生成するリスト内の型</param>
        /// <returns>空のリストオブジェクト</returns>
        public static IList CreateList(this Type type)
        {
            // リストオブジェクトの型を取得
            string listTypeName = String.Format("System.Collections.Generic.List`1[[{0}]]", type.AssemblyQualifiedName);
            Type listType = Type.GetType(listTypeName);

            // 型からオブジェクトを生成
            IList list = (IList)Activator.CreateInstance(listType);

            return list;
        }

        /// <summary>
        /// 現在の型が、数値型かを判定します。
        /// </summary>
        /// <param name="type">判定する型</param>
        /// <returns>数値型であれば、True。数値型でなければ、falseを返します。</returns>
        public static bool IsNumeric(this Type type)
        {
            bool result = false;

            if ((typeof(byte) == type) ||
                (typeof(sbyte) == type) || 
                (typeof(short) == type) || 
                (typeof(ushort) == type) || 
                (typeof(int) == type) || 
                (typeof(uint) == type) || 
                (typeof(long) == type) || 
                (typeof(ulong) == type) || 
                (typeof(float) == type) || 
                (typeof(double) == type) || 
                (typeof(decimal) == type))
            {
                result = true;
            }

            return result;
        }
    }
}
