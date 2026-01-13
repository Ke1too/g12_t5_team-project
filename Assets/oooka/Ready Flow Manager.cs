using UnityEngine;
using System.Collections;

public class ReadyFlowManager : MonoBehaviour
{
    [Header("SE Player")]
    public ReadySEPlayer sePlayer;

    [Header("Fusuma Animators")]
    public Animator leftFusumaAnimator;
    public Animator rightFusumaAnimator;

    [Header("Timing")]
    public float resultSEWaitTime = 1.0f;   // 勝敗SEを聞かせる時間
    public float beforeFusumaDelay = 0.25f; // 襖開始前の間

    void Start()
    {
        StartCoroutine(Flow());
    }

    IEnumerator Flow()
    {
        Debug.Log(
            $"[ReadyFlow] playedOnce={GameFlowState.hasPlayedOnce}, " +
            $"hasResult={GameFlowState.hasLastResult}, " +
            $"lastWin={GameFlowState.lastWin}"
        );

        // -------- 初回 --------
        if (!GameFlowState.hasPlayedOnce)
        {
            sePlayer.PlayClap();
            GameFlowState.hasPlayedOnce = true;

            yield return new WaitForSeconds(resultSEWaitTime);

            TriggerFusuma("First");
        }
        // -------- 2回目以降 --------
        else
        {
            if (GameFlowState.hasLastResult)
            {
                if (GameFlowState.lastWin)
                    sePlayer.PlayWin();
                else
                    sePlayer.PlayLose();

                // 勝敗SEをしっかり聞かせる
                yield return new WaitForSeconds(resultSEWaitTime);
            }

            TriggerFusuma("Normal");
        }
    }

    void TriggerFusuma(string triggerName)
    {
        leftFusumaAnimator.ResetTrigger("First");
        leftFusumaAnimator.ResetTrigger("Normal");
        rightFusumaAnimator.ResetTrigger("First");
        rightFusumaAnimator.ResetTrigger("Normal");

        StartCoroutine(DelayTrigger(triggerName));
    }

    IEnumerator DelayTrigger(string triggerName)
    {
        yield return new WaitForSeconds(beforeFusumaDelay);

        leftFusumaAnimator.SetTrigger(triggerName);
        rightFusumaAnimator.SetTrigger(triggerName);
    }
}
