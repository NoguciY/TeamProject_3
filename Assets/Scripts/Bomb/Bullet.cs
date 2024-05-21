using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    //爆発パーティクル
    //[System.NonSerialized]ではなく[HideInInspector]を設定することで
    //フィールド非表示時も値を保持し続けるようになる
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("爆発パーティクル生成後から破棄するまでの時間(秒)")]
    private float particleLifeSpan = 3f;

    [SerializeField, Header("ダメージ量")]
    private float damage = 3;

    public float fuseTime;          //爆発するまでの時間
    public float explosionRadius;   //範囲

    public float maxUpwardHeight = 5f; // 上昇する最大高さ
    public float moveSpeed = 10f; // 移動速度

    private bool isAscending = true; // 上昇中かどうかのフラグ
    private Vector3 initialPosition; // 初期位置
    private Transform target; // 現在のターゲット
    internal GameObject Target;

    //コライダーコンポーネント
    [SerializeField]
    private CapsuleCollider capsuleCollider;

    // 前方の基準となるローカル空間ベクトル
    [SerializeField] private Vector3 _forward = Vector3.forward;

    //スフィアキャストの最大距離
    private float maxDistance;

    void Start()
    {
        initialPosition = transform.position;
        SetRandomTarget(); // 最初のターゲットを設定
        Invoke("Detonate", fuseTime);
        maxDistance = 0;
    }

    void Update()
    {
        if (isAscending)
        {
            // 上昇
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // 一定の高さまで上昇したら
            if (transform.position.y >= initialPosition.y + maxUpwardHeight)
            {
                isAscending = false;
                SetRandomTarget(); // ターゲットを設定
            }
        }
        else if (isAscending == false)
        {
            if (target == null)
            {
                // 上昇
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                isAscending = true;
                return;
            }
            // ターゲットに向かって移動
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // ターゲットへの向きベクトル計算
            var dir = target.position - transform.position;
            // ターゲットの方向への回転
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
            // 回転補正
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);

            // 回転補正→ターゲット方向への回転の順に、自身の向きを操作する
            transform.rotation = lookAtRotation * offsetRotation;

            // ターゲットに到達したら
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Explode();
                Detonate();
            }
        }
    }

    void SetRandomTarget()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Enemy"); // "Target"タグのオブジェクトを取得
        if (targetObjects.Length > 0)
        {
            // ランダムにターゲットを選択
            int randomIndex = Random.Range(0, targetObjects.Length);
            target = targetObjects[randomIndex].transform;
        }
    }
    void Detonate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.forward, maxDistance);

        foreach (var hit in hits)
        {
            var applicableDamageObject = hit.collider.gameObject.GetComponent<IApplicableDamageEnemy>();

            Player playe = hit.collider.gameObject.GetComponent<Player>();
            if (applicableDamageObject != null)
                applicableDamageObject.ReceiveDamage(damage);
        }

        Destroy(gameObject);
    }
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight =
            transform.localScale.y * capsuleCollider.radius; 
    }

    //爆発させる
    private void Explode()
    {
        if (explosionParticle != null)
        {
            //爆発を生成
            GameObject particle =
                Instantiate(explosionParticle, transform.position, Quaternion.identity);

            particle.GetComponent<PlayParticles>().Play();

            //particleLifeSpan秒後にパーティクルを消す
            Destroy(particle, particleLifeSpan);

            //効果音を再生
            SoundManager.uniqueInstance.Play("爆発1");

            Debug.Log("爆発!!");
        }
        else
            Debug.LogWarning("パーティクルがありません");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Field"))
        {
            Explode();
            Detonate();
        }
    }
}
