using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
	
	public Transform _player;
	private NavMeshAgent _nma;
	
    void Start()
    {
        _nma = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _nma.destination = _player.position;
    }
	
	private void OnCollisionEnter(Collision collision)
	{
		//Player p = collision.transform.GetComponent<Player>();
		
		//if(p)
		//	p.Damage(-collision.GetContact(0).normal);
	}
}
