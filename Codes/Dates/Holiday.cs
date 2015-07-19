// ******************************************************************
// Holiday.cs ： 祝祭日クラス
// 作成日　：2014/06/27
// 更新履歴：2014/07/15 水落　　 振替休日の処理を修正。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mizuochi.Commons.Codes.ClassExtensions;

namespace Mizuochi.Commons.Codes.Dates
{
    /// <summary>
    /// 祝祭日を表す抽象クラスです。
    /// </summary>
    public abstract class Holiday
    {
        #region 静的フィールド
        /// <summary>
        /// 存在しない日付を表す日付値です。
        /// </summary>
        public static readonly DateTime NotExistingDate = default(DateTime);

        /// <summary>
        /// 祝祭日のリストです。
        /// </summary>
        private static IList<Holiday> holidayList;
        #endregion

        #region 静的プロパティ
        /// <summary>
        /// 祝祭日のリストを取得します。
        /// </summary>
        public static IList<Holiday> HolidayList
        {
            get
            {
                return Holiday.holidayList;
            }
        }
        #endregion

        #region 静的コンストラクタ
        /// <summary>
        /// 静的コンストラクタです。
        /// </summary>
        static Holiday()
        {
            Holiday.holidayList = new List<Holiday>();
        }
        #endregion

        #region 静的パブリックメソッド
        /// <summary>
        /// 指定された日付が祝祭日か判定します。
        /// </summary>
        /// <param name="date">判定対象日付</param>
        /// <returns>祝祭日の場合、true。祝祭日でない場合、falseを返します。</returns>
        public static bool CheckHolidayDateTime(DateTime date)
        {
            bool isHoliday = false;

            // 祝祭日か判定
            foreach (var holiday in Holiday.holidayList)
            {
                // 指定された年の祝祭日を取得
                DateTime holidayDate = holiday.GetDate(date.Year);

                // 存在しないか判定
                if (Holiday.NotExistingDate == holidayDate)
                {
                    continue;
                }

                // 祝祭日リスト内に存在するか判定
                if (date == holidayDate)
                {
                    isHoliday = true;
                    break;
                }

                // 振替休日か判定
                DateTime substituteHoliday = holiday.GetSubstituteHoliday(date.Year);
                if (date == substituteHoliday)
                {
                    isHoliday = true;
                    break;
                }
            }

            // 国民の休日か判定
            if (false == isHoliday)
            {
                DateTime preDate = date.AddDays(-1);
                DateTime afterDate = date.AddDays(1);

                bool isPreDateHoliday = Holiday.holidayList.Any(holiday => (holiday.GetDate(preDate.Year) == preDate) && (true == holiday.nationalHolidayFlg));
                bool isAfterDateHoliday = Holiday.holidayList.Any(holiday => holiday.GetDate(afterDate.Year) == afterDate && (true == holiday.nationalHolidayFlg));

                if ((true == isPreDateHoliday) && (true == isAfterDateHoliday))
                {
                    isHoliday = true;
                }
            }

            return isHoliday;
        }
        #endregion

        #region フィールド
        /// <summary>
        /// 祝祭日の日付パターンです。
        /// </summary>
        private HolidayType holidayType;

        /// <summary>
        /// 祝祭日の名前です。
        /// </summary>
        private string name;

        /// <summary>
        /// 振替休日されるかを示します。
        /// </summary>
        private bool substituteFlg;

        /// <summary>
        /// 国民の休日で使用できるかどうかを示すフラグです。
        /// </summary>
        private bool nationalHolidayFlg;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 祝祭日の名前を取得します。
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="holidayType">祝祭日の日付パターン</param>
        /// <param name="name">祝祭日名</param>
        /// <param name="substituteFlg">振替休日フラグ</param>
        /// <param name="nationalHolidayFlg">国民の休日フラグ</param>
        protected Holiday(HolidayType holidayType, string name = null, bool substituteFlg = true, bool nationalHolidayFlg = true)
        {
            this.holidayType = holidayType;
            this.name = name;
            this.substituteFlg = substituteFlg;
            this.nationalHolidayFlg = nationalHolidayFlg;
        }
        #endregion

        #region 抽象メソッド
        /// <summary>
        /// 指定された年の日付を取得します。
        /// </summary>
        /// <param name="year">取得する年</param>
        /// <returns>祝日の日付</returns>
        public abstract DateTime GetDate(int year);
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 振替休日が発生している場合、振替休日の日付を取得します。
        /// </summary>
        /// <param name="year">取得する年</param>
        /// <returns>振替休日が発生している場合、振替休日の日付。発生していない場合は、本来の祝日の日付 を返却します。</returns>
        public DateTime GetSubstituteHoliday(int year)
        {
            DateTime date = this.GetDate(year);

            // 振替休日が発生しているか判定
            if (true == this.HasSubstituteHoliday(date.Year))
            {
                while (true)
                {
                    // 日曜だった場合、次の日を判定
                    if (DayOfWeek.Sunday == date.DayOfWeek)
                    {
                        date = date.AddDays(1);
                        continue;
                    }

                    // 祝祭日だった場合、次の日を判定
                    // ---初日は確実に自身の日付でtrueになるが、
                    // ---HasSubstituteHoliday内で上記のif文内に確実に入るような条件のため、
                    // ---自身の日付ではHitしない
                    if (true == Holiday.holidayList.Any(holiday => date == holiday.GetDate(date.Year)))
                    {
                        date = date.AddDays(1);
                        continue;
                    }

                    break;
                }
            }

            return date;
        }

        /// <summary>
        /// 振替休日が発生しているかどうかを判定します。
        /// </summary>
        /// <param name="year">取得する年</param>
        /// <returns>振替休日が発生している場合、true。発生していない場合は、false を返却します。</returns>
        public bool HasSubstituteHoliday(int year)
        {
            DateTime date = this.GetDate(year);

            bool result = false;
            if ((true == this.substituteFlg) && (DayOfWeek.Sunday == date.DayOfWeek))
            {
                result = true;
            }

            return result;
        }
        #endregion

        #region 内部定義
        /// <summary>
        /// 祝祭日の日付パターンを表す列挙型です。
        /// </summary>
        public enum HolidayType
        {
            /// <summary>
            /// 固定日
            /// </summary>
            Static = 0,
            /// <summary>
            /// 曜日固定
            /// </summary>
            ExtendedWeekend = 1,
            /// <summary>
            /// 算出
            /// </summary>
            Calculation = 2,
        }
        #endregion
    }
}
