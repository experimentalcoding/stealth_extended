using UnityEngine;
using System.Collections;

// The Rotator clss simply rotates the mine prefab around the Y axis 
// to draw attention to it.
public class Rotator : MonoBehaviour 
{
	public float speed = 10f;

	// Keep rotating around Y axis
	void Update ()
	{
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
