using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerationManager : MonoBehaviour
{
    // under each slot transform there are a series of child objects which can be activated at random to generate a level
    [SerializeField] List<Transform> slots;

    private void Start()
    {
        RandomizeLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetRandomize(); RandomizeLevel();
        }
    }

    void BakeNavMesh()
    {

    }

    void ResetRandomize() // for testing
    {
        foreach (Transform slot in slots)
        {
            // turn off all children
            foreach (Transform child in slot)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // randomizes which tile children are active
    void RandomizeLevel()
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount > 0)
            // set this slot to active
            slot.GetChild(Random.Range(0, slot.childCount)).gameObject.SetActive(true);
        }
    }

}
