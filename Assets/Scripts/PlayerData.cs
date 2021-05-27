using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int potions;
    public float[] playerPosition;

    public int Weaponid;
    public int Armourid;

    [System.NonSerialized]
    public Sprite weaponSprite;
    [System.NonSerialized]
    public Sprite armourSprite;

    public PlayerData (PlayerHealth playerHealth)
    {
        Player player = playerHealth.GetComponent<Player>();

        health = playerHealth.playerHealth;
        potions = playerHealth.potions;

        playerPosition = new float[3];
        playerPosition[0] = playerHealth.transform.position.x;
        playerPosition[1] = playerHealth.transform.position.y;
        playerPosition[2] = playerHealth.transform.position.z;
        Weaponid = playerHealth.GetComponentInChildren<Weapon>().itemStats.id;
        Armourid = playerHealth.GetComponentInChildren<Armour>().itemStats.id;
       
        Debug.Log(Weaponid);
    }
    
}
