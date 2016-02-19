using UnityEngine;
using System.Collections;

// Transition interface which different transition classes conform to.
// Each transition instance is setup with a desired translation, rotation, or both.
// Interpolate should then be called during Update or LateUpdate.
public interface Transition
{
	bool Interpolate(float deltaTime);
}

// Create with a source and target translation
public class TranslateTransition : Transition
{
	public TranslateTransition()
	{
		m_PositionFrom = new Vector3();
		m_PositionTo = new Vector3();
	}
	
	public TranslateTransition(Vector3 positionFrom,
	                           Vector3 positionTo,
	                           float duration)
	{
		m_PositionFrom = positionFrom;
		m_PositionTo = positionTo;
	}
	
	public void Set(Vector3 positionFrom,
	                Vector3 positionTo,
	                float duration)
	{
		m_TransitionDuration = duration;
		m_LerpT = 0f;
		
		m_PositionFrom = positionFrom;
		m_PositionTo = positionTo;
	}
	
	public bool Interpolate(float deltaTime)
	{
		m_Position = Vector3.Lerp(m_PositionFrom, m_PositionTo, m_LerpT);
		
		// increase lerp amount and return whether still lerping
		m_LerpT += deltaTime / m_TransitionDuration;
		return m_LerpT < 1f;
	}
	
	public Vector3 Position
	{
		get { return m_Position; }
	}
	
	private Vector3 m_Position;
	private Vector3 m_PositionFrom;
	private Vector3 m_PositionTo;
	
	private float m_TransitionDuration;
	private float m_LerpT;
}

// Create with a source and target rotation
public class RotateTransition : Transition
{
	public RotateTransition()
	{
		m_RotationFrom = new Quaternion();
		m_RotationTo = new Quaternion();
	}
	
	public RotateTransition(Quaternion rotationFrom,
	                        Quaternion rotationTo,
	                        float duration)
	{
		m_RotationFrom = rotationFrom;
		m_RotationTo = rotationTo;
	}
	
	public void Set(Quaternion rotationFrom,
	                Quaternion rotationTo,
	                float duration)
	{
		m_TransitionDuration = duration;
		m_LerpT = 0f;
		m_RotationFrom = rotationFrom;
		m_RotationTo = rotationTo;
	}
	
	public bool Interpolate(float deltaTime)
	{
		m_Rotation = Quaternion.Slerp(m_RotationFrom, m_RotationTo, m_LerpT);
		m_LerpT += deltaTime / m_TransitionDuration;
		
		return m_LerpT < 1f;
	}
	
	public Quaternion Rotation
	{
		get { return m_Rotation; }
	}
	
	private Quaternion m_Rotation;
	private Quaternion m_RotationFrom;
	private Quaternion m_RotationTo;
	
	private float m_TransitionDuration;
	private float m_LerpT;
}

// Create with a source and target translation
public class TranslateRotateTransition : Transition
{
	public TranslateRotateTransition()
	{
		m_TranslateTransition = new TranslateTransition();
		m_RotateTransition = new RotateTransition();
	}
	
	public TranslateRotateTransition(TranslateTransition translateTransition,
	                                 RotateTransition rotateTransition)
	{
		m_TranslateTransition = translateTransition;
		m_RotateTransition = rotateTransition;
	}
	
	public void Set(TranslateTransition translateTransition,
	                RotateTransition rotateTransition)
	{
		m_TranslateTransition = translateTransition;
		m_RotateTransition = rotateTransition;
	}
	
	public void Set(Vector3 positionFrom,
	                Quaternion rotationFrom,
	                Vector3 positionTo,
	                Quaternion rotationTo,
	                float duration)
	{
		m_TranslateTransition.Set(positionFrom, positionTo, duration);
		m_RotateTransition.Set(rotationFrom, rotationTo, duration);
	}
	
	
	public bool Interpolate(float deltaTime)
	{
		bool stillLerping = m_TranslateTransition.Interpolate(deltaTime);
		stillLerping = m_RotateTransition.Interpolate(deltaTime) || stillLerping;
		
		return stillLerping;
	}
	
	public TranslateTransition Translate
	{
		get { return m_TranslateTransition; }
	}
	
	public RotateTransition Rotate
	{
		get { return m_RotateTransition; }
	}
	
	private TranslateTransition m_TranslateTransition;
	private RotateTransition m_RotateTransition;
}
