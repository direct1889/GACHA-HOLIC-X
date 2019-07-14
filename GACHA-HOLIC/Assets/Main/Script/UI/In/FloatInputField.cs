using UnityEngine;

namespace Main.Gacha.UI {

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IFloatInputField {
        float Value { get; }
    }

    public class FloatInputField : MonoBehaviour, IFloatInputField {
        #region field
        [SerializeField] TMPro.TMP_InputField m_if;
        [SerializeField] float m_initialValue;
        #endregion

        #region mono
        private void Awake() {
            Value = m_initialValue;
        }
        #endregion

        #region public
        public float Value {
            get => System.Convert.ToSingle(m_if.text);
            set => m_if.text = value.ToString();
        }
        #endregion
    }

}
