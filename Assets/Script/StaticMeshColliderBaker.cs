using Syacapachi.Manager;
using UnityEngine;
[RequireComponent(typeof(MeshCollider),typeof(SkinnedMeshRenderer))]
public class StaticMeshColliderBaker : MonoBehaviour
{
    // Inspectorから手動で実行できるようにする属性
    [ContextMenu("Bake Current Pose to Collider")]
    public void BakeMeshToCollider()
    {
        // 必要なコンポーネントを取得
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        if (skinnedMeshRenderer != null && meshCollider != null)
        {
            // 現在のポーズを新しいメッシュに焼き付ける
            Mesh bakedMesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(bakedMesh,true);
            
            // 焼き付けたメッシュをMesh Colliderに適用する
            meshCollider.sharedMesh = bakedMesh;
            
            Debug.Log("当たり判定（Mesh Collider）を現在のポーズに更新しました！");
        }
        else
        {
            Debug.LogWarning("SkinnedMeshRenderer または MeshCollider が見つかりません。");
        }
    }

    // 念のため、ゲーム開始時にも自動で反映されるようにしておく
    void Start()
    {
        BakeMeshToCollider();
    }
}