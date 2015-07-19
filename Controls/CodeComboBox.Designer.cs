namespace Mizuochi.Commons.Controls
{
    partial class CodeComboBox
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
            this.label = new System.Windows.Forms.Label();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label.Location = new System.Drawing.Point(1, 5);
            this.label.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(117, 14);
            this.label.TabIndex = 0;
            this.label.Text = "Label";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label.Resize += new System.EventHandler(this.label_Resize);
            // 
            // textBoxCode
            // 
            this.textBoxCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxCode.Location = new System.Drawing.Point(125, 1);
            this.textBoxCode.Margin = new System.Windows.Forms.Padding(3, 1, 3, 2);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(116, 21);
            this.textBoxCode.TabIndex = 1;
            this.textBoxCode.TextChanged += new System.EventHandler(this.textBoxCode_TextChanged);
            this.textBoxCode.Leave += new System.EventHandler(this.textBoxCode_Leave);
            this.textBoxCode.Resize += new System.EventHandler(this.textBoxCode_Resize);
            this.textBoxCode.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxCode_Validating);
            this.textBoxCode.Validated += new System.EventHandler(this.textBoxCode_Validated);
            // 
            // comboBox
            // 
            this.comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(248, 1);
            this.comboBox.Margin = new System.Windows.Forms.Padding(3, 1, 1, 1);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(139, 22);
            this.comboBox.TabIndex = 2;
            this.comboBox.TabStop = false;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // CodeComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.label);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "CodeComboBox";
            this.Size = new System.Drawing.Size(390, 26);
            this.EnabledChanged += new System.EventHandler(this.CodeComboBox_EnabledChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.ComboBox comboBox;

    }
}
