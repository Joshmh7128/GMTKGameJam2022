using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> shopItemGameObjects; // our list of shop items
    [SerializeField] List<Transform> shopSpots; // our list of shop spots

    private void Start()
    {
        ChooseItems();
    }

    void ChooseItems()
    {
        foreach (Transform shopSpot in shopSpots)
        {
            Instantiate(shopItemGameObjects[Random.Range(0, shopItemGameObjects.Count)], shopSpot);
        }
    }
}
