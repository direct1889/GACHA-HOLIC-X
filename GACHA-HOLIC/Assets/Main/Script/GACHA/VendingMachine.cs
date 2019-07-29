using UnityEngine;
using UniRx;
using System;
using System.Collections.Generic;


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
        #region const
        private static readonly float intervalLowerLimit = 0.03f;
        #endregion

        #region field
        /// <summary> ガチャの実体 </summary>
        IVendingMachineImpl m_vendor;
        /// <summary> ガチャを回し続けるストリームをキャッシュ </summary>
        IDisposable m_rollStream;
        /// <summary> ガチャ回したぜストリーム </summary>
        Subject<IResult> m_onRollStream = new Subject<IResult>();
        Subject<(IResult, int)> m_onRollStream2 = new Subject<(IResult, int)>();

        /// <summary> 現在の回し間隔[s] </summary>
        float m_interval = 1f;
        float Interval {
            get => m_interval;
            set => m_interval = Mathf.Max(value, intervalLowerLimit);
        }
        /// <summary> intervalを適用するタイミング </summary>
        int NextAccelTiming => m_accelTimings[m_accelTimingIt];
        /// <summary> intervalを適用するタイミングイテレータ </summary>
        int m_accelTimingIt = 0;
        /// <summary> intervalを適用するタイミングリスト </summary>
        IReadOnlyList<int> m_accelTimings = new List<int>{
            10, 15, 20, 25, 30, 50, 70, 100
        };
        /// <summary> 加速リスト </summary>
        readonly float m_speedUpTheTime = 0.8f;

        /// <summary> 今、何連目? </summary>
        public int RollCount { get; private set; } = 0;

        /// <summary> 各種設定項目 </summary>
        [SerializeField] UI.VendorConfig m_params;
        #endregion

        #region getter
        /// <summary> ガチャ回したぜストリーム </summary>
        public IObservable<IResult> OnRoll => m_onRollStream;
        public IObservable<(IResult, int)> OnRollDetail => m_onRollStream2;

        private bool IsRolling => !(m_rollStream is null);
        #endregion

        #region mono
        private void Start() {
            CheckUpVendor();
            Interval = m_params.RollInterval;
            OnRollDetail
                .Subscribe(pair => {
                    if (pair.Item2 == NextAccelTiming) {
                        ChangeIntervalDinamically();
                        RollEndless();
                        du.Test.LLog.Debug.LogR($"interval = {Interval}");
                    }
                })
                .AddTo(this);
        }
        #endregion

        #region public
        /// <summary> 単発 </summary>
        public void RollOnce() => Roll();
        /// <summary> 出るまで回す </summary>
        public void RollEndless() {
            if (IsRolling) { StopRolling(); }
            // TODO: Timing
            CheckUpVendor();
            m_rollStream = Observable
                .Interval(TimeSpan.FromSeconds(Interval))
                .Subscribe(_ => {
                    if (Roll().IsWants == IsWants.Win) { WonRolling(); }
                })
                .AddTo(this);
        }
        public void CheckUpVendor() {
            m_vendor = m_params.CreateVendor();
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
        }
        /// <summary> ガチャで勝利した </summary>
        private void WonRolling() {
            StopRolling();
            RollCount = 0;
            Interval = m_params.RollInterval;
        }
        /// <summary> 回してる途中でintervalを変更 </summary>
        /// <param name="interval"> [VendingMachine.intervalLowerLimit, ∞) </param>
        private void ChangeIntervalDinamically() {
            Interval *= m_speedUpTheTime;
            if (m_accelTimingIt < m_accelTimings.Count - 1) {
                ++m_accelTimingIt;
            }
        }
        #endregion
    }

}
