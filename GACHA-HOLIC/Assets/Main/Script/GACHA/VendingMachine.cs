using UnityEngine;
using UniRx;
using System;


namespace Main.Gacha {

    /// <summary> 読み取り専用Reactiveガチャ </summary>
    public interface IROReVendingMachine {
        /// <summary> ガチャ回したぜストリーム </summary>
        IObservable<IResult> OnRoll { get; }
        /// <summary> 詳細付きガチャ回したぜストリーム </summary>
        IObservable<(IResult, int)> OnRollDetail { get; }
        /// <summary> 今、何連目? </summary>
        int RollCount { get; }
    }

    /// <summary> ガチャ </summary>
    public class VendingMachine : MonoBehaviour, IROReVendingMachine {
        #region field
        /// <summary> ガチャの実体 </summary>
        IVendingMachineImpl m_vendor;
        /// <summary> ガチャを回し続けるストリームをキャッシュ </summary>
        IDisposable m_rollStream;
        /// <summary> ガチャ回したぜストリーム </summary>
        Subject<IResult> m_onRollStream = new Subject<IResult>();
        Subject<(IResult, int)> m_onRollStream2 = new Subject<(IResult, int)>();

        /// <summary> 今、何連目? </summary>
        public int RollCount { get; private set; } = 0;

        /// <summary> 各種設定項目 </summary>
        [SerializeField] UI.ParamConfig m_params;
        #endregion

        #region getter
        /// <summary> ガチャ回したぜストリーム </summary>
        public IObservable<IResult> OnRoll => m_onRollStream;
        public IObservable<(IResult, int)> OnRollDetail => m_onRollStream2;
        #endregion

        #region mono
        private void Awake() {
            m_vendor = m_params.CreateVendor();
        }
        #endregion

        #region public
        /// <summary> 単発 </summary>
        public void RollOnce() => Roll();
        /// <summary> 出るまで引けば確定 </summary>
        public void RollEndless() {
            if (m_rollStream is null) {
                // TODO: Timing
                m_vendor = m_params.CreateVendor();
                m_rollStream = Observable
                    .Interval(TimeSpan.FromSeconds(m_params.RollInterval))
                    .Subscribe(_ => {
                        if (Roll().IsWants) { StopRolling(); }
                    })
                    .AddTo(this);
            }
            else {
                StopRolling();
            }
        }
        #endregion

        #region private
        /// <summary> ガチャを回す </summary>
        private IResult Roll() {
            var result = m_vendor.Roll();
            du.Test.LLog.Debug.Log($"[{++RollCount}]Rolled... [{result}]");
            m_onRollStream.OnNext(result);
            m_onRollStream2.OnNext((result, RollCount));
            return result;
        }
        /// <summary> 連続ガチャを停止 </summary>
        private void StopRolling() {
            m_rollStream.Dispose();
            m_rollStream = null;
            RollCount = 0;
        }
        #endregion
    }

}
