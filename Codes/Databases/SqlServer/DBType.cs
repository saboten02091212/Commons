// ******************************************************************
// DBType.cs ： SQLServer型クラス
// 作成日　：2013/08/23
// 更新履歴：2013/08/23 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mizuochi.Commons.Codes.Calculations;
using Mizuochi.Commons.Codes.ClassExtensions;

namespace Mizuochi.Commons.Codes.Databases.SqlServer
{
    /// <summary>
    /// SQLServerの型を表すクラス
    /// </summary>
    public class DBType
    {
        #region フィールド
        /// <summary>
        /// 型ID
        /// </summary>
        private EnumDBTypeId dbTypeId;
        /// <summary>
        /// 有効桁数
        /// </summary>
        private int precision;
        /// <summary>
        /// 小数点以下桁数
        /// </summary>
        private int scale;
        #endregion

        #region プロパティ
        /// <summary>
        /// 型ID
        /// </summary>
        public EnumDBTypeId DBTypeId
        {
            get
            {
                return dbTypeId;
            }
        }

        /// <summary>
        /// 有効桁数
        /// </summary>
        public int Precision
        {
            get
            {
                return precision;
            }
        }

        /// <summary>
        /// 小数点以下桁数
        /// </summary>
        public int Scale
        {
            get
            {
                return scale;
            }
        }

        /// <summary>
        /// C#での型
        /// </summary>
        public Type CSharpType
        {
            get
            {
                return DBTypeInfoAttribute.GetCSharpType(this.dbTypeId);
            }
        }

        /// <summary>
        /// 型名
        /// </summary>
        public string Name
        {
            get
            {
                return DBTypeInfoAttribute.GetName(this.dbTypeId);
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタを元にインスタンスを生成します。
        /// </summary>
        /// <param name="dbTypeId">型ID</param>
        /// <param name="precision">有効桁数</param>
        /// <param name="scale">小数点以下桁数</param>
        public DBType(EnumDBTypeId dbTypeId, int precision = 0, int scale = 0)
        {
            this.dbTypeId = dbTypeId;
            this.precision = precision;
            this.scale = scale;
        }

        /// <summary>
        /// 指定されたパラメタを元にインスタンスを生成します。
        /// </summary>
        /// <param name="dbTypeId">型ID</param>
        /// <param name="precision">有効桁数</param>
        /// <param name="scale">小数点以下桁数</param>
        public DBType(int dbTypeId, int precision = 0, int scale = 0)
            : this((EnumDBTypeId)dbTypeId, precision, scale)
        {
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 指定された値を型に合わせて変換します。
        /// </summary>
        /// <param name="obj">変換前の値</param>
        /// <returns>変換後の値</returns>
        public object ConvertValue(object obj)
        {
            // 引数nullチェック
            if (null == obj)
            {
                return null;
            }

            // 型変換
            object value = Convert.ChangeType(obj, this.CSharpType);
            return value;
        }

        /// <summary>
        /// 指定された値を型に合わせた文字列に変換します。
        /// </summary>
        /// <param name="obj">変換前の値</param>
        /// <returns>変換した文字列</returns>
        public string ConvertString(object obj)
        {
            // 引数nullチェック
            if (null == obj)
            {
                return null;
            }

            // 型変換
            object value = Convert.ChangeType(obj, this.CSharpType);

            string convertString;
            switch (this.dbTypeId)
            {
                // decimal型
                case EnumDBTypeId.Decimal:
                case EnumDBTypeId.Numeric:
                    convertString = Account.ConvertString(numericValue: value, decimals: this.scale);
                    break;

                // 日付型
                case EnumDBTypeId.Date:
                    convertString = ((DateTime)value).ToShortDateString();
                    break;

                case EnumDBTypeId.Time:
                    convertString = ((DateTime)value).ToLongTimeString();
                    break;

                case EnumDBTypeId.Datetime:
                case EnumDBTypeId.Datetime2:
                    convertString = ((DateTime)value).ToString();
                    break;

                case EnumDBTypeId.Bit:
                case EnumDBTypeId.Tinyint:
                case EnumDBTypeId.Smallint:
                case EnumDBTypeId.Int:
                case EnumDBTypeId.Smalldatetime:
                case EnumDBTypeId.Real:
                case EnumDBTypeId.Money:
                case EnumDBTypeId.Float:
                case EnumDBTypeId.Smallmoney:
                case EnumDBTypeId.Bigint:
                case EnumDBTypeId.Varchar:
                case EnumDBTypeId.Char:
                case EnumDBTypeId.Nvarchar:
                case EnumDBTypeId.Nchar:
                default:
                    convertString = value.ToString();
                    break;
            }

            return convertString;
        }
        #endregion

        #region 内部型
        /// <summary>
        /// 型ID
        /// </summary>
        public enum EnumDBTypeId
        {
            /// <summary>
            /// 日付
            /// </summary>
            [DBTypeInfo("Date", typeof(DateTime))]
            Date = 40,
            /// <summary>
            /// 時刻
            /// </summary>
            [DBTypeInfo("Time", typeof(DateTime))]
            Time = 41,
            /// <summary>
            /// 日付と有効桁数が指定されている時刻
            /// </summary>
            [DBTypeInfo("Datetime2", typeof(DateTime))]
            Datetime2 = 42,
            /// <summary>
            /// １バイト数値
            /// </summary>
            [DBTypeInfo("Tinyint", typeof(byte))]
            Tinyint = 48,
            /// <summary>
            /// ２バイト数値
            /// </summary>
            [DBTypeInfo("Smallint", typeof(short))]
            Smallint = 52,
            /// <summary>
            /// ４バイト数値
            /// </summary>
            [DBTypeInfo("Int", typeof(int))]
            Int = 56,
            /// <summary>
            /// 範囲の狭い日付と時刻
            /// </summary>
            [DBTypeInfo("Smalldatetime", typeof(DateTime))]
            Smalldatetime = 58,
            /// <summary>
            /// ４バイト浮動小数点数値
            /// </summary>
            [DBTypeInfo("Real", typeof(float))]
            Real = 59,
            /// <summary>
            /// ８バイト通貨
            /// </summary>
            [DBTypeInfo("Money", typeof(long))]
            Money = 60,
            /// <summary>
            /// 日付と時刻
            /// </summary>
            [DBTypeInfo("Datetime", typeof(DateTime))]
            Datetime = 61,
            /// <summary>
            /// 有効桁数が指定されている浮動小数点数値
            /// </summary>
            [DBTypeInfo("Float", typeof(double))]
            Float = 62,
            /// <summary>
            /// １ビット数値
            /// </summary>
            [DBTypeInfo("Bit", typeof(bool))]
            Bit = 104,
            /// <summary>
            /// 固定長の有効桁数と小数点以下桁数を持つ数値
            /// </summary>
            [DBTypeInfo("Decimal", typeof(decimal))]
            Decimal = 106,
            /// <summary>
            /// 固定長の有効桁数と小数点以下桁数を持つ数値
            /// </summary>
            [DBTypeInfo("Numeric", typeof(decimal))]
            Numeric = 108,
            /// <summary>
            /// ４バイト通貨
            /// </summary>
            [DBTypeInfo("Smallmoney", typeof(int))]
            Smallmoney = 122,
            /// <summary>
            /// ８バイト数値
            /// </summary>
            [DBTypeInfo("Bigint", typeof(long))]
            Bigint = 127,
            /// <summary>
            /// 可変長文字列
            /// </summary>
            [DBTypeInfo("Varchar", typeof(string))]
            Varchar = 167,
            /// <summary>
            /// 固定長文字列
            /// </summary>
            [DBTypeInfo("Char", typeof(string))]
            Char = 175,
            /// <summary>
            /// Unicode可変長文字列
            /// </summary>
            [DBTypeInfo("Nvarchar", typeof(string))]
            Nvarchar = 231,
            /// <summary>
            /// Unicode固定長文字列
            /// </summary>
            [DBTypeInfo("Nchar", typeof(string))]
            Nchar = 239,
        }

        /// <summary>
        /// DBの型に関する情報を付与するカスタム属性クラスです。
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
        private class DBTypeInfoAttribute : System.Attribute
        {
            #region フィールド
            /// <summary>
            /// 型名
            /// </summary>
            private string name;

            /// <summary>
            /// C#の型
            /// </summary>
            private Type cSharpType;
            #endregion

            #region プロパティ
            /// <summary>
            /// 型名
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// C#の型
            /// </summary>
            public Type CSharpType
            {
                get
                {
                    return cSharpType;
                }
            }
            #endregion
            
            #region コンストラクタ
            /// <summary>
            /// 指定されたパラメタでインスタンスを生成します。
            /// </summary>
            /// <param name="name">型名</param>
            /// <param name="cSharpType">C#の型</param>
            public DBTypeInfoAttribute(string name, Type cSharpType)
            {
                this.name = name;
                this.cSharpType = cSharpType;
            }
            #endregion

            #region パブリックメソッド
            /// <summary>
            /// 指定されたUserTypeId列挙型からUserTypeTypeInfoAttributeを取得します。
            /// </summary>
            /// <param name="dbTypeId">UserTypeId列挙型</param>
            /// <returns>UserTypeTypeInfoAttribute</returns>
            public static DBTypeInfoAttribute GetAttributes(EnumDBTypeId dbTypeId)
            {
                // フィールドの属性取得
                FieldInfo fInfo = typeof(EnumDBTypeId).GetField(dbTypeId.ToString());
                object[] attributes = fInfo.GetCustomAttributes(typeof(DBTypeInfoAttribute), false);

                // 属性が無ければエラー
                if (1 != attributes.Length)
                {
                    throw new ArgumentException("DBTypeInfo属性が定義されていません。", "userTypeId");
                }

                return (DBTypeInfoAttribute)attributes.Single();
            }

            /// <summary>
            /// 指定されたメンバの型名を取得します。
            /// </summary>
            /// <param name="dbTypeId">UserTypeId列挙型</param>
            /// <returns>型名</returns>
            public static string GetName(EnumDBTypeId dbTypeId)
            {
                var attribute = DBTypeInfoAttribute.GetAttributes(dbTypeId);
                return attribute.name;
            }

            /// <summary>
            /// 指定されたメンバのC#での型を取得します。
            /// </summary>
            /// <param name="dbTypeId">UserTypeId列挙型</param>
            /// <returns>C#での型</returns>
            public static Type GetCSharpType(EnumDBTypeId dbTypeId)
            {
                var attribute = DBTypeInfoAttribute.GetAttributes(dbTypeId);
                return attribute.cSharpType;
            }
            #endregion
        }
        #endregion
    }
}
