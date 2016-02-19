using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{
	public float speed = 10f;

	// Keep rotating around Y axis
	void Update ()
	{
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
