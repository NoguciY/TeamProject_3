//敵がダメージを受けるためのインターフェース
public interface IApplicableDamageEnemy
{
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    void ReceiveDamage(float damage);
}
