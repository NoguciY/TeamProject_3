//定数を定義する
public static class Utilities
{
    //ステージの大きさ
    public static readonly float STAGESIZE = 50f;

    //プレイヤー速さの上限
    public static readonly int SPEEDUPPERLIMIT = 7;

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
}
