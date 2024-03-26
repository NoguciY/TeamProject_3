using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth: MonoBehaviour
{
    //ç≈ëÂHP
    private const float maxHealth = 100f;

    //HP
    private float health = maxHealth;

    public float GetHealth { get { return health; } }

    public PlayerHealth()
    {
        health = maxHealth;
    }
}
