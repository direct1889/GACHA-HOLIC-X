using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Main.Gacha.UI {

    public interface IGachaConfPresetManager {
        /// <value> クリックされたらプリセットを発行 </value>
        IObservable<IGachaConfigPreset> OnClicked { get; }
    }

    /// <summary>
    /// プリセットの適用ボタン群の取りまとめ
    /// </summary>
    public class GachaConfPresetManager : MonoBehaviour, IGachaConfPresetManager {
        #region field
        Subject<IGachaConfigPreset> m_presetStream = new Subject<IGachaConfigPreset>();

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
        public IObservable<IGachaConfigPreset> OnClicked => m_presetStream;
        #endregion
    }

}
