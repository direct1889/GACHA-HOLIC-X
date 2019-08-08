using UnityEngine;
using System.Collections.Generic;

namespace du.Audio {

    /// <summary> AudioをManageする </summary>
    public interface IAudioManager {

    }

    /// <summary> AudioをManageする() </summary>
    public class AudioManager : du.Cmp.SingletonMonoBehaviour<AudioManager>, IAudioManager {
        #region static
        public static float MaxVolume => 1f;
        public static float MasterVolume { get; set; }

        IDictionary<Kind, AudioSource> m_sources;

        // [SerializeField] List<AudioSource> m_serializeSources;
        #endregion

        #region mono
        private void Awake() {
            Test.LLog.Boot.Log("AudioManager awake.");
            m_sources = GetComponent<du.Cmp.AudioKindGameObjectDictionaryFromInspector>().ToDictAsComponent<AudioSource>();
            /*
            m_sources = new Dictionary<Kind, AudioSource>{
                { Kind.MainSE, m_serializeSources[0] },
                { Kind.MenuSE, m_serializeSources[1] },
                { Kind.MusicBGM   , m_serializeSources[2] },
                { Kind.EnvironmentalBGM, m_serializeSources[3] },
                { Kind.Jingle, m_serializeSources[4] },
                { Kind.Voice , m_serializeSources[5] },
            };
            */
        }
        #endregion

        #region public
        /// <summary> 重複無しで再生 </summary>
        public bool Play(Kind kind, string name, float? volume = null) {
            return m_sources[kind].Play(SoundAsset.Instance.At(kind, name), volume);
        }
        /// <summary> 重複をゆるして再生 </summary>
        public bool PlayOneShot(Kind kind, string name, float? volume = null) {
            return m_sources[kind].PlayOneShot(SoundAsset.Instance.At(kind, name), volume);
        }
        #endregion
    }

}