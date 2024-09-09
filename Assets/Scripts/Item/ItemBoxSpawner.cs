using UnityEngine;

public class ItemBoxSpawner : MonoBehaviour
{
    [SerializeField, Header("スポーンさせるオブジェクト")]
    private GameObject spawnObject;

    [SerializeField, Header("プレイヤーのTransform")]
    private Transform playerTransform;

    [SerializeField, Header("スポーン範囲の半径")]
    private float spawnRadius; 

    [SerializeField,Header("スポーン間隔")]
    private float spawnInterval;

    private void Start()
    {
        //箱の生成を指定した間隔で行う
        InvokeRepeating("SpawnObjects", spawnInterval, spawnInterval);
    }

    /// <summary>
    /// ランダムな位置に箱を生成する
    /// </summary>
    private void SpawnObjects()
    {
        Vector3 randomPosition = playerTransform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = playerTransform.position.y;  // 高さをプレイヤーと同じにする
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
    }
}
