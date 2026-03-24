using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public GameObject contentArea;
    [SerializeField] private GameObject photoItemPrefab; // Prefab that has a RawImage component
    [SerializeField] private Button toHomeButton; //タイトルへ戻るボタン
    [SerializeField] private TextMeshProUGUI scoreText;

    public event Action OnToHomeButtonClick;
    public void ClearPhotos()
    {
        foreach (Transform child in contentArea.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddPhoto(Texture2D texture)
    {
        if (photoItemPrefab == null) return;

        GameObject item = Instantiate(photoItemPrefab, contentArea.transform);
        item.SetActive(true);
        UnityEngine.UI.RawImage rawImage = item.GetComponentInChildren<UnityEngine.UI.RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = texture;
        }
    }

    void Awake()
    {
        //イベントリスナーを追加
        toHomeButton.onClick.AddListener(() => OnToHomeButtonClick?.Invoke());
    }

    public void SetScoreText(string text)
    {
        if (scoreText != null)
        {
            scoreText.text = text;
        }
    }
}
