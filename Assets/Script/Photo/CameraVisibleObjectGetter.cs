using System.Collections.Generic;
using UnityEngine;

public class CameraVisibleObjectGetter : MonoBehaviour
{
    public List<GameObject> GetVisibleObjects(Camera cam, LayerMask targetLayer)
    {
        List<GameObject> result = new List<GameObject>();

        GameObject[] allObjects = FindObjectsByType<GameObject>((FindObjectsSortMode)FindObjectsInactive.Exclude);

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
}

