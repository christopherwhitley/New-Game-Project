using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject

    
{
    public int id;
    public Sprite weaponSprite;
    public Sprite armourSprite;
    public int price;
    public int health;
    public int armour;
    public int damage;

}
