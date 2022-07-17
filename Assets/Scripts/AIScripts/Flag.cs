using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Flag : MonoBehaviour
{
    public bool flagIsFollowing;
    public string scene = "ShopScene";


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            this.transform.SetParent(other.gameObject.transform);
            flagIsFollowing = true;

        }
        if(other.tag == "Goal" && flagIsFollowing)
        {
            StartCoroutine(Score());
        }
    }

    IEnumerator Score()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(scene);
        Debug.Log("You Scored");
    }
}
