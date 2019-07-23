using UnityEngine;

namespace du.dui {

    public interface ITextArea {
        string Text { set; get; }
        void Add(string text);
        void Add(char c);
    }

    public abstract class TextArea : MonoBehaviour, ITextArea {
        #region property
        protected abstract ITextGUI TextUI { get; }
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

}
