using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience //: MonoBehaviour
{
    //最大レベル
    private int maxLevel = 100;
    //レベルアップに必要な経験値の基準値
    private int offsetNeedExp = 1;
    //レベルと必要経験値を持つディクショナリ
    private Dictionary<int, int> needExpDictionary;


    //コンストラクタ
    public Experience(int maxLevel, int offsetNeedExp)
    {
        this.maxLevel = maxLevel;
        this.offsetNeedExp = offsetNeedExp;
    }


    /// レベルに必要な経験値を計算してディクショナリに格納する
    public void CalNeedExperience()
    {
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
