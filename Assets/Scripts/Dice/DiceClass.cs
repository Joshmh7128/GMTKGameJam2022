using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceClass : MonoBehaviour
{
    /// <summary>
    /// script will handle all the things on a die, player controller
    /// </summary>

    public FaceClass activeFace; // our active face
    [SerializeField] List<FaceClass> dieFaces = new List<FaceClass>(); // the list of all 6 of our faces

    [SerializeField] Vector3[] faceDirection = new Vector3[6] // directional rotations of each face of the die in euler angles
    {
        new Vector3(0,0,0), // one
        new Vector3(0,90,0), // two
        new Vector3(0,-90,0), // three
        new Vector3(0,180,0), // four
        new Vector3(-90,0,0), // five
        new Vector3(90,0,0) // six
    };

    [SerializeField] Vector3 randomRoll; // the random roll we lerp to before we lerp to our desired die face
    [SerializeField] Vector3 targetRot; // our target rotation that we are always moving towards
    [SerializeField] Transform targetRotTransform; // our target rotation transform

    [SerializeField] float rollSlerpDelta;

    private void Update()
    {
        // for testing
        /*if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StartCoroutine(StartRoll((int)Random.Range(0, 5))); 
        }*/

        // process our roll
        ProcessRoll();
    }

	bool rolling;

    // this rolls the die
    IEnumerator StartRoll(int targetSide)
    {
		rolling = true;
        Debug.Log("StartRoll " + targetSide);
        // roll towards a high number
        randomRoll = new Vector3(Random.Range(-900, 900), Random.Range(-900, 900), Random.Range(-900, 900));
        targetRot = randomRoll;
        yield return new WaitForSeconds(0.2f);
        // then roll towards our target side
        randomRoll = new Vector3(Random.Range(-900, 900), Random.Range(-900, 900), Random.Range(-900, 900));
        targetRot = randomRoll;
        yield return new WaitForSeconds(0.2f);
        randomRoll = new Vector3(Random.Range(-900, 900), Random.Range(-900, 900), Random.Range(-900, 900));
        targetRot = randomRoll;
        StartCoroutine(RollTime(0.5f, targetSide));
    }

    IEnumerator RollTime(float rollTime, int targetSide)
    {
        Debug.Log("Rolltime " + targetSide);
        // wait for a moment before setting our real rotation
        yield return new WaitForSeconds(rollTime);
        // then set the real rotation
        targetRot = faceDirection[targetSide];
        activeFace = dieFaces[targetSide];
		Dice.Player.PlayerCharacterController.instance.SwitchWeapon(activeFace.weapon);
		rolling = false;
    }

    public void ProcessRoll()
    {
        // set our target rot transform to the roll we want
        targetRotTransform.localEulerAngles = targetRot;

        // lerp our rotation to the target rot every frame
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotTransform.localRotation, rollSlerpDelta * Time.deltaTime);
    }

	public static DiceClass instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		RollTheDie();
	}

	public void RollTheDie()
	{
		if (!rolling) {
			StartCoroutine(StartRoll(Random.Range(0, 5)));
		}
	}
}