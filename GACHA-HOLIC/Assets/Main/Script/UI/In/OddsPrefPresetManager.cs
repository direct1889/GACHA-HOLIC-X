using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Main.Gacha.UI {

    public interface IOddsPrefPresetManager {
        /// <value> クリックされたらプリセット名を発行 </value>
        IObservable<IOddsPreferences> OnClicked { get; }
    }

    /// <summary>
    /// プリセットの適用ボタン群の取りまとめ
    /// </summary>
    public class OddsPrefPresetManager : MonoBehaviour, IOddsPrefPresetManager {
        #region field
        Subject<IOddsPreferences> m_presetStream = new Subject<IOddsPreferences>();

        [SerializeField] List<PresetButton> m_buttons;
        #endregion

        #region mono
        private void Awake() {
            foreach (IPresetButton i in m_buttons) {
                i.OnClicked.Subscribe(
                    name => m_presetStream.OnNext(OddsAsset.At(name)))
                    .AddTo(this);
            }
        }
        #endregion

        #region getter
        /// <value> クリックされたらプリセット名を発行 </value>
        public IObservable<IOddsPreferences> OnClicked => m_presetStream;
        #endregion
    }

}
