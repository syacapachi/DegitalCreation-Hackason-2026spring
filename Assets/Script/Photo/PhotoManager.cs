namespace Syacapachi.Camera
{
    using Syacapachi.Contracts;
    using UnityEngine;
    using System.Collections.Generic;
    using Syacapachi.Manager;

    public class PhotoManager : MonoBehaviour, IPhotoAlbum
    {
        [SerializeField] PhotoScoreManager scoreManager;
        [SerializeField] ScoreCalcrator scoreCalcrator;
        public int maxPhotos = 10;
        public int MaxPhotos => maxPhotos;
        public bool IsMaxPhotos => photos.Count >= maxPhotos;
        private readonly List<CameraCapture.PhotoData> photos = new();

        [Header("Event")]
        [SerializeField] VoidEventSO OnPhotoReachMax;

        public bool AddPhoto(CameraCapture.PhotoData photoInfo, bool overwriteOld = false)
        {
            if (photos.Count >= MaxPhotos)
            {
                if (overwriteOld)
                {
                    Destroy(photos[0].texture);
                    photos.RemoveAt(0);
                }
                else
                {
                    Debug.Log("保存上限です");
                    OnPhotoReachMax?.Invoke();
                    return false;
                }
            }

            photos.Add(photoInfo);
            foreach (var info in photoInfo.info)
            {
                scoreManager.AddScore(info.gameObject,scoreCalcrator.CalcrateScore(info));
            }
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