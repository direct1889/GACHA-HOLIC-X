using System.Collections.Generic;
using UnityEngine;
using static du.Ex.ExList;

namespace du.Cmp {

    /// <summary>
    /// インスペクタの値から辞書を生成
    /// </summary>
    public class DictionaryFromInspector<Key, Value> : MonoBehaviour {
        #region field
        [SerializeField] private List<Key> m_keys = new List<Key>();
        [SerializeField] private List<Value> m_values = new List<Value>();
        [SerializeField] int m_count = 0;
        #endregion

        #region property
        public IList<Key> Keys => m_keys;
        public IList<Value> Values => m_values;
        /// <value> 要素数 </value>
        public int Count {
            // get => m_keys.Count;
            get => m_keys.Count;
            set {
                if (value != Count) {
                    Debug.Log($"<color=red>Count: {Count} -> {value}</color>");
                    m_count = value;
                    m_keys.Resize(value);
                    m_values.Resize(value);
                }
            }
        }
        #endregion

        #region mono
        void Awake() => Count = m_count;
        #endregion

        #region public
        /// <summary> List<Key>とList<Value>からDictionaryを生成 </summary>
        public IDictionary<Key, Value> ToDict() {
            var dic = new Dictionary<Key, Value>();
            for (int i = 0; i < Count; ++i) {
                dic.Add(m_keys[i], m_values[i]);
            }
            return dic;
        }
        #endregion
    }

}
