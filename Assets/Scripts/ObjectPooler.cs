using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();
	Dictionary<string, GameObject> origDict = new Dictionary<string, GameObject>();
	
	#region Singleton

	public static ObjectPooler instance;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	#endregion

	#region SpawnFromPool Overloads

	public GameObject SpawnFromPool(GameObject original)
	{
		CheckIfPoolIsValid(original);

		GameObject obj = poolDict[original.name].Dequeue();

		// Do stuff here.
		obj.SetActive(true);

		return obj;
	}

	public GameObject SpawnFromPool(GameObject original, Transform parent)
	{
		CheckIfPoolIsValid(original);

		GameObject obj = poolDict[original.name].Dequeue();

		Vector3 pos = obj.transform.position;
		Quaternion rot = obj.transform.rotation;
		Vector3 sca = obj.transform.lossyScale;

		obj.transform.SetParent(parent);
		obj.transform.localPosition = pos;
		obj.transform.localRotation = rot;
		obj.transform.localScale = sca;
		obj.SetActive(true);

		return obj;
	}

	public GameObject SpawnFromPool(GameObject original, Transform parent, bool instantiateInWorldSpace)
	{
		CheckIfPoolIsValid(original);

		GameObject obj = poolDict[original.name].Dequeue();

		Vector3 pos = obj.transform.position;
		Quaternion rot = obj.transform.rotation;
		Vector3 sca = obj.transform.lossyScale;

		// If we're instantiating in world space, we want to set the parent afterwards, not here.
		if (!instantiateInWorldSpace)
			obj.transform.SetParent(parent);

		obj.transform.position = pos;
		obj.transform.rotation = rot;
		obj.transform.localScale = sca;

		// Parent can get set either way.
		obj.transform.SetParent(parent);

		obj.SetActive(true);

		return obj;
	}

	public GameObject SpawnFromPool(GameObject original, Vector3 position, Quaternion rotation) // Currently debugging this one
	{
		CheckIfPoolIsValid(original);

		GameObject obj = poolDict[original.name].Dequeue();

		obj.transform.position = position;
		obj.transform.rotation = rotation;
		obj.SetActive(true);

		return obj;
	}

	public GameObject SpawnFromPool(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
	{
		CheckIfPoolIsValid(original);

		GameObject obj = poolDict[original.name].Dequeue();

		obj.transform.SetParent(parent);
		obj.transform.localPosition = position;
		obj.transform.localRotation = rotation;
		obj.SetActive(true);

		return obj;
	}

	#endregion

	void CheckIfPoolIsValid(GameObject original)
	{
		// If we aren't pooling this object yet, we start pooling it.
		if (!poolDict.ContainsKey(original.name))
		{
			poolDict.Add(original.name, new Queue<GameObject>());
			origDict.Add(original.name, original);
		}

		// If the pool is empty, we don't have enough. Make another.
		if (poolDict[original.name].Count == 0)
		{
			GameObject newObject = Instantiate(original, transform);
			newObject.name = original.name;
			poolDict[original.name].Enqueue(newObject);
		}
	}

	public void RequeueObject(GameObject obj)
	{
		obj.SetActive(false);
		obj.transform.SetParent(transform);

		GameObject original = origDict[obj.name];
		obj.transform.position = original.transform.position;
		obj.transform.rotation = original.transform.rotation;
		obj.transform.localScale = original.transform.localScale;

		poolDict[obj.name].Enqueue(obj);
	}
}