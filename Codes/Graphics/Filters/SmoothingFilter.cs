// ******************************************************************
// SmoothingFilter.cs  ：平滑化フィルタクラス
// 作成　　：2015/03/30 水落
// 更新履歴：2015/03/30 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mizuochi.Commons.Codes.Graphics.Filters
{
    /// <summary>
    /// 平滑化フィルタを行うクラスです。
    /// </summary>
    public class SmoothingFilter : Filter
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="sourceBmp">フィルタを行う元の画像オブジェクトです。</param>
        /// <param name="mask">マスク値です。</param>
        /// <param name="centerPosition">マスクの中心位置です。</param>
        public SmoothingFilter(Bitmap sourceBmp, int[,] mask, Point centerPosition)
            :base(sourceBmp)
        {
            this.mask = mask;
            this.centerPosition = centerPosition;

            // パラメータチェック
            if ((this.mask.GetLowerBound(0) > centerPosition.X) ||
                (this.mask.GetUpperBound(0) < centerPosition.X) ||
                (this.mask.GetLowerBound(1) > centerPosition.Y) ||
                (this.mask.GetUpperBound(1) > centerPosition.Y))
            {
                throw new ArgumentOutOfRangeException("centerPosition", "centerPositionの値がmaskの配列外を指しています。");
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="sourceBmp">フィルタを行う元の画像オブジェクトです。</param>
        protected SmoothingFilter(Bitmap sourceBmp)
            : base(sourceBmp)
        {
        }

        /// <summary>
        /// マスク値。
        /// </summary>
        protected int[,] mask;

        /// <summary>
        /// マスクの中央位置。
        /// </summary>
        protected Point centerPosition;

        /// <summary>
        /// マスク値を取得します。
        /// </summary>
        public int[,] Mask
        {
            get
            {
                return mask;
            }
        }

        /// <summary>
        /// マスクの中央位置を取得します。
        /// </summary>
        public Point CenterPosition
        {
            get
            {
                return centerPosition;
            }
        }

        /// <summary>
        /// 平滑化フィルタを行います。
        /// </summary>
        /// <returns>フィルタを行った後の画像オブジェクト。</returns>
        public override Bitmap Filtering()
        {
            // 画像をint配列に変換
            int[] sourceData = this.ConvertImageToIntArray(this.SourceBmp);
            int[] filteringData = new int[sourceData.Length];

            // 平滑化
            int height = this.SourceBmp.Height;
            int width = this.SourceBmp.Width;
            Parallel.For(0, sourceData.Length, workPoint =>
            {
                filteringData[workPoint] = this.Smooth(sourceData, workPoint, height, width);
            });

            var filteringBitmap = this.ConvertIntArrayToImage(filteringData, this.SourceBmp.Height, this.SourceBmp.Width);

            return filteringBitmap;
        }

        /// <summary>
        /// 指定したPixelに対して、平滑化したデータを算出します。
        /// </summary>
        /// <param name="sourceData">元画像データ</param>
        /// <param name="workPoint">平滑化する位置</param>
        /// <param name="height">画像の高さ</param>
        /// <param name="width">画像の幅</param>
        /// <returns>平滑化したデータ</returns>
        private int Smooth(int[] sourceData, int workPoint, int height, int width)
        {
            int total = 0;
            int dataRed = 0;
            int dataGreen = 0;
            int dataBlue = 0;

            for (int y = 0; y < this.mask.GetLength(1); ++y)
            {
                for (int x = 0; x < this.mask.GetLength(0); ++x)
                {
                    int targetX = workPoint % width;
                    int targetY = workPoint / width;
                    int offsetX = x - this.CenterPosition.X;
                    int offsetY = y - this.CenterPosition.Y;

                    if ((0 > (targetX + offsetX)) ||
                        (width <= (targetX + offsetX)) ||
                        (0 > (targetY + offsetY)) ||
                        (height <= (targetY + offsetY)))
                    {
                        continue;
                    }

                    int smoothPoint = ((targetY + offsetY) * width) + (targetX + offsetX);
                    dataRed += this.GetDataRed(sourceData[smoothPoint]) * this.mask[x, y];
                    dataGreen += this.GetDataGreen(sourceData[smoothPoint]) * this.mask[x, y];
                    dataBlue += this.GetDataBlue(sourceData[smoothPoint]) * this.mask[x, y];
                    total += this.mask[x, y];
                }
            }

            int dataAlpha = this.GetDataAlpha(sourceData[workPoint]);
            dataRed /= total;
            dataGreen /= total;
            dataBlue /= total;
            int data = this.SetData(dataAlpha, dataRed, dataGreen, dataBlue);

            return data;
        }
    }
}
