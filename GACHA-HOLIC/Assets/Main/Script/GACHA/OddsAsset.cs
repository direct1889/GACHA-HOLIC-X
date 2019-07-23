using System.Collections.Generic;

namespace Main.Gacha {

    /// <summary> 排出率表のプリセット群 </summary>
    public static class OddsAsset {
        #region field
        static IDictionary<string, IOddsPreferences> Preset { get; }
        #endregion

        #region ctor
        static OddsAsset() {
            Preset = new Dictionary<string, IOddsPreferences>();

            Register("Reset", 0, 0, 0, 0, 0, 0);
            Register("S5Fixed", 0, 0, 0, 0, 100000, 100000);
            Register("MercStoria", 0, 23890, 67530, 5580, 3000, 33333);
            Register("BanG_Dream!", 0, 88500, 8500, 3000, 0, 16700);
        }
        #endregion

        #region public
        /// <summary> 指定したプリセットが登録されているか </summary>
        public static bool HasExist(string key) => Preset.ContainsKey(key);
        /// <summary> プリセットを取り出す </summary>
        public static IOddsPreferences At(string key) => Preset[key];
        /// <summary> プリセットを登録/上書きする </summary>
        public static void Register(string key, int accS1, int accS2, int accS3, int accS4, int accS5, int accOIR) {
            if (HasExist(key)) {
                Preset[key] = new OddsPreferences(accS1, accS2, accS3, accS4, accS5, accOIR);
            }
            Preset.Add(key, new OddsPreferences(accS1, accS2, accS3, accS4, accS5, accOIR));
        }
        #endregion
    }

}