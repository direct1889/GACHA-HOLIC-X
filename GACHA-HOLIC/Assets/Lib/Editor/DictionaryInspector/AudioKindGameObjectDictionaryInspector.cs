using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

// 拡張表示したい対象の辞書クラス
using Dict = du.Cmp.AudioCategoryGameObjectDictionaryFromInspector;
// 拡張表示したい対象の辞書クラスのKeyの型
using Key = du.Audio.Category;
// 拡張表示したい対象の辞書クラスのValueの型
using Value = UnityEngine.GameObject;

namespace du.Edit {

    /// <summary> du.Cmp.GameObjectDictionaryFromInspectorの表示をきれいにする </summary>
    [CanEditMultipleObjects] // 複数の同時設定ができるようにする
    [CustomEditor(typeof(Dict))]
    public class AudioKindGameObjectDictionaryInspector : Editor {
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

            serializedObject.Update();
            Resize(Audio.ExCategory.Count);
            for (int i = 0; i < m_count.intValue; ++i) {
                AtKey(i).enumValueIndex = (int)Audio.ExCategory.FromInt(i);
            }
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            for (int i = 0; i < m_count.intValue; ++i) {
                // Begin/EndHorizontal() : 囲んだ要素を横に並べる
                EditorGUILayout.BeginHorizontal();
                {
                    // LabelField([label], [layout]) : labelの内容を表示
                    EditorGUILayout.LabelField($"Clip{i}", GUILayout.MaxWidth(40));
                    // XXXField([label], xxx, [layout]) : XXXの編集可能Boxを表示
                    /// <param name="xxx"> 本体の値を設定 </param>
                    /// <return> 最新の値 </return>
                    EditorGUILayout.LabelField($"{Audio.ExCategory.FromInt(i)}", GUILayout.MaxWidth(110));
                    /// <param name="allowSceneObjects"> Sceneに存在するGameObjectの取得を許可 </param>
                    AtValue(i).objectReferenceValue
                        = EditorGUILayout.ObjectField(AtValue(i).objectReferenceValue, typeof(Value), true, GUILayout.MaxWidth(230)) as Value;
                }
                EditorGUILayout.EndHorizontal();
            }
            // 内部キャッシュに値を保存
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region private
        private SerializedProperty AtKey(int i) => m_keys.GetArrayElementAtIndex(i);
        private SerializedProperty AtValue(int i) => m_values.GetArrayElementAtIndex(i);
        private void Resize(int size) {
            m_count.intValue = size;
            m_keys.arraySize = size;
            m_values.arraySize = size;
        }
        #endregion
    }

}
