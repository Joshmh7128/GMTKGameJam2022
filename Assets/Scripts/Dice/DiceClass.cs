using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceClass : MonoBehaviour
{
    /// <summary>
    /// script will handle all the things on a die, player controller
    /// </summary>

    public FaceClass activeFace; // our active face
    public List<FaceClass> dieFaces = new List<FaceClass>(); // the list of all 6 of our faces
    public List<Transform> faceDisplayTransforms = new List<Transform>(); // all the positions of our die faces

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
		Dice.Player.PlayerCharacterController.instance.SwitchWeapon(activeFace.weapon); Debug.Log("setting player weapon to " + activeFace.weapon);
		rolling = false;

        UpdateInventory();
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
        // get our faces on start, after the Awake() from our inventory
        dieFaces = InventoryManager.instance.faces;
        RollTheDie();
    }

    // whenever we update the weapons we have, update our inventory
    public void UpdateInventory()
    {
        inventoryDisplayText.text = InventoryStringDisplay();
        InventoryManager.instance.faces = dieFaces;

        // update the faces
        int j = 0;
        foreach (FaceClass face in dieFaces)
        {
            face.UpdateDisplay(j); 
            j++;
        }
    }

	public void RollTheDie()
	{
		if (!rolling) {
			StartCoroutine(StartRoll(Random.Range(0, 5)));
		}
	}

    [SerializeField] Text inventoryDisplayText;

    // build a string for testing purposes
    public string InventoryStringDisplay()
    {
        // clean the string
        string inventory = "";
        // run a loop and add to the string
        int i = 0;
        foreach (var face in dieFaces)
        {
            inventory += i + " " + face.name + " | "; i++;
        }

        return inventory;
    }

    // put a weapon in our inventory
    public void SwapWeapon(int diePosition, FaceClass faceClass)
    {
        dieFaces[diePosition] = faceClass;
        UpdateInventory();
    }

}