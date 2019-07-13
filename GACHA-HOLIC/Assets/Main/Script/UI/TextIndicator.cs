using UnityEngine;

namespace Main.Gacha.UI {

    /// <summary> 何らかの情報をTMPテキスト表示 </summary>
    public class TextIndicator<T> : MonoBehaviour, IIndicator<T> {
        #region field
        /// <value> 表示先のTMPテキストUI </value>
        protected TMPro.TextMeshProUGUI Text { get; private set; }
        /// <value> 何らかの情報をstringに変換する </value>
        public System.Func<T, string> Converter { private get; set; }
        #endregion

        #region mono
        protected virtual void Awake() {
            Text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }
        #endregion

        #region public
        /// <summary>
        /// 表示
        /// - Converterを用いてobjをstringに変換
        /// - Converterがnullの場合obj.ToString()
        /// </summary>
        public void Show(T obj) {
            if (Converter is null) {
                Text.text = obj.ToString();
            }
            else {
                Text.text = Converter(obj);
            }
        }

        /// <summary> テキストUIを空文字列に </summary>
        public void Clear() => Text.text = "";
        #endregion
    }

}
