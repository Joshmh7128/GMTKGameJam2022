using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceClass : MonoBehaviour
{
    /// <summary>
    /// script will handle all the things on a die, player controller
    /// </summary>

    [SerializeField] List<FaceClass> dieFaces = new List<FaceClass>(); // the list of all 6 of our faces

    Vector3[] faceDirection = new Vector3[6] // directional rotations of each face of the die in euler angles
    {
        new Vector3(0,0,0), // one
        new Vector3(0,90,0), // two
        new Vector3(0,-90,0), // three
        new Vector3(0,180,0), // four
        new Vector3(-90,0,0), // five
        new Vector3(90,0,0) // six
    };

    Vector3 randomRoll; // the random roll we lerp to before we lerp to our desired die face
    Vector3 targetRot; // our target rotation that we are always moving towards

    [SerializeField] float rollSlerpDelta;

    private void Update()
    {
        // process our roll
        ProcessRoll();
    }

    // this rolls the die
    public void StartRoll(int targetSide)
    {
        // roll towards a high number
        randomRoll = new Vector3(Random.Range(-900, 900), Random.Range(-900, 900));
        // then roll towards our target side
        RollTime(0.5f, targetSide);
    }

    IEnumerator RollTime(float rollTime, int targetSide)
    {
        // wait for a moment before setting our real rotation
        yield return new WaitForSeconds(rollTime);
        // then set the real rotation
        targetRot = faceDirection[targetSide];
    }

    public void ProcessRoll()
    {
        // slerp towards our target rotation
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRot, rollSlerpDelta * Time.deltaTime);

    }
}
