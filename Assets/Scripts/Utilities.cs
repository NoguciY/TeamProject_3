using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定数を定義する
public static class Utilities
{
    //ステージの大きさ
    public static readonly float STAGESIZE = 50f;

    //防御力の上限
    public static readonly int DEFENSEUPPERLIMIT = 3;

    //爆弾の種数
    public static readonly int BOMBTYPENUM = 4;

    //追加される爆弾の種類
    public enum AddedBombType
    {
        Planted,    //設置
        Knockback,  //ノックバック
        Homing,     //誘導
    }


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
}
