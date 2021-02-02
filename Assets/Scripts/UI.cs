using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("UI");

        if (gameObjects.Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
