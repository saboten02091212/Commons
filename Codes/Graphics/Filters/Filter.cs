// ******************************************************************
// Filter.cs  ：フィルタリング抽象クラス
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

namespace Mizuochi.Commons.Codes.Graphics.Filters
{
    /// <summary>
    /// 画像に対して、フィルタを行う機能を保有する抽象クラスです。
    /// </summary>
    public abstract class Filter
    {
        /// <summary>
        /// バイトのビット数。
        /// </summary>
        protected const int BitCountByByte = 8;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="sourceBmp">フィルタを行う元の画像オブジェクトです。</param>
        protected Filter(Bitmap sourceBmp)
        {
            this.sourceBmp = sourceBmp;
        }

        /// <summary>
        /// フィルタを行う元の画像オブジェクト。
        /// </summary>
        protected Bitmap sourceBmp;

        /// <summary>
        /// フィルタを行う元の画像オブジェクトを取得します。
        /// </summary>
        public Bitmap SourceBmp
        {
            get
            {
                return sourceBmp;
            }
        }

        /// <summary>
        /// フィルタリングを行います。
        /// </summary>
        /// <returns>フィルタを行った後の画像オブジェクト。</returns>
        public abstract Bitmap Filtering();

        /// <summary>
        /// 画像データからアルファ値だけを取得します。
        /// </summary>
        /// <param name="data">画像データ</param>
        /// <returns>アルファ値</returns>
        protected int GetDataAlpha(int data)
        {
            return (data >> (3 * Filter.BitCountByByte)) & 0xFF;
        }

        /// <summary>
        /// 画像データから赤の値だけを取得します。
        /// </summary>
        /// <param name="data">画像データ</param>
        /// <returns>赤値</returns>
        protected int GetDataRed(int data)
        {
            return (data >> (2 * Filter.BitCountByByte)) & 0xFF;
        }

        /// <summary>
        /// 画像データから緑の値だけを取得します。
        /// </summary>
        /// <param name="data">画像データ</param>
        /// <returns>緑値</returns>
        protected int GetDataGreen(int data)
        {
            return (data >> (1 * Filter.BitCountByByte)) & 0xFF;
        }

        /// <summary>
        /// 画像データから青の値だけを取得します。
        /// </summary>
        /// <param name="data">画像データ</param>
        /// <returns>青値</returns>
        protected int GetDataBlue(int data)
        {
            return data & 0xFF;
        }

        /// <summary>
        /// 指定されたRGBを画像データに設定します。
        /// </summary>
        /// <param name="dataAlpha">アルファ値</param>
        /// <param name="dataRed">赤値</param>
        /// <param name="dataGreen">緑値</param>
        /// <param name="dataBlue">青値</param>
        /// <returns></returns>
        protected int SetData(int dataAlpha, int dataRed, int dataGreen, int dataBlue)
        {
            int data =
                dataAlpha << (3 * Filter.BitCountByByte) |
                dataRed << (2 * Filter.BitCountByByte) |
                dataGreen << (1 * Filter.BitCountByByte) |
                dataBlue;

            return data;
        }

        /// <summary>
        /// Bitmapをint配列に変換します。
        /// </summary>
        /// <param name="bitmap">画像オブジェクト</param>
        /// <returns>画像オブジェクトから変換したint配列</returns>
        protected int[] ConvertImageToIntArray(Bitmap bitmap)
        {
            int[] data = new int[bitmap.Width * bitmap.Height];

            var bitmapdata = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            Marshal.Copy(bitmapdata.Scan0, data, 0, data.Length);

            bitmap.UnlockBits(bitmapdata);

            return data;
        }

        /// <summary>
        /// int配列をBitmapに変換します。
        /// </summary>
        /// <param name="data">画像配列が格納されたint配列</param>
        /// <param name="height">画像の高さ</param>
        /// <param name="width">画像の幅</param>
        /// <returns>int配列から変換した画像オブジェクト</returns>
        protected Bitmap ConvertIntArrayToImage(int[] data, int height, int width)
        {
            var bitmap = new Bitmap(width, height);

            var bitmapdata = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            Marshal.Copy(data, 0, bitmapdata.Scan0, data.Length);

            bitmap.UnlockBits(bitmapdata);

            return bitmap;
        }
    }
}
