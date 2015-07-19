// ******************************************************************
// ContainerControlExtension.cs ： ContainerControl拡張クラス
// 作成日　：2013/03/06
// 更新履歴：2014/01/09 水落　　 TabIndex設定処理を親コントロールを考慮するよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// ContainerControlクラスを拡張するクラスです。
    /// </summary>
    public static class ContainerControlExtension
    {
        /// <summary>
        /// コンポーネントがアクティブなコントロールを子コンテナを辿って取得します。
        /// </summary>
        /// <param name="container">コンテナコントロール</param>
        /// <returns>アクティブコントロール</returns>
        public static Control GetNestActiveControl(this ContainerControl container)
        {
            Control activeControl = container.ActiveControl;
            if (activeControl is ContainerControl)
            {
                activeControl = ((ContainerControl)activeControl).GetNestActiveControl();
            }

            return activeControl;
        }

        /// <summary>
        /// 指定されたコントロールに順番にTabIndexを設定します。
        /// </summary>
        /// <param name="container">コンテナコントロール</param>
        /// <param name="controlArray">コントロールの配列</param>
        public static void SetTabIndex(this ContainerControl container, params Control[] controlArray)
        {
            int index = 0;
            List<Control> setControlList = new List<Control>();

            foreach (var control in controlArray)
            {
                index = ContainerControlExtension.RecursiveSetTabIndex(index, control, setControlList);
            }

            return;
        }

        /// <summary>
        /// 親のコントロールに対してTabIndexを設定します。
        /// </summary>
        /// <param name="index">設定するTabIndex</param>
        /// <param name="control">設定対象コントロール</param>
        /// <param name="setControlList">設定済みコントロールリスト</param>
        /// <returns></returns>
        private static int RecursiveSetTabIndex(int index, Control control, IList<Control> setControlList)
        {
            // 既に設定済みか判定
            if (true == setControlList.Contains(control))
            {
                return index;
            }

            // 親コントロールのTabIndexを設定
            if (null != control.Parent)
            {
                index = ContainerControlExtension.RecursiveSetTabIndex(index, control.Parent, setControlList);
            }

            // 自コントロールのTabIndexを設定
            control.TabIndex = index;
            ++index;

            // 設定済みに追加
            setControlList.Add(control);

            return index;
        }
    }
}
