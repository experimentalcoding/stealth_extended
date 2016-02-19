using UnityEngine;
using System.Collections;

// This state isn't currently used, since if player taps movement keys at all
// in correct direction while colliding with LeanDetector, RotatingToLean state is entered.
// If a delay is needed before responding to this input, 
// PushingAgainstWall can be used to handle that
public class PushingAgainstWall : MoveState 
{
	public PushingAgainstWall(PlayerMovement playerMovement)
	{
		m_NextState = State;
		m_PlayerMovement = playerMovement;
	}
	
	public override MovementState State
	{
		get
		{
			return MoveState.MovementState.PushingAgainstWall;
		}
	}
	
	public override MoveState.MovementState HandleInput(float horizontal, float vertical)
	{
		return State;
	}
}