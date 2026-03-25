using Syacapachi.Camera;
using Syacapachi.Controller;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ResultPanelView : ImageViewer
{
    [Header("UI References")]
    [SerializeField] public GameObject contentArea;
    [SerializeField] private GameObject photoItemPrefab; // Prefab that has a RawImage component
    [SerializeField] private Button toHomeButton; //タイトルへ戻るボタン
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Circle Prefab References")]
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private Sprite circleSprite;

    private readonly Queue<RawImage> rawImagesActiveQueue = new();
    private readonly Queue<RawImage> rawImagesNoActiveQueue = new();
    public event Action OnToHomeButtonClick;
    void Start()
    {
        //イベントリスナーを追加
        toHomeButton.onClick.AddListener(() => OnToHomeButtonClick?.Invoke());
    }
    public void RefreshPhotos()
    {
        base.RefreshPicture();
        while (rawImagesActiveQueue.Count > 0)
        {
            var rawImg = rawImagesActiveQueue.Dequeue();
            rawImg.gameObject.SetActive(false);
            rawImagesNoActiveQueue.Enqueue(rawImg);
        }
    }

    public void AddPhoto(Texture2D texture)
    {
        if (photoItemPrefab == null) return;

        RawImage rawImage = rawImagesNoActiveQueue.Count > 0 ? 
                                rawImagesNoActiveQueue.Dequeue() : 
                                Instantiate(photoItemPrefab, contentArea.transform).GetComponentInChildren<RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = texture;
        }
        rawImagesActiveQueue.Enqueue(rawImage);
        rawImage.gameObject.SetActive(true);
    }
    public void AddPhotoData(CameraCapture.PhotoData photoData)
    {
        if (photoItemPrefab == null) return;

        RawImage rawImage = rawImagesNoActiveQueue.Count > 0 ?
                                rawImagesNoActiveQueue.Dequeue() :
                                Instantiate(photoItemPrefab, contentArea.transform).GetComponentInChildren<RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = photoData.texture;
        }
        base.display = rawImage;
        ShowDetail(photoData);

        if (photoData.info != null)
        {
            foreach (var info in photoData.info)
            {
                PlaceCircle(rawImage.rectTransform, info.viewportPosition, info.viewportRadius);
            }
        }

        rawImagesActiveQueue.Enqueue(rawImage);
        rawImage.gameObject.SetActive(true);
    }

    public void PlaceCircle(RectTransform parent, Vector2 viewportPos, float size)
    {
        if (imagePrefab == null || circleSprite == null) return;

        Image img = Instantiate(imagePrefab).GetComponent<Image>();
        img.transform.SetParent(parent, false);

        // 円スプライトを設定
        img.sprite = circleSprite;
        img.color = Color.red;

        RectTransform rt = img.GetComponent<RectTransform>();

        rt.anchorMin = rt.anchorMax = viewportPos;
        rt.sizeDelta = parent.sizeDelta * size;
        rt.anchoredPosition = Vector2.zero;

        rt.localScale = Vector3.one;
        img.gameObject.SetActive(true);
    }

    

    public void SetScoreText(string text)
    {
        if (scoreText != null)
        {
            scoreText.text = text;
        }
    }
}
