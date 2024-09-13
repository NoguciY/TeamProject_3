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

    //前回の間隔
    private float beforeInterval;

    private void Start()
    {
        beforeInterval = 0;
    }

    private void Update()
    {
        //経過時間
        float elapsedTime = GameManager.Instance.GetDeltaTimeInMain;

        //間隔 = 経過時間 / 生成間隔 の余り
        float interval = elapsedTime % spawnInterval;

        //設定した秒数毎に実行する
        if (interval < beforeInterval)
        {
            SpawnObjects();
        }

        beforeInterval = interval;
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
