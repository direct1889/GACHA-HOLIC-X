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
        SerializedProperty m_count;
        SerializedProperty m_keys;
        SerializedProperty m_values;
        #endregion

        #region editor
        private void OnEnable() {
            m_count  = serializedObject.FindProperty("m_count");
            m_keys   = serializedObject.FindProperty("m_keys");
            m_values = serializedObject.FindProperty("m_values");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            m_count.intValue = EditorGUILayout.IntField("Count", m_count.intValue);
            m_keys.arraySize = m_count.intValue;
            m_values.arraySize = m_count.intValue;

            for (int i = 0; i < m_count.intValue; ++i) {
                // Begin/EndHorizontal() : 囲んだ要素を横に並べる
                EditorGUILayout.BeginHorizontal();
                {
                    // LabelField([label], [layout]) : labelの内容を表示
                    EditorGUILayout.LabelField($"Clip{i}", GUILayout.MaxWidth(50));
                    // XXXField([label], xxx, [layout]) : XXXの編集可能Boxを表示
                    /// <param name="xxx"> 本体の値を設定 </param>
                    /// <return> 最新の値 </return>
                    m_keys.GetArrayElementAtIndex(i).stringValue
                        = EditorGUILayout.TextField(
                            m_keys.GetArrayElementAtIndex(i).stringValue,
                            GUILayout.MaxWidth(150));
                    /// <param name="allowSceneObjects"> Sceneに存在するGameObjectの取得を不許可 </param>
                    m_values.GetArrayElementAtIndex(i).objectReferenceValue
                        = EditorGUILayout.ObjectField(
                            m_values.GetArrayElementAtIndex(i).objectReferenceValue,
                            typeof(Value), false,
                            GUILayout.MaxWidth(150));
                }
                EditorGUILayout.EndHorizontal();
            }

            // 内部キャッシュに値を保存
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }

}
