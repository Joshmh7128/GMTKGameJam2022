using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandChildActivate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
        transform.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);
    }

}
