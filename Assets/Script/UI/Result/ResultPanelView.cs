using Syacapachi.Camera;
using Syacapachi.Contracts;
using Syacapachi.Manager;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ResultPanelView : MonoBehaviour,IPhotoDetailRenderer
{
    [Header("UI References")]
    [SerializeField] public RectTransform contentArea;
    [SerializeField] private GameObject photoItemPrefab; // Prefab that has a RawImage component
    [SerializeField] private Button toHomeButton; //タイトルへ戻るボタン
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] PhotoScoreManager scoreManager;
    [SerializeField] Sprite circleSprite;
    [SerializeField] GameObject imagePrefab;
    private readonly Queue<Image> imageActiveQueue = new();
    private readonly Queue<Image> imageNoActiveQueue = new();
    private readonly Queue<RawImage> rawImagesActiveQueue = new();
    private readonly Queue<RawImage> rawImagesNoActiveQueue = new();
    public event Action OnToHomeButtonClick;

    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            var obj = Instantiate(imagePrefab, contentArea);
            obj.SetActive(false);
            var img = obj.GetComponent<Image>();
            imageNoActiveQueue.Enqueue(img);
        }
    }
    void Start()
    {
        //イベントリスナーを追加
        toHomeButton.onClick.AddListener(() => OnToHomeButtonClick?.Invoke());
    }

    public RectTransform Show(Texture2D tex)
    {
        if (photoItemPrefab == null) return null;

        RawImage rawImage = rawImagesNoActiveQueue.Count > 0 ?
                                rawImagesNoActiveQueue.Dequeue() :
                                Instantiate(photoItemPrefab, contentArea.transform).GetComponentInChildren<RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = tex;
        }
        rawImagesActiveQueue.Enqueue(rawImage);
        rawImage.gameObject.SetActive(true);
        return rawImage.rectTransform;
    }
    public void ShowDetail(CameraCapture.PhotoData photo)
    {
        var rect = Show(photo.texture);
        foreach (var info in photo.info)
        {
            PlaceCircle(rect, info.centerPosition, info.centerRadius, scoreManager.GetScore(info), info.drawColor);
        }
    }
    public void AddPhotoData(CameraCapture.PhotoData photoData)
    {
        if (photoItemPrefab == null) return;

        ShowDetail(photoData);
    }
    private void PlaceCircle(RectTransform parent, Vector2 viewportPos, float size, float score, Color drawColor)
    {
        Image img = imageNoActiveQueue.Count > 0 ? imageNoActiveQueue.Dequeue() : Instantiate(imagePrefab, parent).GetComponent<Image>();
        img.transform.SetParent(parent);

        // 円スプライトを設定
        img.sprite = circleSprite;
        img.color = Color.red;

        RectTransform rt = img.GetComponent<RectTransform>();

        rt.anchorMin = rt.anchorMax = viewportPos;
        rt.sizeDelta = parent.sizeDelta * size;
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        var textMesh = img.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = $"{score:n1}";
            textMesh.color = drawColor;
        }

        img.gameObject.SetActive(true);
        imageActiveQueue.Enqueue(img);
    }
    public void RefreshPicture()
    {
        while (rawImagesActiveQueue.Count > 0)
        {
            var rawImg = rawImagesActiveQueue.Dequeue();
            rawImg.gameObject.SetActive(false);
            rawImagesNoActiveQueue.Enqueue(rawImg);
        }
        while (imageActiveQueue.Count > 0)
        {
            var img = imageActiveQueue.Dequeue();
            img.gameObject.SetActive(false);
            imageNoActiveQueue.Enqueue(img);
        }
    }
    
    
    

    

    public void SetScoreText(string text)
    {
        if (scoreText != null)
        {
            scoreText.text = text;
        }
    }
}
