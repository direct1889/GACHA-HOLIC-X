using UnityEngine;
using UniRx;

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
                    if (result.IsWants) {
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
            m_source.Play();
        }
        private void Fanfare(Rarity rarity) {
            switch (rarity) {
                case Rarity.S1: { m_source.Play(); return; }
                case Rarity.S2: { m_source.Play(); return; }
                case Rarity.S3: { m_source.Play(); return; }
                case Rarity.S4: { m_source.Play(); return; }
                case Rarity.S5: { m_source.Play(); return; }
                default       : { m_source.Play(); return; }
            }
        }
        #endregion
    }

}
