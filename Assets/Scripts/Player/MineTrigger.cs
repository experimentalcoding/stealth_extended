using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MineTrigger : MonoBehaviour 
{
	public int damage = 100;
	public AudioClip shotClip;

	private PlayerInventory playerInventory;   
	private HashSet<Collider> currentlyIntersectingColliders;
	private GameObject ownGameObject;	
	private ParticleSystem explosionParticles;

	void Awake () 
	{
		GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
		playerInventory = player.GetComponent<PlayerInventory> ();
		playerInventory.currentMine = this;
		currentlyIntersectingColliders = new HashSet<Collider>();
		ownGameObject = gameObject;
		explosionParticles = GetComponent<ParticleSystem>();
	}

	public void Activate()
	{
		// for any colliders that are still within the explosion distance,
		// check if they belong to an enemy robot guard before applying damage
		foreach (Collider collider in currentlyIntersectingColliders) 
		{
			// Check if the colliding object is an enemy before destroying as player not affected by mines
			GameObject collidingGameObject = collider.gameObject;
			EnemyHealth enemyHealth = collidingGameObject.GetComponent<EnemyHealth>();
			if (enemyHealth) 
			{
				enemyHealth.TakeDamage(damage);
				continue;
			}
		}

		// Play the sound effect
		AudioSource.PlayClipAtPoint(shotClip, transform.position);

		// trigger particle explosion and clear object 
		explosionParticles.Play();
		Destroy(gameObject, explosionParticles.duration);

		// hide meshes until parent game object gets destroyed
		foreach (Transform child in transform) 
		{
			child.renderer.enabled = false;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		currentlyIntersectingColliders.Add(other);
	}
	
	public void OnTriggerExit(Collider other)
	{
		currentlyIntersectingColliders.Remove(other);
	}
}
