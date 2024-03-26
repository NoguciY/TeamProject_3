using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //メイン画面の経過時間(秒)
    private float deltaTimeInMain;

    
    //ゲッター
    public float GetDeltaTimeInMain {  get { return deltaTimeInMain; } }

    private void Update()
    {
        //時間を計測
        deltaTimeInMain += Time.deltaTime;
    }
}
