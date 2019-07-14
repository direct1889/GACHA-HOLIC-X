using System.Collections.Generic;
using UnityEngine;


namespace Main.Gacha.UI {

    public interface IParamConfig {
        IReadOnlyDictionary<Rarity, Prob> Odds { get; }
        Rarity TargetRarity { get; }
        Prob RateInRarity { get; }

        float RollInterval { get; }

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
        #endregion

        #region getter
        public IReadOnlyDictionary<Rarity, Prob> Odds
            => new Dictionary<Rarity, Prob>{
                { Rarity.S1, m_odds1.Prob },
                { Rarity.S2, m_odds2.Prob },
                { Rarity.S3, m_odds3.Prob },
                { Rarity.S4, m_odds4.Prob },
                { Rarity.S5, m_odds5.Prob }
            };
        public Rarity TargetRarity {
            get {
                foreach (var i in m_odds) {
                    Debug.LogError($"{i.Key} is {i.Value.IsTarget}");
                }
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
                SetOdds(0, 23890, 67530, 5580, 3000);
                m_rateInRarity.SetProb(33333);
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
        #endregion
    }

}
