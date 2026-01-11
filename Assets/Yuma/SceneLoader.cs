using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Inspectorから制限時間を変更できるようにpublic変数にする
    public float timeLimit = 1.0f; // n秒後にシーンが切り替わる

    // Inspectorからランダムに選びたいシーンのリストを設定
    public string[] gameSceneNames;

    // ゲームが始まった時に一度だけ呼ばれる関数
    void Start()
    {
        // タイマーを開始するコルーチンを呼び出す
        StartCoroutine(LoadRandomSceneAfterDelay());
    }

    // 指定した時間待ってから「ランダムに」シーンをロードする処理
    IEnumerator LoadRandomSceneAfterDelay()
    {
        // timeLimitで指定した秒数だけ処理を待つ
        yield return new WaitForSeconds(timeLimit);

        // --- ここからがランダム選択のロジック ---

        // 読み込むシーンが設定されていなければエラーを防ぐ
        if (gameSceneNames == null || gameSceneNames.Length == 0)
        {
            Debug.LogError("読み込むシーンが設定されていません！");
            yield break; // コルーチンを終了
        }

        // 1. gameSceneNames 配列のインデックスをランダムに選びます。
        int randomIndex = Random.Range(0, gameSceneNames.Length);

        // 2. ランダムに選ばれたインデックスのシーン名を取得します。
        string sceneToLoad = gameSceneNames[randomIndex];

        // 3. 取得した名前のシーンを読み込みます。
        SceneManager.LoadScene(sceneToLoad);
    }
}