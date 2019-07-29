using System.Collections.Generic;
using System.Linq;

namespace Main.Gacha {

    /// <summary>
    /// 確率
    /// - 実体をdecimalで持つことで誤差の発生を防ぐ
    /// </summary>
    public class ProbDeci : IProb {
        #region const
        /// <value> 桁数(精度) </value>
        public static int AccurateMax => 100000;
        #endregion

        #region field
        /// <value> 確率値を精度6桁のintで返す </value>
        /// <value> 誤差防止の為実装上ではintで管理 </value>
        /// <returns> e.g. 32% -> 32000 </returns>
        private decimal Accurate { get; set; }
        #endregion

        #region getter
        /// <value> 確率"1"か </value>
        public bool Is1 => Accurate == decimal.One;
        /// <value> 確率"0"か </value>
        public bool Is0 => Accurate == decimal.Zero;
        /// <value> 確率値を[0.0, 1.0]で近似 </value>
        public float Approximation01 => decimal.ToSingle(Accurate);
        #endregion

        #region ctor
        /// <param name="value"> 確率のint∈[0.0, 1.0] </param>
        public ProbDeci(decimal value) {
            Accurate = value;
        }
        #endregion

        #region public
        /// <summary> 確率値に基づき当落を乱択 </summary>
        public bool Roulette() {
            var prob = Random();
            du.Test.LLog.Debug.LogR($"{prob} in {this} ({prob <= this}))");
            return prob <= this;
        }
        #endregion

        #region static
        /// <value> 確率"1" </value>
        public static ProbDeci One => new ProbDeci(decimal.One);
        /// <value> 確率"0" </value>
        public static ProbDeci Zero => new ProbDeci(decimal.Zero);
        #endregion

        #region override
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) { return false; }
            return Accurate == ((ProbDeci)obj).Accurate;
        }
        public override int GetHashCode() => Accurate.GetHashCode();
        public override string ToString() => Accurate.ToString();
        #endregion

        #region operator
        public static bool operator<  (ProbDeci a, ProbDeci b) => a.Accurate < b.Accurate;
        public static bool operator>  (ProbDeci a, ProbDeci b) => a.Accurate > b.Accurate;
        public static bool operator== (ProbDeci a, ProbDeci b) => a.Accurate == b.Accurate;
        public static bool operator!= (ProbDeci a, ProbDeci b) => a.Accurate != b.Accurate;
        public static bool operator<= (ProbDeci a, ProbDeci b) => a.Accurate <= b.Accurate;
        public static bool operator>= (ProbDeci a, ProbDeci b) => a.Accurate >= b.Accurate;

        /// <summary> 1を上回ったら1に補正 </summary>
        public static ProbDeci operator+  (ProbDeci a, ProbDeci b)
            => new ProbDeci(a.Accurate + b.Accurate); // コンストラクタで補正されるので普通に突っ込む
        /// <summary> 0を下回ったら0に補正 </summary>
        public static ProbDeci operator-  (ProbDeci a, ProbDeci b)
            => new ProbDeci(a.Accurate - b.Accurate);
        #endregion

        #region static
        /// <summary> ランダムに生成 </summary>
        public static ProbDeci Random() {
            return new ProbDeci(((decimal)UnityEngine.Random.Range(0, AccurateMax)) / (decimal)AccurateMax);
        }
        /// <param name="str"> nullや空文字列の場合0とみなす </param>
        public static ProbDeci FromString(string str) {
            if (str is null || str.Length == 0) {
                str = "0";
            }
            return new ProbDeci(decimal.Parse(str));
        }

        public static ProbDeci Sum(IEnumerable<ProbDeci> probs) {
            return probs.Aggregate((a, b) => a + b);
        }
        #endregion
    }

}
