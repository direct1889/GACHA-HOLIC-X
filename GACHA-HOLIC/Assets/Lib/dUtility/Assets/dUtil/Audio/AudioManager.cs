using UnityEngine;
using System.Collections.Generic;

namespace du.Audio {

    /// <summary> AudioをManageする </summary>
    public interface IAudioManager {
        #region getter
        CategorizedAudioSource this[Category c] { get; }
        #endregion
    }

    /// <summary> AudioをManageする() </summary>
    public class AudioManager : du.Cmp.SingletonMonoBehaviour<AudioManager>, IAudioManager {
        #region static
        public static float MaxVolume => 1f;
        public static float MasterVolume { get; set; }

        IDictionary<Category, CategorizedAudioSource> m_sources;
        #endregion

        #region mono
        private void Awake() {
            Test.LLog.Boot.Log("AudioManager awake.");
            m_sources
                = GetComponent<du.Cmp.AudioCategoryGameObjectDictionaryFromInspector>()
                    .ToDictAsComponent<CategorizedAudioSource>();
        }
        #endregion

        #region getter
        public CategorizedAudioSource this[Category c] => m_sources[c];
        #endregion
    }

}