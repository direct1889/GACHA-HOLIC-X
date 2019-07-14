using UnityEngine;

namespace Main.Gacha.UI {

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
