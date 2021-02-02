using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] Item itemStats;
    SceneLoader sceneLoader;

    public void PurchaseItem()
    {
        Debug.Log("purchase item");
        Gems gems = FindObjectOfType<Gems>();
        gems.RemoveGems(itemStats.price);
        Destroy(gameObject);
        sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.LoadNextScene();
    }

}


