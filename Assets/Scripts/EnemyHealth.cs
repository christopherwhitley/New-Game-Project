using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] int enemyHealth = 100;
    [SerializeField] AudioClip deathSFX;
    AudioSource myAudioSource;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponentInChildren<Animator>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HealthLoss(int Damage)
    {
        Debug.Log("health loss");
        Debug.Log(Damage);
        enemyHealth = enemyHealth -= Damage;

        if(enemyHealth <= 0)
        {
            EnemyDeath();
        }



    }

    private void EnemyDeath()
    {
        
        myAnimator.SetTrigger("Death");
        myAudioSource.clip = deathSFX;
        myAudioSource.Play();
        StartCoroutine(Death());

    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
