
namespace Main {

    namespace Gacha {

        /// <summary> レアリティ </summary>
        public enum Rarity {
            None = 0, S1 = 1, S2, S3, S4, S5
        }
        /// <summary> レアリティの関連メソッド群 </summary>
        public static class ExRarity {
            /// <summary> intから </summary>
            public static Rarity ToRarity(this int i) {
                if (0 <= i && i <= 5) { return (Rarity)i; }
                else { return Rarity.None; }
            }
            public static Rarity ToRaritw(this string str) {
                var parsed = System.Enum.Parse(typeof(Rarity), str);
                if (parsed.GetType() == typeof(Rarity)) {
                    return (Rarity)parsed;
                }
                else {
                    return Rarity.None;
                }
            }
        }

        /// <summary> ガチャから出てくるもの </summary>
        public struct Content {
            public readonly Rarity rarity;

            public Content(Rarity rarity) => this.rarity = rarity;
            public override string ToString() {
                return rarity.ToString();
            }
        }

        /// <summary> ガチャの結果 </summary>
        public interface IResult {
            /// <value> 引いたもの </value>
            Content Content { get; }
            /// <value> 引いたものが狙ったものか </value>
            bool IsWants { get; }
        }

        /// <summary> ガチャの結果 </summary>
        public class Result : IResult {
            #region field
            public Content Content { get; }
            public bool IsWants { get; }
            #endregion

            #region mono
            public Result(Content content, bool isWants) {
                Content = content;
                IsWants = isWants;
            }
            #endregion

            #region override
            public override string ToString() {
                return (IsWants ? "○" : "×") + $"[{Content}]";
            }
            #endregion
        }

    }

}
