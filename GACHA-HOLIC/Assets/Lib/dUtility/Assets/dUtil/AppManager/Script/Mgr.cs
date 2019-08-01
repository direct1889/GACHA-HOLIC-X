
namespace du {

    /// <summary> SingletonMonoBehaviour の Manager たち </summary>
    public static class Mgr {
        #region getter
        public static App.IAppManager      App   => du.App.AppManager    .Instance;
        public static Test.IDebugAssistant Debug => Test.DebugAssistant  .Instance;
        public static di.TouchMgr          Touch => di.TouchMgr          .Instance;
        public static Audio.IAudioManager  Audio => du.Audio.AudioManager.Instance;
    //  public static App.OSUI             OSUI  => du.App.OSUI          .Instance;
        #endregion
    }

    /// <summary> SingletonMonoBehaviour の Asset たち </summary>
    public static class Asset {
        #region getter
        public static Audio.ISoundAsset Sound => du.Audio.SoundAsset.Instance;
        #endregion
    }

}
