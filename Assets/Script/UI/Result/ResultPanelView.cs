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

        rawImagesActiveQueue.Enqueue(rawImage);
        rawImage.gameObject.SetActive(true);
    }

    

    public void SetScoreText(string text)
    {
        if (scoreText != null)
        {
            scoreText.text = text;
        }
    }
}
