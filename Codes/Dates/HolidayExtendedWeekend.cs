// ******************************************************************
// HolidayExtendedWeekend.cs ： 曜日固定 祝祭日クラス
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
    public class HolidayExtendedWeekend : Holiday
    {
        /// <summary>
        /// 月
        /// </summary>
        private int month;
        /// <summary>
        /// 週
        /// </summary>
        private int weekNumber;
        /// <summary>
        /// 曜日
        /// </summary>
        private DayOfWeek dayOfWeek;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="month">月</param>
        /// <param name="weekNumber">週</param>
        /// <param name="dayOfWeek">曜日</param>
        /// <param name="name">祝祭日名</param>
        /// <param name="substituteFlg">振替休日フラグ</param>
        /// <param name="nationalHolidayFlg">国民の休日フラグ</param>
        public HolidayExtendedWeekend(int month, int weekNumber, DayOfWeek dayOfWeek, string name = null, bool substituteFlg = true, bool nationalHolidayFlg = true)
            : base(HolidayType.ExtendedWeekend, name, substituteFlg, nationalHolidayFlg)
        {
            if ((1 > month) && (12 < month))
            {
                throw new ArgumentException("指定された月は存在しません", "month");
            }

            if ((1 > weekNumber) && (5 < weekNumber))
            {
                throw new ArgumentException("指定された週は存在しません", "weekNumber");
            }

            if (false == Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
            {
                throw new ArgumentException("指定された曜日は存在しません", "dayOfWeek");
            }

            this.month = month;
            this.weekNumber = weekNumber;
            this.dayOfWeek = dayOfWeek;
        }

        /// <summary>
        /// 指定された年の日付を取得します。
        /// </summary>
        /// <param name="year">取得する年</param>
        /// <returns>祝日の日付</returns>
        public override DateTime GetDate(int year)
        {
            // 祝日月の1日目の曜日との差を取得
            DateTime firstDate = new DateTime(year, this.month, 1);
            int difference = this.dayOfWeek - firstDate.DayOfWeek;
            if (0 > difference)
            {
                difference += 7;
            }

            // 日を算出
            int day = 1 + ((this.weekNumber- 1) * 7) + difference;

            // 日付が存在するか判定
            DateTime tmpDate;
            bool result = DateTime.TryParse(String.Format("{0}/{1}/{2}", year, this.month, day), out tmpDate);
            if (false == result)
            {
                tmpDate = Holiday.NotExistingDate;
            }

            return tmpDate;
        }
    }
}
