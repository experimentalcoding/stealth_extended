using UnityEngine;
using System.Collections;

// Transition interface which different transition classes conform to.
// Each transition instance is setup with a desired translation, rotation, or both.
// Interpolate should then be called during Update or LateUpdate.
public interface Transition
{
	bool Interpolate(float deltaTime);
}