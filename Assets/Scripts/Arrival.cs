using UnityEngine;

public class Arrival : SteeringBehaviour
{
	[SerializeField] protected float arrivalRadius = 200.0f;

	bool hasArrived = false;

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
        //Get The Target Position From The Mouse Input
        Vector3 targetPosition = Helper.GetMousePosition();

		float targetDistance = Vector3.Distance(transform.position, targetPosition);

        //If The Agent Is Within The Target Area, Stop
        if (targetDistance <= 1)
        {
            hasArrived = true;

            //Set Velocity To 0
            desiredVelocity = Vector3.zero;

            //Calculate Steering Velocity
            steeringVelocity = Vector3.zero - steeringAgent.CurrentVelocity;
        }
        else if (targetDistance < arrivalRadius)
        {
            //Once Entered Arrival Radius, Gradually Slow Down
            hasArrived = false;

            //Reduce Speed Based On The Remaining Distance To The Target
            float slowDownFactor = targetDistance / arrivalRadius;
            desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * steeringAgent.MaxCurrentSpeed * slowDownFactor;

            //Calculate Steering Velocity
            steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
        }
        else if(!hasArrived)
		{
			hasArrived = false;

            //Get The Desired Velocity For Arrival And Limit To maxSpeed
            desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * steeringAgent.MaxCurrentSpeed;

            //Calculate Steering Velocity
            steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
        }

        return steeringVelocity;
    }

	public override void DebugDraw(SteeringAgent steeringAgent)
	{
		DebugDrawCircle("DebugCircle " + GetType().Name, Helper.GetMousePosition(), arrivalRadius, Color.magenta);
		base.DebugDraw(steeringAgent);
	}
}
