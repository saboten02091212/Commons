// ******************************************************************
// CodeComboBox.cs ： コード入力ユーザーコントロール
// 作成日　：2013/01/18
// 更新履歴：2014/05/19 水落　　 データ取得時に、コードからコンボを選択するよう修正。
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
using System.Drawing.Design;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Collections;

namespace Mizuochi.Commons.Controls
{
    /// <summary>
    /// コンボボックスでValue値からの入力を行えるようにするコントロールです。
    /// </summary>
    [DefaultEvent("SelectedIndexChanged")]
    [ToolboxBitmap(typeof(ComboBox))]
    public partial class CodeComboBox : UserControl, ISupportInitialize
    {
        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public CodeComboBox()
        {
            InitializeComponent();

            // 初期配置保持
            this.initializeLabelWidth = this.LabelWidth;
            this.initializeCodeWidth = this.CodeWidth;
            this.initializeSize = this.Size;

            // メンバ初期化
            this.keyDictionary = new Dictionary<string, string>();

            // イベント登録
            base.CausesValidationChanged += this.CodeComboBox_CausesValidationChanged;
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
        /// SelectedIndex プロパティの値が変更するときに発生します。
        /// </summary>
        [Category("動作")]
        [Description("SelectedIndex プロパティの値が変更するときに発生します。")]
        public event EventHandler SelectedIndexChanged;
        #endregion

        #region フィールド
        /// <summary>
        /// 表示する値と実際に使用する値が結びついている辞書
        /// </summary>
        private Dictionary<string, string> keyDictionary;

        #region 初期化
        /// <summary>
        /// 初期化中かどうかを示します。
        /// </summary>
        private bool initializingFlg;
        /// <summary>
        /// 初期化時に保持するラベルの幅です。
        /// </summary>
        private int initializeLabelWidth;
        /// <summary>
        /// 初期化時に保持するコードテキストボックスの幅です。
        /// </summary>
        private int initializeCodeWidth;
        /// <summary>
        /// 初期化時に保持するコントロールのサイズです。
        /// </summary>
        private Size initializeSize;
        #endregion
        #endregion

        #region プロパティ
        #region デザイナパブリックプロパティ
        /// <summary>
        /// ラベルの幅を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("ラベルの幅(ピクセル単位)です。")]
        public int LabelWidth
        {
            get
            {
                return this.label.Width;
            }
            set
            {
                if (false == this.initializingFlg)
                {
                    this.label.Width = value;
                }
                else
                {
                    this.initializeLabelWidth = value;
                }
            }
        }

        /// <summary>
        /// ラベルのテキストを取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("ラベルのテキストです。")]
        [DefaultValue("Label")]
        public string LabelText
        {
            get
            {
                return this.label.Text;
            }
            set
            {
                this.label.Text = value;
            }
        }

        /// <summary>
        /// ラベルのテキスト位置を取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("ラベルのテキスト位置です。")]
        [DefaultValue(typeof(ContentAlignment), "MiddleRight")]
        public ContentAlignment LabelTextAlign
        {
            get
            {
                return this.label.TextAlign;
            }
            set
            {
                this.label.TextAlign = value;
            }
        }

        /// <summary>
        /// ラベルの表示、非表示を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("ラベルの表示、非表示を示します。")]
        [DefaultValue(true)]
        public bool LabelVisible
        {
            get
            {
                return this.label.Visible;
            }
            set
            {
                this.label.Visible = value;
            }
        }

        /// <summary>
        /// コードテキストボックスの幅を取得、設定します。
        /// </summary>
        [Category("配置")]
        [Description("コードテキストボックスの幅(ピクセル単位)です。")]
        public int CodeWidth
        {
            get
            {
                return this.textBoxCode.Width;
            }
            set
            {
                if (false == this.initializingFlg)
                {
                    this.textBoxCode.Width = value;
                }
                else
                {
                    this.initializeCodeWidth = value;
                }
            }
        }

        /// <summary>
        /// コードテキストボックスのテキストを取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("コードテキストボックスのテキストです。")]
        [DefaultValue(typeof(string), "")]
        public string CodeText
        {
            get
            {
                return this.textBoxCode.Text;
            }
            set
            {
                this.textBoxCode.Text = value;
                this.ChangeComboBoxFromCodeText();
            }
        }

        /// <summary>
        /// コードテキストボックスのテキスト位置を取得、設定します。
        /// </summary>
        [Category("表示")]
        [Description("コードテキストボックスのテキスト位置です。")]
        [DefaultValue(typeof(HorizontalAlignment), "Left")]
        public HorizontalAlignment CodeTextAlign
        {
            get
            {
                return this.textBoxCode.TextAlign;
            }
            set
            {
                this.textBoxCode.TextAlign = value;
            }
        }

        /// <summary>
        /// ユーザーがテキストボックスコントロールに入力または貼り付けできる最大文字数を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("テキストボックスコントロールに入力できる最大文字数を指定します。")]
        [DefaultValue(typeof(int), "32767")]
        public int MaxLength
        {
            get
            {
                return this.textBoxCode.MaxLength;
            }
            set
            {
                this.textBoxCode.MaxLength = value;
            }
        }

        /// <summary>
        /// テキストボックスの IME (Input Method Editor) モードを取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("テキストボックスが選択されたときのIMEの状態を決定します。")]
        [DefaultValue(typeof(ImeMode), "NoControl")]
        public ImeMode TextImeMode
        {
            get
            {
                return this.textBoxCode.ImeMode;
            }
            set
            {
                this.textBoxCode.ImeMode = value;
            }
        }

        /// <summary>
        /// テキストボックスの CharacterCasing を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("テキストボックスに入力された文字の大文字と小文字を変更するかどうかを取得します。")]
        [DefaultValue(typeof(CharacterCasing), "Normal")]
        public CharacterCasing CharacterCasing
        {
            get
            {
                return this.textBoxCode.CharacterCasing;
            }
            set
            {
                this.textBoxCode.CharacterCasing = value;
            }
        }

        /// <summary>
        /// このコントロールのデータソースを取得、設定します。
        /// </summary>
        [Category("データ")]
        [Description("項目を取得するために、このコントロールが使用するリストを示します。")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue(typeof(string), "")]
        public object DataSource
        {
            get
            {
                return this.comboBox.DataSource;
            }
            set
            {
                this.comboBox.DataSource = value;
                if ((null != this.comboBox.DataSource) &&
                    (String.Empty != this.comboBox.DisplayMember) &&
                    (String.Empty != this.comboBox.ValueMember))
                {
                    this.keyDictionary = this.CreateKeyCollection(this.comboBox.DataSource);
                }
            }
        }

        /// <summary>
        /// 表示するプロパティを取得、設定します。
        /// </summary>
        [Category("データ")]
        [Description("コントロール内の項目に対して、表示するプロパティを示します。")]
        [DefaultValue("")]
        public string DisplayMember
        {
            get
            {
                return this.comboBox.DisplayMember;
            }
            set
            {
                this.comboBox.DisplayMember = value;
                if ((null != this.comboBox.DataSource) &&
                    (String.Empty != this.comboBox.DisplayMember) &&
                    (String.Empty != this.comboBox.ValueMember))
                {
                    this.keyDictionary = this.CreateKeyCollection(this.comboBox.DataSource);
                }
            }
        }

        /// <summary>
        /// 実際の値として使用するプロパティを取得、設定します。
        /// </summary>
        [Category("データ")]
        [Description("コントロール内の項目に対して、実際の値として使用するプロパティを示します。")]
        [DefaultValue("")]
        public string ValueMember
        {
            get
            {
                return this.comboBox.ValueMember;
            }
            set
            {
                this.comboBox.ValueMember = value;
                if ((null != this.comboBox.DataSource) &&
                    (String.Empty != this.comboBox.DisplayMember) &&
                    (String.Empty != this.comboBox.ValueMember))
                {
                    this.keyDictionary = this.CreateKeyCollection(this.comboBox.DataSource);
                }
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
        /// バリデーション処理を行います。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CancelEventHandler Validation
        {
            get
            {
                return this.textBoxCode_Validating;
            }
        }

        /// <summary>
        /// 選択されたデータを取得します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedData
        {
            get
            {
                // コードからコンボボックスを更新
                this.ChangeComboBoxFromCodeText();

                // コンボボックスの選択値を取得
                object obj = null;
                if (0 <= this.comboBox.SelectedIndex)
                {
                    obj = this.comboBox.Items[this.comboBox.SelectedIndex];
                }

                return obj;
            }
        }
        #endregion
        #endregion

        #region イベント
        /// <summary>
        /// Enabledプロパティが変更された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void CodeComboBox_EnabledChanged(object sender, EventArgs e)
        {
            this.TabStop = this.Enabled;
            return;
        }

        /// <summary>
        /// CausesValidationプロパティの値が変更された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void CodeComboBox_CausesValidationChanged(object sender, EventArgs e)
        {
            this.textBoxCode.CausesValidation = this.CausesValidation;
            this.comboBox.CausesValidation = this.CausesValidation;

            return;
        }

        /// <summary>
        /// ラベルのリサイズ時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void label_Resize(object sender, EventArgs e)
        {
            // コントロール移動と共にリサイズ、移動させない
            this.comboBox.Anchor &= ~AnchorStyles.Left;

            // テキストボックスの位置とコントロール幅を修正
            this.textBoxCode.Left = this.label.Right + this.label.Margin.Right + this.textBoxCode.Margin.Left;
            this.comboBox.Left = this.textBoxCode.Right + this.textBoxCode.Margin.Right + this.comboBox.Margin.Left;
            this.Width =
                this.label.Margin.Left + this.label.Width + this.label.Margin.Right +
                this.textBoxCode.Margin.Left + this.textBoxCode.Width + this.textBoxCode.Margin.Right +
                this.comboBox.Margin.Left + this.comboBox.Width + this.comboBox.Margin.Right;

            // コントロール移動と共にリサイズ、移動させる
            this.comboBox.Anchor |= AnchorStyles.Left;

            return;
        }

        /// <summary>
        /// コード入力テキストボックスのリサイズ時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void textBoxCode_Resize(object sender, EventArgs e)
        {
            // コントロール移動と共にリサイズ、移動させない
            this.comboBox.Anchor &= ~AnchorStyles.Left;

            // テキストボックスの位置とコントロール幅を修正
            this.comboBox.Left = this.textBoxCode.Right + this.textBoxCode.Margin.Right + this.comboBox.Margin.Left;
            this.Width =
                this.label.Margin.Left + this.label.Width + this.label.Margin.Right +
                this.textBoxCode.Margin.Left + this.textBoxCode.Width + this.textBoxCode.Margin.Right +
                this.comboBox.Margin.Left + this.comboBox.Width + this.comboBox.Margin.Right;

            // コントロール移動と共にリサイズ、移動させる
            this.comboBox.Anchor |= AnchorStyles.Left;

            return;
        }

        /// <summary>
        /// コード入力テキストボックスのTextプロパティの値が変更された場合に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
            this.ValidateClear();
            return;
        }

        /// <summary>
        /// コード入力テキストボックスから入力フォーカスが離れると発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void textBoxCode_Leave(object sender, EventArgs e)
        {
            if (false == this.CausesValidation)
            {
                this.ChangeComboBoxFromCodeText();
            }
            return;
        }

        /// <summary>
        /// 検証処理を行う直前に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void textBoxCode_Validating(object sender, CancelEventArgs e)
        {
            // ダミーループ
            bool result = true;
            for (; ; )
            {
                string key = this.textBoxCode.Text.TrimEnd();
                if (false == this.keyDictionary.ContainsKey(key))
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
        /// 検証処理が終了すると発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCode_Validated(object sender, EventArgs e)
        {
            this.ChangeComboBoxFromCodeText();
            return;
        }

        /// <summary>
        /// コンボボックスのSelectedIndexプロパティが変更された場合に発生します
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // コードを取得
            string codeText = String.Empty;
            if ((null != this.comboBox.SelectedValue) && (false == String.IsNullOrEmpty(this.comboBox.ValueMember)))
            {
                codeText = this.comboBox.SelectedValue.ToString();
            }
            this.textBoxCode.Text = codeText;

            // 選択項目変更イベント発火
            if (null != this.SelectedIndexChanged)
            {
                this.SelectedIndexChanged(this, e);
            }

            return;
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 検証処理が成功した場合の処理です。
        /// </summary>
        public void ValidateClear()
        {
            this.textBoxCode.BackColor = SystemColors.Window;
            return;
        }

        /// <summary>
        /// 検証処理が失敗した場合の処理です。
        /// </summary>
        public void ValidateFail()
        {
            this.textBoxCode.BackColor = Color.Red;
            return;
        }
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// 表示する値と実際の値を結び付けるコレクションを作成します。
        /// </summary>
        /// <param name="dataSource">表示する値と実際の値が格納されているオブジェクト</param>
        /// <returns>表示する値と実際の値を結び付けたコレクション</returns>
        private Dictionary<string, string> CreateKeyCollection(object dataSource)
        {
            Dictionary<string, string> keyDictionary = null;

            // 型によって、処理を分ける
            if (dataSource is DataTable)
            {
                keyDictionary = this.CreateKeyCollectionFromDataTable((DataTable)dataSource);
            }
            else if (dataSource is IEnumerable)
            {
                keyDictionary = this.CreateKeyCollectionFromCollection((IEnumerable)dataSource);
            }
            else
            {
                throw new InvalidOperationException("サポートされないdataSourceの型です。");
            }

            return keyDictionary;
        }

        /// <summary>
        /// DataTable型のオブジェクトから表示する値と実際の値を結び付けるコレクションを作成します。
        /// </summary>
        /// <param name="dataSource">表示する値と実際の値が格納されているオブジェクト</param>
        /// <returns>表示する値と実際の値を結び付けたコレクション</returns>
        private Dictionary<string, string> CreateKeyCollectionFromDataTable(DataTable dataSource)
        {
            Dictionary<string, string> keyDictionary = new Dictionary<string, string>();

            // DataTable内の指定フィールドをコレクションに登録
            foreach (DataRow row in dataSource.Rows)
            {
                string key = row[this.comboBox.ValueMember].ToString().TrimEnd();
                string value = row[this.comboBox.DisplayMember].ToString();

                keyDictionary.Add(key, value);
            }

            return keyDictionary;
        }

        /// <summary>
        /// IEnumerable型のオブジェクトから表示する値と実際の値を結び付けるコレクションを作成します。
        /// </summary>
        /// <param name="dataSource">表示する値と実際の値が格納されているオブジェクト</param>
        /// <returns>表示する値と実際の値を結び付けたコレクション</returns>
        private Dictionary<string, string> CreateKeyCollectionFromCollection(IEnumerable dataSource)
        {
            Dictionary<string, string> keyDictionary = new Dictionary<string, string>();
            IEnumerable<object> tmpCollection = dataSource.Cast<object>();

            // データがない場合、空コレクションを返す
            if (0 >= tmpCollection.Count())
            {
                return keyDictionary;
            }

            // クラス情報取得
            Type type = tmpCollection.First().GetType();
            PropertyInfo valueProperty = type.GetProperty(this.comboBox.ValueMember);
            PropertyInfo displayProperty = type.GetProperty(this.comboBox.DisplayMember);

            // コレクション内の指定プロパティをコレクションに登録
            foreach (var item in dataSource)
            {
                string key = valueProperty.GetValue(item, null).ToString().TrimEnd();
                string value = (displayProperty.GetValue(item, null) ?? "").ToString();

                keyDictionary.Add(key, value);
            }

            return keyDictionary;
        }

        /// <summary>
        /// コードテキストボックスからコンボボックスの値を設定します。
        /// </summary>
        private void ChangeComboBoxFromCodeText()
        {
            int selectedIndex = -1;
            string key = this.textBoxCode.Text.TrimEnd();
            if (true == this.keyDictionary.ContainsKey(key))
            {
                selectedIndex = this.keyDictionary.Keys.ToList().FindIndex((dictionaryKey) => dictionaryKey == key);
            }

            this.comboBox.SelectedIndex = selectedIndex;

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

            this.LabelWidth = this.initializeLabelWidth;
            this.CodeWidth = this.initializeCodeWidth;
            this.Size = this.initializeSize;

            return;
        }
        #endregion
        #endregion
    }
}
