using System.Collections.Generic;
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

}
