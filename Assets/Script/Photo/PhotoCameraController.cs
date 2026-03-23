using Syacapachi.Attribute;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Syacapachi.Camera
{
    public class PhotoCameraController : MonoBehaviour
    {

        [SerializeField] CameraCapture capture;
        [SerializeField] PhotoManager manager;
        [SerializeField] ImageViewer viewer;
        [SerializeField] InputAction captureAction;
        [SerializeField] bool overridden = false;
        //private void Start()
        //{
        //    //captureAction =
        //}
        private void OnEnable()
        {
            captureAction.performed += OnCaptureAction;
            captureAction.Enable();
            capture.OnCaptureComplete += OnCaptureComplete;
            capture.OnCaptureFailed += OnCaptureFailed;
        }
        private void OnDisable()
        {
            captureAction.performed -= OnCaptureAction;
            captureAction.Disable();
            capture.OnCaptureComplete -= OnCaptureComplete; 
            capture.OnCaptureFailed -= OnCaptureFailed;
        }
        private void OnCaptureAction(InputAction.CallbackContext context)
        {
            OnShotButton();
        }
        [OnInspectorButton]
        public void OnShotButton()
        {
            capture.Capture();
        }
        private void OnCaptureComplete(Texture2D texture) 
        {
            manager.AddPhoto(texture, overridden);
            viewer.Show(texture);
            Debug.Log("capture success");
        }
        private void OnCaptureFailed()
        {
            Debug.Log("Capture Faild");
        }
        private void OnGetPhotoData(byte[] data)
        {
            PhotoSaver.SavePNG(data, Time.time.ToString());
        }
    }

}
