using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�_���[�W���󂯂邽�߂̃C���^�[�t�F�[�X
public interface IApplicableDamage
{
    void RecieveDamage(float damage);
}
public interface EnemyDamage
{ 
    void RecieveDamage(int damage);
}


