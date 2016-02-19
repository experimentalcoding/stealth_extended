using UnityEngine;
using System.Collections;

// Abstract base class for a movement state
public abstract class MoveState
{
	public enum MovementState { Free, PushingAgainstWall, RotatingToLean, Leaning };
	
	abstract public MovementState State { get; }
	
	public abstract MovementState HandleInput(float horizontal, float vertical);
	
	protected bool HasDirectionalInput(float horizontal, float vertical)
	{
		return horizontal != 0f || vertical != 0f;
	}
	
	protected MoveState.MovementState m_NextState;
	protected PlayerMovement m_PlayerMovement;
}