using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGettableItem
{
    //経験値を取得する
    void GetExp(int exp);

    //回復する
    void Heal(float healValue);
}
