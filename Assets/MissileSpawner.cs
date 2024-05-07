using System.Collections;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    GameObject target;
    [SerializeField]
    GameObject prefab;
    [SerializeField, Min(1)]
    int iterationCount = 3;
    [SerializeField]
    float interval = 0.1f;
    [SerializeField]
    float lifeTime = 2;

    //プレイヤーのトランスフォーム
    public Transform playerTransform;

    //爆弾の高さの半分
    private float bombHalfHeight;

    bool isSpawning = false;
    Transform thisTransform;
    WaitForSeconds intervalWait;

    public float GetBombHalfHeight => bombHalfHeight;

    void Start()
    {
        thisTransform = transform;
        intervalWait = new WaitForSeconds(interval);
        Destroy(gameObject, lifeTime);

        //爆弾の高さの半分を取得する
        SphereCollider sphereCollider = prefab.GetComponent<SphereCollider>();
        bombHalfHeight = prefab.transform.localScale.y * sphereCollider.radius; ;
    }

    void Update()
    {
        if (playerTransform != null)
        transform.position = playerTransform.position + Vector3.up * bombHalfHeight * 10;  

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
        HomingSample homing;

        for (int i = 0; i < iterationCount; i++)
        {
            homing = Instantiate(prefab, thisTransform.position, Quaternion.identity).GetComponent<HomingSample>();
            homing.Target = target;
        }

        yield return intervalWait;

        isSpawning = false;
    }
}