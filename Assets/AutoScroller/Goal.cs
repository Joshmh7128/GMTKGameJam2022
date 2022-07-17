using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public string scene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(ToShop());

        }
    }
    IEnumerator ToShop()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(scene);
    }
}
