using UnityEngine;
using System.Collections;

// RotatingToLean manages updating players rotation so that they
// end up facing away from a wall. 
// Once the orientation is less than a threshold, the Leaning state is entered
public class RotatingToLean : MoveState 
{
	public RotatingToLean(PlayerMovement playerMovement)
	{
		m_NextState = State;
		m_PlayerMovement = playerMovement;
	}
	
	public override MovementState State
	{
		get
		{
			return MoveState.MovementState.RotatingToLean;
		}
	}

	public override MoveState.MovementState HandleInput(float horizontal, float vertical)
	{
		// Default to stay in current state
		m_NextState = State;

		// Make sure player's not playing walk/run animation.
		m_PlayerMovement.anim.SetFloat(m_PlayerMovement.hash.speedFloat, 0);

		LeanDetector activeLeanDetector = m_PlayerMovement.ActiveLeanDetector;
		if (activeLeanDetector) 
		{
			Vector3 targetDirection = activeLeanDetector.transform.forward;

			bool finishedRotating = RotateToLean(m_PlayerMovement, targetDirection);
			if (finishedRotating) 
			{
				m_NextState = MovementState.Leaning;
			}
		}

		return m_NextState;
	}

	bool RotateToLean(PlayerMovement playerMovement, Vector3 targetDirection)
	{
		float toRotate = Mathf.Rad2Deg * playerMovement.RadiansToRotateToTarget(targetDirection);
		
		playerMovement.Rotating(targetDirection.x, targetDirection.z, .5f);

		Vector3 targetPosition = playerMovement.ActiveLeanDetector.transform.position;
		targetPosition.y = 0;
		playerMovement.rigidbody.MovePosition(targetPosition);
		
		bool finishedRotating = Mathf.Abs (toRotate) < playerMovement.leanStartRotateThreshold;
		return finishedRotating;
	}
}
