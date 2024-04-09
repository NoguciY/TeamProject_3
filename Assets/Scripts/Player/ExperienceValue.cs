using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceValue : MonoBehaviour
{
    //レベルアップに必要な経験値の基準値
    private int offsetNeedExp = 1;
    
    //レベルと必要経験値を持つディクショナリ
    private Dictionary<int, int> needExpDictionary;


    // レベルに必要な経験値を計算してディクショナリに格納する
    public void CalNeedExperience(int maxLevel)
    {
        //レベルと必要経験値を格納する
        needExpDictionary = new Dictionary<int, int>();

        //レベル1の時は、必要経験値は0
        needExpDictionary[1] = 0;

        for (int i = 2; i <= maxLevel; i++)
        {
            needExpDictionary[i] = offsetNeedExp + needExpDictionary[i - 1];
        }
    }


    //引数で渡されたレベルに必要な経験値を返す
    public int GetNeedExp(int level)
    {
        return needExpDictionary[level];
    }
}
