
namespace du.dui {

    public class TMPArea : TextArea {
        #region mono
        private void Awake() {
            TextUI = TMPTextWrapper4ITextGUI.Instantiate(transform.GetComponentInChildren<TMPro.TextMeshProUGUI>());
        }
        #endregion
    }

}
