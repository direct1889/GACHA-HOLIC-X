using System.Collections.Generic;
using UnityEngine;

namespace Main.Gacha {

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

}
