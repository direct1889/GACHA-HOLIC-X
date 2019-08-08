using UnityEngine;
using System.Collections.Generic;

namespace du.Audio {

    /// <summary> AudioClip のコンテナ </summary>
    public interface ISoundAsset {
        #region getter
        AudioClip At(Category category, string key);
        #endregion
    }

    /// <summary> AudioClip のコンテナ </summary>
    public class SoundAsset : du.Cmp.SingletonMonoBehaviour<SoundAsset>, ISoundAsset {
        #region field
        IDictionary<Category, IDictionary<string, AudioClip>> m_clips
            = new Dictionary<Category, IDictionary<string, AudioClip>>();
        #endregion

        #region getter
        public AudioClip At(Category category, string key) => m_clips[category][key];
        // public IDictionary<string, AudioClip> At(Category category) => m_clips[category];
        #endregion

        #region ctor
        private void Awake() {
            Load();
        }
        #endregion

        #region private
        private void Load() {
            var clips = GetComponent<du.Cmp.AudioCategoryGameObjectDictionaryFromInspector>()
                .ToDictAsComponent<du.Cmp.AudioClipDictionaryFromInspector>();
            foreach (var i in clips) {
                m_clips.Add(i.Key, i.Value.ToDict());
            }
        }
        #endregion
    }

}
