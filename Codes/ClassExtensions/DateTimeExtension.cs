// ******************************************************************
// DateTimeExtension.cs ： DateTime拡張クラス
// 作成日　：2013/02/25
// 更新履歴：2014/10/15 水落　　 月末日取得メソッドのロジックを修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Mizuochi.Commons.Codes.Dates;

namespace Mizuochi.Commons.Codes.ClassExtensions
{
    /// <summary>
    /// DateTimeクラスを拡張するクラスです。
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 曜日を日本語で取得します。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>曜日</returns>
        public static string GetDayOfWeekString(this DateTime date)
        {
            return date.ToString("ddd");
        }

        /// <summary>
        /// 元号の取得します。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>元号</returns>
        public static string GetJapaneseEraString(this DateTime date)
        {
            Calendar calendar = new JapaneseCalendar();
            CultureInfo culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = calendar;

            return date.ToString("gy", culture);
        }

        /// <summary>
        /// 和暦の取得します。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>和暦表記の年月日</returns>
        public static string GetJapaneseCalendarString(this DateTime date)
        {
            Calendar calendar = new JapaneseCalendar();
            CultureInfo culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = calendar;

            return date.ToString("gy年M月d日", culture);
        }

        /// <summary>
        /// 月初日を取得します。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>月初日</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 月末日を取得します。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>月末日</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        /// <summary>
        /// 指定された日に設定します。
        /// </summary>
        /// <param name="date">変更対象日付</param>
        /// <param name="day">日</param>
        /// <returns>変更された日付</returns>
        public static DateTime SetDay(this DateTime date, int day)
        {
            return new DateTime(date.Year, date.Month, day);
        }

        /// <summary>
        /// 指定された月に設定します。
        /// </summary>
        /// <param name="date">変更対象日付</param>
        /// <param name="month">月</param>
        /// <returns>変更された日付</returns>
        public static DateTime SetMonth(this DateTime date, int month)
        {
            return new DateTime(date.Year, month, date.Day);
        }
        
        /// <summary>
        /// 指定された年に設定します。
        /// </summary>
        /// <param name="date">変更対象日付</param>
        /// <param name="year">年</param>
        /// <returns>変更された日付</returns>
        public static DateTime SetYear(this DateTime date, int year)
        {
            return new DateTime(year, date.Month, date.Day);
        }

        /// <summary>
        /// 週末かどうか判定します。
        /// </summary>
        /// <param name="date">判定対象日付</param>
        /// <returns>週末であれば、true。そうでなければ、falseを返します。</returns>
        public static bool IsWeekend(this DateTime date)
        {
            bool isWeekend = false;

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    isWeekend = false;
                    break;

                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    isWeekend = true;
                    break;
            }
            return isWeekend;
        }

        /// <summary>
        /// 祝祭日かどうか判定します。
        /// </summary>
        /// <param name="date">判定対象日付</param>
        /// <returns>祝祭日であれば、true。そうでなければ、falseを返します。</returns>
        public static bool IsHoliday(this DateTime date)
        {
            return Holiday.CheckHolidayDateTime(date);
        }

        /// <summary>
        /// 週末または祝祭日かどうか判定します。
        /// </summary>
        /// <param name="date">判定対象日付</param>
        /// <returns>週末または祝祭日であれば、true。平日であれば、falseを返します。</returns>
        public static bool IsWeekendAndHoliday(this DateTime date)
        {
            bool isWeekendAndHoliday =
                (true == date.IsWeekend()) ? true :
                (true == date.IsHoliday()) ? true :
                                             false;
            return isWeekendAndHoliday;
        }
    }
}
