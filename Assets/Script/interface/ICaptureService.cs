using Syacapachi.Camera;
using System;
using UnityEngine;

namespace Syacapachi.Contracts
{
    /// <summary>撮影トリガーと完了通知（DIP: 解析実装の差し替えとテスト容易性の土台）</summary>
    public interface ICaptureService
    {
        VoidEventSO OnShutter { get; }
        PhotoDataEvent OnCaptureComplete { get; }
        VoidEventSO OnCaptureFailed { get; }
        BurstProgressEvent OnBurstProgress { get; }
        VoidEventSO OnBurstFinished { get; }

        void Capture();
        void CaptureBurst(int count);
    }
}
