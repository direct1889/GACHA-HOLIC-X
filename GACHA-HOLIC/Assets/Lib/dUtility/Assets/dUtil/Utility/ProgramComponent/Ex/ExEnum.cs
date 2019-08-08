#if false
// TODO
using UnityEngine;
using System.Collections.Generic;


namespace du.Audio {

    public static class ExEnums {
        public static ExEnum<Category> m_category;
        public static ExEnum<Category> Category {
            get {
                if (m_category is null) { ExCategory.Create(); }
                return m_category;
            }
            set => m_category = m_category ?? value;
        }
    }

    /// <summary>
    /// enum の値リストの取得、ToString()を補助する
    /// </summary>
    public abstract class ExEnum<T> {
        #region field
        private Cmp.OrderedMap<T, string> Converter { get; }
            = new Cmp.OrderedMap<T, string>();
        #endregion

        #region ctor
        protected ExEnum(List<T> values) {
            var orderedValues = values;
            foreach (var i in orderedValues) {
                Converter.Add((T)i, i.ToString());
            }
        }
        #endregion

        #region getter
        public int                 Count         => Converter.Count ;
        public ICollection<T>      Keys          => Converter.Keys  ;
        public ICollection<string> Names         => Converter.Values;
        public string              ToString(T e) => Converter.At(e) ;
        #endregion
    }

}
#endif