// ******************************************************************
// NumericTextBox.cs ： 数値入力テキストボックスコントロールクラス
// 作成日　：2013/05/30
// 更新履歴：2014/07/10 水落　　 カンマ入力時に桁数チェックするよう修正。
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
using Mizuochi.Commons.Codes.Calculations;
using Mizuochi.Commons.Codes.Windows;
using Mizuochi.Commons.Controls.Events;
using Mizuochi.Commons.Codes.ClassExtensions;

namespace Mizuochi.Commons.Controls
{
    /// <summary>
    /// 数値のみ入力する機能を提供するテキストボックスコントロールです。
    /// </summary>
    [DefaultEvent("ValueChanged")]
    [ToolboxBitmap(typeof(TextBox))]
    public partial class NumericTextBox : TextBox
    {
        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public NumericTextBox()
        {
            InitializeComponent();

            this.addComma = false;
            this.decimals = 0;
            this.integerDigits = -1;
            this.preValue = 0;
        }
        #endregion

        #region フィールド
        /// <summary>
        /// 数値にカンマを付けるかどうかを示します。
        /// </summary>
        private bool addComma;
        /// <summary>
        /// 表示する小数桁数を示します。
        /// </summary>
        private int decimals;
        /// <summary>
        /// 表示する整数桁数を示します。
        /// </summary>
        private int integerDigits;
        /// <summary>
        /// 変更前の値を保持します。
        /// </summary>
        private decimal preValue;
        #endregion

        #region プロパティ
        #region デザイナパブリックプロパティ
        /// <summary>
        /// <para>[使用しないで下さい]互換性のために残してあります。</para>
        /// <para>整数の入力のみの時Trueへ Falseの場合は、小数点ありの入力</para>
        /// </summary>
        [Bindable(true),
        Category("使用禁止"),
        DefaultValue(true),
        Description("整数の入力のみの時Trueへ Falseの場合は、小数点ありの入力"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IntegerPlace
        {
            get
            {
                return (0 != this.decimals) ? true : false;
            }
            set
            {
                this.decimals = (true == value) ? 0 : 3;
                this.addComma = (true == value) ? false : true;
            }
        }
        /// <summary>
        /// 数値にカンマを付けるかどうかを示します。
        /// </summary>
        [Bindable(true),
        Category("表示"),
        DefaultValue(false),
        Description("カンマを付与するかどうかを示します。")]
        public bool AddComma
        {
            get
            {
                return addComma;
            }
            set
            {
                addComma = value;
            }
        }

        /// <summary>
        /// 表示する小数桁数を示します。
        /// </summary>
        [Bindable(true),
        Category("表示"),
        DefaultValue(0),
        Description("表示する小数桁数を示します。")]
        public int Decimals
        {
            get
            {
                return decimals;
            }
            set
            {
                decimals = value;
            }
        }

        /// <summary>
        /// 表示する整数桁数を示します。
        /// </summary>
        [Bindable(true),
        Category("表示"),
        DefaultValue(-1),
        Description("表示する整数桁数を示します。")]
        public int IntegerDigits
        {
            get
            {
                return integerDigits;
            }
            set
            {
                integerDigits = value;
            }
        }
        #endregion

        #region 非デザイナパブリックプロパティ
        /// <summary>
        /// decimal型の値を取得、設定します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal Value
        {
            get
            {
                if (true == String.IsNullOrEmpty(this.Text))
                {
                    return 0;
                }

                return Account.ConvertDecimal(this.Text);
            }
            set
            {
                this.preValue = this.Value;
                this.Text = Account.ConvertString(value, addComma: this.AddComma, decimals: this.Decimals, roundType: RoundType.RoundDown);
                if (this.preValue != value)
                {
                    this.preValue = value;

                    var eventArgs = new ValueChangeEventArgs(this.preValue, value);
                    if (null != ValueChanged)
                    {
                        this.ValueChanged(this, eventArgs);
                    }
                }
            }
        }

        /// <summary>
        /// 入力されている文字列が数値に変換できるかどうかを取得します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNumeric
        {
            get
            {
                string strNumber = this.Text.Replace(",", "");
                decimal decimalNumber;
                bool result = Decimal.TryParse(strNumber, out decimalNumber);
                return result;
            }
        }
        #endregion
        #endregion

        #region イベントハンドラ
        /// <summary>
        /// Value プロパティの値が変更された場合に発生します。
        /// </summary>
        [Category("プロパティ変更")]
        [Description("Value プロパティの値が変更された場合に発生します。")]
        public event ValueChangeEventhandler ValueChanged;
        #endregion

        #region イベント
        /// <summary>
        /// ファーカスを取得した時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void NumericTextBox_Enter(object sender, EventArgs e)
        {
            this.Text = this.Text.Replace(",", "");
            this.preValue = this.Value;
            return;
        }

        /// <summary>
        /// フォーカスが離れた時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void NumericTextBox_Leave(object sender, EventArgs e)
        {
            // 空白チェック
            if (false == String.IsNullOrEmpty(this.Text))
            {
                // 小数点処理
                decimal decimalNumber;
                bool result = Decimal.TryParse(this.Text, out decimalNumber);
                if (false == result)
                {
                    decimalNumber = 0;
                }
                decimalNumber = MizuochiMath.Round(decimalNumber, this.decimals, RoundType.RoundDown);
            
                // カンマ処理
                string format = String.Format(
                    "#{0}0{1}",
                    (true == this.addComma) ? "," : "",
                    (0 < this.decimals) ? String.Format(".{0}", new String('0', this.decimals)) : String.Empty);
                this.Text = decimalNumber.ToString(format);
            }

            // Value変更イベント発火
            if (this.preValue != this.Value)
            {
                this.preValue = this.Value;

                var eventArgs = new ValueChangeEventArgs(this.preValue, this.Value);
                if (null != ValueChanged)
                {
                    this.ValueChanged(this, eventArgs);
                }
            }

            return;
        }

        /// <summary>
        /// キー押下直後に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void NumericTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+C が押された場合、クリップボードに退避
            if ((Keys.C == e.KeyCode) && (true == e.Control))
            {
                if (false == String.IsNullOrEmpty(this.SelectedText))
                {
                    Clipboard.SetDataObject(this.SelectedText, true);
                }
            }
            // Ctrl+V が押された場合、クリップボードから貼付
            else if ((Keys.V == e.KeyCode) && (true == e.Control))
            {
                this.NumericTextBoxPaste();
            }
            // Ctrl+X が押された場合、クリップボードに退避したのち削除
            else if ((Keys.X == e.KeyCode) && (true == e.Control))
            {
                this.NumericTextBoxCut();
            }
            // Ctrl+A が押された場合、全選択
            else if ((Keys.A == e.KeyCode) && (true == e.Control))
            {
                this.SelectAll();
            }
            // Deleteキー が押された場合、削除可能か判定
            else if (Keys.Delete == e.KeyCode)
            {
                // 削除可能か判定
                bool result = this.CanDeleteText(Keys.Delete);
                if (false == result)
                {
                    e.Handled = true;
                }
            }

            return;
        }

        /// <summary>
        /// キー押下時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string replaceText;
            bool isSpecifiedLength;
            switch (e.KeyChar)
            {
                case '\b':
                    // 削除可能か判定
                    bool result = this.CanDeleteText(Keys.Back);
                    if (false == result)
                    {
                        e.Handled = true;
                    }

                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    // 数値に変換可能か判定
                    replaceText = this.GetReplaceText(e.KeyChar.ToString());
                    if (false == Account.CanConvertDecimal(replaceText))
                    {
                        e.Handled = true;
                    }

                    // 桁溢れしていないか判定
                    isSpecifiedLength = replaceText.IsSpecifiedLength(this.integerDigits, this.decimals);
                    if (isSpecifiedLength == false)
                    {
                        e.Handled = true;
                    }

                    break;

                case '.':
                    // 小数桁数が0以下の場合、キャンセル
                    if (0 >= this.decimals)
                    {
                        e.Handled = true;
                    }
                        
                    // 既にドットが存在している場合、キャンセル
                    else if (true == this.GetReplaceText("").Contains('.'))
                    {
                        e.Handled = true;
                    }

                    // 桁溢れしていないか判定
                    replaceText = this.GetReplaceText(e.KeyChar.ToString());
                    if (true == Account.CanConvertDecimal(replaceText))
                    {
                        isSpecifiedLength = replaceText.IsSpecifiedLength(this.integerDigits, this.decimals);
                        if (isSpecifiedLength == false)
                        {
                            e.Handled = true;
                        }
                    }

                    break;

                case '-':
                    // 先頭でない場合、キャンセル
                    if (0 != this.SelectionStart)
                    {
                        e.Handled = true;
                    }
                    // 既に負の符号が存在している場合、キャンセル
                    else if (true == this.GetReplaceText("").Contains('-'))
                    {
                        e.Handled = true;
                    }

                    break;

                default:
                    // その他の文字は受け付けない
                    e.Handled = true;
                    break;
            }

            return;
        }
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// 指定された文字列を、テキストの開始点、または選択されている範囲に挿入した文字列を取得します。
        /// </summary>
        /// <param name="insertText">挿入する文字列</param>
        /// <returns>挿入後の文字列</returns>
        private string GetReplaceText(string insertText)
        {
            // 現在のテキスト取得
            string replaceText = this.Text;

            // 選択範囲を削除
            if (0 != this.SelectionLength)
            {
                replaceText = replaceText.Remove(this.SelectionStart, this.SelectionLength);
            }

            // 文字列を挿入
            replaceText = replaceText.Insert(this.SelectionStart, insertText);

            return replaceText;
        }

        /// <summary>
        /// 表示する文字列の長さを編集し設定します。
        /// </summary>
        /// <param name="targetString">対象の文字列</param>
        /// <returns>編集後の文字列</returns>
        private string DigitsSettings(string targetString)
        {
            string afterString = targetString;
            int digits = this.IntegerDigits;

            // 整数桁数に0桁以下が設定されている場合
            if (digits <= 0)
            {
                digits = this.MaxLength;

                // 対象文字列がMaxLengthより大きい場合
                if (digits < afterString.Length)
                {
                    afterString = afterString.Remove(digits);
                }

                return afterString;
            }

            // 文字列にカンマが存在するか判定します。
            bool decimalCheck = afterString.Contains(".");

            if (decimalCheck == true)
            {
                int dotIndex = afterString.LastIndexOf(".");

                if (digits < dotIndex)
                {
                    afterString = afterString.Remove(digits, (dotIndex - digits));
                }
            }
            else
            {
                if (digits < afterString.Length)
                {
                    afterString = afterString.Remove(digits);
                }
            }

            return afterString;
        }

        /// <summary>
        /// クリップボードから貼り付け処理を行います。
        /// </summary>
        private void NumericTextBoxPaste()
        {
            var dataObject = Clipboard.GetDataObject();
            if (true == dataObject.GetDataPresent(DataFormats.Text))
            {
                string clipboardText = dataObject.GetData(DataFormats.Text) as string;
                string pastedText = this.GetReplaceText(clipboardText);
                if (true == Account.CanConvertDecimal(pastedText))
                {
                    decimal value = Account.ConvertDecimal(pastedText);
                    this.Text = DigitsSettings(value.ToString());
                }
            }
        }

        /// <summary>
        /// 切り取り処理時に、文字数のチェックを行い
        /// 問題なければクリップボードに退避したのち削除をおこないます。
        /// </summary>
        private void NumericTextBoxCut()
        {
            // テキストを選択している場合のみ処理を実施する
            if (false == String.IsNullOrEmpty(this.SelectedText))
            {
                // 編集用文字列にテキストボックスの値を設定
                string cutReplaceText = this.Text;

                // テキストボックスで選択されている範囲を削除
                cutReplaceText = cutReplaceText.Remove(this.SelectionStart, this.SelectionLength);

                // 整数桁数の設定値が0より大きいかつ、編集用文字列が空でない場合処理を実施
                if (String.IsNullOrEmpty(cutReplaceText) == false)
                {
                    // 編集用文字列の整数部と小数部が設定値と一致しているか判定
                    bool isSpecifiedLength = cutReplaceText.IsSpecifiedLength(this.integerDigits, this.decimals);

                    if (isSpecifiedLength == false)
                    {
                        // 一致していない場合は処理を終了
                        return;
                    }
                }
                // クリップボードにテキスト選択範囲を設定
                Clipboard.SetDataObject(this.SelectedText, true);

                // 編集用文字列をテキストボックスに設定する
                this.Text = cutReplaceText;
            }
        }

        /// <summary>
        /// Deleteキー、BackSpaceキーが押下されてTextが削除される際に、削除した後のTextが妥当か判定します。
        /// </summary>
        /// <param name="keyCode">押下キーのキーコード</param>
        /// <returns>削除した後のTextが妥当であれば、true。妥当でなければ、false を返します。</returns>
        private bool CanDeleteText(Keys keyCode)
        {
            // 引数チェック
            if ((Keys.Delete != keyCode) && (Keys.Back != keyCode))
            {
                throw new ArgumentException("引数には、Delete もしくは Back のみ指定可能です。");
            }

            bool result = true;
            for (; ; )
            {
                // 現在のテキスト取得
                string deleteReplaceText = this.Text;

                // テキストが空文字だった場合、何もしない
                if (String.IsNullOrEmpty(deleteReplaceText) == true)
                {
                    result = false;
                    break;
                }

                // 範囲選択されているか判定
                if (0 != this.SelectionLength)
                {
                    // 選択範囲を削除
                    deleteReplaceText = deleteReplaceText.Remove(this.SelectionStart, this.SelectionLength);
                }
                else
                {
                    // Deleteキー押下時に、カーソルが文字列の最後だった場合、何もしない
                    if ((Keys.Delete == keyCode) && (deleteReplaceText.Length == this.SelectionStart))
                    {
                        result = false;
                        break;
                    }

                    // BackSpaceキー押下時に、カーソルが文字列の最初だった場合、何もしない
                    if ((Keys.Back == keyCode) && (0 == this.SelectionStart))
                    {
                        result = false;
                        break;
                    }

                    // １文字分削除
                    deleteReplaceText =
                        (Keys.Delete == keyCode) ? deleteReplaceText.Remove(this.SelectionStart, 1) :
                        (Keys.Back == keyCode) ? deleteReplaceText.Remove(this.SelectionStart - 1, 1) :
                        deleteReplaceText;
                }

                // 数値に変換できるか判定
                if (true == Account.CanConvertDecimal(deleteReplaceText))
                {
                    // 桁数が溢れていないか判定
                    bool isSpecifiedLength = deleteReplaceText.IsSpecifiedLength(this.integerDigits, this.decimals);
                    if (false == isSpecifiedLength)
                    {
                        result = false;
                        break;
                    }
                }

                break;
            }

            return result;
        }

        /// <summary>
        /// Windowsメッセージを処理します。
        /// </summary>
        /// <param name="m">Windowsメッセージ</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                // ペースト処理
                case (ProcessMessage.WM_PASTE):
                    this.NumericTextBoxPaste();
                    return;

                // カット処理
                case (ProcessMessage.WM_CUT):
                    NumericTextBoxCut();
                    return;
            }

            base.WndProc(ref m);

            return;
        }
        #endregion
    }
}
