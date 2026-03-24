using UnityEngine;

public class ResultPanelView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public GameObject contentArea;
    [SerializeField] private GameObject photoItemPrefab; // Prefab that has a RawImage component

    public void ClearPhotos()
    {
        foreach (Transform child in contentArea.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddPhoto(Texture2D texture)
    {
        if (photoItemPrefab == null) return;

        GameObject item = Instantiate(photoItemPrefab, contentArea.transform);
        UnityEngine.UI.RawImage rawImage = item.GetComponentInChildren<UnityEngine.UI.RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = texture;
        }
    }
}
