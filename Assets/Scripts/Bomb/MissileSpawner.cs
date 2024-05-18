using System.Collections;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    GameObject target;

    [SerializeField]
    GameObject prefab;
    
    [SerializeField, Min(1)]
    int iterationCount = 3;     //一度に放出する弾の数
    
    [SerializeField]
    float interval = 0.1f;      //生成間隔
    
    [SerializeField]
    float lifeTime = 2;         //消去時間
    
    [SerializeField]
    float coolTime = 5;         //クールタイム

    //プレイヤーのトランスフォーム
    public Transform playerTransform;

    //爆弾の高さの半分
    private float bombHalfHeight;

    //
    private float offsetHeight;

    bool isSpawning = false;
    Transform myTransform;
    WaitForSeconds intervalWait;

    private Quaternion bombRotation;

    //ゲッター
    public GameObject GetPrefab => prefab;
    public float GetBombHalfHeight => bombHalfHeight;
    public float GetCoolTime => coolTime;

    void Start()
    {
        myTransform = transform;
        intervalWait = new WaitForSeconds(interval);
        Destroy(gameObject, lifeTime);

        //爆弾の高さの半分を取得する
        CapsuleCollider capsuleCollider = prefab.GetComponent<CapsuleCollider>();
        bombHalfHeight = prefab.transform.localScale.y * capsuleCollider.radius;
        bombRotation = prefab.transform.rotation;

        offsetHeight = 10;
    }

    void Update()
    {
        if (playerTransform != null)
        transform.position = playerTransform.position + Vector3.up * bombHalfHeight * offsetHeight;  

        if (isSpawning)
        {
            return;
        }

        StartCoroutine(nameof(SpawnMissile));
    }

    IEnumerator SpawnMissile()
    {
        isSpawning = true;

        //Vector3 euler;
        //Quaternion rot;
        Bullet homing;

        for (int i = 0; i < iterationCount; i++)
        {
            homing = Instantiate(prefab, myTransform.position, bombRotation).GetComponent<Bullet>();
            homing.Target = target;
        }

        yield return intervalWait;

        isSpawning = false;
    }
}