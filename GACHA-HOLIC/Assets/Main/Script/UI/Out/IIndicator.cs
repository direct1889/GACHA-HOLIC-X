using UnityEngine;

namespace Main.Gacha.UI {

    /// <summary> 何らかの情報をGUI表示 </summary>
    public interface IIndicator {
        /// <summary> 表示内容を削除 </summary>
        void Clear();
    }

    /// <summary> 何らかの情報をGUI表示 </summary>
    public interface IIndicator<T> : IIndicator {
        /// <summary> 表示 </summary>
        void Show(T obj);
    }

}
