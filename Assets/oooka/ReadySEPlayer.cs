using UnityEngine;

public class ReadySEPlayer : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource seSource;

    [Header("SE Clips")]
    public AudioClip clapSE;        // 初回：拍木
    public AudioClip winSE;         // 成功：ファンファーレ
    public AudioClip loseSE;        // 失敗：ブーイング
    public AudioClip fusumaOpenSE;  // 襖がバッと開く音

    void Play(AudioClip clip)
    {
        if (seSource != null && clip != null)
        {
            seSource.PlayOneShot(clip);
        }
    }

    // ===== 外から呼ぶ用 =====

    public void PlayClap()
    {
        Play(clapSE);
    }

    public void PlayWin()
    {
        Play(winSE);
    }

    public void PlayLose()
    {
        Play(loseSE);
    }

    public void PlayFusumaOpen()
    {
        Play(fusumaOpenSE);
    }
}
