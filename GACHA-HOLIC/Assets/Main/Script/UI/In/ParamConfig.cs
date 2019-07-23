using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UGUI = UnityEngine.UI;
using UniRx;


namespace Main.Gacha.UI {

    [System.Serializable]
    public struct ParamConfigSerializeFields {
        #region field
        [SerializeField] ProbI6InputField m_ifS5;
        [SerializeField] ProbI6InputField m_ifS4;
        [SerializeField] ProbI6InputField m_ifS3;
        [SerializeField] ProbI6InputField m_ifS2;
        [SerializeField] ProbI6InputField m_ifS1;

        public ProbI6InputField rateInRarity;

        public FloatInputField rollInterval;
        public du.dui.TMPArea total;
        public UGUI.Button saveButton;

        public OddsPrefPresetManager presets;
        #endregion

        #region getter
        public IDictionary<Rarity, IProbI6InputField> InputFields
            => new Dictionary<Rarity, IProbI6InputField>{
            { Rarity.S1, m_ifS1 },
            { Rarity.S2, m_ifS2 },
            { Rarity.S3, m_ifS3 },
            { Rarity.S4, m_ifS4 },
            { Rarity.S5, m_ifS5 },
        };
        #endregion
    }

    /// <summary> 各種パラメータをユーザ入力から受け取るGUI </summary>
    public interface IParamConfig {
        /// <value> 各レアリティの排出率表 </value>
        IOdds Odds { get; }
        /// <value> 狙っているレアリティ </value>
        Rarity TargetRarity { get; }
        /// <value> 狙っているレアリティ内での狙っているものの排出確率 </value>
        IProb RateInRarity { get; }

        /// <value> 自動連続ガチャの間隔 </value>
        float RollInterval { get; }

        /// <summary> VendingMachineごと再生成 </value>
        IVendingMachineImpl CreateVendor();
    }

    public class ParamConfig : MonoBehaviour, IParamConfig {
        #region field
        IDictionary<Rarity, IProbI6InputField> m_inputFields;
        ProbI6InputField m_rateInRarity;
        FloatInputField m_rollInterval;
        du.dui.ITextArea m_total;
        UGUI.Button m_saveButton;
        IOddsPrefPresetManager m_presets;

        [SerializeField] ParamConfigSerializeFields m_serialized;
        #endregion

        #region getter
        // public IOdds Odds { get; private set; }
        public IOdds Odds => new OddsInt6(new Dictionary<Rarity, ProbInt6>{
                    { Rarity.S1, m_inputFields[Rarity.S1].Prob },
                    { Rarity.S2, m_inputFields[Rarity.S2].Prob },
                    { Rarity.S3, m_inputFields[Rarity.S3].Prob },
                    { Rarity.S4, m_inputFields[Rarity.S4].Prob },
                    { Rarity.S5, m_inputFields[Rarity.S5].Prob },
                });
        public Rarity TargetRarity {
            get {
                foreach (var i in m_inputFields) {
                    if (i.Value.IsTarget) { return i.Key; }
                }
                return Rarity.None;
            }
        }
        public IProb RateInRarity {
            get => m_rateInRarity.Prob;
            private set => m_rateInRarity.SetProb(value);
        }
        public float RollInterval => m_rollInterval.Value;
        #endregion

        #region mono
        private void Awake() {
            if (m_inputFields is null) {
                m_inputFields  = m_serialized.InputFields;
                m_rateInRarity = m_serialized.rateInRarity;
                m_rollInterval = m_serialized.rollInterval;
                m_total        = m_serialized.total;
                m_saveButton   = m_serialized.saveButton;
                m_presets      = m_serialized.presets;

                m_presets
                    .OnClicked
                    .Subscribe(odds => Set(odds))
                    .AddTo(this);

                // 初期値設定
                Set(OddsAsset.At("MercStoria"));
                CheckUpTotalOne();

                foreach (var i in m_inputFields.Values) {
                    i.OnProbChanged
                        .Subscribe(_ => CheckUpTotalOne())
                        .AddTo(this);
                }
            }
        }
        #endregion

        #region public
        public IVendingMachineImpl CreateVendor() {
            Debug.LogError($"CreateVendor : {Odds}, {RateInRarity}");
            return new VendingMachineImpl(TargetRarity, new OddsPreferences(Odds, RateInRarity));
        }
        #endregion

        #region private
        private void Set(IOddsPreferences pref) {
            SetOdds(pref.Odds);
            RateInRarity = pref.RateInRarity;
        }
        private void SetOdds(int s1, int s2, int s3, int s4, int s5) {
            m_inputFields[Rarity.S1].SetProb(new ProbInt6(s1));
            m_inputFields[Rarity.S2].SetProb(new ProbInt6(s2));
            m_inputFields[Rarity.S3].SetProb(new ProbInt6(s3));
            m_inputFields[Rarity.S4].SetProb(new ProbInt6(s4));
            m_inputFields[Rarity.S5].SetProb(new ProbInt6(s5));
        }
        private void SetOdds(IOdds odds) {
            foreach (Rarity r in ExRarity.Valids) {
                m_inputFields[r].SetProb(odds[r]);
            }
        }
        /// <summary> Oddsの合計が1になっているか </summary>
        private void CheckUpTotalOne() {
            IProb total = ProbInt6.Sum(m_inputFields.Values.Select(pif => pif.Prob));
            if (total.Is1) { m_total.Text = $"○ Total : {total}"; }
            else           { m_total.Text = $"× Total : {total}"; }
            m_saveButton.interactable = total.Is1;
        }
        #endregion
    }

}
