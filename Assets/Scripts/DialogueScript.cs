using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueScript : MonoBehaviour
{
    [SerializeField] public GameObject buyButton, sellButton;
    [SerializeField] public TMP_Text mainText;

    private void Start()
    {
        mainText.text = "Hello there, what is that I can do for you today matey";
    }

    public void BuyUpgrades()
    {
        mainText.text = "Okay we can erange that!";
        buyButton.SetActive(false);
        sellButton.SetActive(false);
    }

    public void SellTreasure()
    {
        mainText.text = "Okay show me the good stuff!";
        buyButton.SetActive(false);
        sellButton.SetActive(false);
    }
}