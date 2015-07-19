// ******************************************************************
// HolidayCalculation.cs ： 算出 祝祭日クラス
// 作成日　：2014/06/27
// 更新履歴：2014/07/15 水落　　 祝祭日名を設定するよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mizuochi.Commons.Codes.Calculations;

namespace Mizuochi.Commons.Codes.Dates
{
    /// <summary>
    /// 算出する祝祭日のクラスです。
    /// </summary>
    public class HolidayCalculation : Holiday
    {
        /// <summary>
        /// 月を算出する計算式
        /// </summary>
        private string monthFormula;
        /// <summary>
        /// 日を算出する計算式
        /// </summary>
        private string dayFormula;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="monthFormula">月を算出する計算式</param>
        /// <param name="dayFormula">日を算出する計算式</param>
        /// <param name="name">祝祭日名</param>
        /// <param name="substituteFlg">振替休日フラグ</param>
        /// <param name="nationalHolidayFlg">国民の休日フラグ</param>
        public HolidayCalculation(string monthFormula, string dayFormula, string name = null, bool substituteFlg = true, bool nationalHolidayFlg = true)
            : base(HolidayType.Calculation, name, substituteFlg, nationalHolidayFlg)
        {
            try
            {
                var calc = new Calculation(monthFormula.Replace("{year}", "2000"));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("月を算出する計算式が不正です。", ex);
            }

            try
            {
                var calc = new Calculation(dayFormula.Replace("{year}", "2000"));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("日を算出する計算式が不正です。", ex);
            }

            this.monthFormula = monthFormula;
            this.dayFormula = dayFormula;
        }

        /// <summary>
        /// 指定された年の日付を取得します。
        /// </summary>
        /// <param name="year">取得する年</param>
        /// <returns>祝日の日付</returns>
        public override DateTime GetDate(int year)
        {
            var monthCalculation = new Calculation(monthFormula.Replace("{year}", year.ToString()));
            var dayCalculation = new Calculation(dayFormula.Replace("{year}", year.ToString()));

            int month = Convert.ToInt32(Math.Floor(monthCalculation.Value));
            int day = Convert.ToInt32(Math.Floor(dayCalculation.Value));

            DateTime tmpDate;
            bool result = DateTime.TryParse(String.Format("{0}/{1}/{2}", year, month, day), out tmpDate);
            if (false == result)
            {
                tmpDate = Holiday.NotExistingDate;
            }

            return tmpDate;
        }
    }
}
