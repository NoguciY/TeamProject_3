using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //ˆÚ“®‚·‚éŠÖ”
    public void Move(float speed)
    {
        //ˆÚ“®‹——£
        Vector3 distance = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            distance += speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            distance -= speed * transform.forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            distance += speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            distance -= speed * transform.right * Time.deltaTime;
        }

        transform.position += distance.normalized;
    }
}
