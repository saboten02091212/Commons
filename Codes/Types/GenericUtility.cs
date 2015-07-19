// ******************************************************************
// GenericUtility.cs  ：ジェネリック型汎用クラス
// 作成日　：2015/03/20
// 更新履歴：2015/03/20 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Types
{
    /// <summary>
    /// ジェネリック型に関する汎用メソッドを保持するクラスです。
    /// </summary>
    public class GenericUtility
    {
        /// <summary>
        /// ジェネリック型のインスタンスを生成します。
        /// </summary>
        /// <param name="genericDefinitionType">生成するジェネリック型</param>
        /// <param name="genericArgumentTypeArray">ジェネリック型の型引数の配列</param>
        /// <param name="args">生成時に使用するコンストラクタのパラメタの配列リスト</param>
        /// <returns>ジェネリック型のインスタンス</returns>
        public static object CreateGenericTypeInstance(Type genericDefinitionType, Type[] genericArgumentTypeArray, params object[] args)
        {
            // 引数チェック
            if (false == genericDefinitionType.IsGenericTypeDefinition)
            {
                throw new ArgumentException("指定されたgenericTypeは、ジェネリック型定義ではありません。");
            }

            if (genericDefinitionType.GetGenericArguments().Count() != genericArgumentTypeArray.Count())
            {
                throw new ArgumentOutOfRangeException("生成するジェネリック型の引数の数と、genericArgumentTypeArrayの数が異なります。");
            }

            // インスタンス生成
            Type genericType = genericDefinitionType.MakeGenericType(genericArgumentTypeArray);
            return Activator.CreateInstance(genericType, args);
        }
    }
}
