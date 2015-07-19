// ******************************************************************
// ButtonExtension.cs ： Button拡張クラス
// 作成日　：2013/10/01
// 更新履歴：2013/10/01 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// Buttonクラスを拡張するクラスです。
    /// </summary>
    public static class ButtonExtension
    {
        /// <summary>
        /// 指定した透明色をボタンに表示されるイメージに使用します。
        /// </summary>
        /// <param name="button">ボタンコントロール</param>
        /// <param name="transparentColor">透明にする色を表す System.Drawing.Color 構造体。</param>
        public static void MakeTransparent(this Button button, Color transparentColor)
        {
            var icon = new Bitmap(button.Image);
            icon.MakeTransparent(transparentColor);
            button.Image = icon;

            return;
        }
    }
}
