using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{

    [SerializeField] Sprite chestOpen;
    [SerializeField] Sprite chestClosed;
    SpriteRenderer spriteRenderer;
    ParticleSystem particles;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = chestClosed;
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        TreasureChest();
    }

    private void TreasureChest()
    {
        
        
        if (Input.GetButtonDown("Interact") && spriteRenderer.sprite == chestClosed)
        {
            particles.Play();
            spriteRenderer.sprite = chestOpen;
        }
    }
}
