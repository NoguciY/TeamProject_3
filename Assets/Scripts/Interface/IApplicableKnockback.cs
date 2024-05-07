using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ノックバックできるオブジェクトに実装する
public interface IApplicableKnockback
{
    /// <summary>
    /// ノックバックする
    /// </summary>
    /// <param name="knockbackForce">ノックバック力</param>
    /// <param name="bombMovingDirection">ノックバック爆弾の移動方向</param>
    void Knockback(float knockbackForce, Vector3 bombMovingDirection);
}
