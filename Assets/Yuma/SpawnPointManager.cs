using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    // ① インスペクターで設定する「出現候補地」のマスターリスト
    public Transform[] allSpawnPoints;

    // ② まだ使われていない「空き」の場所を管理するリスト
    private List<Transform> availableSpawnPoints;

    void Awake()
    {
        // ゲーム開始時に、マスターリストをコピーして「空き」リストを初期化する
        InitializeList();
    }

    // リストを初期化する（ゲームのリスタート時などにも呼べるように public に）
    public void InitializeList()
    {
        // マスターリスト(allSpawnPoints)の中身を availableSpawnPoints にコピーする
        availableSpawnPoints = new List<Transform>(allSpawnPoints);
    }

    // ③ オブジェクトから呼ばれる「空いている場所を1つちょうだい」という関数
    public Transform GetRandomSpawnPoint()
    {
        // (A) もし空きリストが空っぽだったら（＝全部使い切った）
        if (availableSpawnPoints == null || availableSpawnPoints.Count == 0)
        {
            Debug.LogError("空いているSpawn Pointがありません！");
            return null; // 場所を返せない
        }

        // (B) 空きリストからランダムなインデックス番号を取得
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);

        // (C) その場所を取得
        Transform chosenPoint = availableSpawnPoints[randomIndex];

        // (D) ★重要★：今選んだ場所を「空き」リストから削除する
        //      (これで、次のオブジェクトは二度とこの場所を選べなくなる)
        availableSpawnPoints.RemoveAt(randomIndex);

        // (E) 選んだ場所のTransformを返す
        return chosenPoint;
    }
}