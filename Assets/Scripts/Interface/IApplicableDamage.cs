//プレイヤーがダメージを受けるためのインターフェース
public interface IApplicableDamage
{
    /// <summary>
    /// ダメージ量を受ける
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    void ReceiveDamage(float damage);
}
