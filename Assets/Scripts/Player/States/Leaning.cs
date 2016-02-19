using UnityEngine;
using System.Collections;

// If player is in Leaning state, they have leaned up against the wall.
// This state checks if player has moved in opposite direction, 
// and if so the MovementState.Free state should be entered
public class Leaning : MoveState 
{
	public Leaning(PlayerMovement playerMovement)
	{
		m_NextState = State;
		m_PlayerMovement = playerMovement;
	}
	
	public override MovementState State
	{
		get
		{
			return MoveState.MovementState.Leaning;
		}
	}
	
	public override MoveState.MovementState HandleInput(float horizontal, float vertical)
	{
		// Default to stay in current state
		m_NextState = State;

		// Make sure player's not playing walk/run animation.
		m_PlayerMovement.anim.SetFloat(m_PlayerMovement.hash.speedFloat, 0);
	
		LeanDetector activeLeanDetector = m_PlayerMovement.ActiveLeanDetector;
		if (activeLeanDetector && HasDirectionalInput(horizontal, vertical)) 
		{
			Vector3 targetDirection = activeLeanDetector.transform.forward;
			Vector3 inputDir = new Vector3(horizontal, 0f, vertical);
			float rotateTo = Mathf.Rad2Deg * m_PlayerMovement.RadiansToRotateToTarget(targetDirection, inputDir);

			if (HasDirectionalInput(horizontal, vertical) && rotateTo <= m_PlayerMovement.leanEndRotateThreshold) 
			{
				m_NextState = MovementState.Free;
			}
		}

		return m_NextState;
	}
}
