// ******************************************************************
// DateRange.cs ： 日付範囲入力ユーザーコントロール
// 作成日　：2013/01/18
// 更新履歴：2014/05/20 水落　　 入力補完機能の有無を制御するフラグを追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Controls
{
    /// <summary>
    /// 日付範囲入力を行うコントロールです。
    /// </summary>
    [DefaultEvent("DateRangeChanged")]
    [ToolboxBitmap(typeof(DateTimePicker))]
    public partial class DateRange : UserControl, ISupportInitialize
    {
        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public DateRange()
        {
            InitializeComponent();

            // 初期配置保持
            this.initializeLabelFrontWidth = this.LabelFrontWidth;
            this.initializeTextBoxFromDateWidth = this.TextBoxFromDateWidth;
            this.initializeTextBoxToDateWidth = this.TextBoxToDateWidth;

            // イベント登録
            base.CausesValidationChanged += this.DateRange_CausesValidationChanged;

            // メンバ設定
            this.autoComplete = true;
        }
        #endregion

        #region イベントハンドラ
        /// <summary>
        /// 日付範囲が変更された際に発生します。
        /// </summary>
        [Category("プロパティ変更")]
        [Description("日付範囲がコントロールで変更されたときに発生します。")]
        public event DateRangeEventHandler DateRangeChanged;

        /// <summary>
        /// バリデートエラーになった際に発生します。
        /// </summary>
        [Category("フォーカス")]
        [Description("コントロールの検証時にエラーを検出したときに発生します。")]
        public event EventHandler ValidateError;
        #endregion

        #region フィールド
        #region 初期化
        /// <summary>
        /// 初期化中かどうかを示します。
        /// </summary>
        private bool initializingFlg;
        /// <summary>
        /// 初期化時に保持する前方ラベルの幅です。
        /// </summary>
        private int initializeLabelFrontWidth;
        /// <summary>
        /// 初期化時に保持する開始日付テキストの幅です。
        /// </summary>
        private int initializeTextBoxFromDateWidth;
        /// <summary>
        /// 初期化時に保持する終了日付テキストの幅です。
        /// </summary>
        private int initializeTextBoxToDateWidth;
        /// <summary>
        /// 初期化時に保持するコントロールのサイズです。
        /// </summary>
        private Size initializeSize;
        #endregion

        /// <summary>
        /// 入力補完機能の有無
        /// </summary>
        private bool autoComplete;
        #endregion

        #region プロパティ
        #region デザイナパブリックプロパティ
        /// <summary>
        /// 前方ラベルの幅を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("前方ラベルの幅(ピクセル単位)です。")]
        public int LabelFrontWidth
        {
            get
            {
                return this.labelFront.Width;
            }
            set
            {
                if (false == this.initializingFlg)
                {
                    this.labelFront.Width = value;
                }
                else
                {
                    this.initializeLabelFrontWidth = value;
                }
            }
        }

        /// <summary>
        /// 開始日付テキストの幅を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("開始日付テキストの幅(ピクセル単位)です。")]
        public int TextBoxFromDateWidth
        {
            get
            {
                return this.dateTextBoxFrom.Width;
            }
            set
            {
                if (false == this.initializingFlg)
                {
                    this.dateTextBoxFrom.Width = value;
                }
                else
                {
                    this.initializeTextBoxFromDateWidth = value;
                }
            }
        }

        /// <summary>
        /// 終了日付テキストの幅を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("終了日付テキストの幅(ピクセル単位)です。")]
        public int TextBoxToDateWidth
        {
            get
            {
                return this.dateTextBoxTo.Width;
            }
            set
            {
                if (false == this.initializingFlg)
                {
                    this.dateTextBoxTo.Width = value;
                }
                else
                {
                    this.initializeTextBoxToDateWidth = value;
                }
            }
        }

        /// <summary>
        /// 前方ラベルのテキストを取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("前方ラベルのテキストです。")]
        [DefaultValue("LabelFront")]
        public string LabelFrontText
        {
            get
            {
                return this.labelFront.Text;
            }
            set
            {
                this.labelFront.Text = value;
            }
        }

        /// <summary>
        /// 前方ラベルのテキスト位置を取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("前方ラベルのテキスト位置です。")]
        [DefaultValue(typeof(ContentAlignment), "MiddleRight")]
        public ContentAlignment LabelFrontTextAlign
        {
            get
            {
                return this.labelFront.TextAlign;
            }
            set
            {
                this.labelFront.TextAlign = value;
            }
        }

        /// <summary>
        /// 前方ラベルの表示、非表示を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("前方ラベルの表示、非表示を示します。")]
        [DefaultValue(true)]
        public bool LabelFrontVisible
        {
            get
            {
                return this.labelFront.Visible;
            }
            set
            {
                this.labelFront.Visible = value;
            }
        }

        /// <summary>
        /// 中央ラベルのテキストを取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("中央ラベルのテキストです。")]
        [DefaultValue("～")]
        public string LabelMiddelText
        {
            get
            {
                return this.labelMiddle.Text;
            }
            set
            {
                this.labelMiddle.Text = value;
            }
        }

        /// <summary>
        /// 中央ラベルの表示、非表示を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("中央ラベルの表示、非表示を示します。")]
        [DefaultValue(true)]
        public bool LabelMiddleVisible
        {
            get
            {
                return this.labelMiddle.Visible;
            }
            set
            {
                this.labelMiddle.Visible = value;
            }
        }

        /// <summary>
        /// 入力補完機能の有無を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("入力補完機能の有無を示します。")]
        [DefaultValue(true)]
        public bool AutoComplete
        {
            get
            {
                return autoComplete;
            }
            set
            {
                autoComplete = value;
            }
        }

        /// <summary>
        /// 開始日付テキストボックスの内容を取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("開始日付テキストボックスの内容です。")]
        [DefaultValue("")]
        public string FromDateText
        {
            get
            {
                return this.dateTextBoxFrom.DateText;
            }
            set
            {
                this.dateTextBoxFrom.DateText = value;
            }
        }

        /// <summary>
        /// 終了日付テキストボックスの内容を取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("終了日付テキストボックスの内容です。")]
        [DefaultValue("")]
        public string ToDateText
        {
            get
            {
                return this.dateTextBoxTo.DateText;
            }
            set
            {
                this.dateTextBoxTo.DateText = value;
            }
        }

        /// <summary>
        /// 開始日付テキストボックスのカレンダーコントロールのオフセット位置を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("開始日付テキストボックスのカレンダーコントロールのオフセット位置です。")]
        public Point FromDateCalenderLocationOffset
        {
            get
            {
                return this.dateTextBoxFrom.CalenderLocationOffset;
            }
            set
            {
                this.dateTextBoxFrom.CalenderLocationOffset = value;
            }
        }

        /// <summary>
        /// 終了日付テキストボックスのカレンダーコントロールのオフセット位置を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("終了日付テキストボックスのカレンダーコントロールのオフセット位置です。")]
        public Point ToDateCalenderLocationOffset
        {
            get
            {
                return this.dateTextBoxTo.CalenderLocationOffset;
            }
            set
            {
                this.dateTextBoxTo.CalenderLocationOffset = value;
            }
        }

        /// <summary>
        /// コントロールのサイズ(ピクセル単位)を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("コントロールのサイズ(ピクセル単位)です。")]
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                if (false == this.initializingFlg)
                {
                    base.Size = value;
                }
                else
                {
                    this.initializeSize = value;
                }
            }
        }
        #endregion

        #region 非デザイナパブリックプロパティ
        /// <summary>
        /// 開始日付テキストボックスの内容を日付型で取得、設定します。
        /// </summary>
        /// <exception cref="System.FormatException">開始日付テキストボックスに日付が入力されていない場合、発生します。</exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime FromDateValue
        {
            get
            {
                return this.dateTextBoxFrom.Value;
            }
            set
            {
                this.dateTextBoxFrom.Value = value;
            }
        }

        /// <summary>
        /// 終了日付テキストボックスの内容を日付型で取得、設定します。
        /// </summary>
        /// <exception cref="System.FormatException">終了日付テキストボックスに日付が入力されていない場合、発生します。</exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime ToDateValue
        {
            get
            {
                return this.dateTextBoxTo.Value;
            }
            set
            {
                this.dateTextBoxTo.Value = value;
            }
        }

        /// <summary>
        /// 開始日付が正しい日付型かどうかを示します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ExactFromDateTime
        {
            get
            {
                return this.dateTextBoxFrom.ExactDateTime;
            }
        }

        /// <summary>
        /// 終了日付が正しい日付型かどうかを示します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ExactToDateTime
        {
            get
            {
                return this.dateTextBoxTo.ExactDateTime;
            }
        }

        /// <summary>
        /// バリデーション処理を行います。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CancelEventHandler Validation
        {
            get
            {
                return this.DateRangeValidate;
            }
        }
        #endregion
        #endregion

        #region イベント
        /// <summary>
        /// コントロールロード時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void DateRange_Load(object sender, EventArgs e)
        {
            // イベントバブリングのための登録
            this.dateTextBoxFrom.ValidateError += this.ChildrenValidateError;
            this.dateTextBoxTo.ValidateError += this.ChildrenValidateError;

            return;
        }

        /// <summary>
        /// Enabledプロパティが変更された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void DateRange_EnabledChanged(object sender, EventArgs e)
        {
            this.TabStop = this.Enabled;
            return;
        }

        /// <summary>
        /// CausesValidationプロパティの値が変更された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void DateRange_CausesValidationChanged(object sender, EventArgs e)
        {
            this.dateTextBoxFrom.CausesValidation = this.CausesValidation;
            this.dateTextBoxTo.CausesValidation = this.CausesValidation;

            return;
        }

        /// <summary>
        /// 前方ラベルのリサイズ時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void labelFront_Resize(object sender, EventArgs e)
        {
            // コントロール移動と共にリサイズ、移動させない
            this.labelMiddle.Anchor &= ~AnchorStyles.Left;

            // テキストボックスの位置とコントロール幅を修正
            this.dateTextBoxFrom.Left = this.labelFront.Right + this.labelFront.Margin.Right + this.dateTextBoxFrom.Margin.Left;
            this.labelMiddle.Left = this.dateTextBoxFrom.Right + this.dateTextBoxFrom.Margin.Right + this.labelMiddle.Margin.Left;
            this.Width =
                this.labelFront.Margin.Left + this.labelFront.Width + this.labelFront.Margin.Right +
                this.dateTextBoxFrom.Margin.Left + this.dateTextBoxFrom.Width + this.dateTextBoxFrom.Margin.Right +
                this.labelMiddle.Margin.Left + this.labelMiddle.Width + this.labelMiddle.Margin.Right +
                this.dateTextBoxTo.Margin.Left + this.dateTextBoxTo.Width + this.dateTextBoxTo.Margin.Right;

            // コントロール移動と共にリサイズ、移動させる
            this.labelMiddle.Anchor |= AnchorStyles.Left;
            return;
        }

        /// <summary>
        /// 開始日付コントロールのリサイズ時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void dateTextBoxFrom_Resize(object sender, EventArgs e)
        {
            // コントロール移動と共にリサイズ、移動させない
            this.labelMiddle.Anchor &= ~AnchorStyles.Left;

            // テキストボックスの位置とコントロール幅を修正
            this.labelMiddle.Left = this.dateTextBoxFrom.Right + this.dateTextBoxFrom.Margin.Right + this.labelMiddle.Margin.Left;
            this.Width =
                this.labelFront.Margin.Left + this.labelFront.Width + this.labelFront.Margin.Right +
                this.dateTextBoxFrom.Margin.Left + this.dateTextBoxFrom.Width + this.dateTextBoxFrom.Margin.Right +
                this.labelMiddle.Margin.Left + this.labelMiddle.Width + this.labelMiddle.Margin.Right +
                this.dateTextBoxTo.Margin.Left + this.dateTextBoxTo.Width + this.dateTextBoxTo.Margin.Right;

            // コントロール移動と共にリサイズ、移動させる
            this.labelMiddle.Anchor |= AnchorStyles.Left;
            return;
        }

        /// <summary>
        /// 終了日付コントロールのリサイズ時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void dateTextBoxTo_Resize(object sender, EventArgs e)
        {
            // コントロール移動と共にリサイズ、移動させない
            this.labelMiddle.Anchor &= ~AnchorStyles.Right;

            // コントロール幅を修正
            this.Width = 
                this.labelFront.Margin.Left + this.labelFront.Width + this.labelFront.Margin.Right +
                this.dateTextBoxFrom.Margin.Left + this.dateTextBoxFrom.Width + this.dateTextBoxFrom.Margin.Right +
                this.labelMiddle.Margin.Left + this.labelMiddle.Width + this.labelMiddle.Margin.Right +
                this.dateTextBoxTo.Margin.Left + this.dateTextBoxTo.Width + this.dateTextBoxTo.Margin.Right;

            // コントロール移動と共にリサイズ、移動させる
            this.labelMiddle.Anchor |= AnchorStyles.Right;

            return;
        }

        /// <summary>
        /// 開始日付が選択された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void dateTextBoxFrom_DateSelected(object sender, DateRangeEventArgs e)
        {
            // 開始日付と終了日付を取得
            DateTime fromDate;
            DateTime toDate;
            try
            {
                fromDate = this.FromDateValue;
                toDate = this.ToDateValue;
            }
            catch (FormatException)
            {
                return;
            }

            // 補正機能がtrueか判定
            if (true == this.autoComplete)
            {
                // 終了日付が開始日付より過去の場合、補正
                if ((toDate < fromDate) && (true == this.ExactFromDateTime))
                {
                    this.ToDateValue = this.FromDateValue;
                    toDate = this.ToDateValue;
                }
            }

            // 日付範囲変更イベント発火
            if (null != this.DateRangeChanged)
            {
                DateRangeEventArgs rangeEventArgs = new DateRangeEventArgs(fromDate, toDate);
                this.DateRangeChanged(this, rangeEventArgs);
            }

            return;
        }

        /// <summary>
        /// 終了日付が選択された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void dateTextBoxTo_DateSelected(object sender, DateRangeEventArgs e)
        {
            // 開始日付と終了日付を取得
            DateTime fromDate;
            DateTime toDate;
            try
            {
                fromDate = this.FromDateValue;
                toDate = this.ToDateValue;
            }
            catch (FormatException)
            {
                return;
            }
            
            // 補正機能がtrueか判定
            if (true == this.autoComplete)
            {
                // 開始日付が終了日付より未来の場合、補正
                if ((toDate < fromDate) && (true == this.ExactToDateTime))
                {
                    this.FromDateValue = this.ToDateValue;
                    fromDate = this.FromDateValue;
                }
            }

            // 日付範囲変更イベント発火
            if (null != this.DateRangeChanged)
            {
                DateRangeEventArgs rangeEventArgs = new DateRangeEventArgs(fromDate, toDate);
                this.DateRangeChanged(this, rangeEventArgs);
            }

            return;
        }

        /// <summary>
        /// 開始日付が変更されたときに発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void dateTextBoxFrom_DateTextChanged(object sender, EventArgs e)
        {
            this.dateTextBoxFrom.ValidateClear();
            this.dateTextBoxTo.ValidateClear();
            return;
        }

        /// <summary>
        /// 終了日付が変更されたときに発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void dateTextBoxTo_DateTextChanged(object sender, EventArgs e)
        {
            this.dateTextBoxFrom.ValidateClear();
            this.dateTextBoxTo.ValidateClear();
            return;
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 検証処理が成功した場合の処理です。
        /// </summary>
        public void ValidateClear()
        {
            this.dateTextBoxFrom.ValidateClear();
            this.dateTextBoxTo.ValidateClear();

            return;
        }

        /// <summary>
        /// 検証処理が失敗した場合の処理です。
        /// </summary>
        public void ValidateFail()
        {
            this.dateTextBoxFrom.ValidateFail();
            this.dateTextBoxTo.ValidateFail();

            return;
        }
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// 検証処理を行います。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void DateRangeValidate(object sender, CancelEventArgs e)
        {
            // 日付コントロールの検証処理
            this.dateTextBoxFrom.Validation(sender, e);
            this.dateTextBoxTo.Validation(sender, e);
            if (true == e.Cancel)
            {
                return;
            }

            // ダミーループ
            bool result = true;
            for (; ; )
            {
                if (this.FromDateValue > this.ToDateValue)
                {
                    result = false;
                    break;
                }

                break;
            }

            if (false == result)
            {
                e.Cancel = true;
                this.ValidateFail();

                if (null != this.ValidateError)
                {
                    this.ValidateError(sender, e);
                }
            }
            else
            {
                this.ValidateClear();
            }

            return;
        }

        /// <summary>
        /// 子コントロールから呼ばれ、検証エラー処理を行います。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void ChildrenValidateError(object sender, EventArgs e)
        {
            if (null != this.ValidateError)
            {
                this.ValidateError(sender, e);
            }
            return;
        }

        #region ISupportInitialize メンバー
        /// <summary>
        /// 初期化時の最初に行うべき処理を実施します。
        /// </summary>
        void ISupportInitialize.BeginInit()
        {
            this.initializingFlg = true;

            return;
        }

        /// <summary>
        /// 初期化時の最後に行うべき処理を実施します。
        /// </summary>
        void ISupportInitialize.EndInit()
        {
            this.initializingFlg = false;

            this.LabelFrontWidth = this.initializeLabelFrontWidth;
            this.TextBoxFromDateWidth = this.initializeTextBoxFromDateWidth;
            this.TextBoxToDateWidth = this.initializeTextBoxToDateWidth;
            this.Size = this.initializeSize;

            return;
        }
        #endregion
        #endregion
    }
}
