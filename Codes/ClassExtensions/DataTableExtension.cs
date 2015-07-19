// ******************************************************************
// DataTableExtension.cs ： DataTable拡張クラス
// 作成日　：2013/08/03
// 更新履歴：2013/08/03 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// DataTableクラスを拡張するクラスです。
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// ソートします。
        /// </summary>
        /// <typeparam name="DataTableType">DataTableを継承する型</typeparam>
        /// <param name="sourceDataTable">ソート元</param>
        /// <param name="expression">ソート条件文</param>
        /// <returns>ソートされたDataTable</returns>
        public static DataTableType Sort<DataTableType>(this DataTableType sourceDataTable, string expression)
            where DataTableType : System.Data.DataTable
        {
            // データテーブルのコピーを作成
            DataTableType destination = (DataTableType)sourceDataTable.Clone();

            // ソートされたデータビューの作成
            DataView dataView = new DataView(sourceDataTable);
            dataView.Sort = expression;

            // ソートされたレコードのコピー
            foreach (DataRowView dataViewRow in dataView)
            {
                destination.ImportRow(dataViewRow.Row);
            }

            return destination;
        }

        /// <summary>
        /// コピーします。
        /// </summary>
        /// <typeparam name="DataTableType">DataTableを継承する型</typeparam>
        /// <param name="sourceDataTable">ソート元のデータテーブル</param>
        /// <param name="destinationDataTable">ソート先のデータテーブル</param>
        public static void Copy<DataTableType>(this DataTableType sourceDataTable, DataTableType destinationDataTable)
            where DataTableType : System.Data.DataTable
        {
            // ソートされたレコードのコピー
            foreach (DataRow dataRow in sourceDataTable.Rows)
            {
                destinationDataTable.ImportRow(dataRow);
            }

            return;
        }
    }
}
