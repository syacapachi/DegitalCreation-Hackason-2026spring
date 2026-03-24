namespace Syacapachi.Controller
{
    using Syacapachi.Attribute;
    using Syacapachi.Camera;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    using UnityEngine.UI;

    public class ImageViewer : MonoBehaviour
    {
        [SerializeField] RectTransform rect;
        [SerializeField] RawImage display;
        [SerializeField] Sprite circleSprite;
        [SerializeField] GameObject imagePrefab;    
        [SerializeField] PhotoManager manager;
        private Queue<Image> imageActiveQueue = new ();
        private Queue<Image> imageNoActiveQueue = new();
        private int photoindex = -1;

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
        [OnInspectorButton]
        public void ShowNext()
        {
            if (manager == null)
            {
                Debug.LogError("Manager is null");
                return;
            }
            List<Texture2D> images = manager.GetPhotos();
            if (images.Count > 0)
            {
                Show(images[photoindex++ % images.Count]);
            }
        }
        public void Show(Texture2D tex)
        {
            display.texture = tex;
        }
        public void Show(CameraCapture.PhotoData photo)
        {
            Show(photo.texture);
            Refresh();
            foreach (var info in photo.info)
            {
                PlaceCircle(rect, info.viewportPosition, info.viewportRadius);
            }
        }
        public void PlaceCircle(RectTransform parent, Vector2 viewportPos, float size)
        {
            Image img = imageNoActiveQueue.Dequeue();
            img.transform.SetParent(parent);

            // 円スプライトを設定
            img.sprite = circleSprite;
            img.color = Color.red;

            RectTransform rt = img.GetComponent<RectTransform>();

            rt.anchorMin = rt.anchorMax = viewportPos;
            rt.sizeDelta = parent.sizeDelta * size;
            rt.anchoredPosition = Vector2.zero;

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