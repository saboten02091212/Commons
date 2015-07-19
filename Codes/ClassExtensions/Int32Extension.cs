// ******************************************************************
// 作成日　：2014/10/01
// 更新履歴：2014/10/01 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// Int32クラスを拡張するクラスです。
    /// </summary>
    public static class Int32Extension
    {
        /// <summary>
        /// 指定された処理を回数を渡して、繰り返します。
        /// </summary>
        /// <param name="times">繰り返す回数</param>
        /// <param name="loopAction">繰り返す処理</param>
        public static void Times(this int times, Action<int> loopAction)
        {
            for (int index = 0; index < times; ++index)
            {
                loopAction(index);
            }

            return;
        }

        /// <summary>
        /// 指定された処理を、繰り返します。
        /// </summary>
        /// <param name="times">繰り返す回数</param>
        /// <param name="loopAction">繰り返す処理</param>
        public static void Times(this int times, Action loopAction)
        {
            times.Times(index => loopAction());
            return;
        }
    }
}
