
namespace Main {

    namespace Gacha {

        /// <summary> ガチャ </summary>
        public interface IVendingMachineImpl {
            /// <summary> ガチャを回す </summary>
            IResult Roll();
        }

        /// <summary> ガチャ </summary>
        public class VendingMachineImpl : IVendingMachineImpl {
            #region field
            /// <value> 狙っているレアリティ </value>
            private Rarity Target { get; }
            /// <value> 排出確率表 </value>
            private IOddsPreferences Pref { get; }
            #endregion

            #region ctor
            public VendingMachineImpl(Rarity target, IOddsPreferences pref) {
                Target = target;
                Pref = pref;
                du.Test.LLog.Debug.Log(this);
            }
            #endregion

            #region public
            public IResult Roll() {
                Rarity midResult = Pref.Odds.Roulette();
                var isWants = new System.Func<IsWants>(() => {
                    if (midResult == Target) {
                        // レアリティが狙い通りだった場合、レアリティ内での抽選を行う
                        if (Pref.RateInRarity.Roulette()) { return IsWants.Win; }
                        else { return IsWants.Slip; }
                    }
                    else { return IsWants.Lose; }
                })();
                return new Result(new Content(midResult), isWants);
            }
            #endregion

            #region override
            public override string ToString() => $"Target: {Target}, OddsPref: {Pref}";
            #endregion
        }

    }

}
