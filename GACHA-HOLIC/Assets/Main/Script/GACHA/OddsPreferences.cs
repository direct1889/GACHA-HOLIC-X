
namespace Main {

    namespace Gacha {

        /// <summary> ガチャの確率情報 </summary>
        public interface IOddsPreferences {
            // /// <value> 排出される最高レアリティ </value>
            // Rarity Max { get; }
            /// <value> 各レアリティの排出確率一覧 </value>
            IOdds Odds { get; }
            /// <value> 狙っているContentの同レアリティ内排出確率 </value>
            IProb RateInRarity { get; }
        }

        public class OddsPreferences : IOddsPreferences {
            #region field
            //public Rarity Max { get; }
            public IOdds Odds { get; }
            public IProb RateInRarity { get; }
            #endregion

            #region ctor
            public OddsPreferences(IOdds odds, IProb rateInRarity) {
                Odds = odds;
                RateInRarity = rateInRarity;
            }
            #endregion

            #region override
            public override string ToString() {
                return $"Odds:[{Odds}],OddsInTargetRarity:[{RateInRarity}]";
            }
            #endregion
        }

    }

}
