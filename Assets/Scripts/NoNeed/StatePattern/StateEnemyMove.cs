using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ړ����
public class StateEnemyMove : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("�ړ���Ԃɓ���܂���");
        //�ړ��A�j���[�V�����J�n
        //GetOwner.GetAnimator.SetBool("Move", true);
    }

    public override void OnUpdate()
    {
        //���e�ɓ��������ꍇ�A��_���[�W��Ԃɂ���
        if (GetOwner.isHitting)
        {
            //stateMachine.ChangeState((int)StateType.ReceiveDamage);
        }
    }

    public override void OnFixedupdate()
    {
        //�ړ�����
        //Vector3 moveForce = GetOwner.GetEnemyFlocking.Move();
        //GetOwner.GetRigidb.velocity = new Vector3(moveForce.x, GetOwner.GetRigidb.velocity.y, moveForce.z);
    }

    public override void OnExit()
    {
        GetOwner.isHitting = false;
        //�ړ��A�j���[�V�����̏I��
        //GetOwner.GetAnimator.SetBool("Move", false);
    }
}