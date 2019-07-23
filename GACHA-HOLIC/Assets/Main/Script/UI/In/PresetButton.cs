using UnityEngine;
using UGUI = UnityEngine.UI;
using UniRx;
using System;

namespace Main.Gacha.UI {

    /// <summary>
    /// プリセットの適用ボタン
    /// - ParamConfigに対して適用する
    /// </summary>
    public interface IPresetButton {
        /// <value> クリックされたらプリセット名を発行 </value>
        IObservable<string> OnClicked { get; }
    }

    /// <summary>
    /// プリセットの適用ボタン
    /// - ParamConfigに対して適用する
    /// </summary>
    public class PresetButton : MonoBehaviour, IPresetButton {
        #region field
        UGUI.Button m_button;
        /// <summary>
        /// プリセットの登録キー
        /// - Awake()でのみ参照、それ以降に変更しても効果なし
        /// </summary>
        [SerializeField] string m_name;
        #endregion

        #region mono
        private void Awake() {
            du.dui.ITextArea textUi = GetComponent<du.dui.TMPArea>();
            m_button = GetComponent<UGUI.Button>();

            if (OddsAsset.HasExist(m_name)) {
                textUi.Text = m_name;
            }
            else {
                // 初めに呼ばれた時点で無効な名前だったらずっと無効のまま
                textUi.Text = "ERROR";
                m_button.interactable = false;
            }
        }
        #endregion

        #region public
        /// <value> クリックされたらプリセット名を発行 </value>
        public IObservable<string> OnClicked
            => m_button.OnClickAsObservable().Select(_ => m_name);
        #endregion
    }

}
