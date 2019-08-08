using System.Collections.Generic;

namespace du.Audio {

    /// <summary> 音声を6種類に大別 </summary>
    public enum Kind {
        MainSE, MenuSE, MusicBGM, EnvironmentalBGM, Jingle, Voice
    }

    /// <summary>
    /// enum の値リストの取得、ToString()を補助する
    /// </summary>
    public static class ExKind {
        #region field
        private static Cmp.OrderedMap<Kind, string> Converter { get; }
            = new Cmp.OrderedMap<Kind, string>();
        #endregion

        #region ctor
        static ExKind() {
            var orderedValues = new List<Kind>{
                    Kind.MainSE, Kind.MenuSE,
                    Kind.MusicBGM, Kind.EnvironmentalBGM,
                    Kind.Jingle, Kind.Voice
                };
            foreach (var i in orderedValues) {
                Converter.Add((Kind)i, i.ToString());
            }
        }
        #endregion

        #region getter
        public static int                 Count      => Converter.Count ;
        public static ICollection<Kind>   Values     => Converter.Keys  ;
        public static ICollection<string> Names      => Converter.Values;
        public static string ToString       (Kind e) => Converter.At   (e);
        public static Kind   FromInt        (int  i) => Converter.AtKey(i);
        public static string FromIntToString(int  i) => Converter.At   (i);
        #endregion
    }

#if false
    public class ExKind : ExEnum<Kind> {
        #region ctor
        private ExKind()
            : base(new List<Kind>{
                    Kind.MainSE, Kind.MenuSE,
                    Kind.MusicBGM, Kind.EnvironmentalBGM,
                    Kind.Jingle, Kind.Voice
                }) {}
        #endregion

        #region static
        public static void Create() {
            if (ExEnums.Kind is null) { ExEnums.Kind = new ExKind(); }
        }
        #endregion
    }
#endif

}
