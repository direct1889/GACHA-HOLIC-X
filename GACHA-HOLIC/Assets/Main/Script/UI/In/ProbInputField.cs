using UnityEngine;
using UGUI = UnityEngine.UI;

namespace Main.Gacha.UI {

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IROProbInputField {
        Prob Prob { get; }
        bool IsTarget { get; }
    }

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IProbInputField : IROProbInputField {
        void SetProb(int probAccurate);
        void SetIsTarget(bool isWants);
    }

    public class ProbInputField : MonoBehaviour, IProbInputField {
        #region field
        [SerializeField] TMPro.TMP_InputField m_if;
        [SerializeField] UGUI.Toggle m_toggle;
        #endregion

        #region getter
        public Prob Prob => Prob.FromString(m_if.text);
        public bool IsTarget => m_toggle.isOn;
        #endregion

        #region public
        public void SetProb(int probAccurate) {
            m_if.text = probAccurate.ToString();
        }
        public void SetIsTarget(bool isTarget) {
            m_toggle.isOn = isTarget;
        }
        #endregion
    }

}
