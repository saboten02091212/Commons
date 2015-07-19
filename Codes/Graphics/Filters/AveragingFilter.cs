// ******************************************************************
// AveragingFilter.cs  ：平均化フィルタクラス
// 作成　　：2015/03/30 水落
// 更新履歴：2015/03/30 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Graphics.Filters
{
    /// <summary>
    /// 平均化フィルタを行うクラスです。
    /// </summary>
    public class AveragingFilter : SmoothingFilter
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="sourceBmp">フィルタを行う元の画像オブジェクトです。</param>
        /// <param name="maskSize">マスクのサイズです。</param>
        public AveragingFilter(Bitmap sourceBmp, int maskSize)
            :base(sourceBmp)
        {
            if (2 > maskSize)
            {
                throw new ArgumentException("maskSizeに2未満の値が設定されています", "maskSize");
            }

            int[,] mask = new int[maskSize * 2 - 1, maskSize * 2 - 1];
            for (int i = 0; i < maskSize * 2 - 1; ++i)
            {
                for (int j = 0; j < maskSize * 2 - 1; ++j)
                {
                    mask[i, j] = 1;
                }
            }

            Point centerPosition = new Point(maskSize - 1, maskSize - 1);

            this.mask = mask;
            this.centerPosition = centerPosition;
        }
    }
}
