using UnityEngine;

namespace Main.Gacha.UI {

    public interface IResultColumn {
        string Label { get; }
        string Value { get; }
        string Unit  { get; }
    }

    public class ResultColumn : MonoBehaviour, IResultColumn {
        #region field
        [SerializeField] TMPro.TextMeshProUGUI m_label;
        [SerializeField] TMPro.TextMeshProUGUI m_value;
        [SerializeField] TMPro.TextMeshProUGUI m_unit;
        #endregion

        #region property
        public string Label {
            get => m_label.text;
            set => m_label.text = value;
        }
        public string Value {
            get => m_value.text;
            set => m_value.text = value;
        }
        public string Unit {
            get => m_unit.text;
            set => m_unit.text = value;
        }
        #endregion

        #region mono
        #endregion

        #region public
        public void Initialize(string label, string value, string unit) {
            Label = label; Value = value; Unit = unit;
        }
        public void SetValue(object obj) {
            Value = obj.ToString();
        }
        #endregion
    }

}
