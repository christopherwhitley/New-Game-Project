using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    [SerializeField] private float moveSpeed = 2f;

    public int index;

    int currentIndex;

    bool isMoving;
    bool isMovingRight;
    bool isPlayerClose = false;
    Animator myAnimator;

    DialogueScroll conversation;
    DialogueManager dialogueManager;

    GameObject myCanvas;

    State dialogue;
    


    // Start is called before the first frame update
    void Start()
    {
        currentIndex = index;
        isMoving = true;
        myAnimator = GetComponentInChildren<Animator>();
        
        dialogueManager = FindObjectOfType<DialogueManager>();
        myCanvas = GameObject.Find("Canvas");
        isMovingRight = true;
        conversation = GetComponent<DialogueScroll>();

        
        
        
        
        /*Debug.Log(dialogue);*/
       
    }

    // Update is called once per frame
    void Update()
    {
        /*Move();*/

        TriggerConversation();
        SkipDialogue();

    }

    private void SkipDialogue()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            dialogue = conversation.startingDialogue;
            
        }
    }

    private void Move()
    {
            if (transform.localPosition.x < waypoints[index].position.x && isMoving == true)
            {
              
                myAnimator.SetBool("Walking", true);

              
                
                transform.position = Vector2.MoveTowards(transform.position,
                waypoints[index].position,
                moveSpeed * Time.deltaTime);


            }

            else if (transform.localPosition.x > waypoints[index].position.x && isMoving == true)
            {

                myAnimator.SetBool("Walking", true);
    
                

                transform.position = Vector2.MoveTowards(transform.position,
                waypoints[index].position,
                moveSpeed * Time.deltaTime);
            }

            
            


        if (isMoving == false)
            {
                
                myAnimator.SetBool("Walking", false);
                return;
            }

            if (transform.position.x == waypoints[index].position.x)
            {
                isMoving = false;
                myAnimator.SetBool("Walking", false);
                
                StartCoroutine(Wait());

            }
        }

    private void FlipSprite()
    {
       
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.x);
         
    }





    IEnumerator Wait()
    {
       
        yield return new WaitForSeconds(5);
        index = Random.Range(0, waypoints.Length);
        
        
        while (currentIndex == index)
        {
           
            index = Random.Range(0, waypoints.Length);
         
        }

        if (currentIndex != index && transform.localPosition.x < waypoints[index].position.x && isMovingRight == true)
        {
            
            isMoving = true;
            isMovingRight = true;



            
        }
        else if (currentIndex != index && transform.localPosition.x < waypoints[index].position.x && isMovingRight == false)
        {
          
            isMoving = true;
            isMovingRight = true;
            FlipSprite();


           
        }
        
        if (currentIndex != index && transform.localPosition.x > waypoints[index].position.x && isMovingRight == false)
        {
            
            isMoving = true;
            isMovingRight = false;
         
           

        }

        if (currentIndex != index && transform.localPosition.x > waypoints[index].position.x && isMovingRight == true)
        {
           
            isMoving = true;
            isMovingRight = false;
            
            FlipSprite();


        }

        currentIndex = index;
        if (isMovingRight == false)
        {
            
           
        }
    }

    private void Talk()
    {
        
        
        dialogue = conversation.startingDialogue;
        /*dialogue = Resources.Load<State>("New State");*/

        conversation.TriggerDialogue(dialogue);
        
        

    }

    private void TriggerConversation()
    {
        
        if (isPlayerClose == true && (Input.GetKeyDown(KeyCode.E)) && myCanvas.activeInHierarchy == false)
        {
            Debug.Log("Start talking " + isPlayerClose);

            
            Talk();
        }

            if (Input.GetMouseButtonDown(1) && myCanvas.activeInHierarchy == true)
        {
            dialogueManager.DisplayNextSentence();
        }
        else return;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerClose = true;

            Debug.Log("Player close");

        }
        else isPlayerClose = false;
    }
    void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.tag.Equals("Player"))
            {
                isPlayerClose = false;
            
            }


        }
        
    
}



