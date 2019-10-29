using System.Collections.Generic;

namespace Main.Gacha {

    public interface IGachaConfigPreset : UI.IVendorConfig, UI.IAccomplishResultConfig {}

    public class GachaConfigPreset : IGachaConfigPreset {
        #region vendor
        /// <value> 各レアリティの排出率表 </value>
        public IOdds Odds { get; }
        /// <value> 狙っているレアリティ </value>
        public Rarity TargetRarity { get; }
        /// <value> 狙っているレアリティ内での狙っているものの排出確率 </value>
        public IProb RateInRarity { get; }
        /// <value> 自動連続ガチャの間隔 </value>
        public float RollInterval { get; }

        /// <summary> VendingMachineごと再生成 </value>
        public IVendingMachineImpl CreateVendor()
            => new VendingMachineImpl(TargetRarity, new OddsPreferences(Odds, RateInRarity));
        #endregion

        #region accomplish
        /// <summary> 石単価 </summary>
        public float IshiPrice { get; }
        /// <summary> 何連を単位として引くか </summary>
        public int ConsecutiveNum { get; }
        /// <summary> 上記の単位連1セットに必要な石の個数 </summary>
        public int NumOfIshi4Consecutive { get; }

        /// <summary> n回回すのにかかる石 </summary>
        public float NumOfIshiNth(int n) => NumOfIshi4Consecutive * n / (float)ConsecutiveNum;
        #endregion

        #region ctor
        public GachaConfigPreset(
            IOdds odds,
            Rarity targetRarity,
            IProb rateInRarity,
            float rollInterval,
            float ishiPrice,
            int consecutiveNum,
            int numOfIshi4Consecutive)
        {
            Odds = odds;
            TargetRarity = targetRarity;
            RateInRarity = rateInRarity;
            RollInterval = rollInterval;
            IshiPrice = ishiPrice;
            ConsecutiveNum = consecutiveNum;
            NumOfIshi4Consecutive = numOfIshi4Consecutive;
        }
        #endregion
    }

    /// <summary> 排出率表のプリセット群 </summary>
    public static class OddsAsset {
        #region field
        static IDictionary<string, IGachaConfigPreset> Preset { get; }
        #endregion

        #region ctor
        static OddsAsset() {
            Preset = new Dictionary<string, IGachaConfigPreset>();

            //       KeyName         , OddsAsXXXXXX(S1,   S2,    S3,   S4,     S5),    Target,  RateInTargetRarity, Interval,  IshiPrice,n連,石/n連
            Register("Reset"         , new OddsInt6(0,     0,     0,    0,      0), Rarity.S5, ProbInt6.Zero      , 0.3f,              0f,  1,    1);
            Register("S5Fixed"       , new OddsInt6(0,     0,     0,    0, 100000), Rarity.S5, ProbInt6.One       , 0.3f,              0f,  1,    1);
            Register("MercStoria"    , new OddsInt6(0, 23890, 67530, 5580,   3000), Rarity.S5, new ProbInt6(60000), 0.3f, 10500f /   168f, 10,   45);
            Register("BanG_Dream!"   , new OddsInt6(0, 88500,  8500, 3000,      0), Rarity.S4, new ProbInt6(16700), 0.3f, 10000f /  8400f, 10, 2500);
            Register("LoveLive!SIF"  , new OddsInt6(0, 80000, 15000, 4000,   1000), Rarity.S4, new ProbInt6(50000), 0.3f,  5140f /    86f, 11,   50);
            Register("LoveLive!SIFAS", new OddsInt6(0,     0, 95000,    0,   5000), Rarity.S4, new ProbInt6(10000), 0.3f, 10500f /  1750f, 10,  500);
        }
        #endregion

        #region public
        /// <summary> 指定したプリセットが登録されているか </summary>
        public static bool HasExist(string key) => Preset.ContainsKey(key);
        /// <summary> プリセットを取り出す </summary>
        public static IGachaConfigPreset At(string key) => Preset[key];
        /// <summary> プリセットを登録/上書きする </summary>
        public static void Register(
            string key,
            IOdds odds,
            Rarity targetRarity,
            IProb rateInRarity,
            float rollInterval,
            float ishiPrice,
            int consecutiveNum,
            int numOfIshi4Consecutive
            )
        {
            if (HasExist(key)) {
                Preset[key] = new GachaConfigPreset(odds, targetRarity, rateInRarity, rollInterval, ishiPrice, consecutiveNum, numOfIshi4Consecutive);
            }
            else {
                du.Test.LLog.Debug.Log($"{key} register");
                Preset.Add(key, new GachaConfigPreset(odds, targetRarity, rateInRarity, rollInterval, ishiPrice, consecutiveNum, numOfIshi4Consecutive));
            }
        }
        #endregion
    }

}