using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] float airFriction = -2f;
    [SerializeField] AudioClip jumpSFX;


    //Cached Component References
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    float gravityScaleAtStart;
    BoxCollider2D footCollider;
    CapsuleCollider2D playerCollider;
    PlayerHealth playerHealth;
    Vector2 move;
    public Vector3 characterScale;
    float characterScaleX;
    public int damage = 20;
    [SerializeField] float thrust = 1500f;
    float controlThrow;
    bool isGrounded = true;
    Rigidbody2D floorCollider;
    GameObject foreground;
    float laserLength = 1f;
    Transform myTransform;
    Vector2 gravityAtStart;
    bool isClimbing;
    bool exitLadder;
    

    public GameObject playerPlacement;

    AudioSource audioSource;
    

    Canvas myCanvas;

    float dragAtStart;
    PhysicsMaterial2D friction;

    SceneLoader sceneLoader;
    GameObject startPoint;

    public SpriteRenderer armourSprite;
    public SpriteRenderer weaponSprite;

    private void Awake()
    {
        
        int playerCount = FindObjectsOfType<Player>().Length;
        if (playerCount > 1 && myCanvas.isActiveAndEnabled == false)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        myAnimator.SetBool("Walking", false);
        myAnimator.SetBool("Climbing", false);
        footCollider = GetComponent<BoxCollider2D>();
        foreground = GameObject.Find("Foreground");
        floorCollider = foreground.GetComponent<Rigidbody2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        gravityAtStart = Physics2D.gravity;
        Debug.Log(gravityAtStart);
        dragAtStart = myRigidBody.drag;
        playerHealth = FindObjectOfType<PlayerHealth>();
        startPoint = GameObject.FindGameObjectWithTag("Start");
        SetStartPoint();
        audioSource = GetComponent<AudioSource>();
        isClimbing = false;
        exitLadder = false;
        playerCollider = GetComponent<CapsuleCollider2D>();

        characterScale = transform.localScale;
        characterScaleX = characterScale.x;
        friction = floorCollider.sharedMaterial;

        myCanvas = FindObjectOfType<Canvas>();
        sceneLoader = FindObjectOfType<SceneLoader>();

        armourSprite = GameObject.Find("Torso[Armour]").GetComponent<SpriteRenderer>();
        weaponSprite = GameObject.Find("MeleeWeapon").GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        FlipSprite();
        ClimbLadder();
        Attack();
        

    }
    private void FixedUpdate()
    {
        float controlThrow = Input.GetAxis("Horizontal");

        int layerMask = LayerMask.GetMask("Foreground");

        RaycastHit2D hit = Physics2D.Raycast(footCollider.transform.position, Vector2.right, laserLength, layerMask);
        if (hit.collider != null && !footCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            //WARNING - If there are different sized slopes this won't work so it's better to calculate the force and apply it instead of saying 15f. Look up how to do this.
            
            
            //Physics2D.gravity = new Vector2(15f, -25f);
            if (controlThrow == 0)
            {
                
                
                myRigidBody.gravityScale = 0.01f;
            }
        }
        else if (hit.collider == null)
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
        }

        
    }


    private void Move()
    {
       

        float controlThrow = Input.GetAxis("Horizontal");

            if (controlThrow > 0)
            {
                
                Vector2 velocitytest = new Vector2(1f * moveSpeed, myRigidBody.velocity.y);
                myRigidBody.velocity = velocitytest;

                myAnimator.SetBool("Walking", true);
                

            }
            if (controlThrow == 0)
            {
                
            myAnimator.SetBool("Walking", false);
            
                

            }
            if (controlThrow < 0)
            {
                Vector2 velocitytest = new Vector2(-1f * moveSpeed, myRigidBody.velocity.y);
                myRigidBody.velocity = velocitytest;
                myAnimator.SetBool("Walking", true);
                


        }
            /*if (myCanvas.isActiveAndEnabled)
        {
            myRigidBody.velocity = Vector3.zero;
            myAnimator.SetBool("Walking", false);
            
        }*/
        


    }

    private void Jump()
    {
        if (footCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            

            if (Input.GetButtonDown("Jump"))
            {
                
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                myRigidBody.velocity = jumpVelocityToAdd;
                myRigidBody.drag = 0f;

                
                audioSource.clip = jumpSFX;
                audioSource.Play();
                

                myAnimator.SetBool("Walking", false);
                myAnimator.SetBool("Climbing", false);
                myAnimator.SetTrigger("Jump");
                
               
            }
            else
            {
                myRigidBody.drag = dragAtStart;
                
            }
        }
        if (!footCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")) && !footCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
            {
                myRigidBody.drag = 0f;
            }
    }
        
    

    private void FlipSprite()
    {
        float movement = Input.GetAxisRaw("Horizontal");
        if (movement < 0)
        {
            characterScale.x = -characterScaleX;
        }
        if (movement > 0)
        {
            characterScale.x = characterScaleX;
        }

        /*if (myCanvas.isActiveAndEnabled)
        {           
            characterScale.x = characterScaleX;
        }*/
        if (movement != 0)
        {
            transform.localScale = characterScale;
        }

        /*float movement = Input.GetAxisRaw("Horizontal");
        bool playerHasHorizontalSpeed = movement > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(movement), 1f);


        }*/
    }

    private void ClimbLadder()
    {

        float controlThrow = Input.GetAxis("Vertical");

        if (!footCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("Climbing", false);
            Physics2D.gravity = gravityAtStart;

            return;
        }

        if (controlThrow > 0)
        {
            Debug.Log("I have vertical speed");
            myAnimator.SetBool("Climbing", true);
            myAnimator.SetBool("Climbing Idle", false);
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, 1f * climbSpeed);
            myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0f;
            isClimbing = true;
        }
        if (controlThrow < 0)
        {
            Debug.Log("I have vertical speed");
            myAnimator.SetBool("Climbing", true);
            myAnimator.SetBool("Climbing Idle", false);
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, -1f * climbSpeed);
            myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0f;
            isClimbing = true;
        }

        if (controlThrow == 0)
        {
            myAnimator.SetBool("Climbing Idle", true);
        }

        if (footCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !footCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")) && isClimbing == true && exitLadder == false)
        {
            
            myRigidBody.gravityScale = 0f;
            Physics2D.gravity = new Vector2(0, 0);
            myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        }

    }
        /*if (!footCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("Climbing", false);
            
            return;
        }

        if (footCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            
            bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
            Debug.Log("I have vertical speed");

            controlThrow = Input.GetAxis("Vertical");
            if (playerHasVerticalSpeed)
            {
                myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
                myAnimator.SetBool("Climbing Idle", false);

                float controlThrow = Input.GetAxis("Vertical");
                Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
                myRigidBody.velocity = climbVelocity;
                myRigidBody.gravityScale = 0f;
            }
            if (!playerHasVerticalSpeed && controlThrow == 0)
            {
                myAnimator.SetBool("Climbing Idle", true);
            }
            else if (controlThrow != 0)
            {
                myAnimator.SetBool("Climbing Idle", false);
            }


        }
        if (footCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !footCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            myRigidBody.gravityScale = 0f;
            Physics2D.gravity = new Vector2(0, 0);
            myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
    }*/




        private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myAnimator.SetTrigger("Attack");
        }
    }

    // Hit - Lose Health - Pushed back
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag.Equals("Weapon"))
        {
            int damage = FindObjectOfType<Enemy>().damage;

            playerHealth.HealthLoss(damage);

            myRigidBody.velocity = new Vector2(0, 0);

            Vector2 direction = (transform.position - other.transform.position).normalized;

            myRigidBody.AddForce(direction * thrust);

            
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ladder Exit"))
        {
            float controlThrow = Input.GetAxis("Vertical");
            BoxCollider2D thing = collision.gameObject.GetComponent<BoxCollider2D>();
            if (controlThrow < 0)
            {
                isClimbing = true;
                thing.isTrigger = true;
                exitLadder = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ladder Exit"))
        {
            float controlThrow = Input.GetAxis("Vertical");
            BoxCollider2D thing = collision.gameObject.GetComponent<BoxCollider2D>();
            
                if (thing.isTrigger == false)
                {
                    thing.isTrigger = true;
                    exitLadder = false;
                    
                }
                
            
        }
       else if (collision.gameObject.tag.Equals("Ladder Exit") && Input.GetKeyDown(KeyCode.Space)) 
            
        {
            BoxCollider2D thing = collision.gameObject.GetComponent<BoxCollider2D>();

            if (thing.isTrigger == false)
            {
                thing.isTrigger = true;
                exitLadder = false;

            }
        }






        else
        {
            return;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Level Exit"))
        {
            Debug.Log("Exit please");
            sceneLoader.LoadNextScene();
        }
        if (collision.gameObject.tag.Equals("Last Level"))
        {
            Debug.Log("Last Level");
            sceneLoader.LoadPreviousScene();
        }
        
        




    }
    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.tag.Equals("Ladder Exit") && !playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            float controlThrow = Input.GetAxis("Vertical");
            if (controlThrow >= 0) {
                myRigidBody.constraints = RigidbodyConstraints2D.None;
                myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                exitLadder = true;
                isClimbing = false;
                

                myRigidBody.gravityScale = gravityScaleAtStart;
                BoxCollider2D thing = collision.gameObject.GetComponent<BoxCollider2D>();
                thing.isTrigger = false;
            }
        }
        
    }

    public void SetStartPoint()
    {
        
        startPoint = GameObject.FindGameObjectWithTag("Start");
        transform.position = startPoint.transform.position;
        Debug.Log("Set start point " + startPoint.transform.position);
    }

    public void InstantiateClone()
    {
        GameObject clone = Instantiate(this.gameObject, playerPlacement.transform.position, Quaternion.identity);
        clone.transform.parent = playerPlacement.transform;
        Rigidbody2D cloneRigidbody = clone.GetComponent<Rigidbody2D>();
        cloneRigidbody.isKinematic = true;
        clone.transform.localScale = new Vector3(125, 125, 125);
        SortingGroup sortGroup = clone.GetComponentInChildren<SortingGroup>();
        sortGroup.sortingLayerName = "Canvas";
        sortGroup.sortingOrder = 4;
        SpriteRenderer[] layers = clone.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer i in layers)
        {
            i.sortingLayerName = "Player";
            
        }
       
        
    }

}
