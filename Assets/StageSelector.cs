using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // 新Input System

public class StageSelector : MonoBehaviour
{
    [Header("矢印マーカー（UI RectTransform）")]
    public RectTransform arrow;

    [Header("ステージの丸（UI RectTransform）")]
    public RectTransform[] stagePoints;

    [Header("ステージ名（ラベル用 / 判定用）")]
    public string[] stageSceneNames;

    [Header("矢印のオフセット（丸の少し上に置く）")]
    public Vector2 arrowOffset = new Vector2(0, 80f);

    [Header("SE")]
    public AudioSource seSource;     // StageSelectManagerにAudioSourceを付けてここに入れる
    public AudioClip moveSE;         // 移動音
    public AudioClip decideSE;       // 決定音
    public float decideDelay = 0.15f; // 決定音を鳴らしてから遷移する待ち時間

    private int currentIndex = 0;
    private bool isDeciding = false;

    void Start()
    {
        if (stagePoints == null || stagePoints.Length == 0)
        {
            Debug.LogError("stagePoints が空です。丸の RectTransform を配列に入れてください。");
            return;
        }

        ClampArrays();
        UpdateArrowPosition();
    }

    void Update()
    {
        if (isDeciding) return;

        // 右キー：次へ
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            currentIndex = (currentIndex + 1) % stagePoints.Length;
            UpdateArrowPosition();
            PlayMoveSE();
        }

        // 左キー：前へ
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            currentIndex = (currentIndex - 1 + stagePoints.Length) % stagePoints.Length;
            UpdateArrowPosition();
            PlayMoveSE();
        }

        // Enter：決定
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            isDeciding = true;
            PlayDecideSE();
            Invoke(nameof(LoadSelectedStage), decideDelay);
        }
    }

    void ClampArrays()
    {
        // stagePoints と stageSceneNames の長さがズレていると事故るので警告
        if (stageSceneNames == null || stageSceneNames.Length == 0)
        {
            Debug.LogWarning("stageSceneNames が空です。LoadSelectedStage の判定ができません。");
        }
        else if (stageSceneNames.Length != stagePoints.Length)
        {
            Debug.LogWarning($"stagePoints({stagePoints.Length}) と stageSceneNames({stageSceneNames.Length}) の数が違います。");
            // 数が違っても動かすため、読み取りは stageSceneNames の範囲チェックで守る
        }
    }

    void UpdateArrowPosition()
    {
        if (arrow == null)
        {
            Debug.LogError("arrow が未設定です。矢印の RectTransform を入れてください。");
            return;
        }

        // 同じCanvas配下のUIなら anchoredPosition がズレにくい
        arrow.anchoredPosition = stagePoints[currentIndex].anchoredPosition + arrowOffset;
    }

    void LoadSelectedStage()
    {
        // stageSceneNames が無い場合は止める
        if (stageSceneNames == null || currentIndex < 0 || currentIndex >= stageSceneNames.Length)
        {
            Debug.LogWarning("stageSceneNames の設定が足りない/Indexが範囲外です。");
            isDeciding = false;
            return;
        }

        string stageName = stageSceneNames[currentIndex];
        Debug.Log($"選択されたステージ: {stageName}");

        // 九州のときだけReadySceneへ
        if (stageName == "東北")
        {
            SceneManager.LoadScene("ReadyScene");
        }
        else
        {
            Debug.Log($"{stageName} はまだ未設定です。");
            isDeciding = false; // 未設定なら入力復帰
        }
    }

    void PlayMoveSE()
    {
        if (seSource != null && moveSE != null)
            seSource.PlayOneShot(moveSE);
    }

    void PlayDecideSE()
    {
        if (seSource != null && decideSE != null)
            seSource.PlayOneShot(decideSE);
    }
}
