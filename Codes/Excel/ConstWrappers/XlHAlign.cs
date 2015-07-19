// ******************************************************************
// XlHAlign.cs  ：バージョン互換用Excel.XlHAlign列挙型
// 作成日　：2013/10/08
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
    /// Excel.XlHAlignを模した列挙型です。
    /// </summary>
    public enum XlHAlign
    {
        /// <summary>
        /// 標準
        /// </summary>
        xlHAlignGeneral = 1,
        /// <summary>
        /// 左詰め
        /// </summary>
        xlHAlignLeft = -4131,
        /// <summary>
        /// 中央揃え
        /// </summary>
        xlHAlignCenter = -4108,
        /// <summary>
        /// 右詰め
        /// </summary>
        xlHAlignRight = -4152,
        /// <summary>
        /// 繰り返し
        /// </summary>
        xlHAlignFill = 5,
        /// <summary>
        /// 両端揃え
        /// </summary>
        xlHAlignJustify = -4130,
        /// <summary>
        /// 選択範囲内で中央
        /// </summary>
        xlHAlignCenterAcrossSelection = 7,
        /// <summary>
        /// 均等割り付け
        /// </summary>
        xlHAlignDistributed = -4117,
    }
}
