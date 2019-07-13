
namespace Main {

    namespace Gacha {

        /// <summary> レアリティ </summary>
        public enum Rarity {
            None = 0, S1 = 1, S2, S3, S4, S5
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
