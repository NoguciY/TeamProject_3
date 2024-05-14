using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetBullet : MonoBehaviour
{
    public float maxUpwardHeight = 5f; // 上昇する最大高さ
    public float moveSpeed = 10f; // 移動速度
    public Transform[] targets; // ターゲットの配列

    private bool isAscending = true; // 上昇中かどうかのフラグ
    private Vector3 initialPosition; // 初期位置

    private Transform currentTarget; // 現在のターゲット

    void Start()
    {
        initialPosition = transform.position;
        currentTarget = GetRandomTarget();
    }

    void Update()
    {
        if (isAscending)
        {
            // 上昇
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // 一定の高さまで上昇したら
            if (transform.position.y >= initialPosition.y + maxUpwardHeight)
            {
                isAscending = false;
                currentTarget = GetRandomTarget(); // 新しいランダムなターゲットを選択
            }
        }
        else
        {
            // ターゲットに向かって移動
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

            // ターゲットに到達したら
            if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                transform.position = initialPosition; // 初期位置に戻る
                isAscending = true; // 上昇フラグを再びtrueに設定
            }
        }
    }

    // ランダムなターゲットを選択するメソッド
    Transform GetRandomTarget()
    {
        return targets[Random.Range(0, targets.Length)];
    }
}
