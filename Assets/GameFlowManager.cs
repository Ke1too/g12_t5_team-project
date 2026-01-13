using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ★追加：UI操作用
using TMPro;          // ★追加：文字操作用
using System.Collections;       // ★追加：コルーチン用
using System.Collections.Generic; // ★追加：List用

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [Header("Flow Settings")]
    public int clears = 0;       // クリア回数
    public int fails = 0;        // 失敗回数

    public int maxClears = 10;   // クリアでステージセレクトに戻る回数
    public int maxFails = 3;     // 失敗でステージセレクトに戻る回数

    // --- ★ここから追加：報酬システム用設定 ---
    [System.Serializable]
    public class RewardItem
    {
        public string itemId;
        public string itemName;
        public Sprite itemImage;
        [TextArea(3, 5)] public string itemDescription;
    }

    [Header("Reward Settings")]
    [SerializeField] private List<RewardItem> rewardItems; // 報酬リスト

    [Header("Result UI")]
    [SerializeField] private GameObject resultPanel;    // 結果パネル
    [SerializeField] private Image resultImage;         // 画像
    [SerializeField] private TextMeshProUGUI resultNameText; // 名前
    [SerializeField] private Button nextButton;         // 「次へ」ボタン

    private bool isProcessing = false; // 連打防止用
    // --- ★ここまで ---

    void Awake()
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

    // ミニゲームを1回クリアした時に呼ばれる
    public void ReportClear()
    {
        if (isProcessing) return;

        clears++; // クリア数を増やす

        // ★ここが修正ポイント
        // 「今回のクリアで、目標回数（maxClears）に達したか？」をチェック
        if (clears >= maxClears)
        {
            // 目標達成！ -> 報酬演出（ProcessGameClearReward）へ進む
            StartCoroutine(ProcessGameClearReward());
        }
        else
        {
            // まだ目標に届いていない -> 次のミニゲームへ（報酬なし）
            CheckReturnToStageSelect();
        }
    }

    public void ReportFail()
    {
        if (isProcessing) return;

        fails++;
        // 失敗時は報酬なしですぐ遷移（必要ならここも変更可能）
        CheckReturnToStageSelect();
    }

    // --- ★追加：クリア報酬の抽選と保存 ---
    // 報酬を抽選・保存・表示するコルーチン
    private IEnumerator ProcessGameClearReward()
    {
        isProcessing = true; // 連打防止ロック

        // 演出用の待ち時間（「GAME CLEAR!」のテキストなどを出すならここで）
        yield return new WaitForSeconds(0.5f);

        // アイテムリストが空ならスキップして終了
        if (rewardItems == null || rewardItems.Count == 0)
        {
            CheckReturnToStageSelect();
            yield break;
        }

        // 1. ランダム抽選
        int randomIndex = Random.Range(0, rewardItems.Count);
        RewardItem selectedItem = rewardItems[randomIndex];

        // 2. 保存処理 (DataManager利用)
        if (DataManager.Instance != null)
        {
            yield return DataManager.Instance.SaveItem(
                selectedItem.itemId,
                () => ShowResult(selectedItem), // 保存できたら結果画面を表示
                (error) => {
                    Debug.LogError("保存失敗: " + error);
                    ShowResult(selectedItem); // エラーでも進める
                }
            );
        }
        else
        {
            // テスト用
            ShowResult(selectedItem);
        }
    }

    // --- ★追加：結果画面の表示 ---
    private void ShowResult(RewardItem item)
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true); // パネル表示

            if (resultImage != null)
            {
                resultImage.sprite = item.itemImage;
                resultImage.preserveAspect = true;
            }
            if (resultNameText != null) resultNameText.text = item.itemName;

            // ボタンを押したら「CheckReturnToStageSelect」を実行するように設定
            if (nextButton != null)
            {
                nextButton.onClick.RemoveAllListeners(); // 古い設定を消す
                nextButton.onClick.AddListener(() => {
                    resultPanel.SetActive(false); // パネルを消す
                    CheckReturnToStageSelect();   // シーン遷移へ
                });
            }
        }
        else
        {
            // UI設定がない場合は即座に進む
            CheckReturnToStageSelect();
        }
    }

    private void CheckReturnToStageSelect()
    {
        isProcessing = false; // ロック解除

        if (clears >= maxClears || fails >= maxFails)
        {
            // カウントリセットしてステージセレクトへ
            clears = 0;
            fails = 0;
            SceneManager.LoadScene("StageSelectScene");
        }
        else
        {
            // まだ続くならReadyシーンへ
            SceneManager.LoadScene("ReadyScene");
        }
    }
}