using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // ← 新Input System対応

public class StageSelector : MonoBehaviour
{
    [Header("矢印マーカー（親オブジェクト）")]
    public RectTransform arrow;

    [Header("ステージの丸（UI）")]
    public RectTransform[] stagePoints;

    [Header("ステージシーン名")]
    public string[] stageSceneNames;

    private int currentIndex = 0;

    void Start()
    {
        if (stagePoints.Length > 0)
        {
            UpdateArrowPosition();
        }
    }

    void Update()
    {
        // 右キー
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            currentIndex = (currentIndex + 1) % stagePoints.Length;
            UpdateArrowPosition();
        }

        // 左キー
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            currentIndex = (currentIndex - 1 + stagePoints.Length) % stagePoints.Length;
            UpdateArrowPosition();
        }

        // Enterキーでシーン遷移
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            LoadSelectedStage();
        }
    }

    void UpdateArrowPosition()
    {
        if (arrow != null && stagePoints.Length > 0)
        {
            // 丸の少し上に矢印を配置
            arrow.anchoredPosition = stagePoints[currentIndex].anchoredPosition + new Vector2(0, 80f);
        }
    }

    void LoadSelectedStage()
    {
        if (currentIndex >= 0 && currentIndex < stageSceneNames.Length)
        {
            string sceneName = stageSceneNames[currentIndex];
            Debug.Log($"選択されたステージ: {sceneName}");

            // 🔹 九州が選ばれているときだけ ReadyScene に遷移
            if (sceneName == "九州")
            {
                SceneManager.LoadScene("ReadyScene");
            }
            else
            {
                // 他のステージも将来的にここで分岐可能
                Debug.Log($"{sceneName} はまだ未設定です。");
            }
        }
    }
}
