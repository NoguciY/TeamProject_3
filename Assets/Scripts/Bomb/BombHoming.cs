using UnityEngine;

public class BombHoming : MonoBehaviour
{
    //爆発パーティクル
    //[System.NonSerialized]ではなく[HideInInspector]を設定することで
    //フィールド非表示時も値を保持し続けるようになる
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("爆発パーティクル生成後から破棄するまでの時間(秒)")]
    private float particleLifeSpan;

    [SerializeField, Header("ダメージ量")]
    private float damage;

    [Header("爆発するまでの時間")]
    public float fuseTime;

    [Header("爆発範囲")]
    public float explosionRadius;

    [Header("移動速度")]
    public float moveSpeed;

    [SerializeField, Header("上昇する最大高さ")]
    private float maxUpwardHeight;

    //上昇中かどうかのフラグ
    private bool isAscending;

    //初期位置
    private Vector3 initialPosition;

    //現在のターゲット
    private Transform target; 

    //コライダーコンポーネント
    [SerializeField]
    private CapsuleCollider capsuleCollider;

    // 前方の基準となるローカル空間ベクトル
    [SerializeField] private Vector3 _forward = Vector3.forward;

    //スフィアキャストの最大距離
    private float maxDistance;

    //経過時間
    private float elapsedTime;

    private void Start()
    {
        initialPosition = transform.position;
        maxDistance = 0;
        isAscending = true;

        // 最初のターゲットを設定
        SetRandomTarget();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentSceneType != SceneType.MainGame) return;

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

        //経過時間が爆発時間を過ぎた場合、爆発
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= fuseTime)
        {
            Explode();
            Detonate();
        }

    }

    /// <summary>
    /// ランダムにターゲットを設定する
    /// </summary>
    private void SetRandomTarget()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Enemy"); // "Target"タグのオブジェクトを取得
        if (targetObjects.Length > 0)
        {
            // ランダムにターゲットを選択
            int randomIndex = Random.Range(0, targetObjects.Length);
            target = targetObjects[randomIndex].transform;
        }
    }

    /// <summary>
    /// 対象に当たった場合、ダメージを与える
    /// </summary>
    void Detonate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.forward, maxDistance);

        foreach (var hit in hits)
        {
            var applicableDamageObject = hit.collider.gameObject.GetComponent<IApplicableDamageEnemy>();

            PlayerManager playe = hit.collider.gameObject.GetComponent<PlayerManager>();
            if (applicableDamageObject != null)
            {
                applicableDamageObject.ReceiveDamage(damage);
            }
        }

        Destroy(gameObject);
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
                Instantiate(explosionParticle, transform.position, Quaternion.identity);

            //particleLifeSpan秒後にパーティクルを消す
            Destroy(particle, particleLifeSpan);

            //効果音を再生
            SoundManager.uniqueInstance.PlaySE("爆発4");

            Debug.Log("爆発!!");
        }
        else
        {
            Debug.LogWarning("パーティクルがありません");
        }
    }

    /// <summary>
    /// パーティクルを生成、ダメージを与える
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //フィールドに当たった場合
        if(other.gameObject.CompareTag("Field"))
        {
            Explode();
            Detonate();
        }
    }
}
