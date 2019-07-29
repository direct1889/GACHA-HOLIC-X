using UnityEngine;
using UniRx;
using System;
using System.Collections.Generic;


namespace Main.Gacha {

    /// <summary> ガチャの回し間隔管理 </summary>
    public class RollIntervalMgr {
        #region const
        private static readonly float intervalLowerLimit = 0.03f;
        /// <summary> 加速リスト </summary>
        private static readonly float speedUpRate = 0.8f;
        /// <summary> intervalを適用するタイミングリスト </summary>
        private static IReadOnlyList<int> accelTimings = new List<int>{
            10, 15, 20, 25, 30, 50, 70, 100
        };
        #endregion

        #region field
        /// <summary> 現在の回し間隔[s] </summary>
        float m_interval = 1f;
        private float Interval {
            get => m_interval;
            set => m_interval = Mathf.Max(value, intervalLowerLimit);
        }
        /// <summary> intervalを適用するタイミング </summary>
        public int NextAccelTiming => accelTimings[m_accelTimingIt];
        /// <summary> intervalを適用するタイミングイテレータ </summary>
        int m_accelTimingIt = 0;
        #endregion

        #region getter
        public TimeSpan IntervalSec => TimeSpan.FromSeconds(Interval);
        #endregion

        #region ctor
        /// <param name="initialInterval">
        /// 回し間隔[s]
        /// - [VendingMachine.intervalLowerLimit, ∞)
        /// </param>
        public RollIntervalMgr(float initialInterval) {
            Interval = initialInterval;
        }
        #endregion

        #region public
        /// <summary> 回してる途中でintervalを変更 </summary>
        public bool CheckAndUpdate(int rollCount) {
            if (rollCount == NextAccelTiming) {
                du.Test.LLog.Debug.LogR($"interval = {Interval}");
                if (m_accelTimingIt < accelTimings.Count - 1) { ++m_accelTimingIt; };
                Interval *= speedUpRate;
                return true;
            }
            else { return false; }
        }
        #endregion
    }

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
        /// <summary> ガチャの回し間隔管理 </summary>
        RollIntervalMgr m_intervalMgr;

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
            m_intervalMgr = new RollIntervalMgr(m_params.RollInterval);
            OnRollDetail
                .Where(pair => m_intervalMgr.CheckAndUpdate(pair.Item2))
                .Subscribe(_ => { PauseRolling(); RollEndless(); })
                .AddTo(this);
        }
        #endregion

        #region public
        /// <summary> 単発 </summary>
        public void RollOnce() => Roll();
        /// <summary> 出るまで回す </summary>
        public void RollEndless() {
            // すでに回っているなら何もしない
            if (IsRolling) { return; }

            // TODO: Timing
            CheckUpVendor();
            m_rollStream = Observable
                .Interval(m_intervalMgr.IntervalSec)
                .Subscribe(_ => {
                    if (Roll().IsWants == IsWants.Win) { WonRolling(); }
                })
                .AddTo(this);
        }
        public void CheckUpVendor() {
            m_vendor = m_params.CreateVendor();
        }
        /// <summary> 止まっていれば回す、回していれば一時停止 </summary>
        public void RollEndlessOrPause() {
            if (IsRolling) { PauseRolling(); }
            else { RollEndless(); }
        }
        /// <summary>
        /// ガチャをリセット
        /// - n連回数、回し速度ともにリセット
        /// </summary>
        public void Reset() {
            PauseRolling();
            RollCount = 0;
            m_intervalMgr = new RollIntervalMgr(m_params.RollInterval);
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
        /// <summary> ガチャで勝利した </summary>
        private void WonRolling() {
            Reset();
        }
        /// <summary>
        /// 連続ガチャを一時停止
        /// - n連回数、回し速度ともに維持
        /// </summary>
        private void PauseRolling() {
            m_rollStream.Dispose();
            m_rollStream = null;
        }
#if false
        /// <summary>
        /// 連続ガチャを一時停止
        /// - n連回数維持、回し速度リセット
        /// </summary>
        private void StopRolling() {
            m_rollStream.Dispose();
            m_rollStream = null;
            m_intervalMgr = new RollIntervalMgr(m_params.RollInterval);
        }
#endif
        #endregion
    }

}
