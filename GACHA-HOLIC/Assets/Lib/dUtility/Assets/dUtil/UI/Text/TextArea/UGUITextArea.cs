using UGUI = UnityEngine.UI;

namespace du.dui {

    public class UGUITextArea : TextArea {
        #region field
        ITextGUI m_textUI;
        #endregion

        #region property
        protected override ITextGUI TextUI {
            get {
                if (m_textUI is null) {
                    m_textUI = UGUITextWrapper4ITextGUI.Instantiate(
                        transform.GetComponentInChildren<UGUI.Text>()
                        );
                }
                return m_textUI;
            }
        }
        #endregion
    }

}
