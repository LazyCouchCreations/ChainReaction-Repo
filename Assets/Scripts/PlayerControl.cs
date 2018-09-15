using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour {

	private NavMeshAgent agent;
	public GameObject selectionCircle;
	private bool isSelected = false;

	// Use this for initialization
	void Start () {
		selectionCircle.SetActive(false);
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		selectionCircle.SetActive(isSelected);
	}

	public void Select()
	{
		isSelected = true;
	}

	public void Deselect()
	{
		isSelected = false;
	}
}
