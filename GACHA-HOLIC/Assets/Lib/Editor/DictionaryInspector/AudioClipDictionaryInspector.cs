using UnityEngine;
using UnityEditor;

// 拡張表示したい対象の辞書クラス
using Dict = du.Cmp.AudioClipDictionaryFromInspector;
// 拡張表示したい対象の辞書クラスのValueの型
using Value = UnityEngine.AudioClip;

namespace du.Edit {

    /// <summary> du.Cmp.AudioClipDictionaryFromInspectorの表示をきれいにする </summary>
    [CanEditMultipleObjects] // 複数の同時設定ができるようにする
    [CustomEditor(typeof(Dict))]
    public class AudioClipDictionaryInspector : Editor {
        #region field
        Dict m_data;
        #endregion

        #region editor
        private void OnEnable() {
            m_data = (Dict)serializedObject.targetObject;
        }

        public override void OnInspectorGUI() {
            m_data.Count = EditorGUILayout.IntField("Count", m_data.Count);

            for (int i = 0; i < m_data.Count; ++i) {
                // Begin/EndHorizontal() : 囲んだ要素を横に並べる
                EditorGUILayout.BeginHorizontal();
                // LabelField([label], [layout]) : labelの内容を表示
                EditorGUILayout.LabelField($"Clip{i}", GUILayout.MaxWidth(50));
                // XXXField([label], xxx, [layout]) : XXXの編集可能Boxを表示
                /// <param name="xxx"> 本体の値を設定 </param>
                /// <return> 最新の値 </return>
                m_data.Keys[i] = EditorGUILayout.TextField(m_data.Keys[i], GUILayout.MaxWidth(150));
                /// <param name="allowSceneObjects"> Sceneに存在するGameObjectの取得を不許可 </param>
                m_data.Values[i] = EditorGUILayout.ObjectField(m_data.Values[i], typeof(Value), false, GUILayout.MaxWidth(230)) as Value;
                EditorGUILayout.EndHorizontal();
            }

            // 内部キャッシュに値を保存
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }

}
