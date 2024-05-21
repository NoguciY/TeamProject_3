using System.Collections;
using System.Collections.Generic;
//using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrowingBomb : MonoBehaviour
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

    //爆発SE



    [SerializeField, Header("ダメージ量")]
    private float damage;

    //[SerializeField,Header("発射数")]
    //private int count;

    [SerializeField, Header("弾速(m/s)")]
    private float speed;

    [SerializeField, Header("発射間隔")]
    private float coolTime;

    [SerializeField, Header("1秒間に回転する角度(degree/s)")]
    private float angleOfRotationPerSecond;

    [SerializeField, Header("爆発パーティクル生成後から破棄するまでの時間(秒)")]
    private float particleLifeSpan;

    [SerializeField, Header("破棄する時間(s)")]
    private float bombLifeSpan;
   
    //トランスフォーム
    private Transform myTransform;

    //速度
    private Vector3 velocity;


    //ゲッター
    public float GetCoolTime => coolTime;
    
    private void Start()
    {
        myTransform = transform;
    }

    // 毎フレーム呼び出される関数
    private void Update()
    {
        // 移動する
        myTransform.localPosition += velocity * Time.deltaTime;

        //回転する
        Rotate();

        Destroy(gameObject, bombLifeSpan);
    }

    // 弾を発射する時に初期化するための関数
    public void Init()
    {
        if (playerTransform != null)
        {
            // 弾の発射角度をベクトルに変換する
            var direction = playerTransform.forward;

            // 発射角度と速さから速度を求める
            velocity = direction * speed;
        }
    }


    //x軸を軸に回転する
    private void Rotate()
    {
        //1フレームに回転する角度
        float angle = angleOfRotationPerSecond * Time.deltaTime;

        //毎フレーム回転させる
        myTransform.Rotate(angle, 0, 0);
    }

    //爆発させる
    private void Explode()
    {
        if (explosionParticle != null)
        {
            //爆発を生成
            GameObject particle =
                Instantiate(explosionParticle, myTransform.position, Quaternion.identity);

            particle.GetComponent<ParticleSystem>().Play();

            //particleLifeSpan秒後にパーティクルを消す
            Destroy(particle, particleLifeSpan);

            //効果音を再生
            SoundManager.uniqueInstance.Play("爆発1");

            Debug.Log("爆発!!");
        }
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

            //自身を破壊する
            Destroy(gameObject, bombLifeSpan);
        }
    }
}
