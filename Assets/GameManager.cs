using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameStarted = false;

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;

        Debug.Log("Game Started!");
        // ここにゲーム開始処理を追加
        // 例: プレイヤーの動きを有効にするなど
    }
}
