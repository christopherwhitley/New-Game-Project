using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour
{
    [SerializeField] public Item itemStats;
    SpriteRenderer armourSpriteRenderer;

    public List<Item> armourList = new List<Item>();

    private void Start()
    {
        armourList.Add(itemStats);
        armourSpriteRenderer = GetComponent<SpriteRenderer>();

    }



    public void GetItemFromList(int itemID)
    {
        Item item = armourList.Find(delegate (Item it)
        {
            return it.id == itemID;

        }
        );
        Debug.Log(item.name);
        itemStats = item;


        armourSpriteRenderer.sprite = item.armourSprite;




        Debug.Log(armourSpriteRenderer);
    }


}
