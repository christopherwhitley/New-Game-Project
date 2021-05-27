using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Object shopScene;
    [SerializeField] Object hubScene;
    Vector3 characterScaleAtStart;

    private void OnTriggerStay2D(Collider2D other)
    {


        Debug.Log("Collision");
        characterScaleAtStart = FindObjectOfType<Player>().characterScale;
        EnterShop();

    }

    private void EnterShop()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            
            Debug.Log("load scene");
            SceneManager.LoadScene(shopScene.name);
            
        }
    }

    public void ExitShop()
    {
        Player player = FindObjectOfType<Player>();
        
        player.characterScale = characterScaleAtStart;
        SceneManager.LoadScene(hubScene.name);
        
        
    }
}
