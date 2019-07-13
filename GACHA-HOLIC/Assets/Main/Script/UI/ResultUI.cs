using UnityEngine;

namespace Main.Gacha.UI {

    /// <summary> ガチャ結果表示 </summary>
    public class ResultUI : MonoBehaviour, IIndicator<IResult> {
        #region field
        IIndicator<Content> Content { get; set; }
        IIndicator<bool> IsWants { get; set; }
        #endregion

        #region mono
        private void Awake() {
            Content = GetComponentInChildren<IIndicator<Content>>();
            IsWants = GetComponentInChildren<IIndicator<bool>>();
        }
        #endregion

        #region public
        public void Show(IResult result) {
            Content.Show(result.Content);
            IsWants.Show(result.IsWants);
        }
        public void Clear() {
            Content.Clear();
            IsWants.Clear();
        }
        #endregion
    }

}
