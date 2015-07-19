namespace Mizuochi.Commons.Controls
{
    partial class DateRange
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
            this.labelFront = new System.Windows.Forms.Label();
            this.labelMiddle = new System.Windows.Forms.Label();
            this.dateTextBoxTo = new Mizuochi.Commons.Controls.DateTextBox();
            this.dateTextBoxFrom = new Mizuochi.Commons.Controls.DateTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dateTextBoxTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTextBoxFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFront
            // 
            this.labelFront.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelFront.Location = new System.Drawing.Point(1, 2);
            this.labelFront.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.labelFront.Name = "labelFront";
            this.labelFront.Size = new System.Drawing.Size(117, 22);
            this.labelFront.TabIndex = 0;
            this.labelFront.Text = "LabelFront";
            this.labelFront.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelFront.Resize += new System.EventHandler(this.labelFront_Resize);
            // 
            // labelMiddle
            // 
            this.labelMiddle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMiddle.Location = new System.Drawing.Point(271, 2);
            this.labelMiddle.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.labelMiddle.Name = "labelMiddle";
            this.labelMiddle.Size = new System.Drawing.Size(26, 22);
            this.labelMiddle.TabIndex = 2;
            this.labelMiddle.Text = "～";
            this.labelMiddle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dateTextBoxTo
            // 
            this.dateTextBoxTo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dateTextBoxTo.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.5F);
            this.dateTextBoxTo.LabelFrontVisible = false;
            this.dateTextBoxTo.LabelFrontWidth = 0;
            this.dateTextBoxTo.LabelRearVisible = false;
            this.dateTextBoxTo.LabelRearWidth = 0;
            this.dateTextBoxTo.Location = new System.Drawing.Point(283, 1);
            this.dateTextBoxTo.Margin = new System.Windows.Forms.Padding(0);
            this.dateTextBoxTo.Name = "dateTextBoxTo";
            this.dateTextBoxTo.Size = new System.Drawing.Size(148, 23);
            this.dateTextBoxTo.TabIndex = 3;
            this.dateTextBoxTo.DateTextChanged += new System.EventHandler(this.dateTextBoxTo_DateTextChanged);
            this.dateTextBoxTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.dateTextBoxTo_DateSelected);
            this.dateTextBoxTo.Resize += new System.EventHandler(this.dateTextBoxTo_Resize);
            // 
            // dateTextBoxFrom
            // 
            this.dateTextBoxFrom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dateTextBoxFrom.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.5F);
            this.dateTextBoxFrom.LabelFrontVisible = false;
            this.dateTextBoxFrom.LabelFrontWidth = 0;
            this.dateTextBoxFrom.LabelRearVisible = false;
            this.dateTextBoxFrom.LabelRearWidth = 0;
            this.dateTextBoxFrom.Location = new System.Drawing.Point(118, 1);
            this.dateTextBoxFrom.Margin = new System.Windows.Forms.Padding(0);
            this.dateTextBoxFrom.Name = "dateTextBoxFrom";
            this.dateTextBoxFrom.Size = new System.Drawing.Size(148, 23);
            this.dateTextBoxFrom.TabIndex = 1;
            this.dateTextBoxFrom.DateTextChanged += new System.EventHandler(this.dateTextBoxFrom_DateTextChanged);
            this.dateTextBoxFrom.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.dateTextBoxFrom_DateSelected);
            this.dateTextBoxFrom.Resize += new System.EventHandler(this.dateTextBoxFrom_Resize);
            // 
            // DateRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelMiddle);
            this.Controls.Add(this.labelFront);
            this.Controls.Add(this.dateTextBoxTo);
            this.Controls.Add(this.dateTextBoxFrom);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.5F);
            this.Name = "DateRange";
            this.Size = new System.Drawing.Size(435, 26);
            this.Load += new System.EventHandler(this.DateRange_Load);
            this.EnabledChanged += new System.EventHandler(this.DateRange_EnabledChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dateTextBoxTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTextBoxFrom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelFront;
        private System.Windows.Forms.Label labelMiddle;
        private DateTextBox dateTextBoxFrom;
        private DateTextBox dateTextBoxTo;
    }
}
