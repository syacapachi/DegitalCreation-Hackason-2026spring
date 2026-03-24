namespace Syacapachi.Controller
{
    using Syacapachi.Attribute;
    using Syacapachi.Camera;
    using System.Collections.Generic;
    using UnityEngine;

    using UnityEngine.UI;

    public class ImageViewer : MonoBehaviour
    {
        [SerializeField] RawImage display;
        [SerializeField] PhotoManager manager;
        private int photoindex = -1;

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
    }
}