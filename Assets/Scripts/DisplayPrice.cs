using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPrice : MonoBehaviour
{
    TextMeshProUGUI priceText;
    ItemManager itemManager;

    // Start is called before the first frame update
    void Start()
    {
        priceText = GetComponent<TextMeshProUGUI>();
        itemManager = GetComponentInParent<ItemManager>();

        priceText.text = itemManager.itemStats.price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
