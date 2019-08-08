using System.Collections.Generic;
using Key = du.Audio.Kind;
using Value = UnityEngine.GameObject;

namespace du.Cmp {

    /// <summary>
    /// インスペクタの値から辞書を生成
    /// </summary>
    public class AudioKindGameObjectDictionaryFromInspector
        : DictionaryFromInspector<Key, Value>
    {
        /// <summary> List<Key>とList<Value>.GetComponent<Component>()からDictionaryを生成 </summary>
        public IDictionary<Key, Component> ToDictAsComponent<Component>() where Component : class {
            var dic = new Dictionary<Key, Component>();
            for (int i = 0; i < Count; ++i) {
                dic.Add(Keys[i], Values[i].GetComponent<Component>());
            }
            return dic;
        }
    }

}
