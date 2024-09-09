using UnityEngine;


public class BombPlanted : MonoBehaviour
{
    //爆発パーティクル
    //[System.NonSerialized]ではなく[HideInInspector]を設定することで
    //フィールド非表示時も値を保持し続けるようになる
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("ダメージ量")]
    private float damage;

    [SerializeField, Header("爆発するまでの時間")]
    public float fuseTime;

    [SerializeField, Header("爆発範囲")]
    private float explosionRadius;

    public float ExplosionRadius
    {
        get { return explosionRadius; }
        set
        {
            explosionRadius = value;
            Debug.Log($"設置型爆弾の爆発範囲:{explosionRadius}");
        }
    }

    [SerializeField, Header("爆発後から爆弾を破棄するまでの時間(秒)")]
    private float bombLifeSpan;

    [SerializeField, Header("爆発パーティクル生成後から破棄するまでの時間(秒)")]
    private float particleLifeSpan;

    [SerializeField, Header("クールタイム(秒)")]
    private float coolTime;

    public float GetCoolTime => coolTime;

    //コライダーコンポーネント
    [SerializeField]
    private SphereCollider sphereCollider;

    private Transform myTransform;

    //レイを飛ばす最大距離
    private float maxDistance;

    private void Start()
    {
        myTransform = transform;
        maxDistance = 0;

        //fuseTime後に爆発する
        Invoke("Detonate", fuseTime);
    }

    /// <summary>
    /// 爆発パーティクルを生成する
    /// </summary>
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
            SoundManager.uniqueInstance.Play("爆発2");

            Debug.Log("爆発!!");
        }
    }

    /// <summary>
    /// 範囲内にいるオブジェクトにダメージを与える
    /// </summary>
    private void Detonate()
    {
        //自身を非表示
        gameObject.SetActive(false);
        
        //爆発する
        Explode();

        //球状のレイにヒットした全てのコライダーを取得する
        //引数：球の中心、球の半径、レイを飛ばす方向、飛ばす最大距離
        RaycastHit[] hits = Physics.SphereCastAll(
            myTransform.position,explosionRadius, Vector3.forward, maxDistance);
        
        foreach (var hit in hits)
        {
            //ダメージを受けることができるオブジェクトを取得
            var applicableDamageObject = 
                hit.collider.gameObject.GetComponent<IApplicableDamageEnemy>();

            if (applicableDamageObject != null)
            {
                applicableDamageObject.ReceiveDamage(damage);
            }
        }

        //爆弾を爆発パーティクル破棄後に破棄する
        Destroy(this.gameObject, bombLifeSpan);
    }


    /// <summary>
    /// 爆弾の高さの半分を取得する
    /// </summary>
    /// <returns>爆弾の半分分の高さ</returns>
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight = 
            transform.localScale.y * sphereCollider.radius;
    }
}
