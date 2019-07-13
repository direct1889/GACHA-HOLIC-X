using UnityEngine;
using UniRx;

namespace Main.Gacha.UI {

    /// <summary> ガチャ全体の表示 </summary>
    public class VendingMachineUI : MonoBehaviour {
        #region field
        IIndicator<IResult> Result { get; set; }
        TextIndicator<int> Number { get; set; }
        #endregion

        #region mono
        private void Awake() {
            Result = GetComponentInChildren<IIndicator<IResult>>();
            Number = GetComponentInChildren<TextIndicator<int>>();
            Number.Converter = (n => $"{n}連目");
            GetComponent<IROReVendingMachine>()
                .OnRollDetail
                .Subscribe(resultAndN => Show(resultAndN.Item1, resultAndN.Item2))
                .AddTo(this);
        }
        #endregion

        #region public
        public void Clear() {
            Result.Clear();
        }
        #endregion

        #region private
        private void Show(IResult result, int n) {
            Result.Show(result);
            Number.Show(n);
        }
        #endregion
    }

}
