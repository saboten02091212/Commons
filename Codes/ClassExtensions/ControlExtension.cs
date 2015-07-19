// ******************************************************************
// ControlExtension.cs ： Control拡張クラス
// 作成日　：2013/02/25
// 更新履歴：2014/07/07 水落　　 未生成破棄判定メソッドを追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// Controlクラスを拡張するクラスです。
    /// </summary>
    public static class ControlExtension
    {
        #region デザインモード判定メソッド
        /// <summary>
        /// コンポーネントがデザインモードかどうかを示す値を親コンテナを辿って取得します。
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <returns>デザインモードの場合、true。そうでない場合、falseを返します。</returns>
        public static bool GetNestDesignMode(this Control control)
        {
            bool isDesign = false;

            // 親コンテナを辿ってデザインモードかどうか判定
            Control currentControl = control;
            while (null != currentControl)
            {
                // カレントコントロールのデザインモードかどうか判定
                ISite site = currentControl.Site;
                if (null != site)
                {
                    isDesign = site.DesignMode;
                    if (true == isDesign)
                    {
                        // デザインモードだったらループを抜ける
                        break;
                    }
                }

                // カレントコントロールを親コンテナに移動
                currentControl = currentControl.Parent;
            }

            return isDesign;
        }
        #endregion

        #region 未生成破棄判定メソッド
        /// <summary>
        /// 未生成。または、破棄されたかを判定します。
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <returns>コントロールが生成されていない、もしくは、破棄されていた場合、true。生成され、未だ破棄されていない場合、falseを返します。</returns>
        public static bool IsDisposedOrNull(this Control control)
        {
            bool result = false;

            // コントロールが生成されたか判定
            if (false == result)
            {
                if (null == control)
                {
                    result = true;
                }
            }

            // コントロールが破棄されたか判定
            if (false == result)
            {
                if (true == control.IsDisposed)
                {
                    result = true;
                }
            }

            return result;
        }
        #endregion
    }
}
