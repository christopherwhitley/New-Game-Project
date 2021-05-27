using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Villager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    [SerializeField] private float moveSpeed = 2f;

    public int index;
    [SerializeField] GameObject questItem;

    int currentIndex;

    bool isMoving;
    bool isMovingRight;
    bool isFacingRight;
    bool isPlayerClose = false;
    public static bool isTalking = true;
    Animator myAnimator;

    DialogueScroll conversation;
    DialogueManager dialogueManager;

    [SerializeField] GameObject myCanvas;
    Rigidbody2D myRigidbody;

    State dialogue;

    public GameObject npcPlacement;
    public bool playerHasQuestItem;

    GameObject clone;

    public int conversationCounter;
    public string QuestName;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = index;
        isMoving = true;
        isFacingRight = true;
        myAnimator = GetComponentInChildren<Animator>();

        dialogueManager = FindObjectOfType<DialogueManager>();
        //myCanvas = GameObject.Find("Canvas");
        isMovingRight = true;
        conversation = GetComponent<DialogueScroll>();
        myRigidbody = GetComponent<Rigidbody2D>();
        playerHasQuestItem = false;

        player = FindObjectOfType<Player>();



        /*Debug.Log(dialogue);*/

    }

    // Update is called once per frame
    void Update()
    {
        Move();

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
        if (transform.localPosition.x < waypoints[index].position.x && isMoving == true && isTalking == false)
        {

            myAnimator.SetBool("Walking", true);



            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[index].position,
            moveSpeed * Time.deltaTime);


        }

        else if (transform.localPosition.x > waypoints[index].position.x && isMoving == true && isTalking == false)
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

        if (transform.position.x == waypoints[index].position.x && isTalking == false)
        {
            isMoving = false;
            myAnimator.SetBool("Walking", false);
            

            StartCoroutine(Wait());

        }
        else return;
    }

    private void FlipSprite()
    {
        
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }





    public IEnumerator Wait()
    {

        yield return new WaitForSeconds(5);
        
        index = Random.Range(0, waypoints.Length);


        while (currentIndex == index)
        {
            Debug.Log("random index");
            index = Random.Range(0, waypoints.Length);

        }

        if (currentIndex != index)
        {

            DetermineDirection();
            isMoving = true;
            



        }
        

      

        
       

        currentIndex = index;

    }

    private void Talk()
    {



        if (playerHasQuestItem == true)
        {
            dialogue = conversation.questDialogue;

        }
        else { dialogue = conversation.startingDialogue; }
        conversation.TriggerDialogue(dialogue);



    }

    private void TriggerConversation()
    {

        if (isPlayerClose == true && (Input.GetKeyDown(KeyCode.E)) && myCanvas.activeInHierarchy == false)
        {
            Debug.Log("Start talking " + isPlayerClose);

            isTalking = true;
            StopAllCoroutines();
            Talk();
            InstantiateNPC();
            myAnimator.SetBool("Walking", false);
            Animator [] villagerAnimators = FindObjectsOfType<Animator>();
            foreach(Animator animator in villagerAnimators)
            {
                animator.SetBool("Walking", false);
            }
            
        }

        if (Input.GetMouseButtonDown(1) && myCanvas.activeInHierarchy == true)
        {
            dialogueManager.DisplayNextSentence();
        }

        if (myCanvas.activeInHierarchy == false)
        {
            isTalking = false;
            StartCoroutine(Wait());
        }
        else

            return;


        if (conversationCounter < 1)
        {
            conversationCounter++;
            player.quests.Add(QuestName);

        }
        else conversationCounter++;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerClose = true;
            Player player = other.GetComponent<Player>();
            if (player.questItems.Contains(questItem)) //if player inventory list has item that matches item on vilager script;
            {
                playerHasQuestItem = true;
                Debug.Log("Player has quest item? " + playerHasQuestItem);
            }
            if (questItem == null)
            {
                return;
            }
            /*if (other.transform.position.x > transform.position.x && isTalking == true)
            {
               
                FlipSprite();
            }
            if (other.transform.position.x < transform.position.x && isTalking == true)
            {
                
                FlipSprite();
            }*/


            else 
            {

                return;
            }

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
    public void InstantiateNPC()
    {
       
        clone = Instantiate(this.gameObject, npcPlacement.transform.position, Quaternion.identity);
        


        clone.transform.parent = npcPlacement.transform;
        Rigidbody2D cloneRigidbody = clone.GetComponent<Rigidbody2D>();
        cloneRigidbody.isKinematic = true;
        clone.transform.localScale = new Vector3(125, 125, 125);
        Villager.isTalking = true;

        Vector3 myScale = clone.transform.localScale;
        if (isMovingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            clone.transform.localScale = new Vector3(-myScale.x, myScale.y, myScale.z);
        }

        SortingGroup sortGroup = clone.GetComponentInChildren<SortingGroup>();
        sortGroup.sortingLayerName = "Canvas";
        sortGroup.sortingOrder = 3;
        SpriteRenderer[] layers = clone.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer i in layers)
        {
            i.sortingLayerName = "Canvas";

        }
    }

    public void DetermineDirection()
    {
        Debug.Log("Determine Direction");
       if (transform.localScale.x > 0 && transform.localPosition.x > waypoints[index].position.x )
        {
            
            FlipSprite();
            StopAllCoroutines();
            StartCoroutine(Wait());
            Debug.Log("Determined direction, flip sprite");
        }

        if (transform.localScale.x < 0 && transform.localPosition.x < waypoints[index].position.x )
        {
           
            FlipSprite();
            StopAllCoroutines();
            StartCoroutine(Wait());
            Debug.Log("Determined direction, flip sprite");
        }
        
    }

    
}



