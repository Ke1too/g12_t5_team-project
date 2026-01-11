using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UIがクリックを妨害しないかチェックするために必要

public class clik2 : MonoBehaviour 
{
    public GameManager2 gameManager;
    void Update()
    {
        // 1. マウスがクリックされた瞬間かどうか
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("--- クリック発生 ---");
            // 2. マウスがUI上にあるか？ (重要！)
            // これがtrueの場合、UIをクリックしているのでゲーム内オブジェクトは反応しない
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UIをクリックしたため、ゲームオブジェクトは反応しません。");
                return; // UIをクリックしたので、ここで処理を終了
            }

            // 3. マウスカーソルの位置からRayを飛ばす
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // 4. Rayが何かに当たったか？
            if (hit.collider != null)
            {
                // ★最重要★ どのオブジェクトに当たったか確認
                Debug.Log("Rayがヒットしました。 ヒットしたオブジェクト: " + hit.collider.name);

                // 5. それが「この」オブジェクトか？
                if (hit.collider.gameObject == this.gameObject)
                {
                    Debug.Log("成功: " + this.name + " がクリックされました！");


                    // ここに本来の処理を記述
                    Destroy(gameObject);
                    gameManager.gameClear = true;
                }
                else
                {
                    // このオブジェクト以外に当たった場合
                    Debug.LogWarning(this.name + " をクリックしようとしましたが、手前にある " + hit.collider.name + " に妨害されました。");
                }
            }
            else
            {
                // Rayが何にも当たらなかった場合
                Debug.LogError("RayがどのColliderにもヒットしませんでした。");
            }
        }
    }
}
