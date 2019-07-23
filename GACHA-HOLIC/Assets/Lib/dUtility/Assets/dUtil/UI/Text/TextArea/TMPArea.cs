
namespace du.dui {

    public class TMPArea : TextArea {
        #region field
        ITextGUI m_textUI;
        #endregion

        #region property
        protected override ITextGUI TextUI {
            get {
                if (m_textUI is null) {
                    m_textUI = TMPTextWrapper4ITextGUI.Instantiate(
                        transform.GetComponentInChildren<TMPro.TextMeshProUGUI>()
                        );
                }
                return m_textUI;
            }
        }
        #endregion
    }

}
