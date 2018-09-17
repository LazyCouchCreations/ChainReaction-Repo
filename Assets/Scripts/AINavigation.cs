using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINavigation : MonoBehaviour {

	[SerializeField]
	private List<Transform> POIs;
	public GameObject characterPrefab;
	public int numCharactersToSpawn;
	public Transform POIParent;

	// Use this for initialization
	void Awake () {
		
		foreach(Transform POI in POIParent)
		{
			POIs.Add(POI);
		}

		GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

		for (int i = 0; i < numCharactersToSpawn; i++)
		{
			GameObject character = Instantiate(characterPrefab, POIs[Random.Range(0, POIs.Count - 1)].transform.position, Quaternion.identity);
			gameManager.enemies.Add(character);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<Transform> GetPOIs()
	{
		return POIs;
	}
}
