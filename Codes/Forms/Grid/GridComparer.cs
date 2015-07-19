// ******************************************************************
// GridComparer.cs ： グリッドソート抽象クラス
// 作成日　：2014/04/23
// 更新履歴：2014/04/23 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mizuochi.Commons.Codes.Forms.Grid
{
    /// <summary>
    /// グリッドをソートする為の抽象クラス
    /// </summary>
    public abstract class GridComparer : IComparer
    {
        /// <summary>
        /// 並び替え方法
        /// </summary>
        protected SortOrder sortOrder;

        /// <summary>
        /// ソート列設定リスト
        /// </summary>
        protected CompareColumn[] compareColumnList;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="sortOrder">並び替え方法</param>
        public GridComparer(SortOrder sortOrder = SortOrder.Ascending)
        {
            this.sortOrder = sortOrder;
            this.compareColumnList = null;
        }

        #region IComparer メンバー
        /// <summary>
        ///  2つのオブジェクトを比較し、一方が他方より小さいか、等しいか、大きいかを示す値を返します。
        /// </summary>
        /// <param name="x">比較する最初のオブジェクトです。</param>
        /// <param name="y">比較する 2 番目のオブジェクトです。</param>
        /// <returns>x が y より小さい場合は負値。x と y は等しい場合は0。x が y より大きい場合は正値を返します。</returns>
        public abstract int Compare(object x, object y);
        #endregion
    }

    /// <summary>
    /// ソートを行う列の設定情報を保有します
    /// </summary>
    public struct CompareColumn
    {
        /// <summary>
        /// 列名
        /// </summary>
        private string columnName;
        /// <summary>
        /// 列に格納されている値の型
        /// </summary>
        private Type columnType;
        /// <summary>
        /// 並び順
        /// </summary>
        private SortOrder sortOrder;

        /// <summary>
        /// 列名を取得、設定します。
        /// </summary>
        public Type ColumnType
        {
            get
            {
                return columnType;
            }
            set
            {
                columnType = value;
            }
        }

        /// <summary>
        /// 列に格納されている値の型を取得、設定します。
        /// </summary>
        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = value;
            }
        }

        /// <summary>
        /// 並び順を取得、設定します。
        /// </summary>
        public SortOrder SortOrder
        {
            get
            {
                return sortOrder;
            }
            set
            {
                sortOrder = value;
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="columnType">列に格納されている値の型</param>
        /// <param name="sortOrder">並び順</param>
        public CompareColumn(string columnName, Type columnType = null, SortOrder sortOrder = SortOrder.Ascending)
        {
            this.columnName = columnName;
            this.columnType = columnType ?? typeof(string);
            this.sortOrder = sortOrder;
        }
    }
}
