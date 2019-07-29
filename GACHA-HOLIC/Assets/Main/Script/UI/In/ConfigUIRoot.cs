using UnityEngine;

namespace Main.Gacha.UI {

    /// <summary>
    /// パラメータ設定画面の表示有無
    /// - TODO: ひとまず非アクティブではなく画面外移動で対処中
    /// </summary>
    public class ConfigUIRoot : MonoBehaviour {
        RectTransform m_recT;

        private void Awake() {
            m_recT = GetComponent<RectTransform>();
        }

        public void Activate() {
            m_recT.position = new Vector3(1080, 1920, 0);
            // gameObject.SetActive(true);
        }
        public void Inactivate() {
            m_recT.position = new Vector3(-1080, 1920, 0);
            // gameObject.SetActive(false);
        }
    }

}
