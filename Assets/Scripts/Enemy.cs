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
    [SerializeField] int speed = 2;
    [SerializeField] int attackRange = 4;
    [SerializeField] int defaultRange = 6;
    [SerializeField] float stopMovement = 1.4f;
    [SerializeField] float thrust = 1000;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    Collider2D enemyCollider;
    bool isAttackingPlayer;
    public int damage = 10;
    Vector3 myScale;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<Player>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        enemyTarget = player.GetComponent<Transform>();
        myAnimator = GetComponentInChildren<Animator>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        enemyHealth = FindObjectOfType<EnemyHealth>();
        enemyCollider = GameObject.FindObjectOfType<Player>().GetComponent<CapsuleCollider2D>();
        myScale = transform.localScale;
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
            else if (distance <= attackRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, enemyTarget.position, speed * Time.deltaTime);
                if (enemyTarget.position.x > transform.position.x)
                {
                //face right
                transform.localScale = new Vector3(myScale.x, myScale.y, myScale.z);
                }
                else if (enemyTarget.position.x < transform.position.x)
                {
                //face left
                transform.localScale = new Vector3(-myScale.x, myScale.y, myScale.z);
                }
            }

        IEnumerator Attack()
        {
            
            
            myAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(2);
            isAttackingPlayer = false;


        }
    }
    // Hit - Lose Health - Pushed back
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Weapon"))
        {
            int damage = FindObjectOfType<Player>().damage;

            enemyHealth.HealthLoss(damage);

            myRigidbody.velocity = new Vector2(0, 0);

            Vector2 direction = (transform.position - other.transform.position).normalized;

            myRigidbody.AddForce(direction * thrust);


        }
        else
        {
            return;
        }


    }
    }
