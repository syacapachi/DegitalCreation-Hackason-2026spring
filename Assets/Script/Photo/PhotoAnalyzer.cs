using System.Collections.Generic;
using UnityEngine;

public static class PhotoAnalyzer
{
    public struct PhotoObjectInfo
    {
        public static int baseScore = 100;

        public GameObject gameObject;

        /// <summary>画面中央への近さ（0〜1）</summary>
        public float centerScore;

        /// <summary>画面内での大きさ（0〜1）</summary>
        public float sizeScore;

        /// <summary>総合スコア（用途に応じて）</summary>
        public float totalScore;

        // ===== UI用 =====
        public Vector2 viewportPosition; // (0〜1)
        public float viewportRadius;     // (0〜1基準)
        public Color color;

        public PhotoObjectInfo(GameObject obj, float center, float size, Vector2 position, float radius)
        {
            gameObject = obj;
            centerScore = center;
            sizeScore = size;
            totalScore = center * size;

            viewportPosition = position;
            viewportRadius = radius;
            color = Color.Lerp(Color.red, Color.green, totalScore);
        }
        public float GetScore()
        {
            if(gameObject.TryGetComponent<PhotoTargetController>(out var photo))
            {
                return photo.Score * totalScore;
            }
            return baseScore * totalScore;
        }
        public override readonly string ToString()
        {
            return $"Object:{gameObject.name}\n" +
                $"Center:{centerScore}" +
                $"Size:{sizeScore}" +
                $"Score:{totalScore}";
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

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayer))
                {
                    result.Add(hit.collider.gameObject);
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
            Debug.Log($"GetVisibleObject {rend}");
            if (!visibleObjects.Contains(rend.gameObject))
                continue;

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

            // =========================
            // サイズスコア（0〜1）
            // =========================
            float width = Mathf.Clamp01(maxX - minX);
            float height = Mathf.Clamp01(maxY - minY);
            float size = Mathf.Clamp01((maxX - minX) * (maxY - minY));

            // =========================
            // 中央スコア（0〜1）
            // =========================
            float centerX = (minX + maxX) * 0.5f;
            float centerY = (minY + maxY) * 0.5f;
            float dist = Vector2.Distance(
                new Vector2(centerX, centerY),
                new Vector2(0.5f, 0.5f)
            );

            float centerScore = 1f - Mathf.Clamp01(dist * 2f);

            Vector2 vpPos = new Vector2(centerX, centerY);

            // 半径：バウンディング円として計算
            float radius = Mathf.Sqrt(width * width + height * height) * 0.5f;

            result.Add(new PhotoObjectInfo(
                rend.gameObject,
                centerScore,
                size,
                vpPos,
                radius
            ));
        }

        return result;
    }
}

