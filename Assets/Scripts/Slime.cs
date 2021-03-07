using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<CapsuleCollider2D>();
        float controlThrow = Input.GetAxis("Horizontal");
        moveRight = true;
        stopMove = false;
        scale = transform.localScale;
        scaleX = scale.x;
        playerLayer = LayerMask.GetMask("Player");
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        FindObjectOfType<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, range, playerLayer);
        Debug.DrawRay(myCollider.transform.position, Vector2.up * range, Color.red);

        if (hit && hit.collider.CompareTag("Player"))
        {
            stopMove = true;
            
            
            myAnimator.SetTrigger("Death");
            StartCoroutine(SlimeDeath());
        }


        if (stopMove == true)
        {
            Debug.Log("stop moving)");

            transform.Translate(0, 0, 0);
        }
        if (stopMove == false)
        {
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
        transform.localScale = scale;
        
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
            
        }

      IEnumerator SlimeDeath()
    {
        
        yield return new WaitForSeconds(0.2f);
        audioSource.clip = deathSFX;
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
        
        
        
        Destroy(gameObject);
    }

}
