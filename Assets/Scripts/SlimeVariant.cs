using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeVariant : MonoBehaviour
{
    CapsuleCollider2D myCollider;
    bool moveRight;
    bool stopMove;
    int speed = 3;
    Vector3 scale;
    float scaleX;
    SpriteRenderer mySpriteRenderer;
    Animator myAnimator;
    Transform flip;
    float range = 0.3f;
    LayerMask playerLayer;
    AudioSource audioSource;
    [SerializeField] AudioClip deathSFX;
    Camera mainCamera;
    [SerializeField] public int knockbackForce;
    public float knockBackCount;
    public float knockBackLength;
    public bool hitFromLeft;
    public bool hitFromRight;
    Rigidbody2D myRigidbody;
    public int damage = 10;

    public bool isVulnerable;
    public float hitLength;
    

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<CapsuleCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        float controlThrow = Input.GetAxis("Horizontal");
        moveRight = true;
        stopMove = false;
        scale = transform.localScale;
        scaleX = scale.x;
        playerLayer = LayerMask.GetMask("Player");
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        FindObjectOfType<Camera>();
        isVulnerable = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, range, playerLayer);
        Debug.DrawRay(myCollider.transform.position, Vector2.up * range, Color.red);

        if (hit && hit.collider.CompareTag("Player") && isVulnerable == true)
        {
            Debug.Log("hit");
            stopMove = true;
            
            
            myAnimator.SetTrigger("Death");
            StartCoroutine(SlimeDeath());
        }


        if (stopMove == true)
        {
            

            transform.Translate(0, 0, 0);
            
        }

        
        if (stopMove == false && knockBackCount <= 0)
        {
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            
            if (moveRight)
            {

                transform.Translate(1 * Time.deltaTime * speed, 0, 0);

                scale.x = scaleX;
                
            }
            if (moveRight == false)
            {

                transform.Translate(-1 * Time.deltaTime * speed, 0, 0);

                scale.x = -scaleX;
                
            }
        
        }
        else if (knockBackCount > 0)
        {

            knockBackCount -= Time.deltaTime;
        }

        transform.localScale = scale;
        
    }

    IEnumerator HitCoroutine()
    {
        
        Debug.Log("Hit coroutine");

        myRigidbody.velocity = new Vector2(2, myRigidbody.velocity.y);
        
        yield return new WaitForSeconds(1);

        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        isVulnerable = true;
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        myAnimator.SetBool("Vulnerable", true);
        
        yield return new WaitForSeconds(3);
        myAnimator.SetBool("Vulnerable", false);
        stopMove = false;
        isVulnerable = false;
        
    }
        void OnTriggerEnter2D(Collider2D trig)
        {
            if (trig.gameObject.CompareTag("turn"))
            {
            
            

                if (moveRight)
                {
                    moveRight = false;
                }
                else
                {
                
                    moveRight = true;
                }
            }

        if (trig.gameObject.tag.Equals("Weapon"))
        {

            knockBackCount = knockBackLength;

            hitFromRight = true;

            stopMove = true;

            //myRigidbody.velocity = new Vector2(2, myRigidbody.velocity.y);
            
           

            StartCoroutine(HitCoroutine());


        }
        else return;
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.position.x > transform.position.x && isVulnerable == false)
        {
            
            Player player = collision.gameObject.GetComponent<Player>();
            player.knockbackLength = hitLength;
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRigidbody.velocity = new Vector2(25, playerRigidbody.velocity.y);

        }
        if (collision.gameObject.transform.position.x < transform.position.x && isVulnerable == false)
        {
            
            Player player = collision.gameObject.GetComponent<Player>();
            player.knockbackLength = hitLength;
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRigidbody.velocity = new Vector2(-25, playerRigidbody.velocity.y);
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        player.playerHit = false;
    }

    IEnumerator SlimeDeath()
    {
        
        yield return new WaitForSeconds(0.2f);
        audioSource.clip = deathSFX;
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
        
        
        
        Destroy(gameObject);
    }

}
