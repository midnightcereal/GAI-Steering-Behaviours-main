using UnityEngine;

public class Seek : SteeringBehaviour
{
    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        //Get The Target Position From The Mouse Input
        Vector3 targetPosition = Helper.GetMousePosition();

        //Get The Desired Velocity For Seek And Limit To maxSpeed
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * steeringAgent.MaxCurrentSpeed;

        //Calculate Steering Velocity
        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

        return steeringVelocity;
    }
}
