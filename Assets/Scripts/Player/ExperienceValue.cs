using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceValue : MonoBehaviour
{
    //レベルアップに必要な経験値の基準値
    private float offsetNeedExp = 0.5f;
    
    //レベルと必要経験値を持つディクショナリ
    private Dictionary<int, float> needExpDictionary;

    // レベルに必要な経験値を計算してディクショナリに格納する
    public void SetNeedExperience(int maxLevel)
    {
        //レベルと必要経験値を格納する
        needExpDictionary = new Dictionary<int, float>();

        //レベル1の必要経験値
        //needExpDictionary[1] = 0;
        needExpDictionary.Add(1, 0);

        //レベル2から最大レベルまでの必要経験値
        for (int i = 2; i <= maxLevel; i++)
            //needExpDictionary[i] = offsetNeedExp + needExpDictionary[i - 1];
            needExpDictionary.Add(i, offsetNeedExp + needExpDictionary[i - 1]);
    }


    //引数で渡されたレベルに必要な経験値を返す
    public float GetNeedExp(int currentLevel)
    {
        //後置インクリメントにすると代入後にインクリメントされるので注意
        int nextLevel = ++currentLevel;
        return needExpDictionary[nextLevel];
    }
}
