using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�_���[�W���󂯂��ꍇ�̏��
public class StateEnemyReceiveDamage : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("��_���[�W��Ԃɓ���܂���");
        //��_���[�W�A�j���[�V�����J�n
    }

    public override void OnUpdate()
    {
        //�m�b�N�o�b�N���Ă���ꍇ�A���b���o������ɑҋ@��Ԃɂ���


        //�̗͂�0�̏ꍇ�A���S��Ԃɂ���
        //if()
        //{
        //    stateMachine.ChangeState((int)StateType.Dead);
        //}
    }

    public override void OnExit()
    {
        //��_���[�W�A�j���[�V�����̏I��
    }
}
