using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�U�����
public class StateEnemyAttack : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("�U����Ԃɓ���܂���");
        //�U���A�j���[�V�����J�n
    }

    public override void OnUpdate()
    {
        //���������玟��State�ɕύX����

    }

    public override void OnExit()
    {
        //�U���A�j���[�V�����̏I��
    }
}