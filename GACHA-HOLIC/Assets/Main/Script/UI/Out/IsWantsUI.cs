
namespace Main.Gacha.UI {

    /// <summary> 狙い目かどうか表示 </summary>
    public class IsWantsUI : TextIndicator<bool> {
        #region mono
        protected override void Awake() {
            base.Awake();
            Converter = (isWants => isWants ? "お迎え成功!!" : "死");
        }
        #endregion
    }

}
