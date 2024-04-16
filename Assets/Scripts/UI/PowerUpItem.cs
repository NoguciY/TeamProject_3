using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//‹­‰»€–Ú‚Ìˆ—
//‹­‰»€–Ú‚Ì‰æ‘œ‚ğ‚Ü‚Æ‚ß‚é

public abstract class PowerUpItem : MonoBehaviour
{
    //‹­‰»€–Ú‰æ‘œ
    [SerializeField]
    private Image powerUpItemImage;

    //‹­‰»ŠÖ”
    public virtual void PowerUpFunc(Player player)
    {
    }
}

//Å‘å‘Ì—Í‚ğ‹­‰»‚·‚é
public class MaxLife : PowerUpItem
{
    //‰æ‘œ

    //Å‘å‘Ì—Í‚ğ‹­‰»‚·‚é
    public override void PowerUpFunc(Player player)
    {
        //ˆø”‚ÉŒ»İ‚ÌÅ‘å‘Ì—Í‚ª‚¢‚é
    }
}

//ˆÚ“®‘¬“x‚ğ
