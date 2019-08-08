using UnityEngine;
using UniRx;
using static Main.Gacha.ExRarity;

namespace Main.Gacha.Audio {

    public class VendingMachineAudio : MonoBehaviour {
        #region field
        AudioSource m_source;
        #endregion

        #region mono
        private void Awake() {
            m_source = GetComponent<AudioSource>();
            var vm = GetComponent<IROReVendingMachine>();
            vm  .OnRoll
                .Subscribe(result => {
                    if (result.IsWants == IsWants.Win) {
                        FanfareForWants();
                    }
                    else {
                        Fanfare(result.Content.rarity);
                    }
                })
                .AddTo(this);
        }
        #endregion

        #region private
        private void FanfareForWants() {
            du.Mgr.Audio[du.Audio.Category.MainSE].Play("Win");
        }
        private void Fanfare(Rarity rarity) {
            du.Mgr.Audio[du.Audio.Category.MainSE].Play(rarity.ToStr());
        }
        #endregion
    }

}
