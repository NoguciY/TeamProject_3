using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    //ステージの大きさ
    public static float stageSize = 50f;


    /// <summary>
    /// 角度を取得する
    /// スクリーン座標系での２つのベクトルのなす角を求める場合に使っている
    /// </summary>
    /// <param name="from">ベクトル</param>
    /// <param name="to">ベクトル</param>
    /// <returns>2つのベクトルのなす角度</returns>
    public static float GetAngle(Vector3 from, Vector3 to)
    {
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dx, dy);
        return rad * Mathf.Rad2Deg;
    }

    //角度から方向を取得する
    //public static Vector3 GetDirection(float angle)
    //{
    //    return new Vector3
    //    (
    //        Mathf.Cos(angle * Mathf.Deg2Rad),
    //        Mathf.Sin(angle * Mathf.Deg2Rad),
    //        0
    //    );
    //}
}
