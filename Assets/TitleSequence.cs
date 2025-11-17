using UnityEngine;
using System.Collections;

public class TitleSequence : MonoBehaviour
{
    public Animator yuruttoAnimator;
    public Animator nipponAnimator;

    IEnumerator Start()
    {
        // ゆるっと再生
        yuruttoAnimator.Play("YuruttoBounce");
        // 1.2秒待ってから次へ
        yield return new WaitForSeconds(1.2f);
        // ニッポン伝説再生
        nipponAnimator.Play("StampIn");
    }
}
