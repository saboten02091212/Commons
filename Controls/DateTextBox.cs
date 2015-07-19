// ******************************************************************
// DateTextBox.cs ： 日付ユーザーコントロールクラス
// 作成日　：2013/01/08
// 更新履歴：2014/10/21 水落　　 日付をマウスホイールで操作できるよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mizuochi.Commons.Codes.ClassExtensions;
using Mizuochi.Commons.Codes.Forms;
using Mizuochi.Commons.Codes.Validations;

namespace Mizuochi.Commons.Controls
{
    /// <summary>
    /// 日付入力をサポートするコントロールです。
    /// </summary>
    [DefaultEvent("DateSelected")]
    [ToolboxBitmap(typeof(DateTimePicker))]
    public partial class DateTextBox : UserControl, ISupportInitialize
    {
        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public DateTextBox()
        {
            InitializeComponent();

            // メンバ初期化
            this.notationalDateType = Commons.Controls.NotationalDateType.YearAndMonthAndDay;
            this.monthCalendar.SelectionStart = default(DateTime);
            this.monthCalendar.SelectionEnd = default(DateTime);

            // 初期配置保持
            this.initializeLabelFrontWidth = this.LabelFrontWidth;
            this.initializeLabelRearWidth = this.LabelRearWidth;
            this.initializeSize = this.Size;

            // イベント登録
            base.CausesValidationChanged += this.DateTextBox_CausesValidationChanged;
            this.maskedTextBoxDate.MouseWheel += this.maskedTextBoxDate_MouseWheel;
        }
        #endregion

        #region イベントハンドラ
        /// <summary>
        /// バリデートエラーになった際に発生します。
        /// </summary>
        [Category("フォーカス")]
        [Description("コントロールの検証時にエラーを検出したときに発生します。")]
        public event EventHandler ValidateError;

        /// <summary>
        /// 日付テキストボックスが変更された際に発生します。
        /// </summary>
        [Category("プロパティ変更")]
        [Description("DateTextの値がコントロールで変更された時に発生します。")]
        public event EventHandler DateTextChanged;

        /// <summary>
        /// 日付表記が変更された際に発生します。
        /// </summary>
        [Category("プロパティ変更")]
        [Description("NotationalDateTypeの値がコントロールで変更された時に発生します。")]
        public event EventHandler NotationalDateTypeChanged;

        /// <summary>
        /// 日付カレンダーで日付が選択された際に発生します。
        /// </summary>
        [Category("アクション")]
        [Description("ユーザーがある日付を選択した時、または日数の範囲を指定した時に発生します。")]
        public event DateRangeEventHandler DateSelected;
        #endregion

        #region フィールド
        /// <summary>
        /// カレンダーコントロールのオフセット位置
        /// </summary>
        private Point calenderLocationOffset;

        /// <summary>
        /// カレンダーコントロールの初期位置
        /// </summary>
        private Point monthCalendarLocation;

        /// <summary>
        /// 日付表記種別
        /// </summary>
        private NotationalDateType notationalDateType;

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
        /// 初期化時に保持する後方ラベルの幅です。
        /// </summary>
        private int initializeLabelRearWidth;
        /// <summary>
        /// 初期化時に保持するコントロールのサイズです。
        /// </summary>
        private Size initializeSize;
        #endregion
        #endregion

        #region プロパティ
        #region デザイナパブリックプロパティ
        /// <summary>
        /// 前方ラベルの幅を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("前方ラベルの幅(ピクセル単位)です。")]
        [RefreshProperties(RefreshProperties.Repaint)]
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
        /// 後方ラベルの幅を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("後方ラベルの幅(ピクセル単位)です。")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int LabelRearWidth
        {
            get
            {
                return this.labelRear.Width;
            }
            set
            {
                if (false == this.initializingFlg)
                {
                    this.labelRear.Width = value;
                }
                else
                {
                    this.initializeLabelRearWidth = value;
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
        /// 後方ラベルのテキストを取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("後方ラベルのテキストです。")]
        [DefaultValue("LabelRear")]
        public string LabelRearText
        {
            get
            {
                return this.labelRear.Text;
            }
            set
            {
                this.labelRear.Text = value;
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
        /// 後方ラベルのテキスト位置を取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("後方ラベルのテキスト位置です。")]
        [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
        public ContentAlignment LabelRearTextAlign
        {
            get
            {
                return this.labelRear.TextAlign;
            }
            set
            {
                this.labelRear.TextAlign = value;
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
        /// 後方ラベルの表示、非表示を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("後方ラベルの表示、非表示を示します。")]
        [DefaultValue(true)]
        public bool LabelRearVisible
        {
            get
            {
                return this.labelRear.Visible;
            }
            set
            {
                this.labelRear.Visible = value;
            }
        }

        /// <summary>
        /// 日付テキストボックスの内容を取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("日付テキストボックスの内容です。")]
        [DefaultValue("")]
        public string DateText
        {
            get
            {
                return this.maskedTextBoxDate.Text;
            }
            set
            {
                this.maskedTextBoxDate.Text = value;
                this.OnDateSelected();
            }
        }

        /// <summary>
        /// 日付表記種別を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("日付表記の種別です。")]
        [DefaultValue(typeof(NotationalDateType), "YearAndMonthAndDay")]
        public NotationalDateType NotationalDateType
        {
            get
            {
                return notationalDateType;
            }
            set
            {
                NotationalDateType preNotationalDateType = this.notationalDateType;
                this.notationalDateType = value;
                this.ChangeNotationalDate(this.notationalDateType, preNotationalDateType);

                // 日付表記変更イベント発火
                if (null != this.NotationalDateTypeChanged)
                {
                    EventArgs eventArgs = new EventArgs();
                    this.NotationalDateTypeChanged(this, eventArgs);
                }
            }
        }

        /// <summary>
        /// カレンダーコントロールのオフセット位置を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("カレンダーのオフセット位置です。")]
        [DefaultValue(typeof(Point), "0, 0")]
        public Point CalenderLocationOffset
        {
            get
            {
                return calenderLocationOffset;
            }
            set
            {
                calenderLocationOffset = value;
                this.AdjustCalendarLocation();
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
        /// 正しい日付型かどうかを示します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ExactDateTime
        {
            get
            {
                return this._ExactDateTime(this.notationalDateType);
            }
        }

        /// <summary>
        /// 日付テキストボックスの内容を日付型で取得、設定します。
        /// </summary>
        /// <exception cref="System.FormatException">日付テキストボックスに日付が入力されていない場合、発生します。</exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Value
        {
            get
            {
                // 日付に変換できるか判定
                if (false == this.ExactDateTime)
                {
                    return DateTime.MinValue;
                }

                // 日付を取得
                DateTime resultDateTime = default(DateTime);
                string dateString = "00010101".OverWrite(this.maskedTextBoxDate.Text);
                resultDateTime = DateTime.Parse(this.ConvertDateTimeString(dateString));

                return resultDateTime;
            }
            set
            {
                string formatString;
                switch (this.notationalDateType)
                {
                    case NotationalDateType.YearAndMonthAndDay:
                        formatString = "yyyyMMdd";
                        break;
                    case NotationalDateType.YearAndMonth:
                        formatString = "yyyyMM";
                        break;
                    default:
                        throw new InvalidEnumArgumentException();
                }

                this.maskedTextBoxDate.Text = value.ToString(formatString);
                this.OnDateSelected();
            }
        }

        /// <summary>
        /// バリデーション処理を行います。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CancelEventHandler Validation
        {
            get { return this.maskedTextBoxDate_Validating; }
        }
        #endregion
        #endregion

        #region イベント
        /// <summary>
        /// コントロールロード時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void DateTextBox_Load(object sender, EventArgs e)
        {
            // 日付表記変更
            this.ChangeNotationalDate(this.notationalDateType, this.notationalDateType);

            // カレンダー日付を今日に設定
            this.monthCalendar.SelectionStart = DateTime.Now;
            this.monthCalendar.SelectionEnd = DateTime.Now;

#if DEBUG
            // デザインモード時はカレンダーの移動処理をしない
            if (false == this.GetNestDesignMode())
#endif
            {
                this.AddCalendarToForm();
            }

            return;
        }

        /// <summary>
        /// Enabledプロパティが変更された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void DateTextBox_EnabledChanged(object sender, EventArgs e)
        {
            this.TabStop = this.Enabled;
            return;
        }

        /// <summary>
        /// CausesValidationプロパティの値が変更された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void DateTextBox_CausesValidationChanged(object sender, EventArgs e)
        {
            this.maskedTextBoxDate.CausesValidation = this.CausesValidation;

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
            this.maskedTextBoxDate.Anchor &= ~AnchorStyles.Left;

            // テキストボックスの位置とコントロール幅を修正
            this.maskedTextBoxDate.Left = this.labelFront.Right + this.labelFront.Margin.Right + this.maskedTextBoxDate.Margin.Left;
            this.Width =
                this.labelFront.Margin.Left + this.labelFront.Width + this.labelFront.Margin.Right +
                this.maskedTextBoxDate.Margin.Left + this.maskedTextBoxDate.Width + this.maskedTextBoxDate.Margin.Right +
                this.buttonDispCalender.Margin.Left + this.buttonDispCalender.Width + this.buttonDispCalender.Margin.Right +
                this.labelRear.Margin.Left + this.labelRear.Width + this.labelRear.Margin.Right;

            // コントロール移動と共にリサイズ、移動させる
            this.maskedTextBoxDate.Anchor |= AnchorStyles.Left;
            return;
        }

        /// <summary>
        /// 後方ラベルのリサイズ時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void labelRear_Resize(object sender, EventArgs e)
        {
            // コントロール移動と共にリサイズ、移動させない
            this.maskedTextBoxDate.Anchor &= ~AnchorStyles.Right;
            this.buttonDispCalender.Anchor &= ~AnchorStyles.Right;
            this.buttonDispCalender.Anchor |= AnchorStyles.Left;

            // コントロール幅を修正
            this.Width =
                this.labelFront.Margin.Left + this.labelFront.Width + this.labelFront.Margin.Right +
                this.maskedTextBoxDate.Margin.Left + this.maskedTextBoxDate.Width + this.maskedTextBoxDate.Margin.Right +
                this.buttonDispCalender.Margin.Left + this.buttonDispCalender.Width + this.buttonDispCalender.Margin.Right +
                this.labelRear.Margin.Left + this.labelRear.Width + this.labelRear.Margin.Right;

            // コントロール移動と共にリサイズ、移動させる
            this.maskedTextBoxDate.Anchor |= AnchorStyles.Right;
            this.buttonDispCalender.Anchor |= AnchorStyles.Right;
            this.buttonDispCalender.Anchor &= ~AnchorStyles.Left;

            return;
        }

        /// <summary>
        /// カレンダー表示ボタンを押下した時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void buttonDispCalender_Click(object sender, EventArgs e)
        {
            // カーソルを待機状態に変更
            using (Waiter waiter = new Waiter(this))
            {
                this.monthCalendar.Visible = true;
                this.monthCalendar.Select();
            }

            return;
        }

        /// <summary>
        /// カレンダーの日付を選択した時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            this.maskedTextBoxDate.Text = this.monthCalendar.SelectionStart.ToString("yyyyMMdd");
            this.maskedTextBoxDate.Focus();

            if (null != this.DateSelected)
            {
                this.DateSelected(this, e);
            }

            return;
        }

        /// <summary>
        /// カレンダーから入力フォーカスが離れた時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void monthCalendar_Leave(object sender, EventArgs e)
        {
            this.monthCalendar.Visible = false;

            return;
        }

        /// <summary>
        /// 検証処理を行う直前に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void maskedTextBoxDate_Validating(object sender, CancelEventArgs e)
        {
            // ダミーループ
            bool result = true;
            for (; ; )
            {
                if (false == this.ExactDateTime)
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
        /// 日付テキストボックス上でマウスホイールが動いた時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void maskedTextBoxDate_MouseWheel(object sender, MouseEventArgs e)
        {
            int value = e.Delta / -120;

            if (this.notationalDateType == NotationalDateType.YearAndMonthAndDay)
            {
                this.AddDays(value);
            }
            else if (this.notationalDateType == NotationalDateType.YearAndMonth)
            {
                this.AddMonths(value);
            }

            return;
        }

        /// <summary>
        /// 日付マスクテキストボックス上でキーを押下した時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void maskedTextBoxDate_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (this.notationalDateType == NotationalDateType.YearAndMonthAndDay)
                    {
                        this.AddDays(-1);
                    }
                    else if (this.notationalDateType == NotationalDateType.YearAndMonth)
                    {
                        this.AddMonths(-1);
                    }

                    e.Handled = true;
                    break;

                case Keys.Down:
                    if (this.notationalDateType == NotationalDateType.YearAndMonthAndDay)
                    {
                        this.AddDays(1);
                    }
                    else if (this.notationalDateType == NotationalDateType.YearAndMonth)
                    {
                        this.AddMonths(1);
                    }

                    e.Handled = true;
                    break;

                default:
                    break;
            }

            return;
        }

        /// <summary>
        /// 日付テキストボックスの内容が変更された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void maskedTextBoxDate_TextChanged(object sender, EventArgs e)
        {
            // 検証エラークリア
            this.ValidateClear();

            // 日付テキストボックス変更イベント発火
            if (null != this.DateTextChanged)
            {
                this.DateTextChanged(this, e);
            }

            return;
        }

        /// <summary>
        /// 日付テキストボックス入力フォーカスが離れた時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void maskedTextBoxDate_Leave(object sender, EventArgs e)
        {
            this.OnDateSelected();

            return;
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 検証処理が成功した場合の処理です。
        /// </summary>
        public void ValidateClear()
        {
            this.maskedTextBoxDate.BackColor = SystemColors.Window;
            return;
        }

        /// <summary>
        /// 検証処理が失敗した場合の処理です。
        /// </summary>
        public void ValidateFail()
        {
            this.maskedTextBoxDate.BackColor = Color.Red;
            return;
        }
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// 指定された日付表記で正しい日付型かどうか判定します。
        /// </summary>
        /// <param name="notationalDateType">日付表記</param>
        /// <returns>正しい日付の場合、true。正しい日付でない場合、falseを返します。</returns>
        private bool _ExactDateTime(NotationalDateType notationalDateType)
        {
            // 日付表記によって分岐し、日付文字列を取得
            string dateString = String.Empty;
            switch (notationalDateType)
            {
                case NotationalDateType.YearAndMonthAndDay:
                    dateString = this.maskedTextBoxDate.Text;
                    break;
                case NotationalDateType.YearAndMonth:
                    dateString = this.maskedTextBoxDate.Text + "01";
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            // 日付に変換できるか判定
            DateTime dummy;
            string convertDateTimeString = this.ConvertDateTimeString(dateString);
            bool result = DateTime.TryParse(convertDateTimeString, out dummy);
            return result;
        }

        /// <summary>
        /// 日付表記を変更します。
        /// </summary>
        /// <param name="currentNotationalDateType">現在の日付表記種別</param>
        /// <param name="preNotationalDateType">前回の日付表記種別</param>
        private void ChangeNotationalDate(NotationalDateType currentNotationalDateType, NotationalDateType preNotationalDateType)
        {
            // 日付が入力されているか判定
            bool isExactDateTim = this._ExactDateTime(preNotationalDateType);
            string dateString = "00010101".OverWrite(this.maskedTextBoxDate.Text);

            // 入力マスク変更
            switch (currentNotationalDateType)
            {
                // 年月日
                case NotationalDateType.YearAndMonthAndDay:
                    this.maskedTextBoxDate.Mask = "0000年90月90日";
                    break;

                // 年月
                case NotationalDateType.YearAndMonth:
                    this.maskedTextBoxDate.Mask = "0000年90月";
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }

            // 日付が入力されていれば新たな日付表記に合わせて変更
            if (true == isExactDateTim)
            {
                this.Value = DateTime.Parse(this.ConvertDateTimeString(dateString));
            }

            return;
        }

        /// <summary>
        /// 文字列にスラッシュを入れ日付型に変換できるよう修正します。
        /// </summary>
        /// <param name="value">8桁の日付を表す文字列です。</param>
        /// <exception cref="System.FormatException">valueの文字列長が8桁でない場合、発生します。</exception>
        private string ConvertDateTimeString(string value)
        {
            if (8 != value.Length)
            {
                return String.Empty;
            }

            return String.Format("{0}/{1}/{2}", value.Substring(0, 4), value.Substring(4, 2), value.Substring(6, 2));
        }

        /// <summary>
        /// カレンダーの表示位置を日付テキストボックスの下に表示されるよう修正します。
        /// </summary>
        private void AddCalendarToForm()
        {
            // フォーム取得
            this.monthCalendarLocation = this.maskedTextBoxDate.Location;
            Control currentControl = this;
            while (false == (currentControl is Form))
            {
                this.monthCalendarLocation.Offset(currentControl.Location);
                currentControl = currentControl.Parent;
            }

            // カレンダー位置調整
            this.monthCalendarLocation.Offset(0, 20);
            this.AdjustCalendarLocation();

            // カレンダー移動
            currentControl.Controls.Add(this.monthCalendar);
            this.Controls.Remove(this.monthCalendar);

            // 最前面に表示
            this.monthCalendar.BringToFront();

            return;
        }

        /// <summary>
        /// カレンダーの表示位置を調整します。
        /// </summary>
        private void AdjustCalendarLocation()
        {
            Point location = this.monthCalendarLocation;
            location.Offset(this.calenderLocationOffset);
            this.monthCalendar.Location = location;
            return;
        }

        /// <summary>
        /// DateSelectedイベントを発生させます。
        /// </summary>
        private void OnDateSelected()
        {
            DateTime oldDate = this.monthCalendar.SelectionStart;

            DateTime selectDate = default(DateTime);
            if (true == this.ExactDateTime)
            {
                selectDate = this.Value;

                this.monthCalendar.SelectionStart = selectDate;
                this.monthCalendar.SelectionEnd = selectDate;
            }
            else
            {
                selectDate = DateTime.MinValue;

                this.monthCalendar.SelectionStart = DateTime.Now;
                this.monthCalendar.SelectionEnd = DateTime.Now;
            }

            if ((null != this.DateSelected) && (oldDate != selectDate))
            {
                DateRangeEventArgs dateRangeEventArgs = new DateRangeEventArgs(selectDate, selectDate);
                this.DateSelected(this, dateRangeEventArgs);
            }

            return;
        }

        /// <summary>
        /// 現在の日付に指定された日数を加算します。
        /// </summary>
        /// <param name="days">加算する日数</param>
        private void AddDays(int days)
        {
            // 日付が設定されているか判定
            if (this.ExactDateTime == false)
            {
                return;
            }
            
            // 加算した時に桁あふれしないか判定
            if ((days > 0) && (this.Value >= new DateTime(9999, 12, 31)))
            {
                return;
            }

            // 減算した時に桁あふれしないか判定
            if ((days < 0) && (this.Value < new DateTime(1, 1, 2)))
            {
                return;
            }

            // 日数を加算
            this.Value = this.Value.AddDays(days);

            return;
        }

        /// <summary>
        /// 現在の日付に指定された月数を加算します。
        /// </summary>
        /// <param name="months">加算する月数</param>
        private void AddMonths(int months)
        {
            // 日付が設定されているか判定
            if (this.ExactDateTime == false)
            {
                return;
            }

            // 加算した時に桁あふれしないか判定
            if ((months > 0) && (this.Value >= new DateTime(9999, 12, 1)))
            {
                return;
            }

            // 減算した時に桁あふれしないか判定
            if ((months < 0) && (this.Value < new DateTime(1, 2, 1)))
            {
                return;
            }

            // 月数を加算
            this.Value = this.Value.AddMonths(months);

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
            this.LabelRearWidth = this.initializeLabelRearWidth;
            this.Size = this.initializeSize;

            return;
        }

        #endregion
        #endregion
    }

    /// <summary>
    /// 日付表記種別
    /// </summary>
    public enum NotationalDateType
    {
        /// <summary>
        /// 年月日
        /// </summary>
        YearAndMonthAndDay,
        /// <summary>
        /// 年月
        /// </summary>
        YearAndMonth,
    }
}
