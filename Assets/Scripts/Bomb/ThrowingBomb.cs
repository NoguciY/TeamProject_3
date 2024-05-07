using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class ThrowingBomb : MonoBehaviour
{
    //発射タイミングを管理
    private float timer;
    //発射数
    private int count;
    //発射間隔
    private float interval;
    //弾速
    private float speed;
    //
    private float angleRange;

    private Vector3 m_velocity; // 速度

    // 毎フレーム呼び出される関数
    private void Update()
    {
        // 移動する
        transform.localPosition += m_velocity;

        //                                            プレイヤーのポジション
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        var direction = Input.mousePosition - screenPos;

        var angle = Utilities.GetAngle(Vector3.zero, direction);

        if (timer < interval) return;

        // 弾の発射タイミングを管理するタイマーをリセットする
        timer = 0;

        // 弾を発射する
        ShootNWay(speed, count);
    }

    // 弾を発射する時に初期化するための関数
    public void Init(float speed)
    {
        // 弾の発射角度をベクトルに変換する
        var direction = transform.forward;
        // 発射角度と速さから速度を求める
        m_velocity = direction * speed;
    }

    private void ShootNWay(float speed, int count)
    {
        var pos = transform.localPosition; // プレイヤーの位置
        var rot = transform.localRotation; //プレイヤーの向き

        if (count == 1)
        {
            // 発射する弾を生成する
            var shot = Instantiate(gameObject, pos, rot);

            // 弾を発射する方向と速さを設定する
            Init(speed);
        }
    }

}
