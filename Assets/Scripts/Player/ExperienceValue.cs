using System.Collections.Generic;
using UnityEngine;

public class ExperienceValue : MonoBehaviour
{
    [SerializeField, Header("レベルアップに必要な経験値の基準値")]
    private float offsetNeedExperienceValue;
    
    //レベルと必要経験値を持つディクショナリ
    private Dictionary<int, float> needExperienceValueDictionary;

    /// <summary>
    /// レベルに必要な経験値を計算してディクショナリに格納する
    /// </summary>
    /// <param name="maxLevel">レベル上限値</param>
    public void SetNeedExperience(int maxLevel)
    {
        //レベルと必要経験値を格納する
        needExperienceValueDictionary = new Dictionary<int, float>();

        //レベル1の必要経験値
        needExperienceValueDictionary.Add(1, 0);

        //レベル2から最大レベルまでの必要経験値
        for (int i = 2; i <= maxLevel; i++)
        {
            needExperienceValueDictionary.Add(i, offsetNeedExperienceValue + needExperienceValueDictionary[i - 1]);
        }
    }


    /// <summary>
    /// 引数で渡されたレベルに必要な経験値を返す
    /// </summary>
    /// <param name="currentLevel">現在のレベル</param>
    /// <returns>レベルアップに必要な経験値</returns>
    public float GetNeedExperienceValue(int currentLevel)
    {
        //後置インクリメントにすると代入後にインクリメントされるので注意
        int nextLevel = ++currentLevel;
        return needExperienceValueDictionary[nextLevel];
    }
}
