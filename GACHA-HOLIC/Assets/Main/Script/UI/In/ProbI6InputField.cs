using UnityEngine;
using UGUI = UnityEngine.UI;
using System;
using UniRx;


namespace Main.Gacha.UI {

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IROProbI6InputField {
        /// <summary> 確率 (6桁整数) </summary>
        ProbInt6 Prob { get; }
        /// <summary> 目当てのレアリティか </summary>
        bool IsTarget { get; }
    }

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IReProbI6InputField {
        /// <summary> 値が変更されたときのストリーム </summary>
        IObservable<string> OnProbChanged { get; }
    }

    /// <summary> 各種設定項目の入力UI </summary>
    public interface IProbI6InputField : IROProbI6InputField, IReProbI6InputField {
        /// <summary> 確率を設定 </summary>
        void SetProb(IProb prob);
        /// <summary> 目当てのレアリティかどうか設定 </summary>
        void SetIsTarget(bool isTarget);
    }

    /// <summary> ProbInt6 (有効数字6桁の整数で管理する確率) の入力フォーム </summary>
    public class ProbI6InputField : MonoBehaviour, IProbI6InputField {
        #region field
        /// <summary> テキスト入力 (6桁整数) </summary>
        [SerializeField] TMPro.TMP_InputField m_if;
        /// <summary> チェックボックス </summary>
        [SerializeField] UGUI.Toggle m_toggle;
        #endregion

        #region getter
        /// <summary> 確率 (6桁整数) </summary>
        public ProbInt6 Prob => Gacha.ProbInt6.FromString(m_if.text);
        /// <summary> 目当てのレアリティか </summary>
        public bool IsTarget => m_toggle.isOn;
        /// <summary> 値が変更されたときのストリーム </summary>
        public IObservable<string> OnProbChanged { get; private set; }
        #endregion

        #region mono
        private void Awake() {
            OnProbChanged = m_if.onValueChanged.AsObservable();
        }
        #endregion

        #region public
        /// <summary> 確率を設定 </summary>
        public void SetProb(IProb prob) {
            m_if.text = prob.ToString();
        }
        /// <summary> 目当てのレアリティかどうか設定 </summary>
        public void SetIsTarget(bool isTarget) {
            m_toggle.isOn = isTarget;
        }
        #endregion
    }

}
