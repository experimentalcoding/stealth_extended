using UnityEngine;
using System.Collections;

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
		
		// Also move in closer against wall, todo: tidy
		// todo: working well but put into own function
//		Vector3 target = Vector3.Lerp(m_PositionBeforeLean, ActiveLeanDetector.transform.position, Time.deltaTime * 5.5f);

		Vector3 targetPosition = playerMovement.ActiveLeanDetector.transform.position;
		targetPosition.y = 0;
		playerMovement.rigidbody.MovePosition(targetPosition); // todo: cache any properties
		
		bool finishedRotating = Mathf.Abs (toRotate) < playerMovement.leanStartRotateThreshold;
		return finishedRotating;
	}
}
