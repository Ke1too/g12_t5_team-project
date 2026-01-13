using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class AuthManager : MonoBehaviour
{
    // ★ここから追加：シングルトン設定
    public static AuthManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移しても消えないようにする
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // ★ここまで追加

    // Firebaseのコンソールで取得した「ウェブAPIキー」をInspectorで入力してください
    [SerializeField] private string webApiKey = "ここにAPIキーを貼り付け";

    // ログイン用のAPI URL（Google Identity Toolkit）
    private const string LoginUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=";

    // 取得したIDトークンを保存する変数（これを後のデータ保存で使います）
    public string CurrentIdToken { get; private set; }
    public string CurrentUserId { get; private set; } // User ID (UID)

    // テスト実行用（ゲーム開始時に自動でログインを試みる）
    /*
    void Start()
    {
        // 動作確認のため、適当なメールアドレスとパスワードを入れてみてください
        // ※事前にFirebaseコンソールのAuthenticationでユーザーを作っておくか、
        //   別途「新規登録用API」を叩く必要があります。
        StartCoroutine(SignIn("testuser@example.com", "password123"));
    }
    */

    // ログイン処理の本体
    // ログイン処理（引数に onSuccess と onError を追加）
    public IEnumerator SignIn(string email, string password, Action onSuccess, Action<string> onError)
    {
        AuthRequest requestData = new AuthRequest
        {
            email = email,
            password = password,
            returnSecureToken = true
        };
        string jsonBody = JsonUtility.ToJson(requestData);

        string url = LoginUrl + webApiKey;
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                // ★失敗時：エラー内容を報告する
                Debug.LogError("ログイン失敗: " + request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                // ★成功時：データを保存して、成功報告をする
                AuthResponse responseData = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
                CurrentIdToken = responseData.idToken;
                CurrentUserId = responseData.localId;

                onSuccess?.Invoke();
            }
        }
    }
    // 新規登録用のAPI URL
    private const string SignUpUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=";

    // 新規登録処理（こちらも同様に追加）
    public IEnumerator SignUp(string email, string password, Action onSuccess, Action<string> onError)
    {
        AuthRequest requestData = new AuthRequest
        {
            email = email,
            password = password,
            returnSecureToken = true
        };
        string jsonBody = JsonUtility.ToJson(requestData);

        string url = SignUpUrl + webApiKey;

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                // ★失敗時
                Debug.LogError("登録失敗: " + request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                // ★成功時
                AuthResponse responseData = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
                CurrentIdToken = responseData.idToken;
                CurrentUserId = responseData.localId;

                onSuccess?.Invoke();
            }
        }
    }
}

// --- 以下、JSON変換用のデータクラス ---

[Serializable]
public class AuthRequest
{
    public string email;
    public string password;
    public bool returnSecureToken;
}

[Serializable]
public class AuthResponse
{
    public string kind;
    public string localId;
    public string email;
    public string displayName;
    public string idToken;      // ← これが一番重要！
    public bool registered;
    public string refreshToken;
    public string expiresIn;
}