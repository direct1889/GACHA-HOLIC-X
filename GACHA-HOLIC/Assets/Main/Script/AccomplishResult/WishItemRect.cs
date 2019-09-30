using UnityEngine;

namespace Main.Gacha.Accomplish {

    public class WishItemRect : MonoBehaviour {
        #region field
        WishItem m_item;

        [SerializeField] TMPro.TMP_Text m_name;
        [SerializeField] TMPro.TMP_Text m_price;
        #endregion

        #region public
        public void Initialize(string name, int price) {
            m_name.text = name;
            m_price.text = price.ToString();
        }
        #endregion
    }

}
