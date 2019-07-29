using System.Collections.Generic;
using UnityEngine;
using UGUI = UnityEngine.UI;

namespace Main.Gacha.UI {

    /// <summary> レアリティ表示 </summary>
    public class RarityUI : MonoBehaviour, IIndicator<Rarity> {
        #region field
        UGUI.Image m_image;
        IDictionary<Rarity, Sprite> m_sprites = new Dictionary<Rarity, Sprite>();
        [SerializeField] List<Sprite> m_serializedSprites;
        #endregion

        #region mono
        private void Awake() {
            m_sprites.Add(Rarity.S1, m_serializedSprites[0]);
            m_sprites.Add(Rarity.S2, m_serializedSprites[1]);
            m_sprites.Add(Rarity.S3, m_serializedSprites[2]);
            m_sprites.Add(Rarity.S4, m_serializedSprites[3]);
            m_sprites.Add(Rarity.S5, m_serializedSprites[4]);

            m_image = GetComponent<UGUI.Image>();
            Clear();
        }
        #endregion

        #region field
        public void Clear() => gameObject.SetActive(false);
        public void Show(Rarity obj) {
            m_image.sprite = m_sprites[obj];
            if (!gameObject.activeInHierarchy) {
                gameObject.SetActive(true);
            }
        }
        #endregion
    }

}
