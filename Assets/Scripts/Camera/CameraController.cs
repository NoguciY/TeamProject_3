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

    //public float smoothTime = 0.3f;
    //private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        //�J�����̈ʒu���^�[�Q�b�g��offset�����Z�����l�ɂ���
        transform.position = target.position + offset;
    }
}
