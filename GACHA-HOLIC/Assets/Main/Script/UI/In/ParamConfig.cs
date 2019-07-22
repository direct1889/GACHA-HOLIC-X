using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UGUI = UnityEngine.UI;
using UniRx;


namespace Main.Gacha.UI {

    /// <summary> 各種パラメータをユーザ入力から受け取るGUI </summary>
    public interface IParamConfig {
        /// <value> 各レアリティの排出率表 </value>
        IReadOnlyDictionary<Rarity, Prob> Odds { get; }
        /// <value> 狙っているレアリティ </value>
        Rarity TargetRarity { get; }
        /// <value> 狙っているレアリティ内での狙っているものの排出確率 </value>
        Prob RateInRarity { get; }

        /// <value> 自動連続ガチャの間隔 </value>
        float RollInterval { get; }

        /// <summary> 自動連続ガチャの間隔 </value>
        IOddsPreferences CreateOddsPref();
        IVendingMachineImpl CreateVendor();
    }

    public class ParamConfig : MonoBehaviour, IParamConfig {
        #region field
        Dictionary<Rarity, IProbInputField> m_odds;

        [SerializeField] ProbInputField m_odds5;
        [SerializeField] ProbInputField m_odds4;
        [SerializeField] ProbInputField m_odds3;
        [SerializeField] ProbInputField m_odds2;
        [SerializeField] ProbInputField m_odds1;
        [SerializeField] ProbInputField m_rateInRarity;

        [SerializeField] FloatInputField m_rollInterval;
        [SerializeField] du.dui.TMPArea m_total;
        [SerializeField] UGUI.Button m_saveButton;
        #endregion

        #region property
        du.dui.ITextArea Total => m_total;
        #endregion

        #region getter
        public IReadOnlyDictionary<Rarity, Prob> Odds { get; private set; }
        public Rarity TargetRarity {
            get {
                foreach (var i in m_odds) {
                    if (i.Value.IsTarget) { return i.Key; }
                }
                return Rarity.None;
            }
        }
        public Prob RateInRarity => m_rateInRarity.Prob;
        public float RollInterval => m_rollInterval.Value;
        #endregion

        #region mono
        private void Awake() {
            if (m_odds is null) {
                m_odds = new Dictionary<Rarity, IProbInputField>{
                    { Rarity.S1, m_odds1 },
                    { Rarity.S2, m_odds2 },
                    { Rarity.S3, m_odds3 },
                    { Rarity.S4, m_odds4 },
                    { Rarity.S5, m_odds5 },
                };
                Odds = new Dictionary<Rarity, Prob>{
                    { Rarity.S1, m_odds1.Prob },
                    { Rarity.S2, m_odds2.Prob },
                    { Rarity.S3, m_odds3.Prob },
                    { Rarity.S4, m_odds4.Prob },
                    { Rarity.S5, m_odds5.Prob },
                };

                // TODO: 初期値設定(後ほど消します)
                SetOdds(0, 23890, 67530, 5580, 3000);
                m_rateInRarity.SetProb(33333);
                CheckUpTotalOne();

                foreach (var i in m_odds.Values) {
                    i.OnProbChanged
                        .Subscribe(_ => CheckUpTotalOne())
                        .AddTo(this);
                }
            }
        }
        #endregion

        #region public
        public IOddsPreferences CreateOddsPref() {
            return new OddsPreferences(Odds, RateInRarity);
        }
        public IVendingMachineImpl CreateVendor() {
            return new VendingMachineImpl(TargetRarity, CreateOddsPref());
        }
        #endregion

        #region private
        private void SetOdds(int s1, int s2, int s3, int s4, int s5) {
            m_odds[Rarity.S1].SetProb(s1);
            m_odds[Rarity.S2].SetProb(s2);
            m_odds[Rarity.S3].SetProb(s3);
            m_odds[Rarity.S4].SetProb(s4);
            m_odds[Rarity.S5].SetProb(s5);
        }
        /// <summary> Oddsの合計が1になっているか </summary>
        private void CheckUpTotalOne() {
            var total = Prob.AccurateTotal(m_odds.Values.Select(pif => pif.Prob));
            Total.Text = $"Total : {total}";
            m_saveButton.enabled = Prob.TotalIsOne(total);
        }
        #endregion
    }

}
