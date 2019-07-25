using UnityEngine;

namespace Main.Gacha.UI {

    /// <summary> AlertConfigからSerializeFieldを分離 </summary>
    /// <summary> 各種パラメータをユーザ入力から受け取るGUI </summary>
    public interface IAccomplishResultConfig {
        #region field
        /// <summary> 石単価 </summary>
        float IshiPrice { get; }
        /// <summary> 何連を単位として引くか </summary>
        int ConsecutiveNum { get; }
        /// <summary> 上記の単位連1セットに必要な石の個数 </summary>
        int NumOfIshi4Consecutive { get; }

        /// <summary> n回回すのにかかる石 </summary>
        float NumOfIshiNth(int n);
        #endregion
    }

    public class AccomplishResultConfig : MonoBehaviour, IAccomplishResultConfig {
        #region field
        /// <summary> 石単価 </summary>
        [SerializeField] FloatInputField m_ishiPrice;
        /// <summary> 何連を単位として引くか </summary>
        [SerializeField] IntInputField m_consecutiveNum;
        /// <summary> 上記の単位連1セットに必要な石の個数 </summary>
        [SerializeField] IntInputField m_numOfIshiConsecutive;
        #endregion

        #region getter
        /// <summary> 石単価 </summary>
        public float IshiPrice => m_ishiPrice.Value;
        /// <summary> 何連を単位として引くか </summary>
        public int ConsecutiveNum => m_consecutiveNum.Value;
        /// <summary> 上記の単位連1セットに必要な石の個数 </summary>
        public int NumOfIshi4Consecutive => m_numOfIshiConsecutive.Value;

        /// <summary> n回回すのにかかる石 </summary>
        public float NumOfIshiNth(int n) { return NumOfIshi4Consecutive * n / (float)ConsecutiveNum; }
        #endregion

        #region mono
        private void Awake() { }
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }

}
