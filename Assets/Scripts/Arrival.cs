using UnityEngine;

public class Arrival : SteeringBehaviour
{
	/// <summary>
	/// Controls how far from the target position should the agent start to slow down
	/// </summary>
	[SerializeField]
	protected float arrivalRadius = 200.0f;

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
        //Get The Target Position From The Mouse Input
        Vector3 targetPosition = Helper.GetMousePosition();

		//Get The Desired Velocity For Arrival And Limit To maxSpeed
		var distance = targetPosition - transform.position;
        desiredVelocity = Vector3.Normalize(distance) * steeringAgent.MaxCurrentSpeed;

		Debug.Log("Dist: " + distance.magnitude);

		if(distance.magnitude > targetPosition.magnitude - transform.position.magnitude)
		{
			Debug.Log("Target Pos > Dist");
            desiredVelocity = desiredVelocity - desiredVelocity;
            Debug.Log("Desired Velocity: " + desiredVelocity);
        }
		if (distance.magnitude < 100f)
		{
			//desiredVelocity = desiredVelocity - desiredVelocity;
   //         Debug.Log("Desired Velocity: " + desiredVelocity);
        }

		//Calculate Steering Velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
        return steeringVelocity;
    }

	public override void DebugDraw(SteeringAgent steeringAgent)
	{
		DebugDrawCircle("DebugCircle " + GetType().Name, Helper.GetMousePosition(), arrivalRadius, Color.magenta);
		base.DebugDraw(steeringAgent);
	}
}
