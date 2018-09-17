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
	public Transform headLight;
	public float myVelocity;
	private Animator anim;
	private AudioSource audio;
	//shotgun
	public int charType; //shotgun = 0
	public AudioClip shotgunAttackClip;

	private GameObject target;
	private bool isAttacking;
	private bool isGettingInfected;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		POIs = GameObject.FindGameObjectWithTag("GameController").GetComponent<AINavigation>().GetPOIs();
		agent = GetComponent<NavMeshAgent>();
		speed = agent.speed;
		angularSpeed = agent.angularSpeed;
		SetNewDestination();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		//if (isGettingInfected)
		//{
		//	anim.SetTrigger("infecting");
		//}

		myVelocity = agent.velocity.magnitude;
		anim.SetFloat("velocity", myVelocity);

		if (tag == "Player")
		{
			Destroy(this);
			agent.ResetPath();
		}

		players = gameManager.players;
		foreach (GameObject player in players)
		{
			RaycastHit hit;
			//cast a ray from myself to each bad guy
			if (Physics.Linecast(headLight.position, player.transform.position, out hit))
			{
				if (hit.collider.tag == "Player")
				{
					viewAngle = Vector3.Angle(headLight.forward, hit.transform.position - headLight.position);
					distance = Vector3.Distance(headLight.position, hit.transform.position);
					if (viewAngle <= detectionAngle && distance <= detectionDistance)
					{
						Debug.DrawLine(headLight.position, player.transform.position, Color.red);
						if (hit.collider.gameObject.GetComponent<PlayerControl>().isInfecting)
						{
							if (!isAttacking && !isGettingInfected)
							{
								Attack();
								target = hit.collider.gameObject;
							}							
						}
					}
					else
					{
						Debug.DrawLine(headLight.position, player.transform.position, Color.cyan);
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

	private void Attack()
	{
		isAttacking = true;
		agent.isStopped = true;

		switch (charType)
		{
			case 0:
				anim.SetTrigger("shoot");
				audio.PlayOneShot(shotgunAttackClip);
				break;
			default:
				break;
		}
	}

	private void Kill()
	{
		target.GetComponent<PlayerControl>().Die();
		isAttacking = false;
		agent.isStopped = false;
	}

	//public void ToggleInfection()
	//{
	//	isGettingInfected = !isGettingInfected;
	//}
}
