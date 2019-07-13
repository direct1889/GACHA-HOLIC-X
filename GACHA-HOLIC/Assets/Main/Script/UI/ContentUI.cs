using UnityEngine;

namespace Main.Gacha.UI {

    /// <summary> コンテンツ表示 </summary>
    public class ContentUI : MonoBehaviour, IIndicator<Content> {
        #region field
        // IIndicator<string> Character { get; }
        IIndicator<Rarity> Rarity { get; set; }
        #endregion

        #region mono
        private void Awake() {
            Rarity = GetComponentInChildren<IIndicator<Rarity>>();
        }
        #endregion

        #region public
        public void Show(Content content) {
            // Character.Show(content.name);
            Rarity.Show(content.rarity);
        }
        public void Clear() {
            // Character.Clear();
            Rarity.Clear();
        }
        #endregion
    }

}
