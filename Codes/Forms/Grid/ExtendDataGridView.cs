// ******************************************************************
// ExtendDataGridView.cs ： DataGridView機能拡張クラス
// 作成日　：2014/04/23
// 更新履歴：2014/04/23 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.Forms.Grid
{
    /// <summary>
    /// DataGridViewの機能拡張を行うクラスです。
    /// </summary>
    public class ExtendDataGridView
    {
        /// <summary>
        /// 機能拡張対象グリッド
        /// </summary>
        private DataGridView grid;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="grid">機能拡張対象グリッド</param>
        public ExtendDataGridView(DataGridView grid)
        {
            this.grid = grid;
        }

        #region 複数行選択時同時チェック機能
        /// <summary>
        /// 複数行選択時同時チェック機能有無
        /// </summary>
        private bool isMultiSelectCheck = false;
        /// <summary>
        /// チェック列
        /// </summary>
        private DataGridViewCheckBoxColumn checkColumn = null;
        /// <summary>
        /// 選択行保持メンバ
        /// </summary>
        private IEnumerable<DataGridViewRow> selectedGridRow = null;

        /// <summary>
        /// グリッドに複数行選択時における同時チェック機能を付与する。
        /// </summary>
        /// <param name="checkColumn">チェック列</param>
        public void MultiSelectCheck(DataGridViewCheckBoxColumn checkColumn)
        {
            this.checkColumn = checkColumn;
            if (false == isMultiSelectCheck)
            {
                grid.CellMouseDown += this.MultiSelectCheck_CellMouseDown;
                grid.CellClick += this.MultiSelectCheck_CellClick;
            }

            return;
        }

        /// <summary>
        /// グリッド上のセル上でマウスが押下された時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void MultiSelectCheck_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // チェックボックス列か判定
            if (e.ColumnIndex == this.checkColumn.Index)
            {
                // 複数行選択されているか判定
                // --- 複数選択していてもCellClick時にクリックした行のみ選択されてしまうため、
                // --- 保持していた選択行を元戻す
                if ((1 < this.grid.Rows.GetRowCount(DataGridViewElementStates.Selected)) &&
                    (true == this.grid.Rows[e.RowIndex].Selected))
                {
                    this.selectedGridRow = this.grid.SelectedRows.Cast<DataGridViewRow>();
                }
                else
                {
                    this.selectedGridRow = null;
                }
            }

            return;
        }

        /// <summary>
        /// グリッド上のセルがクリックされた時に発生します。
        /// </summary>
        /// <param name="sender">イベントのソースです。</param>
        /// <param name="e">イベントデータを格納しています。</param>
        private void MultiSelectCheck_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // チェックボックス列か判定
            if (e.ColumnIndex == this.checkColumn.Index)
            {
                // 選択行を元戻す
                // --- 複数選択していてもCellClick時にクリックした行のみ選択されてしまうため、
                // --- 保持していた選択行を元戻す
                if (null != selectedGridRow)
                {
                    foreach (DataGridViewRow gridRow in selectedGridRow)
                    {
                        gridRow.Selected = true;
                    }
                }
            }

            return;
        }
        #endregion
    }
}
