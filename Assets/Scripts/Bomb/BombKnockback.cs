using UnityEngine;

//ノックバック爆弾
//プレイヤーの周りを回る
//当たった敵はノックバックする

public class BombKnockback : MonoBehaviour
{
    //BombManagerで値をセットするためpublicにした
    //プレイヤーの位置
    [System.NonSerialized]
    public Transform playerTransform;

    //爆弾の順番
    [System.NonSerialized]
    public int myID;

    //爆発パーティクル
    //[System.NonSerialized]ではなく[HideInInspector]を設定することで
    //フィールド非表示時も値を保持し続けるようになる
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("ダメージ量")]
    private float damage;

    [SerializeField, Header("ノックバック力")]
    private float knockbackForce;

    [SerializeField, Header("角速度(１秒当たりに進む角度)")]
    private float angleSpeed;

    [SerializeField, Header("プレイヤーからの距離")]
    private float toPlayerDistance;

    public float GetToPlayerDistance => toPlayerDistance;

    [SerializeField, Header("爆発後から爆弾を破棄するまでの時間(秒)")]
    private float bombLifeSpan;

    [SerializeField, Header("爆発パーティクル生成後から破棄するまでの時間(秒)")]
    private float particleLifeSpan;

    [SerializeField, Header("生成される爆弾数")]
    private int generatedBombNum;

    public int GetGeneratedBombNum => generatedBombNum;

    [SerializeField, Header("クールタイム(秒)")]
    private float coolTime;

    public float GetCoolTime => coolTime;

    //コライダーコンポーネント
    [SerializeField]
    private SphereCollider sphereCollider;

    private Transform myTransform;

    //移動方向
    private Vector3 movingDirection;

    //爆弾の高さの半分
    private float halfHeight;

    //現在の回転角度
    private float currentAngle;

    //それぞれのオブジェクトを等間隔に配置するための角度差
    private float angleBetweenObjects;

    private void Start()
    {
        myTransform = transform;
        movingDirection = Vector3.zero;
        halfHeight = GetHalfHeight();
        currentAngle = 0;
        angleBetweenObjects = 360f / generatedBombNum;
    }

    private void Update()
    {
        //プレイヤーの周りを回る
        RotateAroundPlayer();
    }

    /// <summary>
    /// プレイヤーを中心にしてy軸を軸に回転する
    /// 他のノックバック爆弾と等間隔で回転する
    /// </summary>
    private void RotateAroundPlayer()
    {
        if (GameManager.Instance.CurrentSceneType != SceneType.MainGame ||
            playerTransform == null) return;

        //敵に当たった場合に移動方向が欲しいため
        Vector3 previousPosition = myTransform.position;

        //プレイヤーの位置
        Vector3 centerPos = playerTransform.position + Vector3.up * halfHeight;

        //オブジェクト毎の回転角度
        float objectAngle = currentAngle + myID * angleBetweenObjects;

        //オイラー角からクォータニオン生成
        Quaternion rotation = Quaternion.Euler(0, objectAngle, 0);

        //回転後の位置を計算
        Vector3 rotatedPos = rotation * (Vector3.forward * toPlayerDistance);

        myTransform.position = rotatedPos + centerPos;

        //ノックバック相手に渡す爆弾の移動方向
        movingDirection = myTransform.position - previousPosition;

        //１フレームで回転する角度
        float angle = angleSpeed * Time.deltaTime;

        currentAngle += angle;

        Debug.Log($"回転する角度:{objectAngle}");
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

            //particleLifeSpan秒後にパーティクルを消す
            Destroy(particle, particleLifeSpan);

            //効果音を再生
            SoundManager.uniqueInstance.Play("爆発3");

            Debug.Log("爆発!!");
        }
        else
        {
            Debug.LogWarning("パーティクルがありません");
        }
    }


    /// <summary>
    /// 敵に当たった場合の処理
    /// </summary>
    /// <param name="other">敵</param>
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
