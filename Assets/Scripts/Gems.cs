using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gems : MonoBehaviour
{
    [SerializeField] public int gemCount = 0;
    public TMP_Text gemCountText;
    
   

    private void Awake()
    {
        
        int gameStatusCount = FindObjectsOfType<Gems>().Length;
        if(gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            string numberOfGems = gemCount.ToString();
            gemCountText.text = numberOfGems;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    private void Start()
    {
        string numberOfGems = gemCount.ToString();
        gemCountText.text = numberOfGems;
    }

    public void AddGem()
    {
        Debug.Log("Add gem");
        gemCount++;
        string numberOfGems = gemCount.ToString();
        gemCountText.text = numberOfGems;
    }

    public void RemoveGems(int gemsToRemove)
    {
        gemCount -= gemsToRemove;
        string numberOfGems = gemCount.ToString();
        gemCountText.text = numberOfGems;
    }
}
