using Syacapachi.Camera;
using UnityEngine;
using UnityEngine.UI;

namespace Syacapachi.Contracts
{
    /// <summary>1枚の写真表示・詳細オーバーレイ（LSP: ImageViewer の契約として利用可能）</summary>
    public interface IPhotoDetailRenderer
    {
        void RefreshPicture();
        RectTransform Show(Texture2D tex);
        void ShowDetail(CameraCapture.PhotoData photo);
    }
}
