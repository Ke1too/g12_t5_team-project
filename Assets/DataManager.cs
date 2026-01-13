using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // ★重要：Firebaseコンソールの「Realtime Database」にあるURLをここに貼り付けてください
    // 例: "https://your-project-id-default-rtdb.firebaseio.com/"
    // ※末尾のスラッシュはあってもなくても自動調整するようにコード側で対応します
    [SerializeField] private string databaseUrl = "ここにデータベースのURLを貼り付け";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------------------------------------------------------
    // データを保存する機能 (REST API: PATCH または PUT)
    // ---------------------------------------------------------
    public IEnumerator SaveItem(string itemId, Action onSuccess, Action<string> onError)
    {
        // 1. 保存するデータの作成（JSON形式）
        // 例: { "item_01": true } という形で保存します
        string jsonBody = "{\"" + itemId + "\": true}";

        // 2. URLの組み立て
        // 構造: https://[DB_URL]/users/[USER_ID]/collection.json?auth=[ID_TOKEN]
        string userId = AuthManager.Instance.CurrentUserId;
        string idToken = AuthManager.Instance.CurrentIdToken;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(idToken))
        {
            onError?.Invoke("ログインしていません");
            yield break;
        }

        // 末尾のスラッシュ調整
        string baseUrl = databaseUrl.TrimEnd('/');
        string url = $"{baseUrl}/users/{userId}/collection.json?auth={idToken}";

        // 3. リクエストの送信 (PATCHを使うと、既存データを消さずに追記・更新できます)
        using (UnityWebRequest request = new UnityWebRequest(url, "PATCH"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("データ保存失敗: " + request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                Debug.Log("データ保存成功: " + request.downloadHandler.text);
                onSuccess?.Invoke();
            }
        }
    }

    // ---------------------------------------------------------
    // データを読み込む機能 (REST API: GET)
    // ---------------------------------------------------------
    public IEnumerator LoadCollection(Action<Dictionary<string, bool>> onSuccess, Action<string> onError)
    {
        // 1. URLの組み立て
        string userId = AuthManager.Instance.CurrentUserId;
        string idToken = AuthManager.Instance.CurrentIdToken;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(idToken))
        {
            onError?.Invoke("ログインしていません");
            yield break;
        }

        string baseUrl = databaseUrl.TrimEnd('/');
        string url = $"{baseUrl}/users/{userId}/collection.json?auth={idToken}";

        // 2. リクエストの送信 (GET)
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("データ取得失敗: " + request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                string jsonText = request.downloadHandler.text;
                Debug.Log("取得データ: " + jsonText);

                // データが空（null）の場合の対応
                if (jsonText == "null")
                {
                    onSuccess?.Invoke(new Dictionary<string, bool>()); // 空の辞書を返す
                }
                else
                {
                    // JSONを辞書型に変換して返す
                    // シンプルな {"item_A": true, "item_B": true} の形を想定
                    // Unity標準のJsonUtilityはDictionaryに直接対応していないため、簡易的なパーサーを使うか、
                    // ここでは簡単な処理としてライブラリを使わず整形済みとみなして扱います。
                    // ※本来は 'Newtonsoft.Json' などが便利ですが、標準機能でやるなら以下のように処理します（簡易版）

                    var collectionData = ParseSimpleJson(jsonText);
                    onSuccess?.Invoke(collectionData);
                }
            }
        }
    }

    // 簡易的なJSONパーサー（{"key":true, "key2":true} の形専用）
    private Dictionary<string, bool> ParseSimpleJson(string json)
    {
        var result = new Dictionary<string, bool>();
        // 波括弧と引用符を削除してカンマで分割
        string clean = json.Replace("{", "").Replace("}", "").Replace("\"", "");
        string[] pairs = clean.Split(',');

        foreach (var pair in pairs)
        {
            string[] keyValue = pair.Split(':');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim();
                if (bool.TryParse(keyValue[1].Trim(), out bool val))
                {
                    result[key] = val;
                }
            }
        }
        return result;
    }
}