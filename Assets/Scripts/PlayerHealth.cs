using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int playerHealth = 100;
    public int potions;
    [SerializeField] public TMP_Text potionCountText;
    [SerializeField] public TMP_Text playerHealthText;
    Animator myAnimator;
    AudioSource myAudioSource;
    [SerializeField] AudioClip deathSFX;
    public bool isDead;
    public SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        string playerHealthString = playerHealth.ToString();
        playerHealthText.text = playerHealthString;
        string potionCount = potions.ToString();
        potionCountText.text = potionCount;
        myAnimator = GetComponentInChildren<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        isDead = false;
        sceneLoader = FindObjectOfType<SceneLoader>();

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void HealthLoss(int axeDamage)
    {
        
        playerHealth = playerHealth - axeDamage;
        string playerHealthString = playerHealth.ToString();
        playerHealthText.text = playerHealthString;

        if (playerHealth <= 0)
        {
            PlayerDeath();
        }


    }

    public void PlayerDeath()
    {
        
            myAnimator.SetTrigger("Death");
            myAudioSource.clip = deathSFX;
            myAudioSource.Play();
            isDead = true;

            StartCoroutine(ReloadScene());
            
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
        sceneLoader.ReloadScene();
    }

    public void AddPotion()
    {
        Debug.Log("add potion");
        potions++;
        string potionCount = potions.ToString();
        potionCountText.text = potionCount;
    }
    
     public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        playerHealth = data.health;
        string playerHealthString = playerHealth.ToString();
        playerHealthText.text = playerHealthString;

        potions = data.potions;
        string potionCount = potions.ToString();
        potionCountText.text = potionCount;

        Vector3 position;
        position.x = data.playerPosition[0];
        position.y = data.playerPosition[1];
        position.z = data.playerPosition[2];

        Player player = GetComponent<Player>();
        Weapon weapon = GetComponentInChildren<Weapon>();
        int weaponID = data.Weaponid;
        weapon.GetItemFromList(weaponID);

        Armour armour = GetComponentInChildren<Armour>();
        int armourID = data.Armourid;
        armour.GetItemFromList(armourID);

        Debug.Log(weaponID);
        
    }
    
}
