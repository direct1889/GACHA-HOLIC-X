using System.Collections.Generic;

namespace Main.Gacha.UI {

    /// <summary> 狙い目かどうか表示 </summary>
    public class IsWantsUI : TextIndicator<IsWants> {
        #region field
        IReadOnlyDictionary<IsWants, string> m_msgs
            = new Dictionary<IsWants, string>{
                { IsWants.Win , "お迎え!!" },
                { IsWants.Slip, "すり抜け" },
                { IsWants.Lose, "はずれ" },
            };
        #endregion

        #region mono
        protected override void Awake() {
            base.Awake();
            Converter = (isWants => m_msgs[isWants]);
        }
        #endregion
    }

}
