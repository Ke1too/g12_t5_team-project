using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagerKanaQuiz : MonoBehaviour
{
    [Header("問題表示")]
    public TextMeshProUGUI kanjiText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("選択肢ボタン（4つ）")]
    public Button[] choiceButtons;

    [Header("ポップアップ表示")]
    public GameObject dimPanel;
    public GameObject answerPopup;
    public TextMeshProUGUI answerTitleText;
    public Image answerImage;
    public TextMeshProUGUI answerWordText;
    public TextMeshProUGUI answerCommentText;

    [Header("タイマー設定")]
    public float timeLimit = 15f;
    public Color normalTimerColor = Color.black;
    public Color warningTimerColor = Color.red;

    [Header("サウンド")]
    public AudioSource bgmSource;     // BGM用AudioSource（Loop ON推奨）
    public AudioSource seSource;      // SE用AudioSource
    public AudioClip bgmClip;         // BGM
    public AudioClip correctSE;       // 正解SE
    public AudioClip wrongSE;         // 不正解SE
    public AudioClip buttonSE;        // ボタン押したSE

    private int score = 0;
    private int currentQuestionIndex;
    private int charIndex = 0;

    private float timeLeft;

    // 5秒以下の「デカ文字 → 戻る」
    private int lastShownSecond = -1;
    private Vector3 timerDefaultScale;
    private Vector3 timerBigScale;
    private float scaleAnimDuration = 0.2f;
    private float scaleAnimTime = 0f;
    private bool isScaling = false;
    private Vector3 scaleStart;

    private bool isQuestionActive = false;

    [System.Serializable]
    public class KanjiQuestion
    {
        public string kanji;
        public string reading;
        public Sprite image;
        public string comment;
    }

    public List<KanjiQuestion> questions = new List<KanjiQuestion>();


    void Start()
    {
        if (dimPanel != null) dimPanel.SetActive(false);
        if (answerPopup != null) answerPopup.SetActive(false);

        // タイマーの基準スケール
        if (timerText != null)
            timerDefaultScale = timerText.rectTransform.localScale;
        else
            timerDefaultScale = Vector3.one;

        timerBigScale = timerDefaultScale * 1.6f;

        // BGM再生
        PlayBGM();

        NextQuestion();
    }

    void Update()
    {
        if (!isQuestionActive) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            TimerTextUpdate();
            ShowTimeout();
            return;
        }

        TimerTextUpdate();

        // スケールアニメ
        if (isScaling && timerText != null)
        {
            scaleAnimTime += Time.deltaTime;
            float t = Mathf.Clamp01(scaleAnimTime / scaleAnimDuration);
            timerText.rectTransform.localScale = Vector3.Lerp(scaleStart, timerDefaultScale, t);

            if (t >= 1f) isScaling = false;
        }
    }

    // -------- サウンド --------

    void PlayBGM()
    {
        if (bgmSource == null || bgmClip == null) return;

        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        if (!bgmSource.isPlaying) bgmSource.Play();
    }

    void PlaySE(AudioClip clip)
    {
        if (seSource == null || clip == null) return;
        seSource.PlayOneShot(clip);
    }

    // -------- タイマー --------

    void TimerTextUpdate()
    {
        if (timerText == null) return;

        int sec = Mathf.CeilToInt(timeLeft);
        timerText.text = sec.ToString();

        if (sec <= 5)
        {
            timerText.color = warningTimerColor;

            if (sec != lastShownSecond && sec > 0)
            {
                scaleStart = timerBigScale;
                timerText.rectTransform.localScale = timerBigScale;
                scaleAnimTime = 0f;
                isScaling = true;
            }
        }
        else
        {
            timerText.color = normalTimerColor;
        }

        lastShownSecond = sec;
    }

    // -------- ゲーム進行 --------

    void NextQuestion()
    {
        if (dimPanel != null) dimPanel.SetActive(false);
        if (answerPopup != null) answerPopup.SetActive(false);

        currentQuestionIndex = Random.Range(0, questions.Count);
        KanjiQuestion q = questions[currentQuestionIndex];

        if (kanjiText != null) kanjiText.text = q.kanji;
        if (resultText != null) resultText.text = "";

        charIndex = 0;

        timeLeft = timeLimit;
        lastShownSecond = -1;
        if (timerText != null) timerText.rectTransform.localScale = timerDefaultScale;
        TimerTextUpdate();

        isQuestionActive = true;
        SetupChoices();
    }

    void SetupChoices()
    {
        KanjiQuestion q = questions[currentQuestionIndex];
        string correctChar = q.reading[charIndex].ToString();

        string[] hira = {
            "あ","い","う","え","お",
            "か","き","く","け","こ",
            "さ","し","す","せ","そ",
            "た","ち","つ","て","と",
            "な","に","ぬ","ね","の",
            "は","ひ","ふ","へ","ほ",
            "ま","み","む","め","も",
            "や","ゆ","よ",
            "ら","り","る","れ","ろ",
            "わ","を","ん"
        };

        List<string> choices = new List<string> { correctChar };

        while (choices.Count < 4)
        {
            string r = hira[Random.Range(0, hira.Length)];
            if (!choices.Contains(r)) choices.Add(r);
        }

        // シャッフル
        for (int i = 0; i < choices.Count; i++)
        {
            int r = Random.Range(i, choices.Count);
            (choices[i], choices[r]) = (choices[r], choices[i]);
        }

        // ボタンへ反映
        for (int i = 0; i < 4; i++)
        {
            string c = choices[i];

            var tmp = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = c;

            choiceButtons[i].onClick.RemoveAllListeners();

            // どのボタンでも「押したSE」を鳴らす
            choiceButtons[i].onClick.AddListener(() => PlaySE(buttonSE));

            // 正誤判定
            if (c == correctChar)
                choiceButtons[i].onClick.AddListener(CorrectChar);
            else
                choiceButtons[i].onClick.AddListener(WrongChar);
        }
    }

    void CorrectChar()
    {
        if (!isQuestionActive) return;

        charIndex++;
        KanjiQuestion q = questions[currentQuestionIndex];

        if (charIndex >= q.reading.Length)
        {
            score++;
            if (scoreText != null) scoreText.text = "スコア：" + score;

            PlaySE(correctSE);
            ShowPopup(q, true);
            return;
        }

        SetupChoices();
    }

    void WrongChar()
    {
        if (!isQuestionActive) return;

        if (resultText != null) resultText.text = "不正解…";
        PlaySE(wrongSE);
        ShowPopup(questions[currentQuestionIndex], false);
    }

    void ShowTimeout()
    {
        if (!isQuestionActive) return;

        if (resultText != null) resultText.text = "時間切れ…";
        PlaySE(wrongSE); // 時間切れは不正解SEにしてる（別にしたければ別Clipを用意してOK）
        ShowPopup(questions[currentQuestionIndex], false);
    }

    void ShowPopup(KanjiQuestion q, bool isCorrect)
    {
        isQuestionActive = false;

        if (dimPanel != null) dimPanel.SetActive(true);
        if (answerPopup != null) answerPopup.SetActive(true);

        if (answerTitleText != null)
            answerTitleText.text = isCorrect ? "正解！！" : "不正解…";

        if (answerWordText != null) answerWordText.text = q.reading;
        if (answerCommentText != null) answerCommentText.text = q.comment;
        if (answerImage != null) answerImage.sprite = q.image;

        Invoke(nameof(BackToReadyScene), 2.0f);
    }

    void BackToReadyScene()
    {
        SceneManager.LoadScene("ReadyScene");
    }
}
