// ******************************************************************
// XlVAlign.cs  ：バージョン互換用Excel.XlVAlign列挙型
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
    /// Excel.XlVAlignを模した列挙型です。
    /// </summary>
    public enum XlVAlign
    {
        /// <summary>
        /// 上端
        /// </summary>
        xlVAlignTop = -4160,
        /// <summary>
        /// 中央
        /// </summary>
        xlVAlignCenter = -4108,
        /// <summary>
        /// 下端
        /// </summary>
        xlVAlignBottom = -4107,
        /// <summary>
        /// 両端揃え
        /// </summary>
        xlVAlignJustify = -4130,
        /// <summary>
        /// 均等割り付け
        /// </summary>
        xlVAlignDistributed = -4117,
    }
}
