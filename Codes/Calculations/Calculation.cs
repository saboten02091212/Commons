// ******************************************************************
// Calculation.cs ： 計算クラス
// 作成日　：2014/06/27
// 更新履歴：2014/06/27 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2014
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Mizuochi.Commons.Codes.Calculations
{
    /// <summary>
    /// 計算式を処理するクラスです。
    /// </summary>
    public class Calculation
    {
        #region フィールド
        /// <summary>
        /// 計算式
        /// </summary>
        private string formula;
        /// <summary>
        /// 計算結果
        /// </summary>
        private decimal value;
        /// <summary>
        /// 第１計算式
        /// </summary>
        private Calculation firstCalculation = null;
        /// <summary>
        /// 第２計算式
        /// </summary>
        private Calculation secondCalculation = null;
        /// <summary>
        /// 第１、２計算式の演算種別
        /// </summary>
        private OperationType operation;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="formula">計算式</param>
        public Calculation(string formula)
        {
            // 計算式を設定
            this.formula = formula;

            // 計算式を解析
            this.Parse(this.formula.Replace(" ", ""));

            // 単一値でなく、計算する必要があるか判定
            if (null != this.firstCalculation)
            {
                this.Calculate();
            }
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 計算式を取得します。
        /// </summary>
        public string Formula
        {
            get { return formula; }
        }

        /// <summary>
        /// 計算結果を取得します。
        /// </summary>
        public decimal Value
        {
            get { return this.value; }
        }
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// 指定された計算式を解析します。
        /// </summary>
        /// <param name="formula">計算式</param>
        private void Parse(string formula)
        {
            // ダミーループ
            // 演算子の優先が低い順に第１計算式、第２計算式、演算種別を設定
            for (; ; )
            {
                // 最初がマイナス符号
                if ('-' == formula[0])
                {
                    this.FirstMinusProcess(formula);
                    break;
                }

                // 加算演算子検索
                if (true == this.SearchOperation(formula, OperationType.Addition))
                {
                    break;
                }

                // 減算演算子検索
                if (true == this.SearchOperation(formula, OperationType.Subtraction))
                {
                    break;
                }

                // 剰余演算子検索
                if (true == this.SearchOperation(formula, OperationType.Remainder))
                {
                    break;
                }

                // 積算演算子検索
                if (true == this.SearchOperation(formula, OperationType.Multiplication))
                {
                    break;
                }

                // 除算演算子検索
                if (true == this.SearchOperation(formula, OperationType.Division))
                {
                    break;
                }

                // 最初が括弧
                if (true == this.IsStartParen(formula[0]))
                {
                    this.FirstParenProcess(formula);
                    break;
                }

                // 括弧演算子検索
                if (true == this.SearchParenthesis(formula))
                {
                    break;
                }

                // 演算子が見つからない場合、値を設定
                this.SetValue(formula);
                break;
            }

            return;
        }

        /// <summary>
        /// 最初の文字がマイナス符号である時の処理を行います。
        /// </summary>
        /// <param name="formula">計算式</param>
        private void FirstMinusProcess(string formula)
        {
            // 単項演算子-X を 0-X として処理
            this.firstCalculation = new Calculation("0");
            this.secondCalculation = new Calculation(formula.Substring(1));
            this.operation = OperationType.Subtraction;

            return;
        }

        /// <summary>
        /// 最初の文字が括弧である時の処理を行います。
        /// </summary>
        /// <param name="formula">計算式</param>
        private void FirstParenProcess(string formula)
        {
            // 最初の文字が括弧か判定
            if (false == this.IsStartParen(formula[0]))
            {
                throw new Exception();
            }

            // 計算式を1文字ずつチェック
            int parenLevel = 0;
            for (int index = 0; index < formula.Length; ++index)
            {
                char c = formula[index];

                // 括弧か判定
                if (true == this.IsStartParen(c))
                {
                    ++parenLevel;
                }
                else if (true == this.IsEndParen(c))
                {
                    --parenLevel;
                    if (0 > parenLevel)
                    {
                        throw new ArgumentException("括弧の対応が不正な計算式です。", formula);
                    }
                }

                // 括弧が終わった
                if (0 == parenLevel)
                {
                    // 最後の文字か判定
                    if ((formula.Length - 1) == index)
                    {
                        // 括弧しか存在していないか判定
                        if (2 > index)
                        {
                            throw new ArgumentException("括弧の中に値が存在しない不正な計算式です。", formula);
                        }

                        // 括弧の中身を第１計算式に設定し、第２計算式は設定しない
                        this.firstCalculation = new Calculation(formula.Substring(1, formula.Length - 2));
                        this.secondCalculation = null;
                        this.operation = OperationType.None;

                        break;
                    }
                    else
                    {
                        // 終わりの括弧を境に前後を第１計算式、第２計算式として設定
                        this.firstCalculation = new Calculation(formula.Substring(0, index + 1));
                        this.secondCalculation = new Calculation(formula.Substring(index + 1));
                        this.operation = OperationType.Multiplication;

                        break;
                    }
                }
            }

            // 括弧閉じてない
            if (0 < parenLevel)
            {
                throw new ArgumentException("括弧が閉じられていない不正な計算式です。", formula);
            }

            return;
        }

        /// <summary>
        /// 計算式の中から、指定した演算種別を元に第１計算式、第２計算式とすべきか判定し、そうすべきなら設定します。
        /// </summary>
        /// <param name="formula">計算式</param>
        /// <param name="operationType">演算種別</param>
        /// <returns>指定した演算種別を元に第１計算式、第２計算式とすべきなら、true。そうでないなら、falseを返します。</returns>
        private bool SearchOperation(string formula, OperationType operationType)
        {
            // 演算種別が妥当か判定
            if (false == Enum.IsDefined(typeof(OperationType), operationType))
            {
                throw new InvalidEnumArgumentException("存在しない演算種別が指定されました。");
            }

            // 演算子の文字を取得
            char operationChar =
                (OperationType.Addition == operationType)       ? '+' :
                (OperationType.Subtraction == operationType)    ? '-' :
                (OperationType.Multiplication == operationType) ? '*' :
                (OperationType.Division == operationType)       ? '/' :
                (OperationType.Remainder == operationType)      ? '%' :
                '\0';

            // 計算式を1文字ずつチェック
            bool result = false;
            int parenLevel = 0;
            for (int index = 0; index < formula.Length; ++index)
            {
                char c = formula[index];

                // 括弧か判定
                if (true == this.IsStartParen(c))
                {
                    ++parenLevel;
                    continue;
                }
                else if (true == this.IsEndParen(c))
                {
                    --parenLevel;
                    if (0 > parenLevel)
                    {
                        throw new ArgumentException("括弧の対応が不正な計算式です。", formula);
                    }

                    continue;
                }

                // 括弧内であれば無視
                if (0 < parenLevel)
                {
                    continue;
                }

                // 四則演算子か判定
                result = (operationChar == c) ? true : false;

                // 指定した演算子か判定
                if (true == result)
                {
                    // 演算子が先頭の場合
                    if (0 == index)
                    {
                        throw new ArgumentException("演算子の位置が不正な計算式です。", formula);
                    }

                    // 演算子を境に前後を第１計算式、第２計算式として設定
                    this.firstCalculation = new Calculation(formula.Substring(0, index));
                    this.secondCalculation = new Calculation(formula.Substring(index + 1));
                    this.operation = operationType;

                    result = true;
                    break;
                }
            }

            // 括弧閉じてない
            if (0 < parenLevel)
            {
                throw new ArgumentException("括弧が閉じられていない不正な計算式です。", formula);
            }

            return result;
        }

        /// <summary>
        /// 計算式の中から、括弧演算子を検索し、存在していれば第１計算式、第２計算式を設定します。
        /// </summary>
        /// <param name="formula">計算式</param>
        /// <returns>括弧が存在していれば、true。そうでないなら、falseを返します。</returns>
        private bool SearchParenthesis(string formula)
        {
            bool result = false;
            for (int index = 0; index < formula.Length; ++index)
            {
                char c = formula[index];

                // 括弧か判定
                if (true == this.IsStartParen(c))
                {
                    if (0 == index)
                    {
                        throw new ArgumentException("最初の文字に括弧が存在する計算式です。", formula);
                    }

                    // 始まりの括弧を境に前後を第１計算式、第２計算式として設定
                    this.firstCalculation = new Calculation(formula.Substring(0, index));
                    this.secondCalculation = new Calculation(formula.Substring(index));
                    this.operation = OperationType.Multiplication;

                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 計算式から値に設定します。
        /// </summary>
        /// <param name="formula"></param>
        private void SetValue(string formula)
        {
            // 文字列から数値に変換
            decimal val;
            if (false == Decimal.TryParse(formula, out val))
            {
                throw new Exception();
            }

            this.value = val;

            return;
        }

        /// <summary>
        /// 第１計算式、第２計算式を演算種別で計算します。
        /// </summary>
        private void Calculate()
        {
            switch (this.operation)
            {
                case OperationType.None:
                    this.value = this.firstCalculation.Value;
                    break;
                case OperationType.Addition:
                    this.value = this.firstCalculation.Value + this.secondCalculation.Value;
                    break;
                case OperationType.Subtraction:
                    this.value = this.firstCalculation.Value - this.secondCalculation.Value;
                    break;
                case OperationType.Multiplication:
                    this.value = this.firstCalculation.Value * this.secondCalculation.Value;
                    break;
                case OperationType.Division:
                    this.value = this.firstCalculation.Value / this.secondCalculation.Value;
                    break;
                case OperationType.Remainder:
                    this.value = this.firstCalculation.Value % this.secondCalculation.Value;
                    break;
            }

            return;
        }

        /// <summary>
        /// 指定された文字が数字か判定します。
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>数字なら、true。数字でなければ、falseを返します。</returns>
        private bool IsNumeric(char c)
        {
            int val;
            bool result = Int32.TryParse(c.ToString(), out val);
            return result;
        }

        /// <summary>
        /// 指定された文字が演算子か判定します。
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>演算子なら、true。演算子でなければ、falseを返します。</returns>
        private bool IsOperation(char c)
        {
            bool result = false;
            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字が始まりの括弧か判定します。
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>始まりの括弧なら、true。始まりの括弧でなければ、falseを返します。</returns>
        private bool IsStartParen(char c)
        {
            bool result = false;
            switch (c)
            {
                case '(':
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 指定された文字が終わりの括弧か判定します。
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>終わりの括弧なら、true。終わりの括弧でなければ、falseを返します。</returns>
        private bool IsEndParen(char c)
        {
            bool result = false;
            switch (c)
            {
                case ')':
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }
        #endregion

        #region 内部定義
        /// <summary>
        /// 演算種別
        /// </summary>
        private enum OperationType
        {
            /// <summary>
            /// なし
            /// </summary>
            None,
            /// <summary>
            /// 加算
            /// </summary>
            Addition,
            /// <summary>
            /// 減算
            /// </summary>
            Subtraction,
            /// <summary>
            /// 積算
            /// </summary>
            Multiplication,
            /// <summary>
            /// 除算
            /// </summary>
            Division,
            /// <summary>
            /// 剰余
            /// </summary>
            Remainder,
        }
        #endregion
    }
}