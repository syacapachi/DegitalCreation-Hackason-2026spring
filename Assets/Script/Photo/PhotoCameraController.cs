
namespace Syacapachi.Controller
{
    using Syacapachi.Attribute;
    using Syacapachi.Camera;
    using Syacapachi.Utils;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using static Syacapachi.Camera.CameraCapture;

    public class PhotoCameraController : MonoBehaviour
    {

        [SerializeField] CameraCapture capture;
        [SerializeField] PhotoManager manager;
        [SerializeField] ImageViewer viewer;
        [SerializeField] InputAction captureAction;
        [SerializeField] bool overridden = false;
        [SerializeField] float showResultTime = 0.2f;
        private WaitForSeconds waitShow;
        private void Start()
        {
            waitShow = new WaitForSeconds(showResultTime);
        }
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
        private void OnCaptureComplete(PhotoData data) 
        {
            manager.AddPhoto(data, overridden);
            viewer.ShowDetail(data);
            Debug.Log("capture success");
            StartCoroutine(ShowResultCorutine());
        }
        private void OnCaptureFailed()
        {
            Debug.Log("Capture Faild");
        }
        private void OnGetPhotoData(byte[] data)
        {
            PhotoSaver.SavePNG(data, Time.time.ToString());
        }
        private IEnumerator ShowResultCorutine()
        {
            viewer.gameObject.SetActive(true);
            yield return waitShow;
            viewer.gameObject.SetActive(false);
        }
    }

}
