using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; //UnityWebRequestを使うために必要
using System.Text.RegularExpressions;


namespace Main.Gacha.Accomplish {

    /// <summary> 商品 </summary>
    public class WishItem {
        #region property
        /// <value> 名称 </value>
        public string Name { get; set; }
        /// <value> 価格 </value>
        public int Price { get; set; }
        #endregion

        #region ctor
        public WishItem(string name, int price) {
            Name = name; Price = price;
        }
        #endregion
    }

    /// <summary> ほしいものリスト </summary>
    public class WishList : MonoBehaviour {

        #region constant
        /// <summary>
        /// 商品名を抜き出すRE
        /// - ^を行頭の意味では使用できないので注意 (先頭に1つ出現するのみ)
        /// </summary>
        static readonly Regex c_reName = new Regex("\n<div class[^\n]*img alt=\"([^\"]*)\"");
        /// <summary> 価格を抜き出すRE </summary>
        static readonly Regex c_rePrice = new Regex("￥([0-9]+)");
        #endregion

        #region field
        /// <summary> ほしい商品たち </summary>
        IList<WishItem> m_items = new List<WishItem>();

        /// <summary> 公開されたAmazonほしいものリストのURL </summary>
        [SerializeField] string m_url;
        /// <summary> ほしいものを一覧表示するスクロールビュー </summary>
        [SerializeField] WishListScrollView m_scrollView;
        /// <summary> 何点変えたか表示 </summary>
        [SerializeField] TMPro.TMP_Text m_availableItemNum;
        #endregion

        #region getter
        private IList<WishItem> Items => m_items;
        #endregion

        #region mono
        private void Start() {
            StartCoroutine(GetText(m_url));
        }
        #endregion

        #region public
        public void CreateScrollView() {
            int totalSpent = 3000;
            int nonAvailableItemNum = -1;
            for (int i = 0; i < m_items.Count; ++i) {
                m_scrollView.GenerateWishItemRect(m_items[i]);
                // 所持金合計からこの商品の価格を引く
                totalSpent = totalSpent - m_items[i].Price;
                if (totalSpent < 0) { // 所持金が尽きたら買い残した商品数を記録
                    nonAvailableItemNum = m_items.Count - i;
                }
            }
            // 買い残しなし
            if (nonAvailableItemNum <= 0) {
                m_availableItemNum.text = "すべて";
            }
            // 買い残しあり
            else {
                m_availableItemNum.text = $"{m_items.Count}点中{m_items.Count - nonAvailableItemNum}点({(float)nonAvailableItemNum / m_items.Count})";
            }
        }
        #endregion

        #region private
        /// <summary> ネットワーク上からほしいものリストを取得 </summary>
        private IEnumerator GetText(string url) {
            // 取得したいサイトURLを指定
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }

            // Debug.LogError(www.downloadHandler.text);
            FindItems(www.downloadHandler.text);
        }

        /// <summary> ほしいものリストのhtmlソースから商品情報を抽出 </summary>
        private void FindItems(string htmlText) {
            var mName = c_reName.Matches(htmlText);
            var mPrice = c_rePrice.Matches(htmlText);
            for (int i = 0; i < mName.Count; ++i) {
                m_items.Add(new WishItem(mName[i].Groups[1].Value, int.Parse(mPrice[i].Groups[1].Value)));
            }
        }
        #endregion

    }

}