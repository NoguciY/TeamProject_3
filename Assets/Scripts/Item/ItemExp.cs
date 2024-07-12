using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーが当たった場合
//・プレイヤーが持つ値が上昇
//・アイテムが破棄される

public class ItemExp : MonoBehaviour
{
    //経験値
    private int exp = 1;

    [SerializeField]
    private Transform _playerTransform;

    //回収開始を呼んでからすぐに動き出すのを避ける
    [SerializeField]
    private float _delayTimer = 0.5f;

    //何かしらの不具合で消えない場合に備えて回収にかかる最大の時間を設定しておく
    [SerializeField]
    private float _maxTimer = 10.0f;

    //回収の速度
    [SerializeField]
    private float _speed = 0.4f;

    //プレイヤーにどの程度近づいたら回収したことにするか
    [SerializeField]
    private float _collectDistance = 0.3f;

    private float _timer = 0.0f;
    private bool _isCollect = false;

    private void FixedUpdate()
    {
        if (!_isCollect)
        {
            return;
        }

        _timer += Time.deltaTime;
        if (_timer < _delayTimer)
        {
            return;
        }
        //回収の最大時間を超えていないかチェック
        else if (_timer > _maxTimer)
        {
            FinishCollect();
            return;
        }

        //プレイヤーに向かって進ませる
        transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _speed);

        //特定の距離まで近づいたら回収完了
        var diff = _playerTransform.position - transform.position;
        if (diff.magnitude < _collectDistance)
        {
            FinishCollect();
        }
    }

    /// <summary>
    /// 回収を開始する
    /// </summary>
    public void Collect(Transform playerTransform)
    {
        _timer = 0.0f;
        _isCollect = true;
        _playerTransform = playerTransform;
    }

    /// <summary>
    /// 回収を完了させる
    /// </summary>
    public void FinishCollect()
    {
        _isCollect = false;
        this.gameObject.SetActive(false);

        //もし回収完了のタイミングで処理をしたい場合はここに処理を追加する
    }

    private void OnTriggerEnter(Collider other)
    {
        //アイテムを取得できるオブジェクトを取得
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if(gettableItemObject != null)
        {
            //経験値を取得させる
            gettableItemObject.GetExp(exp);

            //リストから削除する
            GameManager.Instance.items.Remove(this);

            Destroy(this.gameObject);
        }
    }
}    
