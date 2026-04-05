namespace Syacapachi.Camera
{
    using Syacapachi.Attribute;
    using Syacapachi.Contracts;
    using Syacapachi.Utils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [RequireComponent(typeof(Camera))]
    public class CameraCapture : MonoBehaviour, ICaptureService
    {
        /// <summary>
        /// 撮影データの構造体(structは値型)
        /// </summary>
        public struct CaptureResult
        {
            public bool success;
            public Texture2D texture;
            public byte[] bytes;

            public int width;
            public int height;

            public double timestamp;

            public CaptureResult(bool success, Texture2D texture, byte[] bytes, int width, int height)
            {
                this.success = success;
                this.texture = texture;
                this.bytes = bytes;
                this.width = width;
                this.height = height;
                this.timestamp = Time.timeAsDouble;
            }
        }
        //[GenerateEvent(typeof(GameEventSOBase<>))]
        public class PhotoData
        {
            public readonly Texture2D texture;
            public readonly byte[] bytes;
            public readonly HashSet<PhotoAnalyzer.PhotoObjectInfo> info;
            public ScoreCalcrator Score {get; private set;}
            public PhotoData(Texture2D texture2D, byte[] bytes, HashSet<PhotoAnalyzer.PhotoObjectInfo> info)
            {
                texture = texture2D;
                this.bytes = bytes;
                this.info = info;
            }
        }
        //[GenerateEvent(typeof(GameEventSOBase<>))]
        public readonly struct BurstProgress
        {
            public readonly int current;
            public readonly int total;

            public BurstProgress(int current,int total)
            {
                this.current = current;
                this.total = total;
            }
            public readonly override string ToString()
            {
                return $"{current}/{total}";
            }
        }
        
        [Header("Camera")]
        [SerializeField] private Camera targetCamera;
        [SerializeField] LayerMask targetLayerMask;

        [Header("RenderTexture")]
        [SerializeField] private RenderTexture outputTexture;
        [SerializeField] private int width = 1024;
        [SerializeField] private int height = 1024;

        [Header("Settings")]
        [SerializeField] private bool useJPG = false;
        [SerializeField] private int jpgQuality = 75;
        [SerializeField] private int m_RayCount = 20;
        [SerializeField] float rayMaxDistance = 300f;

        [Header("Burst")]
        [SerializeField] private float burstInterval = 0.1f;

        [Header("Publish Event")]
        [SerializeField] VoidEventSO OnShutter;
        [SerializeField] PhotoDataEvent OnCaptureComplete;
        [SerializeField] VoidEventSO OnCaptureFailed;
        [SerializeField] BurstProgressEvent OnBurstProgress;
        [SerializeField] VoidEventSO OnBurstFinished;
        // =========================
        // 🎯 イベント（全部メインスレッド）
        // =========================

        //public event Action OnShutter;
        //public event Action<PhotoData> OnCaptureComplete;
        //public event Action OnCaptureFailed;

        ///// <summary>進捗通知（現在枚数, 総枚数）</summary>
        //public event Action<int, int> OnBurstProgress;

        ///// <summary>バースト完了</summary>
        //public event Action OnBurstFinished;

        // =========================

        private bool isCapturing = false;

        VoidEventSO ICaptureService.OnShutter => OnShutter;

        PhotoDataEvent ICaptureService.OnCaptureComplete => OnCaptureComplete;

        VoidEventSO ICaptureService.OnCaptureFailed => OnCaptureFailed;

        BurstProgressEvent ICaptureService.OnBurstProgress => OnBurstProgress;

        VoidEventSO ICaptureService.OnBurstFinished => OnBurstFinished;

        // =========================
        // 単発撮影
        // =========================
        public void Capture()
        {
            if (isCapturing) return;
            StartCoroutine(CaptureRoutine());
        }

        // =========================
        // 🎯 バースト撮影
        // =========================
        public void CaptureBurst(int count)
        {
            if (isCapturing) return;
            StartCoroutine(BurstRoutine(count));
        }

        private IEnumerator BurstRoutine(int count)
        {
            isCapturing = true;

            for (int i = 0; i < count; i++)
            {
                yield return CaptureRoutine_Internal();

                // 📊 進捗通知
                OnBurstProgress?.Invoke(new BurstProgress(i + 1, count));

                if (i < count - 1)
                {
                    yield return new WaitForSeconds(burstInterval);
                }
            }

            OnBurstFinished?.Invoke();

            isCapturing = false;
        }

        // =========================
        // 内部撮影（1枚）
        // =========================
        private IEnumerator CaptureRoutine()
        {
            isCapturing = true;
            yield return CaptureRoutine_Internal();
            isCapturing = false;
        }

        private IEnumerator CaptureRoutine_Internal()
        {
            // 📸 シャッター
            OnShutter?.Invoke();

            RenderTexture rt = outputTexture;
            bool created = false;

            if (rt == null)
            {
                rt = new RenderTexture(width, height, 24);
                created = true;
            }

            RenderTexture prevTarget = targetCamera.targetTexture;

            // 描画
            targetCamera.targetTexture = rt;
            targetCamera.Render();

            //描画完了を待つ
            yield return new WaitForEndOfFrame();

            bool done = false;
            CaptureResult result = default;

            // 非同期GPU読み取り
            AsyncGPUReadback.Request(rt, 0, request =>
            {
                if (request.hasError)
                {
                    result = new CaptureResult(false, null, null, width, height);
                    done = true;
                    return;
                }

                var data = request.GetData<byte>();

                Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
                tex.LoadRawTextureData(data);
                tex.Apply();

                byte[] bytes = useJPG
                    ? tex.EncodeToJPG(jpgQuality)
                    : tex.EncodeToPNG();

                result = new CaptureResult(true, tex, bytes, width, height);
                done = true;
            });

            HashSet<PhotoAnalyzer.PhotoObjectInfo> visibleObj = PhotoAnalyzer.GetVisibleObject(targetCamera, targetLayerMask, m_RayCount, rayMaxDistance);

            DebugObject(visibleObj);

            //GPUからの書き出しを待つ
            yield return new WaitUntil(() => done);

            // 復元
            targetCamera.targetTexture = prevTarget;

            if (created)
            {
                Destroy(rt);
            }

            PhotoData data = new PhotoData(result.texture, result.bytes, visibleObj);
            // 🎯 メインスレッドイベント
            if (result.success)
            {
                OnCaptureComplete?.Invoke(data);
            }
            else
            {
                OnCaptureFailed?.Invoke();
            }
        }
        private void DebugObject(HashSet<PhotoAnalyzer.PhotoObjectInfo> infos)
        {
            foreach(var info in infos)
            {
                Debug.Log($"{info}");
            }
        }
        private void Reset()
        {
            targetCamera ??= GetComponent<Camera>();
            outputTexture ??= targetCamera.targetTexture;
        }
    }
}

