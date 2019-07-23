using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main.Gacha {

    /// <summary> 各レアリティの排出確率表 </summary>
    public interface IOdds {
        /// <value> レアリティの排出確率(読み取り専用) </value>
        IProb this[Rarity r] { get; }
        /// <summary> 確率表に基づきレアリティを乱択 </summary>
        Rarity Roulette();
    }

    /// <summary> 各レアリティの排出確率表 </summary>
    public abstract class AbsOdds<ProbImpl> : IOdds where ProbImpl : IProb {
        #region field
        protected IReadOnlyDictionary<Rarity, ProbImpl> Probs { get; set; }
        #endregion

        #region getter
        /// <value> レアリティの排出確率(読み取り専用) </value>
        public IProb this[Rarity r] => Probs[r];
        #endregion

        #region public
        /// <summary> 確率表に基づきレアリティを乱択 </summary>
        public abstract Rarity Roulette();
        #endregion

        #region override
        public override string ToString() {
            return Probs
                .Select(p => $"[{p.Key}:{p.Value}]")
                .Aggregate((a, b) => a + b);
        }
        #endregion
    }

    /// <summary> 各レアリティの排出確率表 </summary>
    public class OddsInt6 : AbsOdds<ProbInt6> {
        #region mono
        public OddsInt6(IReadOnlyDictionary<Rarity, ProbInt6> probs) {
            // 確率の合計が1でないなら警告
            var sum = ProbInt6.Sum(probs.Values);
            if (!sum.Is1) { du.Test.LLog.Debug.LogR($"Sum == {sum} != 1.0"); }

            Probs = probs;
        }
        /// <param>
        /// - レアリティ低い方から順に
        /// - accS1-5 ∈ [0, 100000]
        /// </param>
        public OddsInt6(int accS1, int accS2, int accS3, int accS4, int accS5)
            : this (new Dictionary<Rarity, ProbInt6>{
                { Rarity.S1, new ProbInt6(accS1) },
                { Rarity.S2, new ProbInt6(accS2) },
                { Rarity.S3, new ProbInt6(accS3) },
                { Rarity.S4, new ProbInt6(accS4) },
                { Rarity.S5, new ProbInt6(accS5) },
            })
        {}
        #endregion

        #region public
        /// <summary> 確率表に基づきレアリティを乱択 </summary>
        public override Rarity Roulette() {
            var value = new ProbInt6(Random.Range(1, ProbInt6.AccurateMax));

            du.Test.LLog.Debug.Log($"value = {value}({this})");

            foreach (var prob in Probs) {
                if (value <= prob.Value) { return prob.Key; }
                value -= prob.Value;
            }
            // ここには到達し得ないはず
            return Rarity.None;
        }
        #endregion

    }

#if false

    /// <summary> 各レアリティの排出確率表 </summary>
    public class Odds : IOdds {
        #region field
        IReadOnlyDictionary<Rarity, IProb> Probs { get; }
        #endregion

        #region mono
        public Odds(IReadOnlyDictionary<Rarity, IProb> probs) {
            // 確率の合計が1でないなら警告
            IProb sum = probs.Sum(p => p.Value.Accurate);
            if (!sum.Is1) { Debug.LogAssertion($"Sum == {sum} != 1.0"); }

            Probs = probs;
        }
        /// <param>
        /// - レアリティ低い方から順に
        /// - 全て確率の100000倍の整数で
        /// </param>
        public Odds(int accS1, int accS2, int accS3, int accS4, int accS5)
            : this (new Dictionary<Rarity, IProb>{
                { Rarity.S1, new ProbInt6(accS1) },
                { Rarity.S2, new ProbInt6(accS2) },
                { Rarity.S3, new ProbInt6(accS3) },
                { Rarity.S4, new ProbInt6(accS4) },
                { Rarity.S5, new ProbInt6(accS5) },
            })
        {}
        #endregion

        #region getter
        /// <value> レアリティの排出確率(読み取り専用) </value>
        public IProb this[Rarity r] => Probs[r];
        #endregion

        #region public
        /// <summary> 確率表に基づきレアリティを乱択 </summary>
        public Rarity Roulette() {
            int value = Random.Range(1, ProbInt6.AccurateMax);

            string log = "|";
            foreach (var prob in Probs.Values) {
                log += prob;
            }
            du.Test.LLog.Debug.Log($"value = {value}({this})");

            foreach (var prob in Probs) {
                if (value <= prob.Value.Accurate) { return prob.Key; }
                value -= prob.Value.Accurate;
            }
            // ここには到達し得ないはず
            return Rarity.None;
        }
        #endregion

        #region override
        public override string ToString() {
            var sum = "";
            foreach (var p in Probs) {
                sum += $"[{p.Key}:{p.Value}]";
            }
            return sum;
        }
        #endregion
    }
#endif

}
