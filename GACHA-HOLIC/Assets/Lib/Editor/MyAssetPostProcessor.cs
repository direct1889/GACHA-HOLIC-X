using UnityEngine;
using UnityEditor;

namespace du.Edit {

    /// <summary> アセットのインポート時に走る設定 </summary>
    public class MyAssetPostProcessor : AssetPostprocessor {
        /// <summary> AudioClip のインポート時の処理 </summary>
        private void OnPostprocessAudio(AudioClip audioClip) {
            var audioImporter = assetImporter as AudioImporter;
            string path = audioImporter.assetPath;
            // Menu/以下は全部モノラル化
            audioImporter.forceToMono = path.Contains("Menu");
            // BGM/以下は全部バックグラウンド読み込み
            audioImporter.loadInBackground = path.Contains("BGM");
        }
    }

}
