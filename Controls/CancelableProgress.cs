// ******************************************************************
// CancelableProgress.cs  ：プログレスコントロール
// 作成　　：2015/03/23 水落
// 更新履歴：2015/03/23 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using Mizuochi.Commons.Codes.ClassExtensions;
using Mizuochi.Commons.Codes.Graphics.Filters;
using System.Runtime.InteropServices;

namespace Mizuochi.Commons.Controls
{
    /// <summary>
    /// キャンセル可能なプログレスバーを表示するコントロールです。
    /// </summary>
    public partial class CancelableProgress : TranslucentControl, IDisposable
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
        public CancelableProgress()
        {
            InitializeComponent();

            this.Visible = false;

#if DEBUG
            // デザインモード時は透過色設定しない
            if (false == this.DesignMode)
#endif
            {
                // 透過色設定
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.panelContent.BackColor = Color.Transparent;
            }


            // 経過時間タイマーを初期化
            Action<object> timerCallbackMethod = ((state) => this.Invoke(new Action(TimerCallbackMethod)));
            this.updateDisplayTimer = new System.Threading.Timer(new TimerCallback(timerCallbackMethod));
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~CancelableProgress()
        {
            this.MemberDispose(false);
        }
        #endregion

        #region フィールド
        /// <summary>
        /// 非同期で動作させるタスク。
        /// </summary>
        private Task progressTask;
        /// <summary>
        /// タスクのキャンセルトークン。
        /// </summary>
        private CancellationTokenSource progressCancellationToken;
        /// <summary>
        /// 表示更新タイマー。
        /// </summary>
        private System.Threading.Timer updateDisplayTimer;
        /// <summary>
        /// 表示更新間隔(ミリ秒)。
        /// </summary>
        private int updateInterval = 1000;
        /// <summary>
        /// タスク開始時刻。
        /// </summary>
        private DateTime startTime;
        /// <summary>
        /// 目標時間。
        /// </summary>
        private TimeSpan targetTimeSpan;
        /// <summary>
        /// 残り時間の算出方法。
        /// </summary>
        private RemainingTimeLogicType remainingTimeLogic = RemainingTimeLogicType.Dynamic;
        /// <summary>
        /// 進捗状況の表示タイミング。
        /// </summary>
        private UpdateDisplayTimingType updateDisplayTiming = UpdateDisplayTimingType.Timer;
        /// <summary>
        /// タスクのキャンセルタイムアウト時間(ミリ秒)。
        /// </summary>
        private int cancelTimeout = Timeout.Infinite;
        /// <summary>
        /// プログレスバーの現在位置
        /// </summary>
        private int value;
        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region プロパティ
        #region デザイナパブリックプロパティ
        /// <summary>
        /// コントロールの範囲の最大値を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("このプログレスバーが処理している範囲の上限です。")]
        [DefaultValue(100)]
        public int Maximum
        {
            get
            {
                return this.progressBar.Maximum;
            }
            set
            {
                this.progressBar.Maximum = value;
            }
        }

        /// <summary>
        /// コントロールの範囲の最小値を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("このプログレスバーが処理している範囲の下限です。")]
        [DefaultValue(0)]
        public int Minimum
        {
            get
            {
                return this.progressBar.Minimum;
            }
            set
            {
                this.progressBar.Minimum = value;
            }
        }

        /// <summary>
        /// PerformStepメソッドを呼び出したときに、プログレスバーの現在の位置を進める量を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("PerformStepメソッドを呼び出したときに、コントロールの現在の値をインクリメントする量です。")]
        [DefaultValue(10)]
        public int Step
        {
            get
            {
                return this.progressBar.Step;
            }
            set
            {
                this.progressBar.Step = value;
            }
        }

        /// <summary>
        /// プログレスバーの現在位置を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("MinimumおよびMaximumプロパティで指定された範囲内のプログレスバーの現在の値です。")]
        [DefaultValue(0)]
        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value.Clamp(this.Minimum, this.Maximum);
            }
        }

        /// <summary>
        /// プログレスバーで進行状況を示す方法を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("プログレスバーのスタイルを設定します。")]
        [DefaultValue(ProgressBarStyle.Blocks)]
        public ProgressBarStyle Style
        {
            get
            {
                return this.progressBar.Style;
            }
            set
            {
                this.progressBar.Style = value;
            }
        }

        /// <summary>
        /// プログレスブロックがプログレスバー内をスクロールするためにかかる時間を、ミリ秒単位で取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("marqueeアニメーションのスピード(ミリ秒)です。")]
        [DefaultValue(1)]
        public int MarqueeAnimationSpeed
        {
            get
            {
                return this.progressBar.MarqueeAnimationSpeed;
            }
            set
            {
                this.progressBar.MarqueeAnimationSpeed = value;
            }
        }

        /// <summary>
        /// 表示更新間隔(ミリ秒)を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("表示更新間隔(ミリ秒)を設定します。")]
        [DefaultValue(1000)]
        public int UpdateInterval
        {
            get
            {
                return updateInterval;
            }
            set
            {
                updateInterval = value;
            }
        }

        /// <summary>
        /// 残り時間の算出方法を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("残り時間の算出方法を設定します。")]
        [DefaultValue(CancelableProgress.RemainingTimeLogicType.Dynamic)]
        public RemainingTimeLogicType RemainingTimeLogic
        {
            get
            {
                return remainingTimeLogic;
            }
            set
            {
                remainingTimeLogic = value;
            }
        }

        /// <summary>
        /// 進捗状況の表示タイミングを取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("進捗状況の表示タイミングを設定します。")]
        [DefaultValue(CancelableProgress.UpdateDisplayTimingType.Timer)]
        public UpdateDisplayTimingType UpdateDisplayTiming
        {
            get
            {
                return updateDisplayTiming;
            }
            set
            {
                updateDisplayTiming = value;
            }
        }

        /// <summary>
        /// タスクのキャンセルタイムアウト時間を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("キャンセルタイムアウト時間を設定します。")]
        [DefaultValue(Timeout.Infinite)]
        public int CancelTimeout
        {
            get
            {
                return cancelTimeout;
            }
            set
            {
                cancelTimeout = value;
            }
        }

        /// <summary>
        /// パーセンテージの表示、非表示を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("パーセンテージの表示、非表示を示します。")]
        [DefaultValue(true)]
        public bool LabelPercentageVisible
        {
            get
            {
                return this.labelPercentage.Visible;
            }
            set
            {
                this.labelPercentage.Visible = value;
            }
        }

        /// <summary>
        /// 残り時間の表示、非表示を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("残り時間の表示、非表示を示します。")]
        [DefaultValue(true)]
        public bool LabelRemainingTimeVisible
        {
            get
            {
                return this.labelRemainingTime.Visible;
            }
            set
            {
                this.labelRemainingTime.Visible = value;
            }
        }

        /// <summary>
        /// 経過時間の表示、非表示を取得、設定します。
        /// </summary>
        [Category("動作")]
        [Description("経過時間の表示、非表示を示します。")]
        [DefaultValue(true)]
        public bool LabelElapsedTimeVisible
        {
            get
            {
                return this.labelElapsedTime.Visible;
            }
            set
            {
                this.labelElapsedTime.Visible = value;
            }
        }
        #endregion

        #region 非デザイナパブリックプロパティ
        /// <summary>
        /// タスクの開始時刻を取得します。
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
        }

        /// <summary>
        /// タスクの経過時間を取得します。
        /// </summary>
        public TimeSpan ElapsedTimeSpan
        {
            get
            {
                return (DateTime.Now - this.StartTime);
            }
        }

        /// <summary>
        /// タスク完了までの目標時間を取得または設定します。
        /// </summary>
        public TimeSpan TargetTimeSpan
        {
            get
            {
                return targetTimeSpan;
            }
            set
            {
                targetTimeSpan = value;
            }
        }

        /// <summary>
        /// タスクの残り時間を取得します。
        /// </summary>
        public TimeSpan RemainingTimeSpan
        {
            get
            {
                TimeSpan remainingTime = default(TimeSpan);
                switch (this.remainingTimeLogic)
                {
                    // 目標時間と経過時間の差分を取得
                    case RemainingTimeLogicType.Static:
                        remainingTime = this.TargetTimeSpan - this.ElapsedTimeSpan;
                        if (remainingTime.Ticks < 0)
                        {
                            // 経過時間が目標時間を過ぎた場合、0に設定
                            remainingTime = TimeSpan.FromTicks(0);
                        }
                        break;

                    // 進捗状況と経過時間から算出
                    case RemainingTimeLogicType.Dynamic:
                        // 進捗率が0%の場合、0に設定
                        long remainingTicks = 0;
                        if (0 != this.PercentOfProgress)
                        {
                            // ( 100% - 進捗率 ) * ( 経過時間 / 進捗率 )
                            long elapsedTicks = this.ElapsedTimeSpan.Ticks;
                            remainingTicks = (long)((1 - this.PercentOfProgress) * (elapsedTicks / this.PercentOfProgress));
                        }
                        remainingTime = TimeSpan.FromTicks(remainingTicks);
                        break;
                }

                return remainingTime;
            }
        }

        /// <summary>
        /// タスクの進捗率を取得します。
        /// </summary>
        public double PercentOfProgress
        {
            get
            {
                double totalCount = (double)this.Maximum - this.Minimum;
                double count = (double)this.Value - this.Minimum;
                return count / totalCount;
            }
        }
        #endregion
        #endregion

        #region イベント
        /// <summary>
        /// コントロールが初めて表示される前に発生します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void CancelableProgress_Load(object sender, EventArgs e)
        {
#if DEBUG
            // デザインモード時は非表示にしない
            if (false == this.DesignMode)
#endif
            {
                this.Visible = false;
                this.Dock = DockStyle.Fill;
                this.BringToFront();
            }

            return;
        }

        /// <summary>
        /// キャンセルボタンが押下された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.progressCancellationToken.Cancel(true);
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 非同期の処理を開始します。
        /// </summary>
        /// <param name="asyncMethod">非同期で動作するメソッド。</param>
        public void Start(Func<bool> asyncMethod)
        {
            if (true == this.Visible)
            {
                throw new InvalidOperationException("既に処理を開始しています。");
            }

            // 開始初期処理
            int updateInterval = (this.UpdateDisplayTiming == UpdateDisplayTimingType.Timer) ? this.updateInterval : 1000;
            this.Value = this.Minimum;
            this.startTime = DateTime.Now;
            this.updateDisplayTimer.Change(0, updateInterval);

            if (this.UpdateDisplayTiming == UpdateDisplayTimingType.Progress)
            {
                this.UpdateDisplay();
            }
            this.Visible = true;

            // タスクの動作判定
            if (null != this.progressTask)
            {
                bool completed = this.progressTask.ReinforceWait(this.CancelTimeout);
                if (false == completed)
                {
                    throw new TimeoutException();
                }
            }

            // タスク動作開始
            this.progressCancellationToken = new CancellationTokenSource();
            this.progressTask = Task.Factory.StartNew(
                    () => this.AsyncProgressMethod(asyncMethod, this.progressCancellationToken),
                    this.progressCancellationToken.Token)
                .ContinueWith((task => this.Finishing()), TaskScheduler.FromCurrentSynchronizationContext());

            return;
        }

        /// <summary>
        /// 指定されたコレクションに対して、非同期の処理を開始します。
        /// </summary>
        /// <typeparam name="T">処理するコレクションの型。</typeparam>
        /// <param name="asyncMethod">非同期で動作するメソッド。</param>
        /// <param name="prosessDataList">処理するコレクション。</param>
        public void Start<T>(Func<T, bool> asyncMethod, IEnumerable<T> prosessDataList)
        {
            if (true == this.Visible)
            {
                throw new InvalidOperationException("既に処理を開始しています。");
            }

            // 開始初期処理
            int updateInterval = (this.UpdateDisplayTiming == UpdateDisplayTimingType.Timer) ? this.updateInterval : 1000;
            this.Minimum = 0;
            this.Maximum = prosessDataList.Count();
            this.Value = this.Minimum;
            this.startTime = DateTime.Now;
            this.updateDisplayTimer.Change(0, updateInterval);

            if (this.UpdateDisplayTiming == UpdateDisplayTimingType.Progress)
            {
                this.UpdateDisplay();
            }
            this.Visible = true;

            // タスクの動作判定
            if (null != this.progressTask)
            {
                bool completed = this.progressTask.ReinforceWait(this.CancelTimeout);
                if (false == completed)
                {
                    throw new TimeoutException();
                }
            }

            // タスク動作開始
            this.progressCancellationToken = new CancellationTokenSource();
            this.progressTask = Task.Factory.StartNew(
                    () => this.AsyncProgressMethod<T>(asyncMethod, prosessDataList, this.progressCancellationToken),
                    this.progressCancellationToken.Token)
                .ContinueWith((task => this.Finishing()), TaskScheduler.FromCurrentSynchronizationContext());

            return;
        }

        /// <summary>
        /// プログレスバーの現在位置をStepプロパティの量だけ進めます。
        /// </summary>
        public void PerformStep()
        {
            this.Value += this.Step;
            return;
        }

        /// <summary>
        /// 指定した量だけプログレスバーの現在位置を進めます。
        /// </summary>
        /// <param name="value">プログレスバーの現在位置をインクリメントする量。</param>
        public void Increment(int value)
        {
            this.Value += value;
            return;
        }
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// 非同期で進捗処理を行います。
        /// </summary>
        /// <param name="asyncMethod">進捗処理を行うメソッド。</param>
        /// <param name="cancellationTokenSource">キャンセルトークン。</param>
        private void AsyncProgressMethod(Func<bool> asyncMethod, CancellationTokenSource cancellationTokenSource)
        {
            bool result = true;
            while (result)
            {
                // キャンセル確認
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                // 表示更新
                if (this.UpdateDisplayTiming == UpdateDisplayTimingType.Progress)
                {
                    this.Invoke(new Action(this.UpdateDisplay));
                }

                // 非同期処理
                result = asyncMethod();
            }

            return;
        }

        /// <summary>
        /// 指定されたコレクションに対して、非同期で進捗処理を行います。
        /// </summary>
        /// <typeparam name="T">処理するコレクションの型。</typeparam>
        /// <param name="asyncMethod">進捗処理を行うメソッド。</param>
        /// <param name="prosessDataList">処理するコレクション。</param>
        /// <param name="cancellationTokenSource">キャンセルトークン。</param>
        private void AsyncProgressMethod<T>(Func<T, bool> asyncMethod, IEnumerable<T> prosessDataList, CancellationTokenSource cancellationTokenSource)
        {
            bool result = true;
            foreach (var data in prosessDataList)
            {
                // キャンセル確認
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                // 表示更新
                if (this.UpdateDisplayTiming == UpdateDisplayTimingType.Progress)
                {
                    this.Invoke(new Action(this.UpdateDisplay));
                }

                // 非同期処理
                result = asyncMethod(data);
                if (false == result)
                {
                    break;
                }

                this.Increment(1);
            }

            return;
        }

        /// <summary>
        /// コントロールの表示を更新します。
        /// </summary>
        private void UpdateDisplay()
        {
            this.progressBar.Value = this.Value;
            this.labelPercentage.Text = this.PercentOfProgress.ToString("P0");
            this.labelRemainingTime.Text = String.Format("残りおよそ {0}", this.ConvertTimeSpanToMinSec(this.RemainingTimeSpan));

            return;
        }

        /// <summary>
        /// 進捗処理が完了した際の処理を行います。
        /// </summary>
        private void Finishing()
        {
            this.updateDisplayTimer.Change(Timeout.Infinite, Timeout.Infinite);
            this.Visible = false;

            return;
        }

        /// <summary>
        /// 規定時間毎にタイマーで呼ばれる処理です。
        /// </summary>
        private void TimerCallbackMethod()
        {
            this.labelElapsedTime.Text = String.Format("({0})", this.ConvertTimeSpanToMinSec(this.ElapsedTimeSpan));
            this.labelRemainingTime.Text = String.Format("残りおよそ {0}", this.ConvertTimeSpanToMinSec(this.RemainingTimeSpan));

            if (this.UpdateDisplayTiming == UpdateDisplayTimingType.Timer)
            {
                this.UpdateDisplay();
            }

            return;
        }

        /// <summary>
        /// 指定された時間間隔を分秒の表示に変換します。
        /// </summary>
        /// <param name="timeSpan">時間間隔。</param>
        /// <returns>分秒で表記された文字列。</returns>
        private string ConvertTimeSpanToMinSec(TimeSpan timeSpan)
        {
            // 1分未満の場合、分を表示させないようにする
            string timeString = (0 < (int)timeSpan.TotalMinutes) ?
                String.Format("{0:0}分{1:00}秒", (int)timeSpan.TotalMinutes, timeSpan.Seconds) :
                String.Format("{0:0}秒", timeSpan.Seconds);

            return timeString;
        }

        #region リソース解放
        /// <summary>
        /// 保有しているリソースを解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースを解放する場合、true。それ以外の場合、falseを設定します。</param>
        private void MemberDispose(bool disposing)
        {
            // 既に解放されていた場合、再度 解放しない
            if (true == this.disposed)
            {
                return;
            }

            // マネージリソースを解放
            if (true == disposing)
            {
                this.ReleaseManagedResource();
            }

            // アンマネージリソースを解放
            this.ReleaseUnmanagedResource();

            this.disposed = true;

            return;
        }

        /// <summary>
        /// マネージリソースを解放します。
        /// </summary>
        private void ReleaseManagedResource()
        {
            // 非同期処理中なら中断
            if ((null != this.progressTask) && (true == this.progressTask.IsRunning()))
            {
                // 非同期表示処理をキャンセル
                this.progressCancellationToken.Cancel(true);
                this.progressTask.ReinforceWait();
            }

            // キャンセルトークンを解放
            if (null != this.progressCancellationToken)
            {
                this.progressCancellationToken.Dispose();
            }

            // タスクを解放
            if (null != this.progressTask)
            {
                this.progressTask.Dispose();
            }

            // タイマーを解放
            if (null != this.updateDisplayTimer)
            {
                this.updateDisplayTimer.Change(Timeout.Infinite, Timeout.Infinite);
                this.updateDisplayTimer.Dispose();
            }

            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            return;
        }
        #endregion
        #endregion

        #region IDisposable メンバー
        /// <summary>
        /// このインスタンスで管理しているリソースを解放します。
        /// </summary>
        void IDisposable.Dispose()
        {
            this.MemberDispose(true);
            GC.SuppressFinalize(this);

            return;
        }
        #endregion

        #region 内部定義
        /// <summary>
        /// 残り時間の算出方法を表す列挙型です。
        /// </summary>
        public enum RemainingTimeLogicType
        {
            /// <summary>
            /// 目標時間から経過時間を引いた値を表示。
            /// </summary>
            Static,
            /// <summary>
            /// 進捗状況と経過時間から算出。
            /// </summary>
            Dynamic,
        }

        /// <summary>
        /// 表示を更新するタイミングを表す列挙型です。
        /// </summary>
        public enum UpdateDisplayTimingType
        {
            /// <summary>
            /// 規定時間毎。
            /// </summary>
            Timer,
            /// <summary>
            /// 進捗状況更新毎。
            /// </summary>
            Progress,
        }
        #endregion
    }
}
