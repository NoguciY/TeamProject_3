using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ҋ@���
public class StateEnemyIdle : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("�ҋ@��Ԃɓ���܂���");

        //�ҋ@�A�j���[�V�����J�n
        //GetOwner.GetAnimator.SetBool("Idle", true);
    }

    public override void OnUpdate()
    {
        //�ҋ@��Ԃł�邱�Ƃ��Ȃ��̂ł����Ɉړ���ԂɈڍs����
        stateMachine.ChangeState((int)StateType.Move);
    }

    public override void OnExit()
    {
        //�ҋ@�A�j���[�V�����̏I��
        //GetOwner.GetAnimator.SetBool("Idle", false);
    }
}