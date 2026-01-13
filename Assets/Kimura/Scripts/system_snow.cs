using UnityEngine;
using UnityEngine.UI;

public class System_snow : MonoBehaviour
{
    [Header("ゲージ")]
    [SerializeField] Image gaugeSlider;

    [Header("タイマー（時間切れ判定用）")]
    public Timer timer;

    int maxCount = 15;
    int currentCount = 0;
    bool isFinished = false;

    void Start()
    {
        if (gaugeSlider != null)
            gaugeSlider.fillAmount = 1f;
    }

    // ボタンを押した時に呼ばれる
    public void OnButtonClik()
    {
        if (isFinished) return;

        currentCount++;
        if (gaugeSlider != null)
            gaugeSlider.fillAmount = 1f - (float)currentCount / maxCount;

        if (currentCount >= maxCount)
        {
            GameClear();
        }
    }

    // -------- クリア --------
    void GameClear()
    {
        if (isFinished) return;
        isFinished = true;

        // ★ 共通フローに報告
        GameFlowState.hasLastResult = true;
        GameFlowState.lastWin = true;
        GameFlowManager.Instance.ReportClear();
    }

    // -------- 失敗（時間切れなど）--------
    public void GameFail()
    {
        if (isFinished) return;
        isFinished = true;

        // ★ 共通フローに報告
        GameFlowState.hasLastResult = true;
        GameFlowState.lastWin = false;
        GameFlowManager.Instance.ReportFail();
    }
}
