using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class system : MonoBehaviour
{
    public Image[] cornerImages;        // 四隅に置く Image
    public Sprite winningSprite;        // 当たり画像
    public List<Sprite> losingSprites;  // 外れ画像（3つ）
    public Timer timer;
    
    void Start()
    {
        SetRandomImages();
    }

    void SetRandomImages()
    {
        // --- 配置用リストを作る（勝ち1 + 負け3） ---
        List<Sprite> sprites = new List<Sprite>();
        sprites.Add(winningSprite);
        sprites.AddRange(losingSprites);

        // --- シャッフル ---
        for (int i = 0; i < sprites.Count; i++)
        {
            Sprite temp = sprites[i];
            int r = Random.Range(i, sprites.Count);
            sprites[i] = sprites[r];
            sprites[r] = temp;
        }

        // --- 四隅に適用 ---
        for (int i = 0; i < cornerImages.Length; i++)
        {
            cornerImages[i].sprite = sprites[i];

            // Button取得（親画像の子に透明ボタンがある）
            Button btn = cornerImages[i].GetComponentInChildren<Button>();

            btn.onClick.RemoveAllListeners(); // 念のためクリア

            if (sprites[i] == winningSprite)
            {
                // 当たり
                btn.onClick.AddListener(() => OnWin());
            }
            else
            {
                // ハズレ
                btn.onClick.AddListener(() => OnLose());
            }
        }
    }

    void OnWin()
    {
        timer.GameClear();
     }

    void OnLose()
    {
        timer.GameOver();
    }
}

