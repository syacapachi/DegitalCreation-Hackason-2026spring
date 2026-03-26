namespace Syacapachi.Controller
{
    using Syacapachi.Camera;
    using Syacapachi.Contracts;
    using Syacapachi.Manager;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    using UnityEngine.UI;

    public class ImageViewer : MonoBehaviour, IPhotoDetailRenderer
    {
        [SerializeField] PhotoScoreManager scoreManager;
        [SerializeField] protected RawImage display;
        [SerializeField] Sprite circleSprite;
        [SerializeField] GameObject imagePrefab;    
        private readonly Queue<Image> imageActiveQueue = new ();
        private readonly Queue<Image> imageNoActiveQueue = new();

        private void Awake()
        {
            for (int i = 0; i < 10; i++)
            {
                var obj = Instantiate(imagePrefab,display.rectTransform);
                obj.SetActive(false);
                var img = obj.GetComponent<Image>();
                imageNoActiveQueue.Enqueue(img);
            }
        }
        public RectTransform Show(Texture2D tex)
        {
            display.texture = tex;
            return display.rectTransform;
        }
        public void ShowDetail(CameraCapture.PhotoData photo)
        {
            var rect = Show(photo.texture);
            foreach (var info in photo.info)
            {
                PlaceCircle(rect, info.centerPosition, info.centerRadius, scoreManager.GetScore(info),info.drawColor);
            }
        }
        public void PlaceCircle(RectTransform parent, Vector2 viewportPos, float size, float score,Color drawColor)
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
            while(imageActiveQueue.Count > 0)
            {
                var img = imageActiveQueue.Dequeue();
                img.gameObject.SetActive(false);
                imageNoActiveQueue.Enqueue(img);
            }
        }
    }
}