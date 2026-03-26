using System;
using Syacapachi.Camera;

namespace Syacapachi.Contracts
{
    /// <summary>撮影トリガーと完了通知（DIP: 解析実装の差し替えとテスト容易性の土台）</summary>
    public interface ICaptureService
    {
        event Action OnShutter;
        event Action<CameraCapture.PhotoData> OnCaptureComplete;
        event Action OnCaptureFailed;
        event Action<int, int> OnBurstProgress;
        event Action OnBurstFinished;

        void Capture();
        void CaptureBurst(int count);
    }
}
