using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChest : MonoBehaviour
{
    public string QuestName;

    // Start is called before the first frame update
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player.quests.Contains(QuestName))
        {
            this.gameObject.SetActive(false);
        }
        else this.gameObject.SetActive(true);
    }


}
