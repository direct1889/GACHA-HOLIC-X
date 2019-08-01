
namespace du.Test {

    public interface IDebugAssistant {
        TestLogger TestLog { get; }
        void SetTestLog(TestLogger log);
    }

    public class DebugAssistant : Cmp.SingletonMonoBehaviour<DebugAssistant>, IDebugAssistant {
        #region field
        ITestCode m_test;
        #endregion

        #region field property
        public TestLogger TestLog { get; private set; }
        #endregion

        #region mono
        private void Awake() {
            LLog.Boot.Log("DebugAssistant awoke.");
            Instance.m_test = new TestCodeCalledByAppMgr();
        }
        private void Start() => Instance.m_test?.OnStart();
        private void Update() => m_test?.OnUpdate();
        #endregion

        #region public
        public void SetTestLog(TestLogger log) => TestLog = log;
        #endregion

    }

}
