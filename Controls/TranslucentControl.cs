// ******************************************************************
// TranslucentControl.cs ： 背景透過コントロールクラス
// 作成日　：2015/03/25
// 更新履歴：2015/03/25 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Mizuochi.Commons.Codes.Windows;

namespace Mizuochi.Commons.Controls
{
    /// <summary>
    /// 背景を透過させる機能を保有した継承用のコントロールです。
    /// </summary>
    public partial class TranslucentControl : UserControl
    {
        #region API宣言
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        private const uint PRF_NONCLIENT = 0x00000002;
        private const uint PRF_CLIENT = 0x00000004;
        private const uint PRF_ERASEBKGND = 0x00000008;
        private const uint PRF_CHILDREN = 0x00000010;
        private const int WM_PRINT = 0x0317;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public TranslucentControl()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// コントロールの背景を描画します。
        /// </summary>
        /// <param name="e">描画イベントデータを格納しています。</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

#if DEBUG
            // デザインモード時は透過しない
            if (true == this.DesignMode)
            {
                return;
            }
#endif

            // 親がいない場合はそのまま
            if (null == this.Parent)
            {
                return;
            }

            // 描画座標の原点を親コントロールの座標へ
            e.Graphics.TranslateTransform(-this.Left, -this.Top);

            // 親コントロールを描画
            this.InvokePaintBackground(this.Parent, e);
            this.InvokePaint(this.Parent, e);

            // 背面にコントロールを描画
            foreach (Control control in this.Parent.Controls)
            {
                // 自身のコントロールの場合、処理しない
                if (control == this)
                {
                    continue;
                }

                // コントロールを背面に描画
                this.DrawBackground(control, e.Graphics);

                control.Invalidated += this.HidingControl_Invalidated;
            }
        }

        /// <summary>
        /// 背面に隠れているコントロールを背景として描画します。
        /// </summary>
        /// <param name="control">背面に隠れているコントロール</param>
        /// <param name="graphics">描画インスタンス</param>
        protected virtual void DrawBackground(Control control, Graphics graphics)
        {
            // 非表示、背面以外 の場合、処理しない
            if ((false == control.Visible) ||
                (false == control.Bounds.IntersectsWith(this.Bounds)))
            {
                return;
            }

            // ビットマップ生成
            using (Bitmap bmp = new Bitmap(control.Width, control.Height))
            {
                // 背面のコントロールを描画
                this.DrawControl(control, bmp);
                graphics.DrawImage(bmp, control.Left, control.Top);
            }

            // 子コントロールを描画
            foreach (Control childControl in control.Controls)
            {
                this.DrawBackground(childControl, graphics);
            }

            return;
        }

        /// <summary>
        /// Bitmapに指定されたコントロールを描画します。
        /// </summary>
        /// <param name="control">描画するコントロール</param>
        /// <param name="bmp">描画されるBitmap</param>
        protected virtual void DrawControl(Control control, Bitmap bmp)
        {
            using (Graphics bmpGraphics = Graphics.FromImage(bmp))
            {
                // 背面のコントロールをビットマップに描画
                var handle = bmpGraphics.GetHdc();
                SendMessage(
                    new HandleRef(control, control.Handle),
                    WM_PRINT,
                    handle,
                    (IntPtr)(PRF_CHILDREN | PRF_CLIENT | PRF_ERASEBKGND | PRF_NONCLIENT));
                bmpGraphics.ReleaseHdc(handle);
            }

            return;
        }

        /// <summary>
        /// 後ろに隠れているコントロールが再描画するときに発生します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void HidingControl_Invalidated(object sender, InvalidateEventArgs e)
        {
            this.Invalidate();
            return;
        }
    }
}