using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UGUI = UnityEngine.UI;
using UniRx;


namespace Main.Gacha.UI {

    [System.Serializable]
    /// <summary> VendorConfigからSerializeFieldを分離 </summary>
    public struct VendorConfigSerializeFields {
        #region field
        /// <summary> 各レアリティの排出確率 </summary>
        [SerializeField] ProbI6InputField m_ifS5;
        [SerializeField] ProbI6InputField m_ifS4;
        [SerializeField] ProbI6InputField m_ifS3;
        [SerializeField] ProbI6InputField m_ifS2;
        [SerializeField] ProbI6InputField m_ifS1;
        /// <summary> 狙っているレアリティ内での狙っているコンテンツの確率 </summary>
        public ProbI6InputField rateInRarity;
        /// <summary> 各レアリティ排出率の合計 </summary>
        public du.dui.TMPArea total;

        /// <summary> ガチャを連続で回すときの間隔[s] </summary>
        public FloatInputField rollInterval;
        /// <summary> OddsをVendingMachineに適用するボタン </summary>
        public UGUI.Button saveButton;

        /// <summary> プリセット設定ボタン群 </summary>
        public GachaConfPresetManager presets;
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
    public interface IVendorConfig {
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

    public class VendorConfig : MonoBehaviour, IVendorConfig {
        #region field
        IDictionary<Rarity, IProbI6InputField> m_inputFields;
        ProbI6InputField m_rateInRarity;
        FloatInputField m_rollInterval;
        du.dui.ITextArea m_total;
        UGUI.Button m_saveButton;
        IGachaConfPresetManager m_presets;

        [SerializeField] VendorConfigSerializeFields m_serialized;
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
            private set {
                var current = TargetRarity;
                if (current != value) {
                    m_inputFields[current].SetIsTarget(false);
                    m_inputFields[value].SetIsTarget(true);
                }
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
                    .Subscribe(conf => Set(conf))
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
            du.Test.LLog.Debug.Log($"CreateVendor : {Odds}, {RateInRarity}");
            return new VendingMachineImpl(TargetRarity, new OddsPreferences(Odds, RateInRarity));
        }
        #endregion

        #region private
        private void Set(IVendorConfig conf) {
            du.Test.Log.IsNull(conf, nameof(conf));
            du.Test.Log.IsNull(conf.Odds, nameof(conf.Odds));
            du.Test.Log.IsNull(m_inputFields, nameof(m_inputFields));
            foreach (Rarity r in ExRarity.Valids) {
                du.Test.Log.IsNull(m_inputFields[r], r.ToString());
                du.Test.Log.IsNull(conf.Odds[r], r.ToString());
                m_inputFields[r].SetProb(conf.Odds[r]);
            }
            RateInRarity = conf.RateInRarity;
            m_rollInterval.Value = conf.RollInterval;
            TargetRarity = conf.TargetRarity;
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
