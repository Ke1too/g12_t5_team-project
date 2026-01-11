using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI名前空間を忘れずに

public class ImageSwitcher : MonoBehaviour
{
    // 1. 切り替えたいImageコンポーネント
    public Image targetImage;

    // 2. 切り替える画像のリスト（インスペクターで設定）
    public Sprite[] sprites;

    // 3. 画像を切り替える間隔（秒）
    public float switchInterval = 1.0f;

    // 4. 現在表示している画像のインデックス
    private int currentIndex = 0;

    void Start()
    {
        // 最初の画像を設定
        if (targetImage != null && sprites.Length > 0)
        {
            targetImage.sprite = sprites[currentIndex];
        }

        // コルーチンを開始
        StartCoroutine(SwitchImageRoutine());
    }

    private IEnumerator SwitchImageRoutine()
    {
        while (true) // 無限ループ（ゲーム中ずっと切り替える場合）
        {
            // 指定した秒数だけ待機
            yield return new WaitForSeconds(switchInterval);

            // 次の画像のインデックスを計算
            // (%) を使うと、配列の最後まで行ったら0に戻る
            currentIndex = (currentIndex + 1) % sprites.Length;

            // Imageコンポーネントのspriteプロパティを差し替え
            if (targetImage != null)
            {
                targetImage.sprite = sprites[currentIndex];
            }
        }
    }
}