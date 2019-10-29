using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace Main.Gacha {

    /// <summary> フェス(=一連のガチャ)の結果管理 </summary>
    public interface IAccomplishResultMgr {
        /// <summary> ガチャの結果を記録 </summary>
        void Rolled(Rarity rarity, int rollCount);
    }

    /// <summary> フェス(=一連のガチャ)の結果管理 </summary>
    public class AccomplishResultMgr : MonoBehaviour, IAccomplishResultMgr {
        #region module
        /// <summary> フェス結果画面の表示項目群 </summary>
        [Serializable]
        private struct Columns {
            /// <summary> ガチャ回数 </summary>
            public UI.ResultColumn rollCount;
            /// <summary> ☆5出現数 </summary>
            public UI.ResultColumn s5s;
            /// <summary> ☆4出現数 </summary>
            public UI.ResultColumn s4s;
            /// <summary> ☆3出現数 </summary>
            public UI.ResultColumn s3s;
            /// <summary> ☆2出現数 </summary>
            public UI.ResultColumn s2s;
            /// <summary> ☆1出現数 </summary>
            public UI.ResultColumn s1s;
            /// <summary> 石消費数 </summary>
            public UI.ResultColumn spentIshis;
            /// <summary> 消費金額 </summary>
            public UI.ResultColumn spentMoney;

            /// <summary> 指定レアリティの出現回数 </summary>
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
        /// <summary> 各レアリティの出現回数 </summary>
        IDictionary<Rarity, int> m_count = new Dictionary<Rarity, int>{
            { Rarity.S5, 0 },
            { Rarity.S4, 0 },
            { Rarity.S3, 0 },
            { Rarity.S2, 0 },
            { Rarity.S1, 0 },
        };
        float m_spentMoneyCache;

        /// <summary> フェス結果画面の表示項目群 </summary>
        [SerializeField] Columns m_columns;
        /// <summary> 設定 </summary>
        [SerializeField] UI.AccomplishResultConfig m_config;
        /// <summary> ガチャマシン </summary>
        [SerializeField] VendingMachine m_vendor;

        [SerializeField] Main.Gacha.Accomplish.WishList m_wishList;
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
            m_spentMoneyCache = m_config.IshiPrice * ishis;
            m_columns.spentMoney.SetValue(m_spentMoneyCache);
        }

        public void OpenWishList() {
            m_wishList.gameObject.SetActive(true);
            m_wishList.CreateScrollView((int)m_spentMoneyCache);
        }
        #endregion
    }

}
