// ******************************************************************
// WhiteningFilter.cs  ：白化フィルタクラス
// 作成　　：2015/03/30 水落
// 更新履歴：2015/03/30 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2015
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuochi.Commons.Codes.Graphics.Filters
{
    /// <summary>
    /// 白化フィルタを行うクラスです。
    /// </summary>
    public class WhiteningFilter : Filter
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="sourceBmp">フィルタを行う元の画像オブジェクトです。</param>
        /// <param name="rate">白化する率です。0.0は元画像のまま、1.0は完全に白になります。</param>
        public WhiteningFilter(Bitmap sourceBmp, double rate)
            :base(sourceBmp)
        {
            if ((0.0 > rate) || (1.0 < rate))
            {
                throw new ArgumentOutOfRangeException("rate", "rateに0.0～1.0以外の値が設定されています。");
            }

            this.rate = rate;
        }

        /// <summary>
        /// 白化率。
        /// </summary>
        private double rate;

        /// <summary>
        /// 白化率を取得します。
        /// </summary>
        public double Rate
        {
            get
            {
                return rate;
            }
        }

        /// <summary>
        /// 白化フィルタを行います。
        /// </summary>
        /// <returns>フィルタを行った後の画像オブジェクト。</returns>
        public override Bitmap Filtering()
        {
            // 画像をint配列に変換
            int[] sourceData = this.ConvertImageToIntArray(this.SourceBmp);
            int[] filteringData = new int[sourceData.Length];

            // 白化
            int height = this.SourceBmp.Height;
            int width = this.SourceBmp.Width;
            Parallel.For(0, sourceData.Length, workPoint =>
            {
                filteringData[workPoint] = this.Whitening(sourceData, workPoint, height, width);
            });

            var filteringBitmap = this.ConvertIntArrayToImage(filteringData, this.SourceBmp.Height, this.SourceBmp.Width);

            return filteringBitmap;
        }

        /// <summary>
        /// 指定したPixelに対して、白化したデータを算出します。
        /// </summary>
        /// <param name="sourceData">元画像データ</param>
        /// <param name="workPoint">白化する位置</param>
        /// <param name="height">画像の高さ</param>
        /// <param name="width">画像の幅</param>
        /// <returns>平滑化したデータ</returns>
        private int Whitening(int[] sourceData, long workPoint, int height, int width)
        {
            int dataAlpha = this.GetDataAlpha(sourceData[workPoint]);
            int dataRed = this.GetDataRed(sourceData[workPoint]);
            int dataGreen = this.GetDataGreen(sourceData[workPoint]);
            int dataBlue = this.GetDataBlue(sourceData[workPoint]);

            dataRed = (int)((dataRed * (1 - this.rate)) + (255 * this.rate));
            dataGreen = (int)((dataGreen * (1 - this.rate)) + (255 * this.rate));
            dataBlue = (int)((dataBlue * (1 - this.rate)) + (255 * this.rate));

            int data = this.SetData(dataAlpha, dataRed, dataGreen, dataBlue);

            return data;
        }
    }
}
