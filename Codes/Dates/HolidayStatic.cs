// ******************************************************************
// HolidayStatic.cs ： 固定月日 祝祭日クラス
// 作成日　：2014/06/27
// 更新履歴：2014/07/15 水落　　 祝祭日名を設定するよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Dates
{
    /// <summary>
    /// 固定月日の祝祭日のクラスです。
    /// </summary>
    public class HolidayStatic : Holiday
    {
        /// <summary>
        /// 月
        /// </summary>
        private int month;
        /// <summary>
        /// 日
        /// </summary>
        private int day;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="name">祝祭日名</param>
        /// <param name="substituteFlg">振替休日フラグ</param>
        /// <param name="nationalHolidayFlg">国民の休日フラグ</param>
        public HolidayStatic(int month, int day, string name = null, bool substituteFlg = true, bool nationalHolidayFlg = true)
            : base(HolidayType.Static, name, substituteFlg, nationalHolidayFlg)
        {
            DateTime tmpDate;
            bool result = DateTime.TryParse(String.Format("{0}/{1}/{2}", 2000, month, day), out tmpDate);
            if (false == result)
            {
                throw new ArgumentException("存在しない月日です。");
            }

            this.month = month;
            this.day = day;
        }

        /// <summary>
        /// 指定された年の日付を取得します。
        /// </summary>
        /// <param name="year">取得する年</param>
        /// <returns>祝日の日付</returns>
        public override DateTime GetDate(int year)
        {
            DateTime tmpDate;
            bool result = DateTime.TryParse(String.Format("{0}/{1}/{2}", year, this.month, this.day), out tmpDate);
            if (false == result)
            {
                tmpDate = Holiday.NotExistingDate;
            }

            return tmpDate;
        }
    }
}
