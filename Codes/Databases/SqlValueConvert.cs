// ******************************************************************
// SqlValueConvert.cs ： SQLフィールド用変換クラス
// 作成日　：2013/02/25
// 更新履歴：2013/03/17 水落　　 long型処理追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mizuochi.Commons.Codes.Types;

namespace Mizuochi.Commons.Codes.Databases
{
    /// <summary>
    /// null許容型を他の型のnull許容型に変換します。
    /// </summary>
    public static class SqlValueConvert
    {
        #region 静的パブリックメソッド
        #region ToNullableBooleanメソッド
        /// <summary>
        /// 指定したブール値を返します。実際の変換処理は実行されません。
        /// </summary>
        /// <param name="value">返されるブール値。</param>
        /// <returns>value は変更されずに返されます。</returns>
        public static bool? ToNullableBoolean(bool? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 8 ビット符号なし整数の値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する 8 ビット符号なし整数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(byte? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// このメソッドを呼び出すと、必ず System.InvalidCastException がスローされます。
        /// </summary>
        /// <param name="value">変換する Unicode 文字。</param>
        /// <returns>この変換はサポートされていません。値は返されません。</returns>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        public static bool? ToNullableBoolean(char? value)
        {
            throw new InvalidCastException("この変換はサポートされていません。");
        }

        /// <summary>
        /// このメソッドを呼び出すと、必ず System.InvalidCastException がスローされます。
        /// </summary>
        /// <param name="value">変換する日時の値。</param>
        /// <returns>この変換はサポートされていません。値は返されません。</returns>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        public static bool? ToNullableBoolean(DateTime? value)
        {
            throw new InvalidCastException("この変換はサポートされていません。");
        }

        /// <summary>
        /// 指定した 10 進数値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する数値。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(decimal? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した倍精度浮動小数点数値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する倍精度浮動小数点数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(double? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した単精度浮動小数点数値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する単精度浮動小数点数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(float? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 32 ビット符号付き整数の値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する 32 ビット符号付き整数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(int? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 64 ビット符号付き整数の値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する 64 ビット符号付き整数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(long? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定したオブジェクトの値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">System.IConvertible インターフェイスを実装するオブジェクトか、または null。</param>
        /// <returns>true または false。基になる value の型に対して System.IConvertible.ToBoolean(System.IFormatProvider)メソッドを呼び出すことで返される値を反映します。value が null の場合、メソッドは null を返します。</returns>
        /// <exception cref="System.FormatException">value は、System.Boolean.TrueString または System.Boolean.FalseString と等しくない文字列です。</exception>
        /// <exception cref="System.InvalidCastException">value が System.IConvertible インターフェイスを実装していません。またはvalue から System.Booleanへの変換はサポートされていません。</exception>
        public static bool? ToNullableBoolean(object value)
        {
            bool? result = null;

            if ((null != value) && (DBNull.Value != value))
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 8 ビット符号付き整数の値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する 8 ビット符号付き整数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(sbyte? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 16 ビット符号付き整数の値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する 16 ビット符号付き整数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(short? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した論理値の文字列形式をそれと等価なブール値に変換します。
        /// </summary>
        /// <param name="value">System.Boolean.TrueString または System.Boolean.FalseString のいずれかの値を格納する文字列。</param>
        /// <returns>value が System.Boolean.TrueString に等しい場合は true。value が System.Boolean.FalseString に等しい場合は false。</returns>
        /// <exception cref="System.FormatException">value は、System.Boolean.TrueString または System.Boolean.FalseString と等しくない文字列です。</exception>
        public static bool? ToNullableBoolean(string value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 32 ビット符号なし整数の値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する 32 ビット符号なし整数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(uint? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 64 ビット符号なし整数の値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する 64 ビット符号なし整数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(ulong? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 16 ビット符号なし整数の値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">変換する 16 ビット符号なし整数。</param>
        /// <returns>value が 0 以外の場合は true。0 の場合は false。value が null の場合は null。</returns>
        public static bool? ToNullableBoolean(ushort? value)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        /// <summary>
        /// 指定したカルチャに固有の書式情報を使用して、指定したオブジェクトの値を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">System.IConvertible インターフェイスを実装するオブジェクトか、または null。</param>
        /// <param name="provider">カルチャ固有の書式情報を提供するオブジェクト。</param>
        /// <returns>true または false。基になる value の型に対して System.IConvertible.ToBoolean(System.IFormatProvider)メソッドを呼び出すことで返される値を反映します。value が null の場合、メソッドは null を返します。</returns>
        /// <exception cref="System.FormatException">value は、System.Boolean.TrueString または System.Boolean.FalseString と等しくない文字列です。</exception>
        /// <exception cref="System.InvalidCastException">value が System.IConvertible インターフェイスを実装していません。またはvalue から System.Booleanへの変換はサポートされていません。</exception>
        public static bool? ToNullableBoolean(object value, IFormatProvider provider)
        {
            bool? result = null;

            if ((null != value) && (DBNull.Value != value))
            {
                result = Convert.ToBoolean(value, provider);
            }

            return result;
        }

        /// <summary>
        /// 指定したカルチャに固有の書式情報を使用して、指定した論理値の文字列形式を等価のブール値に変換します。
        /// </summary>
        /// <param name="value">System.Boolean.TrueString または System.Boolean.FalseString のいずれかの値を格納する文字列。</param>
        /// <param name="provider">カルチャ固有の書式情報を提供するオブジェクト。このパラメーターは無視されます。</param>
        /// <returns>value が System.Boolean.TrueString に等しい場合は true。value が System.Boolean.FalseString に等しい場合は false。</returns>
        /// <exception cref="System.FormatException">value は、System.Boolean.TrueString または System.Boolean.FalseString と等しくない文字列です。</exception>
        /// <exception cref="System.InvalidCastException">value が System.IConvertible インターフェイスを実装していません。またはvalue から System.Booleanへの変換はサポートされていません。</exception>
        public static bool? ToNullableBoolean(string value, IFormatProvider provider)
        {
            bool? result = null;

            if (null != value)
            {
                result = Convert.ToBoolean(value, provider);
            }

            return result;
        }

        /// <summary>
        /// このメソッドを呼び出すと、必ず System.InvalidCastException がスローされます。
        /// </summary>
        /// <param name="value">変換する 列挙型のメンバー。</param>
        /// <returns>この変換はサポートされていません。値は返されません。</returns>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        public static bool? ToNullableBoolean<EnumType>(EnumType? value)
            where EnumType : struct
        {
            throw new InvalidCastException("この変換はサポートされていません。");
        }
        #endregion

        #region ToNullableInt32メソッド
        /// <summary>
        /// 指定したブール値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換するブール値。</param>
        /// <returns>value が true の場合は数値の 1。それ以外の場合は 0。</returns>
        public static int? ToNullableInt32(bool? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 8 ビット符号なし整数の値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 8 ビット符号なし整数。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        public static int? ToNullableInt32(byte? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した Unicode 文字の値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する Unicode 文字。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        public static int? ToNullableInt32(char? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// このメソッドを呼び出すと、必ず System.InvalidCastException がスローされます。
        /// </summary>
        /// <param name="value">変換する日時の値。</param>
        /// <returns>この変換はサポートされていません。値は返されません。</returns>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        public static int? ToNullableInt32(DateTime? value)
        {
            throw new InvalidCastException("この変換はサポートされていません。");
        }

        /// <summary>
        /// 指定した 10 進数値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する数値。</param>
        /// <returns>近似値の 32 ビット符号付き整数に丸められた value。value が 2 つの整数の中間にある場合は、偶数が返されます。</returns>
        /// <exception cref="System.OverflowException">value が System.Int32.MaxValue より大きい値か、System.Int32.MinValue より小さい値です。</exception>
        public static int? ToNullableInt32(decimal? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した倍精度浮動小数点数値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する倍精度浮動小数点数。</param>
        /// <returns>近似値の 32 ビット符号付き整数に丸められた value。value が 2 つの整数の中間にある場合は、偶数が返されます。</returns>
        /// <exception cref="System.OverflowException">value が System.Int32.MaxValue より大きい値か、System.Int32.MinValue より小さい値です。</exception>
        public static int? ToNullableInt32(double? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した単精度浮動小数点数値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する単精度浮動小数点数。</param>
        /// <returns>近似値の 32 ビット符号付き整数に丸められた value。value が 2 つの整数の中間にある場合は、偶数が返されます。</returns>
        /// <exception cref="System.OverflowException">value が System.Int32.MaxValue より大きい値か、System.Int32.MinValue より小さい値です。</exception>
        public static int? ToNullableInt32(float? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 32 ビット符号付き整数が返されます。実際の変換は行われません。
        /// </summary>
        /// <param name="value">返される 32 ビット符号付き整数。</param>
        /// <returns>value は変更されずに返されます。</returns>
        public static int? ToNullableInt32(int? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 64 ビット符号付き整数の値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 64 ビット符号付き整数。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        /// <exception cref="System.OverflowException">value が System.Int32.MaxValue より大きい値か、System.Int32.MinValue より小さい値です。</exception>
        public static int? ToNullableInt32(long? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定したオブジェクトの値を 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">System.IConvertible インターフェイスを実装するオブジェクトか、または null。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        /// <exception cref="System.FormatException">value の形式が適切ではありません。</exception>
        /// <exception cref="System.InvalidCastException">value が System.IConvertible インターフェイスを実装していません。または変換はサポートされていません。</exception>
        /// <exception cref="System.OverflowException">value が System.Int32.MinValue 未満の数値か、System.Int32.MaxValue を超える数値を表しています。</exception>
        public static int? ToNullableInt32(object value)
        {
            int? result = null;

            if ((null != value) && (DBNull.Value != value))
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 8 ビット符号付き整数の値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 8 ビット符号付き整数。</param>
        /// <returns>value と等価の 8 ビット符号付き整数。</returns>
        public static int? ToNullableInt32(sbyte? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 16 ビット符号付き整数の値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 16 ビット符号付き整数。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        public static int? ToNullableInt32(short? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した数値の文字列形式を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する数値を含んだ文字列。</param>
        /// <returns>value の数値と等価の 32 ビット符号付き整数。</returns>
        /// <exception cref="System.FormatException">value の構成が、省略可能な符号と、それに続く 0 から 9 までの一連の数字ではありません。</exception>
        /// <exception cref="System.OverflowException">value が System.Int32.MinValue 未満の数値か、System.Int32.MaxValue を超える数値を表しています。</exception>
        public static int? ToNullableInt32(string value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 32 ビット符号なし整数の値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 32 ビット符号なし整数。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        /// <exception cref="System.OverflowException">value は System.Int32.MaxValue より大。</exception>
        public static int? ToNullableInt32(uint? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 64 ビット符号なし整数の値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 64 ビット符号なし整数。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        /// <exception cref="System.OverflowException">value は System.Int32.MaxValue より大。</exception>
        public static int? ToNullableInt32(ulong? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 16 ビット符号なし整数の値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 16 ビット符号なし整数。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        public static int? ToNullableInt32(ushort? value)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        /// <summary>
        /// 指定したカルチャに固有の書式情報を使用して、指定したオブジェクトの値を 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">System.IConvertible インターフェイスを実装するオブジェクト。</param>
        /// <param name="provider">カルチャ固有の書式情報を提供するオブジェクト。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        /// <exception cref="System.FormatException">value の形式が適切ではありません。</exception>
        /// <exception cref="System.InvalidCastException">value が System.IConvertible を実装していません。</exception>
        /// <exception cref="System.OverflowException">value が System.Int32.MinValue 未満の数値か、System.Int32.MaxValue を超える数値を表しています。</exception>
        public static int? ToNullableInt32(object value, IFormatProvider provider)
        {
            int? result = null;

            if ((null != value) && (DBNull.Value != value))
            {
                result = Convert.ToInt32(value, provider);
            }

            return result;
        }

        /// <summary>
        /// 指定したカルチャに固有の書式情報を使用して、指定した数値の文字列形式を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する数値を含んだ文字列。</param>
        /// <param name="provider">カルチャ固有の書式情報を提供するオブジェクト。</param>
        /// <returns>value の数値と等価の 32 ビット符号付き整数。</returns>
        /// <exception cref="System.FormatException">value の構成が、省略可能な符号と、それに続く 0 から 9 までの一連の数字ではありません。</exception>
        /// <exception cref="System.OverflowException">value が System.Int32.MinValue 未満の数値か、System.Int32.MaxValue を超える数値を表しています。</exception>
        public static int? ToNullableInt32(string value, IFormatProvider provider)
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value, provider);
            }

            return result;
        }

        /// <summary>
        /// 指定した 列挙型のメンバー値を等価の 32 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 列挙型のメンバー値。</param>
        /// <returns>value と等価の 32 ビット符号付き整数。</returns>
        /// <exception cref="System.OverflowException">value が System.Int32.MaxValue より大きい値か、System.Int32.MinValue より小さい値です。</exception>
        public static int? ToNullableInt32<EnumType>(EnumType? value)
            where EnumType : struct
        {
            int? result = null;

            if (null != value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }
        #endregion

        #region ToNullableInt64メソッド
        /// <summary>
        /// 指定したブール値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換するブール値。</param>
        /// <returns>value が true の場合は数値の 1。それ以外の場合は 0。</returns>
        public static long? ToNullableInt64(bool? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 8 ビット符号なし整数の値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 8 ビット符号なし整数。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        public static long? ToNullableInt64(byte? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した Unicode 文字の値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する Unicode 文字。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        public static long? ToNullableInt64(char? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// このメソッドを呼び出すと、必ず System.InvalidCastException がスローされます。
        /// </summary>
        /// <param name="value">変換する日時の値。</param>
        /// <returns>この変換はサポートされていません。値は返されません。</returns>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        public static long? ToNullableInt64(DateTime? value)
        {
            throw new InvalidCastException("この変換はサポートされていません。");
        }

        /// <summary>
        /// 指定した 10 進数値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する数値。</param>
        /// <returns>近似値の 64 ビット符号付き整数に丸められた value。value が 2 つの整数の中間にある場合は、偶数が返されます。</returns>
        /// <exception cref="System.OverflowException">value が System.Int64.MaxValue より大きい値か、System.Int64.MinValue より小さい値です。</exception>
        public static long? ToNullableInt64(decimal? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した倍精度浮動小数点数値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する倍精度浮動小数点数。</param>
        /// <returns>近似値の 64 ビット符号付き整数に丸められた value。value が 2 つの整数の中間にある場合は、偶数が返されます。</returns>
        /// <exception cref="System.OverflowException">value が System.Int64.MaxValue より大きい値か、System.Int64.MinValue より小さい値です。</exception>
        public static long? ToNullableInt64(double? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した単精度浮動小数点数値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する単精度浮動小数点数。</param>
        /// <returns>近似値の 64 ビット符号付き整数に丸められた value。value が 2 つの整数の中間にある場合は、偶数が返されます。</returns>
        /// <exception cref="System.OverflowException">value が System.Int64.MaxValue より大きい値か、System.Int64.MinValue より小さい値です。</exception>
        public static long? ToNullableInt64(float? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 32 ビット符号付き整数の値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 32 ビット符号付き整数。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        public static long? ToNullableInt64(int? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }


        /// <summary>
        /// 指定した 64 ビット符号付き整数が返されます。実際の変換は行われません。
        /// </summary>
        /// <param name="value">返される 64 ビット符号付き整数。</param>
        /// <returns>value は変更されずに返されます。</returns>
        public static long? ToNullableInt64(long? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定したオブジェクトの値を 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">System.IConvertible インターフェイスを実装するオブジェクトか、または null。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        /// <exception cref="System.FormatException">value の形式が適切ではありません。</exception>
        /// <exception cref="System.InvalidCastException">value が System.IConvertible インターフェイスを実装していません。または変換はサポートされていません。</exception>
        /// <exception cref="System.OverflowException">value が System.Int64.MinValue 未満の数値か、System.Int64.MaxValue を超える数値を表しています。</exception>
        public static long? ToNullableInt64(object value)
        {
            long? result = null;

            if ((null != value) && (DBNull.Value != value))
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 8 ビット符号付き整数の値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 8 ビット符号付き整数。</param>
        /// <returns>value と等価の 8 ビット符号付き整数。</returns>
        public static long? ToNullableInt64(sbyte? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 16 ビット符号付き整数の値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 16 ビット符号付き整数。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        public static long? ToNullableInt64(short? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した数値の文字列形式を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する数値を含んだ文字列。</param>
        /// <returns>value の数値と等価の 64 ビット符号付き整数。</returns>
        /// <exception cref="System.FormatException">value の構成が、省略可能な符号と、それに続く 0 から 9 までの一連の数字ではありません。</exception>
        /// <exception cref="System.OverflowException">value が System.Int64.MinValue 未満の数値か、System.Int64.MaxValue を超える数値を表しています。</exception>
        public static long? ToNullableInt64(string value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 32 ビット符号なし整数の値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 32 ビット符号なし整数。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        public static long? ToNullableInt64(uint? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 64 ビット符号なし整数の値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 64 ビット符号なし整数。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        /// <exception cref="System.OverflowException">value は System.Int64.MaxValue より大。</exception>
        public static long? ToNullableInt64(ulong? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 16 ビット符号なし整数の値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 16 ビット符号なし整数。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        public static long? ToNullableInt64(ushort? value)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }

        /// <summary>
        /// 指定したカルチャに固有の書式情報を使用して、指定したオブジェクトの値を 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">System.IConvertible インターフェイスを実装するオブジェクト。</param>
        /// <param name="provider">カルチャ固有の書式情報を提供するオブジェクト。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        /// <exception cref="System.FormatException">value の形式が適切ではありません。</exception>
        /// <exception cref="System.InvalidCastException">value が System.IConvertible を実装していません。</exception>
        /// <exception cref="System.OverflowException">value が System.Int64.MinValue 未満の数値か、System.Int64.MaxValue を超える数値を表しています。</exception>
        public static long? ToNullableInt64(object value, IFormatProvider provider)
        {
            long? result = null;

            if ((null != value) && (DBNull.Value != value))
            {
                result = Convert.ToInt64(value, provider);
            }

            return result;
        }

        /// <summary>
        /// 指定したカルチャに固有の書式情報を使用して、指定した数値の文字列形式を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する数値を含んだ文字列。</param>
        /// <param name="provider">カルチャ固有の書式情報を提供するオブジェクト。</param>
        /// <returns>value の数値と等価の 64 ビット符号付き整数。</returns>
        /// <exception cref="System.FormatException">value の構成が、省略可能な符号と、それに続く 0 から 9 までの一連の数字ではありません。</exception>
        /// <exception cref="System.OverflowException">value が System.Int64.MinValue 未満の数値か、System.Int64.MaxValue を超える数値を表しています。</exception>
        public static long? ToNullableInt64(string value, IFormatProvider provider)
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value, provider);
            }

            return result;
        }

        /// <summary>
        /// 指定した 列挙型のメンバー値を等価の 64 ビット符号付き整数に変換します。
        /// </summary>
        /// <param name="value">変換する 列挙型のメンバー値。</param>
        /// <returns>value と等価の 64 ビット符号付き整数。</returns>
        /// <exception cref="System.OverflowException">value が System.Int64.MaxValue より大きい値か、System.Int64.MinValue より小さい値です。</exception>
        public static long? ToNullableInt64<EnumType>(EnumType? value)
            where EnumType : struct
        {
            long? result = null;

            if (null != value)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }
        #endregion

        #region ToNullableEnumメソッド
        /// <summary>
        /// このメソッドを呼び出すと、必ず System.InvalidCastException がスローされます。
        /// </summary>
        /// <returns>この変換はサポートされていません。値は返されません。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(bool? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            throw new InvalidCastException("この変換はサポートされていません。");
        }

        /// <summary>
        /// 指定した 8 ビット符号なし整数の値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する 8 ビット符号なし整数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(byte? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// このメソッドを呼び出すと、必ず System.InvalidCastException がスローされます。
        /// </summary>
        /// <param name="value">変換する Unicode 文字。</param>
        /// <returns>この変換はサポートされていません。値は返されません。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(char? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            throw new InvalidCastException("この変換はサポートされていません。");
        }

        /// <summary>
        /// このメソッドを呼び出すと、必ず System.InvalidCastException がスローされます。
        /// </summary>
        /// <param name="value">変換する日時の値。</param>
        /// <returns>この変換はサポートされていません。値は返されません。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(DateTime? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            throw new InvalidCastException("この変換はサポートされていません。");
        }

        /// <summary>
        /// 指定した 10 進数値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する数値。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(decimal? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した倍精度浮動小数点数値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する倍精度浮動小数点数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(double? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した単精度浮動小数点数値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する単精度浮動小数点数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(float? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 32 ビット符号付き整数の値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する 32 ビット符号付き整数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(int? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 64 ビット符号付き整数の値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する 64 ビット符号付き整数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(long? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定したオブジェクトの値を 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">System.IConvertible インターフェイスを実装するオブジェクトか、または null。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        /// <exception cref="System.FormatException">value の形式が適切ではありません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(object value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if ((null != value) && (DBNull.Value != value))
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 8 ビット符号付き整数の値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する 8 ビット符号付き整数。</param>
        /// <returns>value と等価の 8 ビット符号付き整数。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(sbyte? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 16 ビット符号付き整数の値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する 16 ビット符号付き整数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(short? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した数値の文字列形式を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する数値を含んだ文字列。</param>
        /// <returns>value の数値と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        /// <exception cref="System.FormatException">value の構成が、省略可能な符号と、それに続く 0 から 9 までの一連の数字ではありません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(string value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 32 ビット符号なし整数の値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する 32 ビット符号なし整数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(uint? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 64 ビット符号なし整数の値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する 64 ビット符号なし整数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(ulong? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定した 16 ビット符号なし整数の値を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する 16 ビット符号なし整数。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(ushort? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(value);
            }

            return result;
        }

        /// <summary>
        /// 指定したカルチャに固有の書式情報を使用して、指定したオブジェクトの値を 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">System.IConvertible インターフェイスを実装するオブジェクト。</param>
        /// <param name="provider">カルチャ固有の書式情報を提供するオブジェクト。</param>
        /// <returns>value と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        /// <exception cref="System.FormatException">value の形式が適切ではありません。</exception>
        /// <exception cref="System.InvalidCastException">value が System.IConvertible を実装していません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(object value, IFormatProvider provider)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if ((null != value) && (DBNull.Value != value))
            {
                result = Enumeration.Parse<EnumType>(Convert.ToInt32(value, provider));
            }

            return result;
        }

        /// <summary>
        /// 指定したカルチャに固有の書式情報を使用して、指定した数値の文字列形式を等価の 列挙型のメンバー値に変換します。
        /// </summary>
        /// <param name="value">変換する数値を含んだ文字列。</param>
        /// <param name="provider">カルチャ固有の書式情報を提供するオブジェクト。</param>
        /// <returns>value の数値と等価の 列挙型のメンバー値。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        /// <exception cref="System.FormatException">value の構成が、省略可能な符号と、それに続く 0 から 9 までの一連の数字ではありません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(string value, IFormatProvider provider)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            EnumType? result = null;

            if (null != value)
            {
                result = Enumeration.Parse<EnumType>(Convert.ToInt32(value, provider));
            }

            return result;
        }

        /// <summary>
        /// 指定した列挙型のメンバー値が返されます。実際の変換は行われません。
        /// </summary>
        /// <param name="value">返される列挙型のメンバー値。</param>
        /// <returns>value は変更されずに返されます。</returns>
        /// <exception cref="System.NotSupportedException">指定された型はこのメソッドでサポートされていません。</exception>
        public static EnumType? ToNullableEnum<EnumType>(EnumType? value)
            where EnumType : struct
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            return value;
        }
        #endregion
        #endregion
    }
}
