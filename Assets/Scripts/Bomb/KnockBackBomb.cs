using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

//ノックバック爆弾
//プレイヤーの周りを回る
//当たった敵はノックバックする

public class KnockbackBomb : MonoBehaviour
{
    //BombManagerで値をセットするためpublicにした
    //プレイヤーの位置
    [System.NonSerialized]
    public Transform playerTransform;

    //爆発パーティクル
    //[System.NonSerialized]ではなく[HideInInspector]を設定することで
    //フィールド非表示時も値を保持し続けるようになる
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("ダメージ量")]
    private float damage = 0;

    [SerializeField, Header("ノックバック力")]
    private float knockbackForce = 10f;

    [SerializeField, Header("角速度(１秒当たりに進む角度)")]
    private float angleSpeed = 180f;

    [SerializeField, Header("プレイヤーからの距離")]
    private float toPlayerDistance = 5f;

    [SerializeField, Header("爆発後から爆弾を破棄するまでの時間(秒)")]
    private float bombLifeSpan = 5f;

    [SerializeField, Header("爆発パーティクル生成後から破棄するまでの時間(秒)")]
    private float particleLifeSpan = 3f;

    [SerializeField, Header("生成される爆弾数")]
    private int generatedBombNum = 2;

    [SerializeField, Header("クールタイム(秒)")]
    private float coolTime = 60f;

    //コライダーコンポーネント
    [SerializeField]
    private SphereCollider sphereCollider;

    //回転軸
    private Vector3 axis = Vector3.up;

    //transformに毎回アクセスすると重いので、キャッシュしておく
    private Transform myTransform;

    //爆弾の高さの半分
    private float bombHalfHeight;

    //移動方向
    private Vector3 movingDirection = Vector3.zero;

    //ゲッター
    public float GetToPlayerDistance => toPlayerDistance;
    public int GetGeneratedBombNum => generatedBombNum;
    public float GetCoolTime => coolTime;

    private void Start()
    {
        myTransform = transform;
        bombHalfHeight = GetHalfHeight();
    }

    private void Update()
    {
        //プレイヤーの周りを回る
        RevolveAboutPlayer();
    }


    //爆弾の高さの半分を取得する
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight =
            transform.localScale.y * sphereCollider.radius;
    }

    //プレイヤーを中心にしてy軸を軸に回転する
    private void RevolveAboutPlayer()
    {
        if (playerTransform != null)
        {
            //敵に当たった場合に移動方向が欲しいため
            Vector3 previousPosition = myTransform.position;

            //プレイヤーの位置を中心に回転する
            Vector3 centerPos = playerTransform.position;

            //１フレームで回転する角度
            var angle = angleSpeed * Time.deltaTime;

            //指定した角度と軸から回転(クオータニオン)を作成
            var angleAxis = Quaternion.AngleAxis(angle, axis);

            //円運動の位置計算
            //中心からの相対ベクトル(爆弾の現在位置 - プレイヤーの位置)
            Vector3 fromCenterVec = myTransform.position - centerPos;

            //相対ベクトルの向き
            Vector3 direction = fromCenterVec.normalized;
            
            //位置を中心点からの相対的な向きだけ残して、円運動の半径を設定
            Vector3 pos = direction * toPlayerDistance;

            //円運動の軌道上の位置に移動させる
            pos = angleAxis * pos;

            //中心点を加算し、爆弾の位置を更新
            myTransform.position = pos + centerPos;

            //ノックバック相手に渡す爆弾の移動方向
            movingDirection = myTransform.position - previousPosition;
        }
    }

    //爆発させる
    private void Explode()
    {
        if (explosionParticle != null)
        {
            //爆発を生成
            GameObject particle =
                Instantiate(explosionParticle, myTransform.position, Quaternion.identity);

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


    //敵に当たった場合の処理
    private void OnTriggerEnter(Collider other)
    {
        //ダメージを受けることができるオブジェクトを取得
        var applicableDamageObject = other.gameObject.GetComponent<IApplicableDamageEnemy>();
        if (applicableDamageObject != null)
        {
            //自身を非表示
            gameObject.SetActive(false);

            //爆発する
            Explode();

            //ダメージを与える
            applicableDamageObject.ReceiveDamage(damage);

            //ノックバックできるオブジェクトの場合
            var knockbackObject = other.gameObject.GetComponent<IApplicableKnockback>();
            if (knockbackObject != null)
            {
                //ノックバックする
                knockbackObject.Knockback(knockbackForce, movingDirection);
            }

            //自身を破壊する
            Destroy(gameObject, bombLifeSpan);
        }
    }
}
