using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //���͒l����ړ��x�N�g����Ԃ�
    public Vector3 InputMove(float speed)
    {
        //�ړ��x�N�g��
        Vector3 moveVec = Vector3.zero;

        //���͒l���擾����
        if (Input.GetKey(KeyCode.W))
            moveVec += Vector3.forward;
        
        if (Input.GetKey(KeyCode.S))
            moveVec -= Vector3.forward;
        
        if (Input.GetKey(KeyCode.D))
            moveVec += Vector3.right;

        if (Input.GetKey(KeyCode.A))
           moveVec -= Vector3.right;

        //�^�������Ǝ΂߂̈ړ��Ńx�N�g���̑傫�����ς��Ȃ��悤�ɂ���
        if (moveVec.magnitude > 1f)
            moveVec = moveVec.normalized;

        return moveVec * speed * Time.deltaTime;
    }
}
