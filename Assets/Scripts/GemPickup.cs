﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour
{
    public ParticleSystem particles;
    Gems gems;

    private void Start()
    {
        gems = FindObjectOfType<Gems>();
        Debug.Log("Gem pickup start");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Gem pickup");
            GameObject go = Instantiate(particles.gameObject, transform.position, transform.rotation);
            gems.AddGem();
            Destroy(go, 2f);
            Destroy(this.gameObject);
        }
    }
}
