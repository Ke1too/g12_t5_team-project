using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    [SerializeField] private string nextSceneName = "GachaScene";

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);

        statusText.text = "メールとパスワードを入力してください";
        statusText.color = Color.white;
    }

    // ログインボタン処理
    private void OnLoginClicked()
    {
        string email = emailInput.text;
        string pass = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
        {
            SetStatus("メールアドレスまたはパスワードが空です", Color.red);
            return;
        }

        SetStatus("ログイン中...", Color.yellow);

        // AuthManagerのSignInを呼び出す
        StartCoroutine(AuthManager.Instance.SignIn(
            email,
            pass,
            () => // 成功時の処理
            {
                SetStatus("ログイン成功！", Color.green);
                Invoke("LoadNextScene", 1.0f);
            },
            (error) => // 失敗時の処理
            {
                SetStatus("ログイン失敗: " + error, Color.red);
            }
        ));
    }

    // 新規登録ボタン処理
    private void OnRegisterClicked()
    {
        string email = emailInput.text;
        string pass = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
        {
            SetStatus("メールアドレスまたはパスワードが空です", Color.red);
            return;
        }

        SetStatus("登録処理中...", Color.yellow);

        // AuthManagerのSignUpを呼び出す
        StartCoroutine(AuthManager.Instance.SignUp(
            email,
            pass,
            () => // 成功時の処理
            {
                SetStatus("登録成功！ログインします...", Color.green);
                Invoke("LoadNextScene", 1.0f);
            },
            (error) => // 失敗時の処理
            {
                SetStatus("登録失敗: " + error, Color.red);
            }
        ));
    }

    private void SetStatus(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
