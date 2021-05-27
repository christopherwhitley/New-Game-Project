using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Cached Component References
    Rigidbody2D myRigidbody;
    Rigidbody2D playerRigidbody;
    Animator myAnimator;
    public Player player;
    public Transform enemyTarget;
    GameObject playerTarget;
    [SerializeField] float speed = 2;
    [SerializeField] int attackRange = 4;
    [SerializeField] int defaultRange = 6;
    [SerializeField] float stopMovement = 1.4f;
    [SerializeField] float thrust = 1000;
    [SerializeField] float attackRate;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    Collider2D enemyCollider;
    public bool isAttackingPlayer;
    public int damage = 10;
    Vector3 myScale;
    public bool isBeingAttacked;
    [SerializeField] BoxCollider2D myWeaponCollider;
    [SerializeField] public int knockbackForce;
    public float knockBackCount;
    public float knockBackLength;
    public bool hitFromLeft;
    public bool hitFromRight;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<Player>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        enemyTarget = player.GetComponent<Transform>();
        myAnimator = GetComponentInChildren<Animator>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyCollider = GameObject.FindObjectOfType<Player>().GetComponent<CapsuleCollider2D>();
        myScale = transform.localScale;

        myWeaponCollider.isTrigger = false;

    }

    // Update is called once per frame
    void Update()
    {

        
        //If distance is greater than default range don't do anything
        float distance = Vector2.Distance(enemyTarget.position, transform.position);
        if (distance >= defaultRange)
        {

            return;
        }
        //if distance is less than the stopMovement float + isAttackingPlayer is false change to true and start Attack coroutine.
        if (distance <= stopMovement)
        {
            if (isAttackingPlayer == false)
            {
                isAttackingPlayer = true;
                myAnimator.ResetTrigger("Attack");
                
                StartCoroutine(Attack());

            }

        }
        //Face direction of player


        else if (knockBackCount <= 0 && distance <= attackRange)
        {

            transform.position = Vector2.MoveTowards(transform.position, enemyTarget.position, speed * Time.deltaTime);
            myAnimator.SetBool("Walking", true);
            if (enemyTarget.position.x > transform.position.x && distance <= attackRange)
            {
                //face right
                transform.localScale = new Vector3(myScale.x, myScale.y, myScale.z);
            }
            if (enemyTarget.position.x < transform.position.x && distance <= attackRange)
            {
                //face left
                transform.localScale = new Vector3(-myScale.x, myScale.y, myScale.z);

            }
            
        }

        else if (knockBackCount > 0)
        {

            myAnimator.SetBool("Walking", false);

            knockBackCount -= Time.deltaTime;
        }


        IEnumerator Attack()
        {

            
            myAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackRate);
            isAttackingPlayer = false;
            
            

        }

    }
    // Hit - Lose Health - Pushed back
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");
        if (other.gameObject.tag.Equals("Weapon") && other.transform.position.x > transform.position.x)
        {
           
            int damage = FindObjectOfType<Player>().damage;

            enemyHealth.HealthLoss(damage);

            knockBackCount = knockBackLength;

            hitFromRight = true;
            
            myRigidbody.velocity = new Vector2(-25, myRigidbody.velocity.y);


            //Vector2 direction = (transform.position - other.transform.position).normalized;

            //myRigidbody.AddForce(direction * thrust);


        }
        if (other.gameObject.tag.Equals("Weapon") && other.transform.position.x < transform.position.x)
        {
            Debug.Log(isBeingAttacked);

            int damage = FindObjectOfType<Player>().damage;

            enemyHealth.HealthLoss(damage);

            knockBackCount = knockBackLength;

            hitFromLeft = true;
            myRigidbody.velocity = new Vector2(25, myRigidbody.velocity.y);

            //Vector2 direction = (transform.position - other.transform.position).normalized;

            //myRigidbody.AddForce(direction * thrust);


        }
        else
        {
            return;
        }

    }

}
    





    
