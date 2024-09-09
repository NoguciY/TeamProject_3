using UnityEngine;

//プレイヤーが当たった場合
//・プレイヤーが持つ値が上昇
//・アイテムが破棄される

public class ItemExperienceValue : MonoBehaviour
{
    [SerializeField, Header("経験値")]
    private float experienceValue;

    //回収開始を呼んでからすぐに動き出すのを避ける
    [SerializeField, Header("マグネットを取ってから反応するまでの時間")]
    private float delayTimer;

    //何かしらの不具合で消えない場合に備えて回収にかかる最大の時間を設定しておく
    [SerializeField, Header("マグネットでの最大回収時間")]
    private float maxTimer;

    [SerializeField, Header("回収の速度")]
    private float speed;

    //プレイヤーにどの程度近づいたら回収したことにするか
    [SerializeField, Header("マグネットでの回収可能距離")]
    private float collectDistance;

    private Transform playerTransform;

    private float timer;

    private bool isCollect;

    private void Start()
    {
        timer = 0.0f;

        isCollect = false;
    }

    private void FixedUpdate()
    {
        if (!isCollect)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer < delayTimer)
        {
            return;
        }
        //回収の最大時間を超えていないかチェック
        else if (timer > maxTimer)
        {
            FinishCollect();
            return;
        }

        //プレイヤーに向かって進ませる
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed);

        //特定の距離まで近づいたら回収完了
        var diff = playerTransform.position - transform.position;
        if (diff.magnitude < collectDistance)
        {
            FinishCollect();
        }
    }

    /// <summary>
    /// 回収を開始する
    /// </summary>
    public void Collect(Transform playerTransform)
    {
        timer = 0.0f;
        isCollect = true;
        this.playerTransform = playerTransform;
    }

    /// <summary>
    /// 回収を完了させる
    /// </summary>
    public void FinishCollect()
    {
        isCollect = false;
        this.gameObject.SetActive(false);

        //もし回収完了のタイミングで処理をしたい場合はここに処理を追加する
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">プレイヤー</param>
    private void OnTriggerEnter(Collider other)
    {
        //アイテムを取得できるオブジェクトを取得
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if(gettableItemObject != null)
        {
            //経験値を取得させる
            gettableItemObject.GetExperienceValue(experienceValue);

            //リストから削除する
            GameManager.Instance.items.Remove(this);

            Destroy(this.gameObject);
        }
    }
}    
