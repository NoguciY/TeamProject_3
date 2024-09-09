using System.Collections;
using UnityEngine;

public class BombHomingSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject homingBomb;

    public GameObject GetPrefab => homingBomb;

    [SerializeField, Min(1), Header("一度に放出する弾の数")]
    int iterationCount;     
    
    [SerializeField, Header("生成間隔")]
    float interval;      
    
    [SerializeField, Header("消去時間")]
    float lifeTime;         
    
    [SerializeField, Header("クールタイム")]
    float coolTime;

    [SerializeField, Header("生成する高さの基準値")]
    private float offsetHeight;

    //追尾爆弾のコライダーコンポーネント
    [SerializeField]
    private CapsuleCollider bombCapsuleCollider;

    public float GetCoolTime => coolTime;

    //プレイヤーのトランスフォーム
    public Transform playerTransform;

    //爆弾の高さの半分
    private float bombHalfHeight;
    
    private bool isSpawning = false;
    
    private Transform myTransform;

    private WaitForSeconds intervalWait;

    private Quaternion bombRotation;
    

    private void Start()
    {
        myTransform = transform;
        intervalWait = new WaitForSeconds(interval);
        Destroy(gameObject, lifeTime);

        //爆弾の高さの半分を取得する
        bombHalfHeight = GetBombHalfHeight();

        bombRotation = homingBomb.transform.rotation;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + Vector3.up * bombHalfHeight * offsetHeight;
        }

        if (isSpawning) return;

        //誘導爆弾を生成する
        StartCoroutine(nameof(SpawnHomingBomb));
    }

    /// <summary>
    /// 誘導型爆弾を生成するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnHomingBomb()
    {
        isSpawning = true;

        BombHoming homing;

        for (int i = 0; i < iterationCount; i++)
        {
            homing = Instantiate(homingBomb, myTransform.position, bombRotation).GetComponent<BombHoming>();
        }

        yield return intervalWait;

        isSpawning = false;
    }

    /// <summary>
    /// 爆弾の高さの半分を取得する
    /// </summary>
    /// <returns>爆弾の半分分の高さ</returns>
    public float GetBombHalfHeight()
    {
        float halfHeight;
        return halfHeight =
            homingBomb.transform.localScale.y * bombCapsuleCollider.radius;
    }
}