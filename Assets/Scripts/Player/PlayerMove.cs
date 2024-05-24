using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //入力値から移動ベクトルを返す
    public Vector3 InputMove(float speed)
    {
        //移動ベクトル
        Vector3 moveVec = Vector3.zero;

        //入力値を取得する
        if (Input.GetKey(KeyCode.W))
            moveVec += Vector3.forward;
        
        if (Input.GetKey(KeyCode.S))
            moveVec -= Vector3.forward;
        
        if (Input.GetKey(KeyCode.D))
            moveVec += Vector3.right;

        if (Input.GetKey(KeyCode.A))
           moveVec -= Vector3.right;

        //真っ直ぐと斜めの移動でベクトルの大きさが変わらないようにする
        if (moveVec.magnitude > 1f)
            moveVec = moveVec.normalized;

        return moveVec * speed * Time.deltaTime;
    }
}
