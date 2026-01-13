using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CollectionManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string itemId;
        public string itemName;
        public Sprite itemImage;
        [TextArea(3, 5)] public string itemDescription;
    }

    [Header("Item Master List")]
    [SerializeField] private List<ItemData> allItems;

    [Header("UI References")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform gridContent;
    [SerializeField] private Button backButton;
    [SerializeField] private string restSceneName = "StageSelectScene";

    [Header("Unknown Item Settings")]
    [SerializeField] private Sprite unknownSprite;
    [SerializeField] private string unknownName = "???";

    // ★追加：詳細パネル用の参照
    [Header("Detail Panel Settings")]
    [SerializeField] private GameObject detailPanel;         // 詳細パネル全体（DetailPanel）
    [SerializeField] private Image detailImage;              // 詳細画像のImage
    [SerializeField] private TextMeshProUGUI detailName;     // 詳細名前のText
    [SerializeField] private TextMeshProUGUI detailDesc;     // 詳細説明のText
    [SerializeField] private Button closeButton;             // 戻るボタン

    private void Start()
    {
        backButton.onClick.AddListener(() => SceneManager.LoadScene(restSceneName));

        // ★追加：閉じるボタンの処理
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseDetail);
        }
        // ★追加：最初はパネルを隠す
        if (detailPanel != null) detailPanel.SetActive(false);

        StartCoroutine(DataManager.Instance.LoadCollection(
            (ownedData) => DisplayCollection(ownedData),
            (error) => Debug.LogError("読み込みエラー: " + error)
        ));
    }

    private void DisplayCollection(Dictionary<string, bool> ownedItems)
    {
        foreach (Transform child in gridContent) Destroy(child.gameObject);

        foreach (var item in allItems)
        {
            GameObject obj = Instantiate(itemPrefab, gridContent);

            Image icon = obj.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI text = obj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            // プレハブのButtonコンポーネントを取得
            Button btn = obj.GetComponent<Button>();

            bool isOwned = ownedItems.ContainsKey(item.itemId);

            if (isOwned)
            {
                icon.sprite = item.itemImage;
                text.text = item.itemName;
                icon.color = Color.white;

                // ★追加：持っている場合、クリックしたら詳細を開く
                if (btn != null)
                {
                    btn.onClick.AddListener(() => OnItemClicked(item));
                }
            }
            else
            {
                if (unknownSprite != null) icon.sprite = unknownSprite;
                text.text = unknownName;
                icon.color = Color.gray;

                // ★追加：持っていない場合はクリックできないようにする（または反応なし）
                if (btn != null) btn.interactable = false;
            }
        }
    }

    // ★追加：アイテムがクリックされた時の処理
    private void OnItemClicked(ItemData item)
    {
        // 詳細パネルにデータをセット
        detailImage.sprite = item.itemImage;
        detailName.text = item.itemName;
        detailDesc.text = item.itemDescription; // ここで説明文を表示！

        // パネルを表示
        detailPanel.SetActive(true);
    }

    // ★追加：閉じるボタンの処理
    private void CloseDetail()
    {
        detailPanel.SetActive(false);
    }
}