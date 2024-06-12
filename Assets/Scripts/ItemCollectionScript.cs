using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollectionScript : MonoBehaviour
{
    [SerializeField] private Image emptyItemTracker;
    [SerializeField] public PuzzelGenerator puzzelGenerator;
    public float itemCollectionPercentage = 0f;

    private void Update()
    {
        emptyItemTracker.fillAmount = itemCollectionPercentage;
    }

    public void ItemCollection()
    {
        itemCollectionPercentage += 1 / puzzelGenerator.amountOfItems;
    }
}
