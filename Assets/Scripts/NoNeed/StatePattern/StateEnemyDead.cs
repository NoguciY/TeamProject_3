using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���S���
public class StateEnemyDead : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("���S��Ԃɓ���܂���");
        //���S�A�j���[�V�����J�n
    }

    public override void OnUpdate()
    {
        //���������玟��State�ɕύX����
    }

    public override void OnExit()
    {
        //���S�A�j���[�V�����̏I��

    }
}