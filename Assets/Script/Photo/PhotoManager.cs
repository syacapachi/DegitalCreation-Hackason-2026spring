namespace Syacapachi.Camera
{
    using UnityEngine;
    using System.Collections.Generic;

    public class PhotoManager : MonoBehaviour
    {
        public int maxPhotos = 10;

        private readonly List<CameraCapture.PhotoData> photos = new();

        public bool AddPhoto(CameraCapture.PhotoData photoInfo, bool overwriteOld)
        {
            if (photos.Count >= maxPhotos)
            {
                if (overwriteOld)
                {
                    Destroy(photos[0].texture);
                    photos.RemoveAt(0);
                }
                else
                {
                    Debug.Log("保存上限です");
                    return false;
                }
            }

            photos.Add(photoInfo);
            return true;
        }

        public List<Texture2D> GetPhotos()
        {
            List<Texture2D> result = new List<Texture2D>();
            foreach(var data in photos)
            {
                result.Add(data.texture);
            }
            return result;
        }
        public List<CameraCapture.PhotoData> GetPhotoDatas()
        {
            return photos;
        }
    }
}