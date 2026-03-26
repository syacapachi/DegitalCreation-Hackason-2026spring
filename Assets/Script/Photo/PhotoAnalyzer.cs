namespace Syacapachi.Utils
{
    using Syacapachi.Controller;
    using System.Collections.Generic;
    using UnityEngine;

    public static class PhotoAnalyzer
    {
        public struct PhotoObjectInfo
        {
            public readonly int Score;

            public readonly GameObject gameObject;

            //画面の左上を(0,0),右下を(1,1)とした。
            public readonly float MinX;
            public readonly float MaxX;
            public readonly float MinY;
            public readonly float MaxY;

            // ===== UI用 =====
            public readonly Vector2 centerPosition; // (0〜1)
            public readonly float centerRadius;     // (0〜1基準)
            public readonly Color drawColor;

            public readonly float width;
            public readonly float height;
            public readonly float size;

            public PhotoObjectInfo(GameObject obj,float minX, float maxX, float minY, float maxY)
            {
                gameObject = obj;
                MinX = minX;
                MaxX = maxX;
                MinY = minY;
                MaxY = maxY;
                
                height = Mathf.Clamp01(maxY - minY);
                size = Mathf.Clamp01((maxX - minX) * (maxY - minY));

                float centerX = (minX + maxX) * 0.5f;
                float centerY = (minY + maxY) * 0.5f;

                width = Mathf.Clamp01(maxX - minX);
                centerPosition = new Vector2(centerX, centerY); ;
                // 半径：バウンディング円として計算
                centerRadius = Mathf.Sqrt(width * width + height * height) * 0.5f; ;
                var targetData = gameObject.GetComponentInParent<PhotoTargetController>(true);
                targetData ??= gameObject.GetComponentInChildren<PhotoTargetController>(true);
                if(targetData == null)
                {
                    Debug.LogWarning($"Cant Find PhotoTargetController At {gameObject.name}");
                    Score = 100;
                    drawColor = Color.white;
                }
                else
                {
                    Score = targetData.Score;
                    drawColor = targetData.Color;
                }
            }
            public override readonly string ToString()
            {
                return
                    $"Size:{size}\n" +
                    $"Object:{gameObject.name}\n" +
                    $"Center:{centerPosition}\n" +
                    $"Raduis:{centerRadius}";
            }
        }
        /// <summary>
        /// カメラに写っているRendererをすべて取得 (一部写っているだけでも取得)
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="targetLayer"></param>
        /// <returns></returns>
        public static List<Renderer> GetVisibleComponent(Camera cam, LayerMask targetLayer)
        {
            List<Renderer> result = new();

            Renderer[] allRenderer = GameObject.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
            //Debug.Log($"TargetLayer{targetLayer.value}");
            foreach (var rend in allRenderer)
            {
                //Debug.Log($"GetVisibleRenderer {rend.gameObject.layer}");
                if (((1 << rend.gameObject.layer) & targetLayer) == 0) continue;

                Vector3 viewportPos = cam.WorldToViewportPoint(rend.bounds.center);

                // カメラ前方 & 画面内
                if (viewportPos.z > 0 &&
                    viewportPos.x >= 0 && viewportPos.x <= 1 &&
                    viewportPos.y >= 0 && viewportPos.y <= 1)
                {
                    result.Add(rend);
                }
            }

            return result;
        }
        /// <summary>
        /// カメラの領域内のGameOjbectを取得、距離に応じて取得範囲を変えられる
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="targetLayer"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static HashSet<GameObject> GetObjectsByRaycast(Camera cam, LayerMask targetLayer, int resolution = 10)
        {
            HashSet<GameObject> result = new();

            //画面を上下左右にresolution等分,そこの中心に向かってRayをとばす
            for (int x = 0; x < resolution; x++)
            {
                for (int y = 0; y < resolution; y++)
                {
                    Vector3 viewport = new Vector3(
                        (float)x / (resolution - 1),
                        (float)y / (resolution - 1),
                        0
                    );

                    Ray ray = cam.ViewportPointToRay(viewport);
                    //Layer指定を外す。
                    if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                    {
                        //ここで判別
                        if (((1 << hit.collider.gameObject.layer) & targetLayer) != 0)
                        {
                            result.Add(hit.collider.gameObject);
                        }
                    }

                }
            }

            return result;
        }
        public static HashSet<PhotoObjectInfo> GetVisibleObject(Camera cam, LayerMask targetLayer, int rayResolution = 5)
        {
            List<Renderer> candidates = GetVisibleComponent(cam, targetLayer);
            HashSet<GameObject> visibleObjects = GetObjectsByRaycast(cam, targetLayer, rayResolution);
            HashSet<PhotoObjectInfo> result = new();
            foreach (var rend in candidates)
            {
                //Debug.Log($"GetVisibleObject {rend}");
                if (!visibleObjects.Contains(rend.gameObject))
                {
                    Debug.Log($"Dont Get Object{rend}");
                    continue;
                }
                    

                Bounds b = rend.bounds;

                // 8頂点取得
                Vector3[] points = new Vector3[8];
                //縦横高さそれぞれの中心からの距離
                Vector3 ext = b.extents;
                //中心の位置
                Vector3 cen = b.center;

                int i = 0;
                for (int x = -1; x <= 1; x += 2)
                    for (int y = -1; y <= 1; y += 2)
                        for (int z = -1; z <= 1; z += 2)
                        {
                            //縦横高さにそれぞれ-1,1をかけたものを中心に足す->8頂点
                            points[i++] = cen + Vector3.Scale(ext, new Vector3(x, y, z));
                        }

                float minX = 1, minY = 1;
                float maxX = 0, maxY = 0;
                bool anyVisible = false;

                foreach (var p in points)
                {
                    Vector3 vp = cam.WorldToViewportPoint(p);

                    if (vp.z <= 0) continue;

                    anyVisible = true;

                    minX = Mathf.Min(minX, vp.x);
                    minY = Mathf.Min(minY, vp.y);
                    maxX = Mathf.Max(maxX, vp.x);
                    maxY = Mathf.Max(maxY, vp.y);
                }

                if (!anyVisible) continue;

                result.Add(new PhotoObjectInfo(
                    rend.gameObject,
                    minX, maxX,
                    minY,maxY
                ));
            }

            return result;
        }
    }

}