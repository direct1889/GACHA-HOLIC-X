using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace Main.Gacha {
    public interface IAccomplishResultMgr {
        /// <summary> ガチャの結果を記録 </summary>
        void Rolled(Rarity rarity, int rollCount);
    }

    public class AccomplishResultMgr : MonoBehaviour, IAccomplishResultMgr {
        #region module
        [Serializable]
        private struct Columns {
            public UI.ResultColumn rollCount;
            public UI.ResultColumn s5s;
            public UI.ResultColumn s4s;
            public UI.ResultColumn s3s;
            public UI.ResultColumn s2s;
            public UI.ResultColumn s1s;
            public UI.ResultColumn spentIshis;
            public UI.ResultColumn spentMoney;

            public UI.ResultColumn CountOfRarity(Rarity rarity) {
                switch (rarity) {
                    case Rarity.S5: return s5s;
                    case Rarity.S4: return s4s;
                    case Rarity.S3: return s3s;
                    case Rarity.S2: return s2s;
                    case Rarity.S1: return s1s;
                    default       : return null;
                }
            }
        }
        #endregion

        #region field
        IDictionary<Rarity, int> m_count = new Dictionary<Rarity, int>{
            { Rarity.S5, 0 },
            { Rarity.S4, 0 },
            { Rarity.S3, 0 },
            { Rarity.S2, 0 },
            { Rarity.S1, 0 },
        };
        [SerializeField] Columns m_columns;
        [SerializeField] UI.AccomplishResultConfig m_config;
        [SerializeField] VendingMachine m_vendor;
        #endregion

        #region mono
        private void Awake() {
            m_vendor.OnRollDetail
                .Subscribe(resAndN => {
                    Rolled(resAndN.Item1.Content.rarity, resAndN.Item2);
                })
                .AddTo(this);
        }
        #endregion

        #region public
        /// <summary> ガチャの結果を記録 </summary>
        public void Rolled(Rarity rarity, int rollCount) {
            m_columns.rollCount.SetValue(rollCount);
            if (rollCount == 1) {
                foreach (var i in ExRarity.Valids) {
                    m_count[i] = 0;
                }
            }
            m_columns.CountOfRarity(rarity).SetValue(++m_count[rarity]);
            var ishis = m_config.NumOfIshiNth(rollCount);
            m_columns.spentIshis.SetValue(ishis);
            m_columns.spentMoney.SetValue(m_config.IshiPrice * ishis);
        }
        #endregion
    }

}
