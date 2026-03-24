using Syacapachi.Camera;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public GameObject contentArea;
    [SerializeField] private GameObject photoItemPrefab; // Prefab that has a RawImage component
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private Sprite circleSprite;
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
    public void AddPhotoData(CameraCapture.PhotoData photoData)
    {
        if (photoItemPrefab == null) return;

        GameObject item = Instantiate(photoItemPrefab, contentArea.transform);
        item.SetActive(true);
        UnityEngine.UI.RawImage rawImage = item.GetComponentInChildren<UnityEngine.UI.RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = photoData.texture;
        }
        foreach(var info in photoData.info)
        {
            PlaceCircle(rawImage.rectTransform, info.viewportPosition, info.viewportRadius);
        }
    }
    public void PlaceCircle(RectTransform parent, Vector2 viewportPos, float size)
    {
        Image img = Instantiate(photoItemPrefab).GetComponent<Image>();
        img.transform.SetParent(parent);

        // 円スプライトを設定
        img.sprite = circleSprite;
        img.color = Color.red;

        RectTransform rt = img.GetComponent<RectTransform>();

        rt.anchorMin = rt.anchorMax = viewportPos;
        rt.sizeDelta = parent.sizeDelta * size;
        rt.anchoredPosition = Vector2.zero;

        img.gameObject.SetActive(true);
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
