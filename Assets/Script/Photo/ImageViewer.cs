namespace Syacapachi.Controller
{
    using Syacapachi.Attribute;
    using Syacapachi.Camera;
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    using UnityEngine.UI;

    public class ImageViewer : MonoBehaviour
    {
        [SerializeField] RectTransform rect;
        [SerializeField] RawImage display;
        [SerializeField] Sprite circleSprite;
        [SerializeField] GameObject imagePrefab;    
        private readonly Queue<Image> imageActiveQueue = new ();
        private readonly Queue<Image> imageNoActiveQueue = new();

        private void Awake()
        {
            for (int i = 0; i < 10; i++)
            {
                var obj = Instantiate(imagePrefab,rect.transform);
                obj.SetActive(false);
                var img = obj.GetComponent<Image>();
                imageNoActiveQueue.Enqueue(img);
            }
        }
        public void Show(Texture2D tex)
        {
            display.texture = tex;
        }
        public void ShowDetail(CameraCapture.PhotoData photo)
        {
            Show(photo.texture);
            Refresh();
            foreach (var info in photo.info)
            {
                PlaceCircle(rect, info.viewportPosition, info.viewportRadius, info.GetScore(),info.drawColor);
            }
        }
        public void PlaceCircle(RectTransform parent, Vector2 viewportPos, float size, float score,Color drawColor)
        {
            Image img = imageNoActiveQueue.Count > 0 ? imageNoActiveQueue.Dequeue() : Instantiate(imagePrefab, rect).GetComponent<Image>();
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
        private void Refresh()
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