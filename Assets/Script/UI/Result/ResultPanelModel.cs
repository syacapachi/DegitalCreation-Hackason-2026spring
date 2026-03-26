using Syacapachi.Camera;
using Syacapachi.Contracts;
using Syacapachi.Manager;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanelModel : MonoBehaviour
{
    [SerializeField] private PhotoManager photoManager;
    [SerializeField] private CameraCapture cameraCapture;
    [SerializeField] PhotoScoreManager scoreManager;

    private IPhotoAlbum Album => photoManager;
    private ICaptureService Capture => cameraCapture;
    public List<Texture2D> GetPhotos()
    {
        return Album != null ? Album.GetPhotos() : new List<Texture2D>();
    }
    public List<CameraCapture.PhotoData> GetPhotoData()
    {
        return Album != null ? Album.GetPhotoDatas() : new List<CameraCapture.PhotoData>();
    }
    public string ScoreText
    {
        get { return scoreManager.TotalScore.ToString("n1"); }
    }
}
