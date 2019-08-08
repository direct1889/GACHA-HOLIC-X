using System.Collections.Generic;
using UnityEngine;


namespace du.Audio {

    public interface ICategorizedAudioSource {
        #region public
        /// <summary> 重複無しで再生 </summary>
        bool Play(string clipName, float? volume = null);
        /// <summary> 重複をゆるして再生 </summary>
        bool PlayOneShot(string clipName, float? volume = null);
        #endregion
    }

    public class CategorizedAudioSource : MonoBehaviour {
        #region field
        IDictionary<string, AudioClip> m_clips;
        #endregion

        #region property
        private AudioSource Source { get; set; }
        // public AudioSource Source { get; private set; }
        #endregion

        #region mono
        private void Awake() {
            Source = GetComponent<AudioSource>();
            m_clips = GetComponent<du.Cmp.AudioClipDictionaryFromInspector>().ToDict();
        }
        #endregion

        #region public
        /// <summary> 重複無しで再生 </summary>
        public bool Play(string clipName, float? volume = null)
            => Source.Play(m_clips[clipName], volume);
        /// <summary> 重複をゆるして再生 </summary>
        public bool PlayOneShot(string clipName, float? volume = null)
            => Source.PlayOneShot(m_clips[clipName], volume);
        #endregion
    }

}
