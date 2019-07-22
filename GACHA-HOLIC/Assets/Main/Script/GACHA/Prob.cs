using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main {

    namespace Gacha {

        /// <summary>
        /// 確率
        /// - 実体をintで持つことで確率同士の比較を正確に
        /// </summary>
        public struct Prob {
            #region field
            /// <value> 誤差防止の為実装上ではintで管理 </value>
            private int ValueInt { get; }
            /// <value> 桁数(精度) </value>
            private int SignificantFigures { get; }
            #endregion

            #region getter
            /// <value> 確率"1"か </value>
            public bool Is1 => ValueInt >= SignificantFigures;
            /// <value> 確率"0"か </value>
            public bool Is0 => ValueInt <= 0;

            /// <value> 確率値を[0.0, 1.0]で近似 </value>
            public float Approximation01 => Mathf.Clamp01(ValueInt / SignificantFigures);
            /// <value> 確率値を精度5桁のintで返す </value>
            /// <returns> e.g. 32% -> 32000 </returns>
            public int Accurate => ValueInt * (SignificantFiguresMax / SignificantFigures);
            #endregion

            #region ctor
            /// <param name="value"> 確率のint∈[0, 10^arg2] </param>
            /// <param name="numberOfSignificantFigures"> valueの桁数∈[0, 5] </param>
            /// <param> e.g. Prob(234, 3) => 確率0.234 </param>
            /// <param> e.g. Prob(234, 4) => 確率0.0234 </param>
            public Prob(int value, int numberOfSignificantFigures) {
                numberOfSignificantFigures = Mathf.Clamp(numberOfSignificantFigures, 0, numberOfSignificantFiguresMax);
                SignificantFigures = 1;
                for (int i = 0; i < numberOfSignificantFigures; ++i) {
                    SignificantFigures *= 10;
                }
                ValueInt = System.Math.Min(System.Math.Max((int)value, 0), SignificantFigures);
            }
            public Prob(uint value, int numberOfSignificantFigures)
                : this((int)value, numberOfSignificantFigures) {}
            #endregion

            #region public
            /// <summary> 確率値に基づき当落を乱択 </summary>
            public bool Roulette() {
                var value = Random.Range(1, SignificantFigures);
                du.Test.LLog.Debug.LogError($"{value} in {ValueInt} ({value <= ValueInt}))");
                return value <= ValueInt;
            }
            #endregion

            #region static
            /// <summary> 確率同士の桁数を揃える </summary>
            private static (int, int) AlignedFigures(Prob a, Prob b) {
                // 桁数が等しければそのまま
                if (a.SignificantFigures == b.SignificantFigures) {
                    return (a.ValueInt, b.ValueInt);
                }
                // 桁数が違う場合は少ない方を拡張する
                else if (a.SignificantFigures < b.SignificantFigures) {
                    return (a.ValueInt * (b.SignificantFigures / a.SignificantFigures), b.ValueInt);
                }
                else /*æa.Sig > b.Sig */ {
                    return (a.ValueInt, b.ValueInt * (a.SignificantFigures / b.SignificantFigures));
                }
            }
            /// <summary> 確率系列の合計が1になっているか </summary>
            public static bool TotalIsOne(IEnumerable<Prob> probs) {
                return probs.Select(p => p.Accurate).Sum() == SignificantFiguresMax;
            }
            public static bool TotalIsOne(int accurateTotal) {
                return accurateTotal == SignificantFiguresMax;
            }
            public static int AccurateTotal(IEnumerable<Prob> probs) {
                return probs.Select(p => p.Accurate).Sum();
            }

            /// <summary> 桁数(精度)の上限 </summary>
            private static readonly int numberOfSignificantFiguresMax = 5;
            /// <summary> 桁数(精度)の上限 </summary>
            public static int SignificantFiguresMax => 100000;

            /// <value> 確率"1" </value>
            public static Prob One => new Prob(1, 0);
            /// <value> 確率"0" </value>
            public static Prob Zero => new Prob(0, 0);
            #endregion

            #region override
            public override bool Equals(object obj) {
                if (obj == null || GetType() != obj.GetType()) { return false; }
                (var a, var b) = Prob.AlignedFigures(this, (Prob)obj);
                return a == b;
            }
            public override int GetHashCode() => ValueInt * (SignificantFiguresMax / SignificantFigures);
            public override string ToString() => $"{ValueInt}/{SignificantFigures}";
            #endregion

            #region operator
            public static bool operator< (Prob a, Prob b) {
                (var va, var vb) = Prob.AlignedFigures(a, b);
                return va < vb;
            }
            public static bool operator> (Prob a, Prob b) {
                (var va, var vb) = Prob.AlignedFigures(a, b);
                return va > vb;
            }
            public static bool operator== (Prob a, Prob b) {
                (var va, var vb) = Prob.AlignedFigures(a, b);
                return va == vb;
            }
            public static bool operator!= (Prob a, Prob b) {
                (var va, var vb) = Prob.AlignedFigures(a, b);
                return va != vb;
            }
            public static bool operator<= (Prob a, Prob b) {
                (var va, var vb) = Prob.AlignedFigures(a, b);
                return va <= vb;
            }
            public static bool operator>= (Prob a, Prob b) {
                (var va, var vb) = Prob.AlignedFigures(a, b);
                return va >= vb;
            }
            #endregion

            #region static
            public static Prob FromString(string str) {
                if (str is null || str.Length == 0) {
                    str = "0";
                }
                return new Prob(System.Convert.ToUInt32(str), numberOfSignificantFiguresMax);
            }
            #endregion
        }

    }

}
