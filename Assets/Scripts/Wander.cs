using System.Buffers.Text;
using System.Net.NetworkInformation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Wander : SteeringBehaviour
{
	/// <summary>
	/// Controls how large the imaginary circle is
	/// NOTE: [SerializeField] exposes a C# variable to Unity's inspector without making it public. Useful for encapsulating code
	/// while still giving access to the Unity inspector
	/// </summary>
	[SerializeField] protected float circleRadius = 150.0f;

	/// <summary>
	/// Controls how far from the agent position should the centre of the circle be
	/// NOTE: [SerializeField] exposes a C# variable to Unity's inspector without making it public. Useful for encapsulating code
	/// while still giving access to the Unity inspector
	/// </summary>
	[SerializeField] protected float circleDistance = 250.0f;

    //Affects How Fast The Path Changes
    [SerializeField] float noiseScale = 0.5f;

    //Gives Random Noise Pattern
    float timeOffsetX;
    float timeOffsetY;

    protected override void Start()
	{
		base.Start();

        //Create Random Perlin Noise Offsets To Create A Unique Wandering Pattern
        timeOffsetX = Random.Range(0f, 100f);
		timeOffsetY = Random.Range(0f, 100f);
	}

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
        //Generate Perlin Noise Values For X And Y Directions Based On Time
        //This Generates A Perlin Noise Value For Each Axis Based On:
        //a) The Elapsed Time
        //b) Scaled By noiseScale
        //c) Offset By timeOffsetX
        float noiseX = Mathf.PerlinNoise(Time.time * noiseScale + timeOffsetX, 0f);
        float noiseY = Mathf.PerlinNoise(Time.time * noiseScale + timeOffsetY, 0f);

        //Map Perlin Noise For Smooth Movement Along Both Axis
        //Instead Of Being In The Range[0, 1] It Now Falls Within The Range[-0.5, 0.5]
        //This Makes The Values More Balanced Between + And - Directions
        noiseX = (noiseX - 0.5f) * 2f;
        noiseY = (noiseY - 0.5f) * 2f;

        //Calculate The Desired Direction Based On The Perlin Noise
        Vector3 desiredDirection = new Vector3(noiseX, noiseY, 0).normalized;

        //Get The Desired Velocity For Wander And Limit To maxSpeed
        Vector3 desiredVelocity = desiredDirection * steeringAgent.MaxCurrentSpeed;

        //Calculate Steering Velocity
        steeringVelocity = desiredVelocity;

        return steeringVelocity;
	}

	public override void DebugDraw(SteeringAgent steeringAgent)
	{
        //Display Circle Infront Of The Current Direction Of Movement
        Vector3 circlePosition = transform.position + (steeringAgent.CurrentVelocity.normalized * circleDistance);
        DebugDrawCircle("DebugCircle " + GetType().Name, circlePosition, circleRadius, Color.magenta);

        base.DebugDraw(steeringAgent);
	}
}
