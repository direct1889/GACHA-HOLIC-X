using System.Collections.Generic;

namespace du.Audio {

    /// <summary> 音声を6種類に大別 </summary>
    public enum Category {
        MainSE, MenuSE, MusicBGM, EnvironmentalBGM, Jingle, Voice
    }

    /// <summary>
    /// enum の値リストの取得、ToString()を補助する
    /// </summary>
    public static class ExCategory {
        #region field
        private static Cmp.OrderedMap<Category, string> Converter { get; }
            = new Cmp.OrderedMap<Category, string>();
        #endregion

        #region ctor
        static ExCategory() {
            var orderedValues = new List<Category>{
                    Category.MainSE, Category.MenuSE,
                    Category.MusicBGM, Category.EnvironmentalBGM,
                    Category.Jingle, Category.Voice
                };
            foreach (var i in orderedValues) {
                Converter.Add((Category)i, i.ToString());
            }
        }
        #endregion

        #region getter
        public static int                   Count      => Converter.Count ;
        public static ICollection<Category> Values     => Converter.Keys  ;
        public static ICollection<string>   Names      => Converter.Values;
        public static string   ToString       (Category e) => Converter.At   (e);
        public static Category FromInt        (int      i) => Converter.AtKey(i);
        public static string   FromIntToString(int      i) => Converter.At   (i);
        #endregion
    }

}
