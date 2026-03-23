namespace Syacapachi.Camera
{
    using UnityEngine;
    using System.Collections.Generic;

    public class PhotoManager : MonoBehaviour
    {
        public int maxPhotos = 10;

        private readonly List<Texture2D> photos = new List<Texture2D>();

        public bool AddPhoto(Texture2D photo, bool overwriteOld)
        {
            if (photos.Count >= maxPhotos)
            {
                if (overwriteOld)
                {
                    Destroy(photos[0]);
                    photos.RemoveAt(0);
                }
                else
                {
                    Debug.Log("保存上限です");
                    return false;
                }
            }

            photos.Add(photo);
            return true;
        }

        public List<Texture2D> GetPhotos()
        {
            return photos;
        }
    }
}