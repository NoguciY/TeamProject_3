using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //���C����ʂ̌o�ߎ���(�b)
    private float deltaTimeInMain;

    
    //�Q�b�^�[
    public float GetDeltaTimeInMain {  get { return deltaTimeInMain; } }

    private void Update()
    {
        //���Ԃ��v��
        deltaTimeInMain += Time.deltaTime;
    }
}
