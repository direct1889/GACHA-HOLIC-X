using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; //UnityWebRequestを使うために必要
using System.Text.RegularExpressions;


namespace Main.Gacha.Accomplish {

    /// <summary> 商品 </summary>
    public class Item {
        #region property
        /// <value> 名称 </value>
        public string Name { get; set; }
        /// <value> 価格 </value>
        public int Price { get; set; }
        #endregion

        #region ctor
        public Item(string name, int price) {
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
        IList<Item> m_items = new List<Item>();

        /// <summary> 公開されたAmazonほしいものリストのURL </summary>
        [SerializeField] string m_url;
        #endregion

        #region getter
        IList<Item> Items => m_items;
        #endregion

        #region mono
        private void Start() {
            StartCoroutine(GetText(m_url));
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
                Debug.Log($"{i}: {mName[i].Groups[1].Value}, {mPrice[i].Groups[1].Value}");
                m_items.Add(new Item(mName[i].Groups[1].Value, int.Parse(mPrice[i].Groups[1].Value)));
            }
        }
        #endregion

    }

}