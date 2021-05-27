using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemManager : MonoBehaviour
{
    [SerializeField] public Item itemStats;
    SceneLoader sceneLoader;
    Sprite newArmour;
    Sprite newWeapon;
    SpriteRenderer currentWeapon;
    SpriteRenderer currentArmour;

    private void Start()
    {
        currentArmour = FindObjectOfType<Player>().armourSprite;
        currentWeapon = FindObjectOfType<Player>().armourSprite;
    }



    public void PurchaseItem()
    {
        Debug.Log("purchase item");
        Gems gems = FindObjectOfType<Gems>();
        gems.RemoveGems(itemStats.price);
        Destroy(gameObject);
        sceneLoader = FindObjectOfType<SceneLoader>();
        //sceneLoader.LoadNextScene();        
        
    }

    public void PurchaseArmour()
    {
        Debug.Log("purchase armour");
        Gems gems = FindObjectOfType<Gems>();

        if (itemStats.price < gems.gemCount)
        {

            gems.RemoveGems(itemStats.price);
            Destroy(gameObject);
            sceneLoader = FindObjectOfType<SceneLoader>();
            //sceneLoader.LoadNextScene();
            newArmour = this.gameObject.GetComponentInChildren<Image>().sprite;

            SwapArmour();
        }
        else return;
    }

    public void PurchaseWeapon()
    {
        Debug.Log("purchase weapon");
        Gems gems = FindObjectOfType<Gems>();
        gems.RemoveGems(itemStats.price);
        Destroy(gameObject);
        sceneLoader = FindObjectOfType<SceneLoader>();
        //sceneLoader.LoadNextScene();
        newWeapon = this.gameObject.GetComponentInChildren<Image>().sprite;

        SwapWeapon();
    }

    private void SwapWeapon()
    {
        
        currentWeapon.sprite = newWeapon;
    }

    private void SwapArmour()
    {
        
        
        currentArmour.sprite = newArmour;
    }
    public void AddPotion()
    {
        Debug.Log("Button press");
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.AddPotion();

    }
}


