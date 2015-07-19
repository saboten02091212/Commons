// ******************************************************************
// TaskExtension.cs ： Task拡張クラス
// 作成日　：2013/08/26
// 更新履歴：2014/07/07 水落　　 強化待機メソッドをデッドロックが発生しないよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// Taskクラスを拡張するクラスです。
    /// </summary>
    public static class TaskExtension
    {
        /// <summary>
        /// デフォルトの確認間隔です。
        /// </summary>
        private const int DefaultCheckMillisecondsTimeSpan = 100;

        /// <summary>
        /// 実行が完了するまで待機します。
        /// </summary>
        /// <param name="task">タスク</param>
        /// <param name="millisecondsTimeout">タスクが完了する待ち時間</param>
        /// <param name="checkMillisecondsTimeSpan">タスクの状態を確認する間隔</param>
        /// <returns>タスクの実行が完了した場合、true。完了しなかった場合、falseを返します。</returns>
        public static bool ReinforceWait(this Task task,
            int millisecondsTimeout = Timeout.Infinite,
            int checkMillisecondsTimeSpan = TaskExtension.DefaultCheckMillisecondsTimeSpan)
        {
            bool result = false;

            // タイムアウトするまでループ
            int timeSpan = 0;
            while ((0 > millisecondsTimeout) || (timeSpan < millisecondsTimeout))
            {
                // タスク状態確認
                switch (task.Status)
                {
                    // 実行中の場合
                    case TaskStatus.WaitingForActivation:
                    case TaskStatus.WaitingToRun:
                    case TaskStatus.Running:
                    case TaskStatus.WaitingForChildrenToComplete:
                        try
                        {
                            result = task.Wait(checkMillisecondsTimeSpan);
                        }
                        catch (AggregateException)
                        {
                            result = false;
                        }
                        break;

                    // 終了している場合
                    case TaskStatus.Created:
                    case TaskStatus.RanToCompletion:
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                    default:
                        result = true;
                        break;
                }

                // 終了していればループから抜ける
                if (true == result)
                {
                    break;
                }

                // Invoke等で呼ばれた処理を片づける
                Application.DoEvents();
            }

            return result;
        }

        /// <summary>
        /// 実行中かどうか判定します。
        /// </summary>
        /// <param name="task">タスク</param>
        /// <returns>タスクの実行中の場合、true。未実行もしくは完了済の場合、falseを返します。</returns>
        public static bool IsRunning(this Task task)
        {
            bool isRunning = false;

            switch (task.Status)
            {
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    isRunning = true;
                    break;

                case TaskStatus.Created:
                case TaskStatus.RanToCompletion:
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                default:
                    isRunning = false;
                    break;
            }

            return isRunning;
        }
    }
}
