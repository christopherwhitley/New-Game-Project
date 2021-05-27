using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public Item itemStats;
    SpriteRenderer weaponSpriteRenderer;

    public List<Item> weaponList = new List<Item>();

    private void Start()
    {
        weaponList.Add(itemStats);
        weaponSpriteRenderer = GetComponent<SpriteRenderer>();
       
    }

   

    public void GetItemFromList(int itemID)
    {
        Item item = weaponList.Find(delegate (Item it)
        {
            return it.id == itemID;
            
        }
        );
        Debug.Log(item.name);
        itemStats = item;

       
        weaponSpriteRenderer.sprite = item.weaponSprite;
        
     


        Debug.Log(weaponSpriteRenderer);
    }
}
