// ******************************************************************
// UtilityStopwatch.cs ： 時間計測クラス
// 作成日　：2014/01/16
// 更新履歴：2014/07/15 水落　　 既存メソッドを上書きするnewキーワードを追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Diagnostics
{
    /// <summary>
    /// 時間を計測するクラスです。
    /// </summary>
    public class UtilityStopwatch : Stopwatch
    {
        /// <summary>
        /// 静的で定義しどこでも使用できる時間計測オブジェクトです。
        /// </summary>
        private static UtilityStopwatch _static;

        /// <summary>
        /// 時間計測インスタンスです。
        /// </summary>
        public static UtilityStopwatch Static
        {
            get
            {
                return UtilityStopwatch._static;
            }
        }

        /// <summary>
        /// 静的コンストラクタです。
        /// </summary>
        static UtilityStopwatch()
        {
            UtilityStopwatch._static = new UtilityStopwatch();
        }

        /// <summary>
        /// 詳細な時間計測の結果を蓄積する辞書オブジェクトです。
        /// </summary>
        private Dictionary<string, List<TimeUnit>> timeUnitListDictionary;
        /// <summary>
        /// 詳細な時間計測が実行された順を保持しておくリストです。
        /// </summary>
        private List<TimeUnit> timeUnitList;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        private UtilityStopwatch()
        {
            this.timeUnitListDictionary = new Dictionary<string, List<TimeUnit>>();
            this.timeUnitList = new List<TimeUnit>();
        }

        /// <summary>
        /// 計測を停止し、経過時間と詳細な時間計測の結果をリセットします。
        /// </summary>
        [Conditional("DEBUG")]
        public new void Reset()
        {
            this.timeUnitListDictionary.Clear();
            this.timeUnitList.Clear();
            base.Reset();
            return;
        }

        /// <summary>
        /// 計測を停止し、経過時間と詳細な時間計測の結果をリセットした後、再度計測を開始します。
        /// </summary>
        [Conditional("DEBUG")]
        public new void Restart()
        {
            this.timeUnitListDictionary.Clear();
            this.timeUnitList.Clear();
            base.Restart();
            return;
        }

        /// <summary>
        /// 詳細な時間計測を開始します。
        /// </summary>
        /// <param name="name">詳細な時間計測の名称</param>
        /// <param name="displayName">表示時の名称</param>
        [Conditional("DEBUG")]
        public void Begin(string name, string displayName = null)
        {
            bool isRunning = base.IsRunning;
            base.Stop();

            TimeUnit timeUnit = new TimeUnit()
            {
                BeginTime = base.Elapsed,
                IsStoped = false,
            };

            bool contained = this.timeUnitListDictionary.ContainsKey(name);
            if (false == contained)
            {
                timeUnit.Name = (null != displayName) ? displayName : name;
                this.timeUnitListDictionary.Add(name, new List<TimeUnit> { timeUnit });
            }
            else
            {
                string tmpDisplayName = (null != displayName) ? displayName : name;
                timeUnit.Name = String.Format("{0}_{1}", tmpDisplayName, this.timeUnitListDictionary[name].Count + 1);
                this.timeUnitListDictionary[name].Add(timeUnit);
            }

            this.timeUnitList.Add(timeUnit);

            if (true == isRunning)
            {
                base.Start();
            }
            return;
        }

        /// <summary>
        /// 詳細な時間計測を終了します。
        /// </summary>
        /// <param name="name">詳細な時間計測の名称</param>
        [Conditional("DEBUG")]
        public void End(string name)
        {
            bool isRunning = base.IsRunning;
            base.Stop();

            bool contained = this.timeUnitListDictionary.ContainsKey(name);
            if (false == contained)
            {
                if (true == isRunning)
                {
                    base.Start();
                }
                return;
            }

            foreach (var timeUnit in this.timeUnitListDictionary[name])
            {
                if (false == timeUnit.IsStoped)
                {
                    timeUnit.EndTime = base.Elapsed;
                    timeUnit.IsStoped = true;
                    break;
                }
            }

            if (true == isRunning)
            {
                base.Start();
            }
            return;
        }

        /// <summary>
        /// 計測結果を取得します。
        /// </summary>
        /// <returns>計測結果</returns>
        public override string ToString()
        {
            string outputString = String.Empty;

            var timeStringList = this.timeUnitList.Select(item => item.ToString());
            outputString = (0 < timeStringList.Count()) ?
                String.Format("{0, -30}:{1, 6}\n{3}\n{2}\n{3}\n{0, -30}:{1, 6}", "Total", this.ElapsedMilliseconds, String.Join("\n", timeStringList), new String('-', 27)) :
                String.Format("{0, -30}:{1, 6}", "Total", this.ElapsedMilliseconds);

            return outputString;
        }

        /// <summary>
        /// 詳細な時間計測の記録を保持するクラスです。
        /// </summary>
        private class TimeUnit
        {
            /// <summary>
            /// 詳細な時間計測の名称を取得、変更します。
            /// </summary>
            public string Name
            {
                get;
                set;
            }
            /// <summary>
            /// 計測開始時の経過時間を取得、変更します。
            /// </summary>
            public TimeSpan BeginTime
            {
                get;
                set;
            }
            /// <summary>
            /// 計測終了時の経過時間を取得、変更します。
            /// </summary>
            public TimeSpan EndTime
            {
                get;
                set;
            }
            /// <summary>
            /// 計測が終了したかどうかを取得、変更します。
            /// </summary>
            public bool IsStoped
            {
                get;
                set;
            }

            /// <summary>
            /// 計測した結果の経過時間を取得します。
            /// </summary>
            public TimeSpan TimeSpan
            {
                get
                {
                    return (true == this.IsStoped) ? this.EndTime.Subtract(this.BeginTime) : new TimeSpan(0); 
                }
            }

            /// <summary>
            /// 計測結果を取得します。
            /// </summary>
            /// <returns>計測結果</returns>
            public override string ToString()
            {
                return String.Format("{0, -20}:{1, 6}", this.Name, Convert.ToInt64(this.TimeSpan.TotalMilliseconds));
            }
        }
    }
}
