using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour 
{
	public bool isDead;
	public float secondsToReactivate;

	private Animator anim;
	private HashIDs hash;
	private float deactivateTime;

	void Awake () 
	{
		anim = GetComponentInParent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		anim.SetBool(hash.enemyDyingState, false);
	}

	void Update () 
	{
		// Enemies wake up after a delay. When this has passed, 
		// set enemyDyingState in animator controller to false 
		// which will trigger a transition to the standard Locomotion blend tree
		if (isDead && (Time.time - deactivateTime) >= secondsToReactivate) 
		{
			isDead = false;
			anim.SetBool(hash.enemyDyingState, false);
		}
	}

	public void TakeDamage(float amount)
	{
		// Enemies die as soon as activating mine, regardless of the amount of damage
		isDead = true;
		anim.SetBool(hash.enemyDyingState, true);
		deactivateTime = Time.time;
	}
}
