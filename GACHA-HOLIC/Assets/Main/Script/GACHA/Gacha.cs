using System.Collections.Generic;

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
            public static Rarity ToRarity(this string str) {
                var parsed = System.Enum.Parse(typeof(Rarity), str);
                if (parsed.GetType() == typeof(Rarity)) {
                    return (Rarity)parsed;
                }
                else {
                    return Rarity.None;
                }
            }
            /// <summary> S1-S5列挙 </summary>
            public static IEnumerable<Rarity> Valids
                => new List<Rarity>{ Rarity.S1, Rarity.S2, Rarity.S3, Rarity.S4, Rarity.S5 };
        }

        /// <summary> 狙い目かどうか </summary>
        public enum IsWants {
            /// <value> 当たり </value>
            Win,
            /// <value> すり抜け (同レア/異コンテンツ) </value>
            Slip,
            /// <value> はずれ (レア度違い) </value>
            Lose
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
            IsWants IsWants { get; }
        }

        /// <summary> ガチャの結果 </summary>
        public class Result : IResult {
            #region field
            public Content Content { get; }
            public IsWants IsWants { get; }
            #endregion

            #region mono
            public Result(Content content, IsWants isWants) {
                Content = content;
                IsWants = isWants;
            }
            #endregion

            #region override
            public override string ToString() {
                return (IsWants == IsWants.Win ? "○" : "×") + $"[{Content}]";
            }
            #endregion
        }

    }

}
