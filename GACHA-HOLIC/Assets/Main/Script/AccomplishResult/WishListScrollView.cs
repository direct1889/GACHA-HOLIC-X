using UnityEngine;

namespace Main.Gacha.Accomplish {

    public interface IWishListScrollView {
        /// <summary> ほしいものContentのパネルを生成 </summary>
        void GenerateWishItemRect(WishItem item);
    }

    public class WishListScrollView : MonoBehaviour, IWishListScrollView {
        #region field
        [SerializeField] GameObject m_content;
        [SerializeField] GameObject m_wishItemPref;
        #endregion

        #region public
        public void GenerateWishItemRect(WishItem item) {
            Instantiate(m_wishItemPref, m_content.transform)
                .GetComponent<WishItemRect>()
                .Initialize(item.Name, item.Price);
        }
        #endregion
    }

}
