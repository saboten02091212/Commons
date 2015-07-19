// ******************************************************************
// ColorExtension.cs ： Color拡張クラス
// 作成日　：2015/03/24
// 更新履歴：2015/03/24 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// Colorクラスを拡張するクラスです。
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// アルファ コンポーネントの値を変更したColorオブジェクトを返します。
        /// </summary>
        /// <param name="color">カラー</param>
        /// <param name="alpha">アルファ コンポーネントの値</param>
        /// <returns>変更したカラー</returns>
        public static Color SetA(this Color color, byte alpha)
        {
            return Color.FromArgb(alpha, color.R, color.G, color.B);
        }

        /// <summary>
        /// 赤のコンポーネントの値を変更したColorオブジェクトを返します。
        /// </summary>
        /// <param name="color">カラー</param>
        /// <param name="red">赤のコンポーネントの値</param>
        /// <returns>変更したカラー</returns>
        public static Color SetR(this Color color, byte red)
        {
            return Color.FromArgb(color.A, red, color.G, color.B);
        }

        /// <summary>
        /// 緑のコンポーネントの値を変更したColorオブジェクトを返します。
        /// </summary>
        /// <param name="color">カラー</param>
        /// <param name="green">緑のコンポーネントの値</param>
        /// <returns>変更したカラー</returns>
        public static Color SetG(this Color color, byte green)
        {
            return Color.FromArgb(color.A, color.R, green, color.B);
        }

        /// <summary>
        /// 青のコンポーネントの値を変更したColorオブジェクトを返します。
        /// </summary>
        /// <param name="color">カラー</param>
        /// <param name="blue">青のコンポーネントの値</param>
        /// <returns>変更したカラー</returns>
        public static Color SetB(this Color color, byte blue)
        {
            return Color.FromArgb(color.A, color.R, color.G, blue);
        }
    }
}
