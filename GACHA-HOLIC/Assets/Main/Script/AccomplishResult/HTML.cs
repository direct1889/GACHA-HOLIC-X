using System;
using System.IO;
using System.Net;

namespace du.Cmp {

    public static class HTML {

        public static void Download(string url) {
            try {
                // WebClientを作成
                WebClient wc = new WebClient();

                // WebClientからStreamとStreamReaderを作成
                // args[0]にはURLが入っているものとする
                Stream st = wc.OpenRead(url);
                StreamReader sr = new StreamReader(st);

                // リソースからすべて読み取ってコンソールに書き出す
                // UnityEngine.Debug.Log(sr.ReadToEnd());
                UnityEngine.Debug.Log(sr.ReadToEnd().Length);
                // Console.WriteLine(sr.ReadToEnd());

                // StreamとStreamReaderを閉じる
                sr.Close();
                st.Close();

            }
            catch (Exception e) {
                // URLのファイルが見つからない等のエラーが発生
                Console.WriteLine("エラーが発生しました\r\n\r\n" + e.ToString());
            }

            return;
        }
    }
}
