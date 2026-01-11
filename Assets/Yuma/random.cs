using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random : MonoBehaviour
{
    public GameManager2 gameManager;
    public float speed;

    // ▼▼▼ 変更点 ▼▼▼
    // (変更前) public Transform[] spawnPoints; 
    // ↓
    // (変更後) インスペクターで設定する「マネージャー」
    public SpawnPointManager spawnManager;
    // ▲▲▲ 変更ここまで ▲▲▲

    // --- Update()で使う変数は残します ---
    private Vector2 direction;
    private Vector2 screenMin;
    private Vector2 screenMax;
    private Vector2 objectSize;


    // ▼▼▼ Start() メソッドを書き換え ▼▼▼
    void Start()
    {
        // ----------------------------------------------------
        // (A) マネージャーに問い合わせて、空いている場所をもらう
        // ----------------------------------------------------

        // (A-1) マネージャーが設定されているかチェック
        if (spawnManager == null)
        {
            Debug.LogError("'" + gameObject.name + "' のインスペクターに 'Spawn Manager' が設定されていません。");
            return; // 処理を中断
        }

        // (A-2) マネージャーに「空き場所をください」とお願いする
        Transform chosenSpawnPoint = spawnManager.GetRandomSpawnPoint();

        // (A-3) もし場所がもらえなかったら（＝nullが返ってきたら）
        if (chosenSpawnPoint == null)
        {
            Debug.LogError("'" + gameObject.name + "' はSpawn場所を取得できませんでした。");
            return; // 処理を中断
        }

        // (A-4) もらえた場所（chosenSpawnPoint）に自分を移動させる
        transform.position = new Vector3(
            chosenSpawnPoint.position.x,
            chosenSpawnPoint.position.y,
            transform.position.z
        );


        // ----------------------------------------------------
        // (B) Update()での移動・反射用の初期設定 (元のコード)
        // ----------------------------------------------------

        direction = Random.insideUnitCircle.normalized;
        Camera mainCamera = Camera.main;
        screenMin = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        screenMax = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
        objectSize = GetComponent<SpriteRenderer>().bounds.size;
    }
    // ▲▲▲ Start() メソッドの書き換えここまで ▲▲▲


    // ▼▼▼ Update() メソッドは元のまま変更なし ▼▼▼
    void Update()
    {
        // ★ gameManager.isGameActive が true の時だけ動くようにする
        if (gameManager.isGameActive && !gameManager.gameClear && !gameManager.gameOver)
        {
            // (移動と反射の処理)
            transform.Translate(direction * speed * Time.deltaTime);
            if (transform.position.x < screenMin.x + objectSize.x / 2 || transform.position.x > screenMax.x - objectSize.x / 2)
            {
                direction.x *= -1;
            }
            if (transform.position.y < screenMin.y + objectSize.y / 2 || transform.position.y > screenMax.y - objectSize.y / 2)
            {
                direction.y *= -1;
            }
            float clampedX = Mathf.Clamp(transform.position.x, screenMin.x + objectSize.x / 2, screenMax.x - objectSize.x / 2);
            float clampedY = Mathf.Clamp(transform.position.y, screenMin.y + objectSize.y / 2, screenMax.y - objectSize.y / 2);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}