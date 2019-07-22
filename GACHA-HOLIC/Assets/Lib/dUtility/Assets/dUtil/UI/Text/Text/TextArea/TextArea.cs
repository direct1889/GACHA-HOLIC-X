using UnityEngine;
using UGUI = UnityEngine.UI;

namespace du.dui {

    public interface ITextArea {
        string Text { set; get; }
        void Add(string text);
        void Add(char c);
    }

    public abstract class TextArea : MonoBehaviour, ITextArea {
        #region field
        ITextGUI m_textUI;
        #endregion

        #region property
        protected ITextGUI TextUI {
            private get => m_textUI;
            set => m_textUI = m_textUI ?? value;
        }
        public string Text {
            set => TextUI.Text = value;
            get => TextUI.Text;
        }
        #endregion

        #region public
        public void Add(string text){ TextUI.Text += text; }
        public void Add(char c)     { TextUI.Text += c;    }
        #endregion
    }

    public class UGUITextArea : TextArea {
        #region mono
        private void Awake() {
            TextUI = UGUITextWrapper4ITextGUI.Instantiate(transform.GetComponentInChildren<UGUI.Text>());
        }
        #endregion
    }

}
