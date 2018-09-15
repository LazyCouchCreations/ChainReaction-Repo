using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArtificialIntelligence : MonoBehaviour {

	private AINavigation aiNav;
	private List<Transform> POIs;
	private NavMeshAgent agent;
	public float maxDestinationCD;
	public float minDestinationCD;
	private float destinationCDRemaning;
	private float destinationCD;

	// Use this for initialization
	void Start () {
		POIs = GameObject.FindGameObjectWithTag("GameController").GetComponent<AINavigation>().GetPOIs();
		agent = GetComponent<NavMeshAgent>();
		SetNewDestination();		
	}
	
	// Update is called once per frame
	void Update () {
		if (destinationCDRemaning >= 0)
		{
			destinationCDRemaning -= Time.deltaTime;
		}
		else
		{
			SetNewDestination();
		}
	}

	void SetNewDestination()
	{
		destinationCD = Random.Range(minDestinationCD, maxDestinationCD);
		destinationCDRemaning = destinationCD;
		agent.SetDestination(POIs[Random.Range(0, POIs.Count-1)].position);
	}	
}
