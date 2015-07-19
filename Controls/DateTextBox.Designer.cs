namespace Mizuochi.Commons.Controls
{
    partial class DateTextBox
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelRear = new System.Windows.Forms.Label();
            this.labelFront = new System.Windows.Forms.Label();
            this.maskedTextBoxDate = new System.Windows.Forms.MaskedTextBox();
            this.buttonDispCalender = new System.Windows.Forms.Button();
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // labelRear
            // 
            this.labelRear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelRear.Location = new System.Drawing.Point(269, 2);
            this.labelRear.Margin = new System.Windows.Forms.Padding(3, 1, 1, 0);
            this.labelRear.Name = "labelRear";
            this.labelRear.Size = new System.Drawing.Size(117, 22);
            this.labelRear.TabIndex = 3;
            this.labelRear.Text = "LabelRear";
            this.labelRear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelRear.Resize += new System.EventHandler(this.labelRear_Resize);
            // 
            // labelFront
            // 
            this.labelFront.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelFront.Location = new System.Drawing.Point(1, 2);
            this.labelFront.Margin = new System.Windows.Forms.Padding(1, 1, 3, 0);
            this.labelFront.Name = "labelFront";
            this.labelFront.Size = new System.Drawing.Size(117, 22);
            this.labelFront.TabIndex = 0;
            this.labelFront.Text = "LabelFront";
            this.labelFront.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelFront.Resize += new System.EventHandler(this.labelFront_Resize);
            // 
            // maskedTextBoxDate
            // 
            this.maskedTextBoxDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.maskedTextBoxDate.CausesValidation = false;
            this.maskedTextBoxDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.maskedTextBoxDate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.maskedTextBoxDate.Location = new System.Drawing.Point(125, 2);
            this.maskedTextBoxDate.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.maskedTextBoxDate.Mask = "0000年90月90日";
            this.maskedTextBoxDate.Name = "maskedTextBoxDate";
            this.maskedTextBoxDate.Size = new System.Drawing.Size(104, 21);
            this.maskedTextBoxDate.TabIndex = 1;
            this.maskedTextBoxDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.maskedTextBoxDate.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.maskedTextBoxDate.ValidatingType = typeof(System.DateTime);
            this.maskedTextBoxDate.TextChanged += new System.EventHandler(this.maskedTextBoxDate_TextChanged);
            this.maskedTextBoxDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.maskedTextBoxDate_KeyDown);
            this.maskedTextBoxDate.Leave += new System.EventHandler(this.maskedTextBoxDate_Leave);
            this.maskedTextBoxDate.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBoxDate_Validating);
            // 
            // buttonDispCalender
            // 
            this.buttonDispCalender.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonDispCalender.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonDispCalender.Location = new System.Drawing.Point(237, 1);
            this.buttonDispCalender.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonDispCalender.Name = "buttonDispCalender";
            this.buttonDispCalender.Size = new System.Drawing.Size(26, 23);
            this.buttonDispCalender.TabIndex = 2;
            this.buttonDispCalender.TabStop = false;
            this.buttonDispCalender.Text = "...";
            this.buttonDispCalender.UseVisualStyleBackColor = true;
            this.buttonDispCalender.Click += new System.EventHandler(this.buttonDispCalender_Click);
            // 
            // monthCalendar
            // 
            this.monthCalendar.Location = new System.Drawing.Point(6, 40);
            this.monthCalendar.Margin = new System.Windows.Forms.Padding(10);
            this.monthCalendar.MaxSelectionCount = 1;
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.ShowTodayCircle = false;
            this.monthCalendar.TabIndex = 6;
            this.monthCalendar.TabStop = false;
            this.monthCalendar.Visible = false;
            this.monthCalendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_DateSelected);
            this.monthCalendar.Leave += new System.EventHandler(this.monthCalendar_Leave);
            // 
            // DateTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelRear);
            this.Controls.Add(this.monthCalendar);
            this.Controls.Add(this.buttonDispCalender);
            this.Controls.Add(this.maskedTextBoxDate);
            this.Controls.Add(this.labelFront);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.5F);
            this.Name = "DateTextBox";
            this.Size = new System.Drawing.Size(387, 26);
            this.Load += new System.EventHandler(this.DateTextBox_Load);
            this.EnabledChanged += new System.EventHandler(this.DateTextBox_EnabledChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label labelRear;
        protected System.Windows.Forms.Label labelFront;
        protected System.Windows.Forms.MaskedTextBox maskedTextBoxDate;
        private System.Windows.Forms.Button buttonDispCalender;
        private System.Windows.Forms.MonthCalendar monthCalendar;

    }
}
