using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random_mover : MonoBehaviour
{
    public GameManager2 gameManager;

    // オブジェクトの移動スピード
    public float speed;

    // 移動方向を格納する変数
    private Vector2 direction;

    // カメラから取得した画面の境界（ワールド座標）
    private Vector2 screenMin;
    private Vector2 screenMax;

    // オブジェクトの画像のサイズ（画面外へのはみ出し防止用）
    private Vector2 objectSize;

    void Start()
    {
        // ① ゲーム開始時にランダムな方向を決定
        direction = Random.insideUnitCircle.normalized;

        // ② カメラのビューポートから画面の境界をワールド座標で取得
        Camera mainCamera = Camera.main;
        screenMin = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        screenMax = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        // ③ オブジェクトのサイズを取得して、はみ出しを防止
        // SpriteRendererコンポーネントのbounds.sizeで画像の大きさを取得
        objectSize = GetComponent<SpriteRenderer>().bounds.size;
        // ▼▼▼ ここから追加 ▼▼▼
        // ----------------------------------------------------
        // ▼▼▼ 重なり防止処理の追加 ▼▼▼
        // ----------------------------------------------------

        // (A) このオブジェクト自身のコライダーを取得（必須）
        Collider2D selfCollider = GetComponent<Collider2D>();
        if (selfCollider == null)
        {
            Debug.LogError("'" + gameObject.name + "' に Collider2D がアタッチされていません。重なり防止が機能しません。");
            // コライダーがない場合、元のロジックを（重なる可能性ありで）実行
            float errorX = Random.Range(screenMin.x + objectSize.x / 2, screenMax.x - objectSize.x / 2);
            float errorY = Random.Range(screenMin.y + objectSize.y / 2, screenMax.y - objectSize.y / 2);
            transform.position = new Vector3(errorX, errorY, transform.position.z);
            return; // Start() を抜ける
        }

        // ▼▼▼ 追加 ▼▼▼
        // 判定に使うサイズを、SpriteRendererではなくColliderの境界から取得する
        // (これ以降、objectSize は削除してもよい)
        Vector2 colliderSize = selfCollider.bounds.size;
        // ▲▲▲ 追加ここまで ▲▲▲

        // (B) 無限ループを防ぐための最大試行回数
        int maxAttempts = 100;
        bool positionFound = false;
        Vector3 newPosition = Vector3.zero;

        // (C) 空いている場所が見つかるまで最大回数試行
        for (int i = 0; i < maxAttempts; i++)
        {
            // ④' ランダムな座標の「候補」を生成
            Debug.Log("'" + gameObject.name + "' にて Collider2D を検知。重なり防止が機能します。");
            float randomX = Random.Range(screenMin.x + objectSize.x / 2, screenMax.x - objectSize.x / 2);
            float randomY = Random.Range(screenMin.y + objectSize.y / 2, screenMax.y - objectSize.y / 2);
            newPosition = new Vector3(randomX, randomY, transform.position.z);

            // (D) 重なりチェック
            //     (1) 自分自身を検知しないよう、一時的に自分のコライダーを無効化
            selfCollider.enabled = false;

            //     (2) 候補位置(newPosition)に、objectSizeの大きさで重なり判定
            //         ※BoxCollider2DのサイズがobjectSizeと一致している前提
            Collider2D overlap = Physics2D.OverlapBox(newPosition, objectSize, 0);

            //     (3) チェックが終わったらコライダーを有効に戻す
            selfCollider.enabled = true;

            // (E) 重なりがなかった場合
            if (overlap == null)
            {
                positionFound = true; // 安全な場所が見つかった
                break; // forループを抜ける
            }
            // (overlap != null の場合は、forループが継続され、次の座標候補が試される)
        }

        // ⑤ オブジェクトを最終的な位置に移動させる
        transform.position = newPosition;

        // (F) もし最大回数試しても見つからなかった場合（画面が埋まっている場合など）
        if (!positionFound)
        {
            Debug.LogWarning("'" + gameObject.name + "' の安全な配置場所が見つかりませんでした。重なっている可能性があります。");
            // この場合、最後に試した（＝重なっている）場所に配置される
        }
        // ----------------------------------------------------
        // ▲▲▲ 変更ここまで ▲▲▲
        // ----------------------------------------------------
    }
    // ▲▲▲ Start() メソッドの置き換えここまで ▲▲▲

    void Update()
    {
        // ★ gameManager.isGameActive が true の時だけ動くようにする
        if (gameManager.isGameActive && !gameManager.gameClear && !gameManager.gameOver)
        {
            // ⑥ 毎フレーム、設定された方向にオブジェクトを移動させる
            transform.Translate(direction * speed * Time.deltaTime);

            // ➆ オブジェクトが画面の境界に達したら方向を反転させる
            // 左右の壁での反射
            if (transform.position.x < screenMin.x + objectSize.x / 2 || transform.position.x > screenMax.x - objectSize.x / 2)
            {
                direction.x *= -1; // X方向を反転
            }

            // 上下の壁での反射
            if (transform.position.y < screenMin.y + objectSize.y / 2 || transform.position.y > screenMax.y - objectSize.y / 2)
            {
                direction.y *= -1; // Y方向を反転
            }

            // 念のため、オブジェクトが画面外に出てしまった場合に位置を強制的に内側に戻す
            float clampedX = Mathf.Clamp(transform.position.x, screenMin.x + objectSize.x / 2, screenMax.x - objectSize.x / 2);
            float clampedY = Mathf.Clamp(transform.position.y, screenMin.y + objectSize.y / 2, screenMax.y - objectSize.y / 2);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}