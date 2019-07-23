using UGUI = UnityEngine.UI;

namespace du.dui {

    /// <summary> UGUI.TextとTMP_Textの統一インタフェース </summary>
    public interface ITextGUI {
        string Text { get; set; }
    }

    public class UGUITextWrapper4ITextGUI : ITextGUI {
        #region field
        UGUI.Text Impl { get; }
        #endregion

        #region ctor
        /// <summary> nullチェックしたいのでstaticを経由させる </summary>
        private UGUITextWrapper4ITextGUI(UGUI.Text impl) {
            Impl = impl;
        }
        #endregion

        #region public
        public string Text {
            set => Impl.text = value;
            get => Impl.text;
        }
        #endregion

        #region static
        /// <summary> nullチェック付きインスタンス生成 </summary>
        /// <param name="impl"> nullを渡すと返り値もnullに </param>
        public static UGUITextWrapper4ITextGUI Instantiate(UGUI.Text impl) {
            if (impl is null) { return null; }
            else { return new UGUITextWrapper4ITextGUI(impl); }
        }
        #endregion
    }

    public class TMPTextWrapper4ITextGUI : ITextGUI {
        #region field
        TMPro.TMP_Text Impl { get; }
        #endregion

        #region ctor
        /// <summary> nullチェックしたいのでstaticを経由させる </summary>
        private TMPTextWrapper4ITextGUI(TMPro.TMP_Text impl) {
            Impl = impl;
        }
        #endregion

        #region public
        public string Text {
            set => Impl.text = value;
            get => Impl.text;
        }
        #endregion

        #region static
        /// <summary> nullチェック付きインスタンス生成 </summary>
        /// <param name="impl"> nullを渡すと返り値もnullに </param>
        public static TMPTextWrapper4ITextGUI Instantiate(TMPro.TMP_Text impl) {
            if (impl is null) { return null; }
            else { return new TMPTextWrapper4ITextGUI(impl); }
        }
        #endregion
    }

}
