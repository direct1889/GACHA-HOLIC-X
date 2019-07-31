using UnityEngine;
using System.Collections;

namespace du.Audio {

    public class AudioManager : MonoBehaviour {
        public static float MaxVolume => 1f;
        public static float MasterVolume { get; set; }
    }

    public static class ExAudio {
        /// <summary> 音声の再生が重複しない </summary>
        public static bool Play(this AudioSource source, AudioClip clip, float? volume = null) {
            // vol が null(指定なし) なら MasterVolume に従う
            var vol = volume ?? AudioManager.MasterVolume;
            // 引数が不正な場合
            if (clip is null) { throw new System.ArgumentNullException(nameof(clip)); }
            // vol が 0 なら鳴らさない
            if (Mathf.Approximately(vol, 0f)) { return false; }
            // 引数が不正な場合2
            if (vol < 0f) { throw new System.ArgumentOutOfRangeException(nameof(volume)); }

            source.clip = clip;
            source.volume = vol;
            source.Play();
            return true;
        }

        /// <summary> 音声が複数重複して再生できる </summary>
        public static bool PlayOneShot(this AudioSource source, AudioClip clip, float? volume = null) {
            // vol が null(指定なし) なら MasterVolume に従う
            var vol = volume ?? AudioManager.MasterVolume;
            // 引数が不正な場合
            if (clip is null) { throw new System.ArgumentNullException(nameof(clip)); }
            // vol が 0 なら鳴らさない
            if (Mathf.Approximately(vol, 0f)) { return false; }
            // 引数が不正な場合2
            if (vol < 0f) { throw new System.ArgumentOutOfRangeException(nameof(volume)); }

            source.PlayOneShot(clip, vol);
            return true;
        }

        /// <summary> コルーチンを利用したフェード再生 </summary>
        public static IEnumerator PlayFadeByCoroutine(
            this AudioSource source,
            AudioClip clip,
            float fadeSec,
            float volumeBegin,
            float volumeEnd)
        {
            // フェードイン(音量が大きくなっていく)か
            bool isFadeIn = volumeBegin < volumeEnd;
            volumeEnd = Mathf.Min(volumeEnd, AudioManager.MaxVolume);

            source.Play(clip, volumeBegin);
            while (isFadeIn ? (source.volume < volumeEnd) : (source.volume > volumeEnd)) {
                source.volume = volumeBegin + (volumeEnd - volumeBegin) * Time.deltaTime / fadeSec;
                yield return null;
            }
        }
    }

}