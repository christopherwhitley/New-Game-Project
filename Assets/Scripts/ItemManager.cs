using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField] Item itemStats;
    SceneLoader sceneLoader;
    Sprite newArmour;
    Sprite newWeapon;

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
        gems.RemoveGems(itemStats.price);
        Destroy(gameObject);
        sceneLoader = FindObjectOfType<SceneLoader>();
        //sceneLoader.LoadNextScene();
        newArmour = this.gameObject.GetComponentInChildren<Image>().sprite;

        SwapArmour();
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
        SpriteRenderer weapon = FindObjectOfType<Player>().weaponSprite;
        Debug.Log(weapon.sprite);
        weapon.sprite = newWeapon;
    }

    private void SwapArmour()
    {
        SpriteRenderer armour = FindObjectOfType<Player>().armourSprite;
        Debug.Log(armour.sprite);
        armour.sprite = newArmour;
    }
}


