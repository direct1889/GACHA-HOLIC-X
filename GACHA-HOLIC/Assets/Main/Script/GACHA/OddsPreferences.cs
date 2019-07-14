using System.Collections.Generic;

namespace Main {

    namespace Gacha {

        /// <summary> ガチャの確率情報 </summary>
        public interface IOddsPreferences {
            // /// <value> 排出される最高レアリティ </value>
            // Rarity Max { get; }
            /// <value> 各レアリティの排出確率一覧 </value>
            IOdds Odds { get; }
            /// <value> 狙っているContentの同レアリティ内排出確率 </value>
            Prob OddsInRarity { get; }
        }

        public class OddsPreferences : IOddsPreferences {
            #region field
            //public Rarity Max { get; }
            public IOdds Odds { get; }
            public Prob OddsInRarity { get; }
            #endregion

            #region ctor
            public OddsPreferences(IReadOnlyDictionary<Rarity, Prob> odds)
            : this(odds, Prob.One) {}
            public OddsPreferences(IReadOnlyDictionary<Rarity, Prob> odds, Prob oddsInRarity) {
                Odds = new Odds(odds);
                OddsInRarity = oddsInRarity;
            }
            #endregion

            #region override
            public override string ToString() {
                return $"Odds:[{Odds}],OddsInTargetRarity:[{OddsInRarity}]";
            }
            #endregion
        }

    }

}
