using UnityEngine;

namespace Main.Gacha.UI {

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IIntInputField {
        int Value { get; }
    }

    public class IntInputField : MonoBehaviour, IIntInputField {
        #region field
        [SerializeField] TMPro.TMP_InputField m_if;
        [SerializeField] int m_initialValue;
        #endregion

        #region mono
        private void Awake() {
            Value = m_initialValue;
        }
        #endregion

        #region public
        public int Value {
            get => System.Convert.ToInt32(m_if.text);
            set => m_if.text = value.ToString();
        }
        #endregion
    }

}
