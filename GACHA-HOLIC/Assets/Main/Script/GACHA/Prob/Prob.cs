
namespace Main {

    namespace Gacha {

        public interface IProb {
            #region getter
            /// <value> 確率"1"か </value>
            bool Is1 { get; }
            /// <value> 確率"0"か </value>
            bool Is0 { get; }
            /// <value> 確率値を[0.0, 1.0]で近似 </value>
            float Approximation01 { get; }
            #endregion

            #region public
            /// <summary> 確率値に基づき当落を乱択 </summary>
            bool Roulette();
            #endregion
        }

    }

}
