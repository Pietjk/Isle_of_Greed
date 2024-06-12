using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float distanceFromItem = 5f;
    [SerializeField] private float distanceFromNPC = 5f;
    [SerializeField] private float distanceFromChest = 5f;
    [SerializeField] private float maxDockDistance = 15f;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private TMP_Text chestCondition;
    [SerializeField] private TMP_Text chestInInventoryText;
    [SerializeField] private TMP_Text leaveText;
    [SerializeField] private GameObject ship;

    private bool dialogueShowing;
    private bool chestIsLocked = true;
    [SerializeField] private bool canLeave = false;

    public int itemsInInventory = 0;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private DialogueScript dialogueScript;
    [SerializeField] public PuzzelGenerator puzzelGenerator;
    [SerializeField] private ItemCollectionScript itemCollectionScript;
    private GameTracker gameTracker;

    [SerializeField] private GameObject compass;
    [SerializeField] private GameObject BoatUI;
    [SerializeField] private GameObject playerUI;

    private void Start()
    {
        gameTracker = GameObject.Find("GameTracker").GetComponent<GameTracker>();
    }

    private void Update()
    {
        //Look up and down
        if (characterController.canMove)
        {
        transform.Rotate(Input.GetAxis("Mouse Y") * -1, 0, 0);
        }

        itemCollectionScript.puzzelGenerator = puzzelGenerator;

        PickUpItems();
        UnlockChest();
        Talk();
        UnlockDock();
    }

    private void PickUpItems()
    {
        //Sends out a Raycast and puts it in hit
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanceFromItem))
        {
            //When the raycast hits a gameobject with the tag item, It gets destroyed and inventoryItems' value rises
            if (hit.collider.tag == "Item")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    Destroy(hit.collider.gameObject);
                    itemsInInventory ++;
                    itemCollectionScript.ItemCollection();
                }
            }
        }

    }

    private void UnlockChest()
    {
        chestCondition.text = "";
        chestInInventoryText.text = gameTracker.chests + " X ";

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanceFromChest))
        {
            if (hit.collider.tag == "Chest")
            {
                if (chestIsLocked == true)
                {
                    chestCondition.text = "Chest is locked";
                }
                else if (chestIsLocked == false)
                {
                    chestCondition.text = "Chest is unlocked";

                    if (Input.GetKey(KeyCode.E))
                    {
                        Destroy(hit.collider.gameObject);
                        gameTracker.chests++;
                        itemsInInventory = 0;
                        canLeave = true;
                        chestIsLocked = true;
                        itemCollectionScript.itemCollectionPercentage = 0f;
                    }
                }
            }
        }

        if (itemsInInventory >= puzzelGenerator.amountOfItems)
        {
            chestIsLocked = false;
        }
    }

    private void UnlockDock()
    {
        leaveText.text = "";
        if (canLeave == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDockDistance))
            {
                if (hit.collider.tag == "Dock")
                {
                    leaveText.text = "[Press E]";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        leaveText.text = "";
                        ship.SetActive(true);
                        transform.parent.gameObject.SetActive(false);

                        compass.SetActive(true);
                        BoatUI.SetActive(true);
                        playerUI.SetActive(false);
                    }
                }
            }
        }
    }

    private void Talk()
    {
        dialogue.SetActive(dialogueShowing);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanceFromNPC))
        {
            if (hit.collider.tag == "NPC")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (characterController.canMove == true)
                    {
                        characterController.canMove = false;
                        dialogueScript.buyButton.SetActive(true);
                        dialogueScript.sellButton.SetActive(true);

                        dialogueScript.mainText.text = "Hello there, what is that I can do for you today matey";
                    }
                    else
                    {
                        characterController.canMove = true;
                    }

                    dialogueShowing = !dialogueShowing;
                    dialogue.SetActive(dialogueShowing);
                }
            }
        }
    }
}
