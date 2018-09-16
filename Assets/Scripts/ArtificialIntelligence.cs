using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArtificialIntelligence : MonoBehaviour {

	private AINavigation aiNav;
	private List<Transform> POIs;
	private NavMeshAgent agent;
	private float speed;
	private float angularSpeed;
	public float maxDestinationCD;
	public float minDestinationCD;
	private float destinationCDRemaning;
	private float destinationCD;
	private GameManager gameManager;
	private List<GameObject> players;
	public float detectionAngle;
	public float viewAngle;
	public float detectionDistance;
	public float distance;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		POIs = GameObject.FindGameObjectWithTag("GameController").GetComponent<AINavigation>().GetPOIs();
		agent = GetComponent<NavMeshAgent>();
		speed = agent.speed;
		angularSpeed = agent.angularSpeed;
		SetNewDestination();		
	}
	
	// Update is called once per frame
	void Update () {
		

		if (tag == "Player")
		{
			Destroy(this);
			agent.ResetPath();
		}

		players = gameManager.players;
		foreach (GameObject player in players)
		{
			RaycastHit hit;
			if (Physics.Linecast(transform.position, player.transform.position, out hit))
			{
				if (hit.collider.tag == "Player")
				{
					viewAngle = Vector3.Angle(transform.forward, hit.transform.position - transform.position);
					distance = Vector3.Distance(transform.position, hit.transform.position);
					if (viewAngle <= detectionAngle && distance <= detectionDistance)
					{
						Debug.DrawLine(transform.position, player.transform.position, Color.red);
						if (hit.collider.gameObject.GetComponent<PlayerControl>().isInfecting)
						{
							//attack
							hit.collider.gameObject.GetComponent<PlayerControl>().Die();
						}
					}
					else
					{
						Debug.DrawLine(transform.position, player.transform.position, Color.cyan);
					}
					
				}
			}
		}


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

	public void Stun()
	{
		agent.speed = 0;
		agent.angularSpeed = 0;
		//play stun animation
	}

	public void Unstun()
	{
		agent.speed = speed;
		agent.angularSpeed = angularSpeed;
		//resume normal animation
	}
}
