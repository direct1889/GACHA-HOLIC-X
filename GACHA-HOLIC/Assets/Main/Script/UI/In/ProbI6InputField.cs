using UnityEngine;
using UGUI = UnityEngine.UI;
using System;
using UniRx;


namespace Main.Gacha.UI {

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IROProbI6InputField {
        ProbInt6 Prob { get; }
        bool IsTarget { get; }
    }

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IReProbI6InputField {
        IObservable<string> OnProbChanged { get; }
    }

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IProbI6InputField : IROProbI6InputField, IReProbI6InputField {
        void SetProb(IProb prob);
        void SetIsTarget(bool isTarget);
    }

    public class ProbI6InputField : MonoBehaviour, IProbI6InputField {
        #region field
        [SerializeField] TMPro.TMP_InputField m_if;
        [SerializeField] UGUI.Toggle m_toggle;
        #endregion

        #region getter
        public ProbInt6 Prob => Gacha.ProbInt6.FromString(m_if.text);
        public bool IsTarget => m_toggle.isOn;
        public IObservable<string> OnProbChanged { get; private set; }
        #endregion

        #region mono
        private void Awake() {
            OnProbChanged = m_if.onValueChanged.AsObservable();
        }
        #endregion

        #region public
        public void SetProb(IProb prob) {
            m_if.text = prob.ToString();
        }
        public void SetIsTarget(bool isTarget) {
            m_toggle.isOn = isTarget;
        }
        #endregion
    }

}
