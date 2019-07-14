using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main {

    namespace Gacha {

        /// <summary> 各レアリティの排出確率表 </summary>
        public interface IOdds {
            /// <summary> 確率表に基づきレアリティを乱択 </summary>
            Rarity Roulette();
        }

        /// <summary> 各レアリティの排出確率表 </summary>
        public class Odds : IOdds {
            #region field
            IReadOnlyDictionary<Rarity, Prob> Probs { get; }
            #endregion

            #region mono
            public Odds(IReadOnlyDictionary<Rarity, Prob> probs) {
                if (probs.Sum(p => p.Value.Accurate) != Prob.SignificantFiguresMax) {
                    // 確率の合計が1でないならエラー
                    Debug.LogError("Sum of probability != 1.0");
                }
                Probs = probs;
            }
            #endregion

            #region public
            /// <summary> 確率表に基づきレアリティを乱択 </summary>
            public Rarity Roulette() {
                int value = Random.Range(1, Prob.SignificantFiguresMax);

                string log = "|";
                foreach (var prob in Probs) {
                    log += prob.Value.Accurate;
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
                    sum += $"[{p.Key}:{p.Value.Accurate}]";
                }
                return sum;
            }
            #endregion
        }

    }

}
