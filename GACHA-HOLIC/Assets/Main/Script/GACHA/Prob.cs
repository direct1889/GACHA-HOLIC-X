using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main {

    namespace Gacha {

        public interface IProb {
            #region getter
            /// <value> 確率"1"か </value>
            bool Is1 { get; }
            /// <value> 確率"0"か </value>
            bool Is0 { get; }
            /// <value> 確率値を[0.0, 1.0]で近似 </value>
            float Approximation01 { get; }
            #endregion

            #region public
            /// <summary> 確率値に基づき当落を乱択 </summary>
            bool Roulette();
            #endregion
        }

        /// <summary>
        /// 確率
        /// - 実体をintで持つことで誤差の発生を防ぐ
        /// </summary>
        public class ProbInt6 : IProb {
            #region const
            /// <value> 桁数(精度) </value>
            public static int AccurateMax => 100000;
            #endregion

            #region field
            /// <value> 確率値を精度6桁のintで返す </value>
            /// <value> 誤差防止の為実装上ではintで管理 </value>
            /// <returns> e.g. 32% -> 32000 </returns>
            private int Accurate { get; set; }
            #endregion

            #region getter
            /// <value> 確率"1"か </value>
            public bool Is1 => Accurate == AccurateMax;
            /// <value> 確率"0"か </value>
            public bool Is0 => Accurate == 0;
            /// <value> 確率値を[0.0, 1.0]で近似 </value>
            public float Approximation01 => Mathf.Clamp01(Accurate / AccurateMax);
            #endregion

            #region ctor
            /// <param name="value"> 確率のint∈[0, 10^arg2] </param>
            /// <param name="NoSF"> valueの桁数∈[0, 5] </param>
            /// <param> e.g. Prob(234, 3) => 確率0.234 </param>
            /// <param> e.g. Prob(234, 4) => 確率0.0234 </param>
            public ProbInt6(int value) {
                Accurate = System.Math.Min(System.Math.Max((int)value, 0), AccurateMax);
            }
            public ProbInt6(uint value)
                : this((int)value) {}
            #endregion

            #region public
            /// <summary> 確率値に基づき当落を乱択 </summary>
            public bool Roulette() {
                var value = Random.Range(1, AccurateMax);
                du.Test.LLog.Debug.LogR($"{value} in {Accurate} ({value <= Accurate}))");
                return value <= Accurate;
            }
            #endregion

            #region static
            /// <value> 確率"1" </value>
            public static ProbInt6 One => new ProbInt6(AccurateMax);
            /// <value> 確率"0" </value>
            public static ProbInt6 Zero => new ProbInt6(0);
            #endregion

            #region override
            public override bool Equals(object obj) {
                if (obj == null || GetType() != obj.GetType()) { return false; }
                return Accurate == ((ProbInt6)obj).Accurate;
            }
            public override int GetHashCode() => Accurate;
            public override string ToString() => Accurate.ToString();
            #endregion

            #region operator
            public static bool operator<  (ProbInt6 a, ProbInt6 b) => a.Accurate < b.Accurate;
            public static bool operator>  (ProbInt6 a, ProbInt6 b) => a.Accurate > b.Accurate;
            public static bool operator== (ProbInt6 a, ProbInt6 b) => a.Accurate == b.Accurate;
            public static bool operator!= (ProbInt6 a, ProbInt6 b) => a.Accurate != b.Accurate;
            public static bool operator<= (ProbInt6 a, ProbInt6 b) => a.Accurate <= b.Accurate;
            public static bool operator>= (ProbInt6 a, ProbInt6 b) => a.Accurate >= b.Accurate;

            public static ProbInt6 operator+  (ProbInt6 a, ProbInt6 b)
                => new ProbInt6(a.Accurate + b.Accurate);
            public static ProbInt6 operator-  (ProbInt6 a, ProbInt6 b)
                => new ProbInt6(System.Math.Max(a.Accurate - b.Accurate, 0));
            #endregion

            #region static
            /// <param name="str"> nullや空文字列の場合0とみなす </param>
            public static ProbInt6 FromString(string str) {
                if (str is null || str.Length == 0) {
                    str = "0";
                }
                return new ProbInt6(System.Convert.ToUInt32(str));
            }

            public static ProbInt6 Sum(IEnumerable<ProbInt6> probs) {
                return probs.Aggregate((a, b) => a + b);
            }
            #endregion
        }

    }

}
