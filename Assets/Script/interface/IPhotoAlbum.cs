using System.Collections.Generic;
using Syacapachi.Camera;
using UnityEngine;

namespace Syacapachi.Contracts
{
    /// <summary>撮影枚数の保持・取得（DIP: UI は PhotoManager 具象ではなく契約に依存可能）</summary>
    public interface IPhotoAlbum
    {
        public int MaxPhotos { get; }
        public bool IsMaxPhotos { get; }
        public bool AddPhoto(CameraCapture.PhotoData photoInfo, bool overwriteOld = false);
        public List<Texture2D> GetPhotos();
        public List<CameraCapture.PhotoData> GetPhotoDatas();
    }
}
