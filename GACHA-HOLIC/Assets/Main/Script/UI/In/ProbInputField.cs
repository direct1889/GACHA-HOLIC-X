using UnityEngine;
using UGUI = UnityEngine.UI;
using System;
using UniRx;


namespace Main.Gacha.UI {

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IROProbInputField {
        Prob Prob { get; }
        bool IsTarget { get; }
    }

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IReProbInputField {
        IObservable<string> OnProbChanged { get; }
    }

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IProbInputField : IROProbInputField, IReProbInputField {
        void SetProb(int probAccurate);
        void SetIsTarget(bool isTarget);
    }

    public class ProbInputField : MonoBehaviour, IProbInputField {
        #region field
        [SerializeField] TMPro.TMP_InputField m_if;
        [SerializeField] UGUI.Toggle m_toggle;
        #endregion

        #region getter
        public Prob Prob => Prob.FromString(m_if.text);
        public bool IsTarget => m_toggle.isOn;
        public IObservable<string> OnProbChanged { get; private set; }
        #endregion

        #region mono
        private void Awake() {
            OnProbChanged = m_if.onValueChanged.AsObservable();
        }
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
