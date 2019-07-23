
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
            public OddsPreferences(IOdds odds)
            : this(odds, ProbInt6.One) {}
            public OddsPreferences(int accS1, int accS2, int accS3, int accS4, int accS5, int accOIR)
            : this(new OddsInt6(accS1, accS2, accS3, accS4, accS5), new ProbInt6(accOIR)) {}
            public OddsPreferences(IOdds odds, IProb oddsInRarity) {
                Odds = odds;
                RateInRarity = oddsInRarity;
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
