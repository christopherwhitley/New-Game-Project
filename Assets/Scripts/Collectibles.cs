using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{

    [SerializeField] Sprite chestOpen;
    [SerializeField] Sprite chestClosed;
    [SerializeField] public GameObject item;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject itemSpriteRenderer;
    ParticleSystem particles;
    GameObject itemCanvas;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = chestClosed;
        particles = GetComponent<ParticleSystem>();
        itemCanvas = GameObject.Find("Item Canvas");
        itemCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TreasureChest();
    }

    private void TreasureChest()
    {
        
        
        if (Input.GetKeyDown(KeyCode.G) && spriteRenderer.sprite == chestClosed)
        {
            particles.Play();
            spriteRenderer.sprite = chestOpen;
            itemCanvas.SetActive(true);
            text = itemCanvas.GetComponentInChildren<TextMeshProUGUI>();
            Player player = FindObjectOfType<Player>();
            player.AddItemToInventory(item);
            
            Debug.Log("item name " + item.name);
            Debug.Log(text.text);
            text.text += item.name;
            Image itemImage = itemSpriteRenderer.GetComponent<Image>();
            itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
                
        }
    }
    public void CloseMenu()
    {
        itemCanvas.SetActive(false);
    }

}
