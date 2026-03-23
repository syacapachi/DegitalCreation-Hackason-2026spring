using System.Collections.Generic;
using UnityEngine;

public class CameraVisibleObjectGetter
{
    public static List<GameObject> GetVisibleObjects(Camera cam, LayerMask targetLayer)
    {
        List<GameObject> result = new();

        List<GameObject> allObjects = new();
        GameObject.FindGameObjectsWithTag("photoTarget", allObjects);

        foreach (var obj in allObjects)
        {
            if (((1 << obj.layer) & targetLayer) == 0) continue;

            Renderer rend = obj.GetComponent<Renderer>();
            if (rend == null) continue;

            Vector3 viewportPos = cam.WorldToViewportPoint(rend.bounds.center);

            // カメラ前方 & 画面内
            if (viewportPos.z > 0 &&
                viewportPos.x >= 0 && viewportPos.x <= 1 &&
                viewportPos.y >= 0 && viewportPos.y <= 1)
            {
                result.Add(obj);
            }
        }

        return result;
    }
    public static HashSet<GameObject> GetObjectsByRaycast(Camera cam, LayerMask mask, int resolution = 10)
    {
        HashSet<GameObject> result = new HashSet<GameObject>();

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

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
                {
                    result.Add(hit.collider.gameObject);
                }
            }
        }

        return result;
    }
}

