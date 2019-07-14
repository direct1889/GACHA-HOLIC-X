
namespace Main.Gacha.UI {

    /// <summary> 今、何連目?表示 </summary>
    public class NumberUI : TextIndicator<int> {
        #region mono
        protected override void Awake() {
            base.Awake();
            Converter = (n => $"{n}連目");
        }
        #endregion
    }

}
