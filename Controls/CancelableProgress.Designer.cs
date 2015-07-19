namespace Mizuochi.Commons.Controls
{
    partial class CancelableProgress
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelPercentage = new System.Windows.Forms.Label();
            this.labelRemainingTime = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panelContent = new System.Windows.Forms.Panel();
            this.labelElapsedTime = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPercentage
            // 
            this.labelPercentage.Location = new System.Drawing.Point(3, 29);
            this.labelPercentage.Name = "labelPercentage";
            this.labelPercentage.Size = new System.Drawing.Size(29, 12);
            this.labelPercentage.TabIndex = 0;
            this.labelPercentage.Text = "100%";
            this.labelPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRemainingTime
            // 
            this.labelRemainingTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRemainingTime.Location = new System.Drawing.Point(258, 29);
            this.labelRemainingTime.Name = "labelRemainingTime";
            this.labelRemainingTime.Size = new System.Drawing.Size(111, 12);
            this.labelRemainingTime.TabIndex = 1;
            this.labelRemainingTime.Text = "残りおよそ 999分99秒";
            this.labelRemainingTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 3);
            this.progressBar.MarqueeAnimationSpeed = 1;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(366, 23);
            this.progressBar.TabIndex = 2;
            this.progressBar.Value = 50;
            // 
            // panelContent
            // 
            this.panelContent.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelContent.Controls.Add(this.labelElapsedTime);
            this.panelContent.Controls.Add(this.buttonCancel);
            this.panelContent.Controls.Add(this.progressBar);
            this.panelContent.Controls.Add(this.labelRemainingTime);
            this.panelContent.Controls.Add(this.labelPercentage);
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(372, 70);
            this.panelContent.TabIndex = 3;
            // 
            // labelElapsedTime
            // 
            this.labelElapsedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelElapsedTime.Location = new System.Drawing.Point(38, 29);
            this.labelElapsedTime.Name = "labelElapsedTime";
            this.labelElapsedTime.Size = new System.Drawing.Size(67, 12);
            this.labelElapsedTime.TabIndex = 4;
            this.labelElapsedTime.Text = "(999分99秒)";
            this.labelElapsedTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(149, 44);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // CancelableProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContent);
            this.Name = "CancelableProgress";
            this.Size = new System.Drawing.Size(372, 70);
            this.Load += new System.EventHandler(this.CancelableProgress_Load);
            this.panelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelPercentage;
        private System.Windows.Forms.Label labelRemainingTime;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelElapsedTime;
    }
}
