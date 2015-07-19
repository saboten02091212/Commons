// ******************************************************************
// EnumComboHelper.cs  ：列挙型リスト作成クラス
// 作成日　：2013/02/14
// 更新履歴：2014/05/15 水落　　 空要素を含むリストを生成するメソッドを追加。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mizuochi.Commons.Codes.Types
{
    /// <summary>
    /// 列挙型のメンバをリストに変換する機能を保持するクラスです。
    /// </summary>
    /// <typeparam name="EnumType">列挙型</typeparam>
    public class EnumComboHelper<EnumType>
        where EnumType : struct, IConvertible
    {
        #region コンストラクタ
        /// <summary>
        /// 指定されたパラメタでインスタンスを生成します。
        /// </summary>
        /// <param name="value">列挙型メンバの値</param>
        /// <param name="display">列挙型メンバの名前</param>
        /// <param name="enumerate">列挙型メンバ</param>
        private EnumComboHelper(int value, string display, EnumType enumerate)
        {
            if (false == typeof(EnumType).IsEnum)
            {
                throw new NotSupportedException();
            }

            this.Value = value;
            this.Display = display;
            this.EnumValue = enumerate;
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 列挙型メンバの値を取得します。
        /// </summary>
        public int Value
        {
            get;
            protected set;
        }

        /// <summary>
        /// 列挙型メンバの名前を取得します。
        /// </summary>
        public string Display
        {
            get;
            protected set;
        }

        /// <summary>
        /// 列挙型メンバを取得します。
        /// </summary>
        public EnumType EnumValue
        {
            get;
            protected set;
        }
        #endregion

        #region 静的パブリックメソッド
        /// <summary>
        /// 指定された列挙型のリストを生成します。
        /// </summary>
        /// <returns>列挙型のメンバリスト</returns>
        public static IEnumerable<EnumComboHelper<EnumType>> CreateList()
        {
            var enumList = new List<EnumComboHelper<EnumType>>();
            foreach (EnumType enumValue in typeof(EnumType).GetEnumValues())
            {
                string display;
                try
                {
                    display = EnumDisplayAttribute.GetDisplayValue(enumValue);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                int value = enumValue.ToInt32(null);
                var item = new EnumComboHelper<EnumType>(value, display, enumValue);
                enumList.Add(item);
            }

            return enumList;
        }

        /// <summary>
        /// 指定された列挙型の空要素を含んだリストを生成します。
        /// </summary>
        /// <param name="emptyDisplay">空要素の表示値</param>
        /// <param name="emptyEnum">空要素の列挙型</param>
        /// <returns>列挙型のメンバリスト</returns>
        public static IEnumerable<EnumComboHelper<EnumType>> CreateList(string emptyDisplay, EnumType emptyEnum)
        {
            var enumList = EnumComboHelper<EnumType>.CreateList().ToList();

            int emptyValue = emptyEnum.ToInt32(null);
            var emptyItem = new EnumComboHelper<EnumType>(emptyValue, emptyDisplay, emptyEnum);
            enumList.Insert(0, emptyItem);

            return enumList;
        }
        #endregion
    }
}
