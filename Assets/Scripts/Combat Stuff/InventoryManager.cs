using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // modify this whenever we change our faces 
    public List<FaceClass> faces; // all of our faces

    public static InventoryManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        // set up our instance so that other classes can easily find us
        instance = this;
        // make sure we keep this alive
        DontDestroyOnLoad(this);
    }
}
