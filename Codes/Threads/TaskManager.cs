// ******************************************************************
// TaskManager.cs ： タスク管理クラス
// 作成日　：2014/03/07
// 更新履歴：2014/03/07 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mizuochi.Commons.Codes.ClassExtensions;

namespace Mizuochi.Commons.Codes.Threads
{
    /// <summary>
    /// タスクを管理するクラスです。
    /// </summary>
    public class TaskManager: IEnumerable<Task>
    {
        /// <summary>
        /// 全体管理用インスタンス
        /// </summary>
        private static TaskManager global = new TaskManager();

        /// <summary>
        /// 全体管理用インスタンス
        /// </summary>
        public static TaskManager Global
        {
            get
            {
                return TaskManager.global;
            }
        }

        /// <summary>
        /// 管理タスクリスト
        /// </summary>
        private Dictionary<Task, CancellationTokenSource> taskList;

        /// <summary>
        /// タスク実行中かどうかを示します。
        /// </summary>
        private bool IsRunning
        {
            get
            {
                return this.taskList.Any(item => item.Key.IsRunning());
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public TaskManager()
        {
            this.taskList = new Dictionary<Task, CancellationTokenSource>();
        }

        /// <summary>
        /// 指定されたタスクを管理タスクに追加します。
        /// </summary>
        /// <param name="task">管理するタスク</param>
        /// <param name="cancellationTokenSource">キャンセル通知用トークン</param>
        /// <returns>追加されたタスク</returns>
        public Task Add(Task task, CancellationTokenSource cancellationTokenSource = null)
        {
            // タスク完了時にタスクリストから削除
            task.ContinueWith(completeTask => this.taskList.Remove(completeTask));

            // リスト追加
            CancellationTokenSource tokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            this.taskList.Add(task, tokenSource);

            // 全体管理に追加
            if (TaskManager.global != this)
            {
                this.taskList.Add(task, tokenSource);
            }

            return task;
        }

        /// <summary>
        /// 指定された戻り値のない処理を管理タスクに追加します。
        /// </summary>
        /// <param name="action">管理するタスクの処理</param>
        /// <param name="cancellationTokenSource">キャンセル通知用トークン</param>
        /// <param name="creationOptions">タスクのオプション動作</param>
        /// <returns>追加されたタスク</returns>
        public Task Add(Action action, CancellationTokenSource cancellationTokenSource = null, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            // タスク生成
            CancellationTokenSource tokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            Task task = new Task(action, tokenSource.Token, creationOptions);

            // リスト追加
            this.Add(task, tokenSource);

            return task;
        }

        /// <summary>
        /// 指定された戻り値のある処理を管理タスクに追加します。
        /// </summary>
        /// <typeparam name="ResultType">タスクの戻り値</typeparam>
        /// <param name="function">管理するタスクの処理</param>
        /// <param name="cancellationTokenSource">キャンセル通知用トークン</param>
        /// <param name="creationOptions">タスクのオプション動作</param>
        /// <returns>追加されたタスク</returns>
        public Task<ResultType> Add<ResultType>(Func<ResultType> function, CancellationTokenSource cancellationTokenSource = null, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            // タスク生成
            CancellationTokenSource tokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            Task<ResultType> task = new Task<ResultType>(function, tokenSource.Token, creationOptions);

            // リスト追加
            this.Add(task, tokenSource);

            return task;
        }

        /// <summary>
        /// 指定されたタスクを管理タスクに追加、開始します。
        /// </summary>
        /// <param name="task">管理するタスク</param>
        /// <param name="cancellationTokenSource">キャンセル通知用トークン</param>
        /// <returns>追加されたタスク</returns>
        public Task AddStart(Task task, CancellationTokenSource cancellationTokenSource = null)
        {
            // リスト追加
            this.Add(task, cancellationTokenSource);

            // タスク開始
            task.Start();

            return task;
        }

        /// <summary>
        /// 指定された戻り値のない処理を管理タスクに追加、開始します。
        /// </summary>
        /// <param name="action">管理するタスクの処理</param>
        /// <param name="cancellationTokenSource">キャンセル通知用トークン</param>
        /// <param name="creationOptions">タスクのオプション動作</param>
        /// <returns>追加されたタスク</returns>
        public Task AddStart(Action action, CancellationTokenSource cancellationTokenSource = null, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            // タスク生成
            Task task = this.Add(action, cancellationTokenSource, creationOptions);

            // タスク開始
            task.Start();

            return task;
        }

        /// <summary>
        /// 指定された戻り値のある処理を管理タスクに追加、開始します。
        /// </summary>
        /// <typeparam name="ResultType">タスクの戻り値</typeparam>
        /// <param name="function">管理するタスクの処理</param>
        /// <param name="cancellationTokenSource">キャンセル通知用トークン</param>
        /// <param name="creationOptions">タスクのオプション動作</param>
        /// <returns>追加されたタスク</returns>
        public Task<ResultType> AddStart<ResultType>(Func<ResultType> function, CancellationTokenSource cancellationTokenSource = null, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            // タスク生成
            Task<ResultType> task = this.Add(function, cancellationTokenSource, creationOptions);

            // タスク開始
            task.Start();

            return task;
        }

        /// <summary>
        /// 管理しているタスクが全て完了するまで待機します。
        /// </summary>
        /// <param name="millisecondsTimeout">待ち時間</param>
        /// <returns>全てのタスクの実行が完了した場合、true。完了しなかった場合、falseを返します。</returns>
        public bool Wait(int millisecondsTimeout = Timeout.Infinite)
        {
            bool result = true;
            foreach (var item in this.taskList)
            {
                result &= item.Key.ReinforceWait(millisecondsTimeout);
            }

            return result;
        }

        /// <summary>
        /// 管理しているタスクを全てキャンセルします。
        /// </summary>
        public void Cancel()
        {
            foreach (var item in this.taskList)
            {
                item.Value.Cancel();
            }

            return;
        }

        #region IEnumerable<Task> メンバー
        /// <summary>
        /// 管理しているタスクリストを反復処理する列挙子を返します。
        /// </summary>
        /// <returns></returns>
        IEnumerator<Task> IEnumerable<Task>.GetEnumerator()
        {
            return this.taskList.Keys.GetEnumerator();
        }
        #endregion

        #region IEnumerable メンバー
        /// <summary>
        /// 管理しているタスクリストを反復処理する列挙子を返します。
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Task>)this).GetEnumerator();
        }
        #endregion
    }
}
