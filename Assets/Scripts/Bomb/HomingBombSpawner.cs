using System.Collections;
using UnityEngine;

public class HomingBombSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    
    [SerializeField, Min(1), Header("一度に放出する弾の数")]
    int iterationCount;     
    
    [SerializeField, Header("生成間隔")]
    float interval;      
    
    [SerializeField, Header("消去時間")]
    float lifeTime;         
    
    [SerializeField, Header("クールタイム")]
    float coolTime;         

    //プレイヤーのトランスフォーム
    public Transform playerTransform;

    //爆弾の高さの半分
    private float bombHalfHeight;

    //生成物の基準となる高さ
    private float offsetHeight;

    private bool isSpawning = false;
    
    private Transform myTransform;

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
            return;

        //誘導爆弾を生成する
        StartCoroutine(nameof(SpawnHomingBomb));
    }

    //誘導型爆弾を生成するコルーチン
    IEnumerator SpawnHomingBomb()
    {
        isSpawning = true;

        BombHoming homing;

        for (int i = 0; i < iterationCount; i++)
        {
            homing = Instantiate(prefab, myTransform.position, bombRotation).GetComponent<BombHoming>();
        }

        yield return intervalWait;

        isSpawning = false;
    }
}