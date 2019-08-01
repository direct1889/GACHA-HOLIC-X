using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace du.Audio {

    /// <summary> Audio関連拡張メソッド群 </summary>
    public static class ExAudio {
        /// <summary> 重複無しで再生 </summary>
        public static bool Play(this AudioClip clip, AudioSource source, float? volume = null) {
            return source.Play(clip, volume);
        }

        /// <summary> 重複をゆるして再生 </summary>
        public static bool PlayOneShot(this AudioClip clip, AudioSource source, float? volume = null) {
            return source.PlayOneShot(clip, volume);
        }

        /// <summary> 重複無しで再生 </summary>
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

        /// <summary> 重複をゆるして再生 </summary>
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

        /// <summary> DOTweenを利用したフェード再生 </summary>
        public static void PlayFadeByDOTween(
            this AudioSource source,
            AudioClip clip,
            float fadeSec,
            float volumeBegin,
            float volumeEnd)
        {
            // フェードイン(音量が大きくなっていく)か
            bool isFadeIn = volumeBegin < volumeEnd;
            volumeBegin = Mathf.Clamp01(volumeBegin);
            volumeEnd   = Mathf.Clamp01(volumeEnd);

            source.Play(clip, volumeBegin);
            source.DOFade(volumeEnd, fadeSec);
        }

        /// <summary> DOTweenを利用したフェード再生 </summary>
        public static void PlayFadeByDOTween(
            this AudioSource source,
            AudioClip clip,
            float fadeSec,
            float volumeBegin,
            float volumeEnd,
            Ease ease)
        {
            // フェードイン(音量が大きくなっていく)か
            bool isFadeIn = volumeBegin < volumeEnd;
            volumeBegin = Mathf.Clamp01(volumeBegin);
            volumeEnd   = Mathf.Clamp01(volumeEnd);

            source.Play(clip, volumeBegin);
            DOTween.To(
                () => source.volume,
                value => source.volume = value,
                volumeEnd,
                fadeSec).SetEase(ease);
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
            volumeBegin = Mathf.Clamp01(volumeBegin);
            volumeEnd   = Mathf.Clamp01(volumeEnd);

            source.Play(clip, volumeBegin);
            while (isFadeIn ? (source.volume < volumeEnd) : (source.volume > volumeEnd)) {
                source.volume = volumeBegin + (volumeEnd - volumeBegin) * Time.deltaTime / fadeSec;
                yield return null;
            }
        }
    }

}