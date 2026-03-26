using System.Collections.Generic;
using Syacapachi.Utils;
using UnityEngine;

namespace Syacapachi.Contracts
{
    /// <summary>フレーム内被写体解析（OCP: 採点・レイキャスト方針の差し替え用）</summary>
    public interface IPhotoAnalysisStrategy
    {
        HashSet<PhotoAnalyzer.PhotoObjectInfo> GetVisibleObjects(UnityEngine.Camera camera, LayerMask targetLayerMask, int rayResolution);
    }
}
