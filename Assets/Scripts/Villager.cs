using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Villager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    [SerializeField] private float moveSpeed = 2f;

    public int index;

    int currentIndex;

    bool isMoving;
    bool isMovingRight;
    bool isPlayerClose = false;
    public static bool isTalking = true;
    Animator myAnimator;

    DialogueScroll conversation;
    DialogueManager dialogueManager;

    [SerializeField] GameObject myCanvas;
    Rigidbody2D myRigidbody;

    State dialogue;

    public GameObject npcPlacement;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = index;
        isMoving = true;
        
        myAnimator = GetComponentInChildren<Animator>();

        dialogueManager = FindObjectOfType<DialogueManager>();
        //myCanvas = GameObject.Find("Canvas");
        isMovingRight = true;
        conversation = GetComponent<DialogueScroll>();
        myRigidbody = GetComponent<Rigidbody2D>();

        if (transform.localPosition.x > waypoints[index].position.x)
        {

            Debug.Log("Flip");
        }
        else return;



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





    IEnumerator Wait()
    {

        yield return new WaitForSeconds(5);
        index = Random.Range(0, waypoints.Length);


        while (currentIndex == index)
        {

            index = Random.Range(0, waypoints.Length);

        }

        if (currentIndex != index && transform.localPosition.x < waypoints[index].position.x && isMovingRight == false)
        {

            isMoving = true;
            isMovingRight = true;

            FlipSprite();



        }
        else if (currentIndex != index && transform.localPosition.x < waypoints[index].position.x && isMovingRight == true)
        {

            isMoving = true;
            isMovingRight = true;




        }

        if (currentIndex != index && transform.localPosition.x > waypoints[index].position.x && isMovingRight == true)
        {

            isMoving = true;
            isMovingRight = false;

            FlipSprite();

        }

        if (currentIndex != index && transform.localPosition.x > waypoints[index].position.x && isMovingRight == false)
        {

            isMoving = true;
            isMovingRight = false;





        }

        currentIndex = index;

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

            isTalking = true;
            Talk();
            InstantiateNPC();
        }

        if (Input.GetMouseButtonDown(1) && myCanvas.activeInHierarchy == true)
        {
            dialogueManager.DisplayNextSentence();
        }

        if (myCanvas.activeInHierarchy == false)
        {
            isTalking = false;
        }
        else

            return;
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
    public void InstantiateNPC()
    {
       
        GameObject clone = Instantiate(this.gameObject, npcPlacement.transform.position, Quaternion.identity);
        


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

    
}



