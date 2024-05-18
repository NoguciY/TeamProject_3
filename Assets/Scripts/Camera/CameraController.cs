using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�J�������^�[�Q�b�g��Ǐ]����N���X

public class CameraController : MonoBehaviour
{
    //�^�[�Q�b�g
    [SerializeField]
    private Transform target;
    
    //�^�[�Q�b�g����̋���(�x�N�g��)
    [SerializeField]
    private Vector3 offset;

    void LateUpdate()
    {
        //�J�����̈ʒu���^�[�Q�b�g��offset�����Z�����l�ɂ���
        transform.position = target.position + offset;
    }
}
