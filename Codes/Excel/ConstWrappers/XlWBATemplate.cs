// ******************************************************************
// XlWBATemplate.cs  ：バージョン互換用Excel.XlWBATemplate列挙型
// 作成日　：2013/08/13
// 更新履歴：2013/10/09 水落　　 Excelのバージョン互換性がないため、定数に修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Excel.ConstWrappers
{
    /// <summary>
    /// Excel.XlWBATemplateを模した列挙型です。
    /// </summary>
    public enum XlWBATemplate
    {
        /// <summary>
        /// ワークシート
        /// </summary>
        xlWBATWorksheet = -4167,
        /// <summary>
        /// グラフ
        /// </summary>
        xlWBATChart = -4109,
        /// <summary>
        /// Excelバージョン4マクロシート
        /// </summary>
        xlWBATExcel4MacroSheet = 4,
        /// <summary>
        /// Excelバージョン4インターナショナルマクロシート
        /// </summary>
        xlWBATExcel4IntlMacroSheet = 3,
    }
}
