using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ダメージを受けるためのインターフェース
public interface IApplicableDamage
{
    void RecieveDamage(float damage);
}
public interface EnemyDamage
{ 
    void RecieveDamage(int damage);
}


