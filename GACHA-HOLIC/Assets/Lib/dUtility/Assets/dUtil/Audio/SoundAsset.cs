using UnityEngine;
using System.Collections.Generic;

namespace du.Audio {

    /// <summary> AudioClip のコンテナ </summary>
    public interface ISoundAsset {
        #region getter
        AudioClip At(Kind kind, string key);
        #endregion
    }

    /// <summary> AudioClip のコンテナ </summary>
    public class SoundAsset : du.Cmp.SingletonMonoBehaviour<SoundAsset>, ISoundAsset {
        #region field
        IDictionary<Kind, IDictionary<string, AudioClip>> m_clips
            = new Dictionary<Kind, IDictionary<string, AudioClip>>();
        #endregion

        #region getter
        public AudioClip At(Kind kind, string key) => m_clips[kind][key];
        // public IDictionary<string, AudioClip> At(Kind kind) => m_clips[kind];
        #endregion

        #region ctor
        private void Awake() {
            Load();
        }
        #endregion

        #region private
        private void Load() {
            var m_assets = GetComponent<du.Cmp.AudioKindGameObjectDictionaryFromInspector>().ToDict();
            // m_clips.Add(Kind.MainSE, m_assets[Kind.MainSE];
        }
        #endregion
    }

}
