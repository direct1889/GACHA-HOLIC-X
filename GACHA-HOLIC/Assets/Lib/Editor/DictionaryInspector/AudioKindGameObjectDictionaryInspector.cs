using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 拡張表示したい対象の辞書クラス
using Dict = du.Cmp.AudioKindGameObjectDictionaryFromInspector;
// 拡張表示したい対象の辞書クラスのKeyの型
using Key = du.Audio.Kind;
// 拡張表示したい対象の辞書クラスのValueの型
using Value = UnityEngine.GameObject;

namespace du.Edit {

    /// <summary> du.Cmp.GameObjectDictionaryFromInspectorの表示をきれいにする </summary>
    [CanEditMultipleObjects] // 複数の同時設定ができるようにする
    [CustomEditor(typeof(Dict))]
    public class AudioKindGameObjectDictionaryInspector : Editor {
        #region field
        Dict m_data;
        #endregion

        #region editor
        private void OnEnable() {
            if (m_data is null) {
                m_data = (Dict)serializedObject.targetObject;
                m_data.Count = Audio.ExKind.Count;
                int i = 0;
                foreach (var key in Audio.ExKind.Keys) {
                    m_data.Keys[i] = key;
                    ++i;
                }
            }
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            Undo.RecordObject(m_data, "Undo.RecordObject");
            int i = 0;
            foreach (var key in Audio.ExKind.Keys) {
                // Begin/EndHorizontal() : 囲んだ要素を横に並べる
                EditorGUILayout.BeginHorizontal();
                // LabelField([label], [layout]) : labelの内容を表示
                EditorGUILayout.LabelField($"Clip{i}", GUILayout.MaxWidth(40));
                // XXXField([label], xxx, [layout]) : XXXの編集可能Boxを表示
                /// <param name="xxx"> 本体の値を設定 </param>
                /// <return> 最新の値 </return>
                EditorGUILayout.LabelField($"{key}", GUILayout.MaxWidth(110));
                // m_data.Keys[i] = (Key)EditorGUILayout.EnumPopup(m_data.Keys[i], GUILayout.MaxWidth(150));
                /// <param name="allowSceneObjects"> Sceneに存在するGameObjectの取得を許可 </param>
                m_data.Values[i] = EditorGUILayout.ObjectField(m_data.Values[i], typeof(Value), true, GUILayout.MaxWidth(230)) as Value;
                EditorGUILayout.EndHorizontal();
                ++i;
            }
            EditorUtility.SetDirty(m_data);

            // 内部キャッシュに値を保存
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }

}
