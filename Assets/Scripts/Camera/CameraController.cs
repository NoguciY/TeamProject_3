using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//カメラがターゲットを追従するクラス

public class CameraController : MonoBehaviour
{
    //ターゲット
    [SerializeField]
    private Transform target;
    
    //ターゲットからの距離(ベクトル)
    [SerializeField]
    private Vector3 offset;

    void LateUpdate()
    {
        //カメラの位置をターゲットをoffset分加算した値にする
        transform.position = target.position + offset;
    }
}
