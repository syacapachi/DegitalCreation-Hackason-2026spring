using Syacapachi.Camera;
using UnityEngine;
using System.Collections.Generic;

public class ResultPanelModel : MonoBehaviour
{
    [SerializeField] private PhotoManager photoManager;

    public List<Texture2D> GetPhotos()
    {
        return photoManager != null ? photoManager.GetPhotos() : new List<Texture2D>();
    }
}
