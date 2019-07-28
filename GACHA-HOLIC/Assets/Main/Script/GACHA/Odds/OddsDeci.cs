using System.Collections.Generic;
using UnityEngine;

namespace Main.Gacha {

    /// <summary> 各レアリティの排出確率表 </summary>
    public class OddsDeci : AbsOdds<ProbDeci> {
        #region mono
        public OddsDeci(IReadOnlyDictionary<Rarity, ProbDeci> probs) {
            // 確率の合計が1でないなら警告
            var sum = ProbDeci.Sum(probs.Values);
            if (!sum.Is1) { du.Test.LLog.Debug.LogR($"Sum == {sum} != 1.0"); }

            Probs = probs;
        }
        /// <param>
        /// - レアリティ低い方から順に
        /// - accS1-5 ∈ [0, 100000]
        /// </param>
        public OddsDeci(decimal accS1, decimal accS2, decimal accS3, decimal accS4, decimal accS5)
            : this (new Dictionary<Rarity, ProbDeci>{
                { Rarity.S1, new ProbDeci(accS1) },
                { Rarity.S2, new ProbDeci(accS2) },
                { Rarity.S3, new ProbDeci(accS3) },
                { Rarity.S4, new ProbDeci(accS4) },
                { Rarity.S5, new ProbDeci(accS5) },
            })
        {}
        #endregion

        #region public
        /// <summary> 確率表に基づきレアリティを乱択 </summary>
        public override Rarity Roulette() {
            var value = new ProbDeci(Random.Range(1, ProbDeci.AccurateMax));

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

}
