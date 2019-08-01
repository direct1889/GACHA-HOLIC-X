using System.Collections.Generic;


namespace du.Ex {

    public static class ExCollection {

        public static bool IsValidIndex<T>(this ICollection<T> list, int index) {
            return !(list is null) && 0 <= index && index < list.Count;
        }
        public static bool IsValidIndex<T>(this IReadOnlyCollection<T> list, int index) {
            return !(list is null) && 0 <= index && index < list.Count;
        }

        public static bool IsEmpty<T>(this ICollection<T> list) {
            return list is null || list.Count == 0;
        }
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> list) {
            return list is null || list.Count == 0;
        }

    }

    public static class ExList {

        /// <returns> listが空の場合はnull </returns>
        public static T Back<T>(this IList<T> list) {
            if (list.IsEmpty()) { return default; }
            else { return list[list.Count - 1]; }
        }

        /// <summary>
        /// リストのサイズを変更
        /// - 要素数を増やす場合 : 末尾にnullを追加
        /// - 要素数を減らす場合 : 末尾から順に削除
        /// </sumamry>
        /// <param name="list"> nullだと例外 </param>
        /// <param name="size"> 負だと例外 </param>
        public static void Resize<T>(this List<T> list, int size) {
            if (list is null) { throw new System.ArgumentNullException($"{nameof(list)} is null."); }
            if (size < 0) { throw new System.ArgumentOutOfRangeException($"{nameof(size)}({size}) is less than 0."); }

            if (list.Count < size) {
                while (list.Count < size) { list.Add(default); }
            }
            else if (list.Count > size) {
                list.RemoveRange(size, list.Count - size);
            }
        }

        /// <summary>
        /// リストのサイズを変更
        /// - 要素数を増やす場合 : 末尾にnullを追加
        /// - 要素数を減らす場合 : 末尾から順に削除
        /// </sumamry>
        /// <param name="list"> nullだと例外 </param>
        /// <param name="size"> 負だと例外 </param>
        public static void Resize<T>(this IList<T> list, int size) {
            if (list is null) { throw new System.ArgumentNullException($"{nameof(list)} is null."); }
            if (size < 0) { throw new System.ArgumentOutOfRangeException($"{nameof(size)}({size}) is less than 0."); }

            if (list.Count < size) {
                while (list.Count < size) { list.Add(default); }
            }
            else if (list.Count > size) {
                list.RemoveRange(size, list.Count - size);
            }
        }

        /// <summary>
        /// リストのサイズを変更
        /// - 要素数を増やす場合 : 末尾にnullを追加
        /// - 要素数を減らす場合 : 末尾から順に削除
        /// </sumamry>
        /// <exception cref="ArgumentNullException"> listがnullのとき </exception>
        /// <exception cref="AugumentOutOfRangeException"> index, countが0より小さい </exception>
        /// <exception cref="AugumentException"> index, countがlistに存在しない要素を参照している </exception>
        public static void RemoveRange<T>(this IList<T> list, int index, int count) {
            if (list is null) { throw new System.ArgumentNullException($"{nameof(list)} is null."); }
            if (index < 0) { throw new System.ArgumentOutOfRangeException($"{nameof(index)}({index}) is less than 0."); }
            if (count < 0) { throw new System.ArgumentOutOfRangeException($"{nameof(count)}({count}) is less than 0."); }
            if (index > list.Count) { throw new System.ArgumentException($"{nameof(index)}({index}) do not denote a valid range of elements in the {nameof(list)}(count:{list.Count})."); }
            if (index + count > list.Count) { throw new System.ArgumentException($"{nameof(index)}({index}) or {nameof(count)}({count}) do not denote a valid range of elements in the {nameof(list)}(count:{list.Count})."); }

            for (; 0 < count; --count) { list.RemoveAt(index); }
        }

    }

    public static class ExDictionary {

        /// <summary> 指定したキーが存在しなければAdd、存在すればSet </summary>
        public static void AddSet<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value) {
            if (dic.ContainsKey(key)) { dic[key] = value; }
            else { dic.Add(key, value); }
        }

        /// <summary> 指定したキーが存在しなければnullを返すAt </summary>
        public static TValue At<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key) where TValue : class {
            return dic.ContainsKey(key) ? dic[key] : null;
        }
        /// <summary> 指定したキーが存在しなかった場合の返り値を指定するAt </summary>
        public static TValue At<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue) {
            return dic.ContainsKey(key) ? dic[key] : defaultValue;
        }

    }

}
