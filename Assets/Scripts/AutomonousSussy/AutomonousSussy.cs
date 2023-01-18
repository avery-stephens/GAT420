using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomonousSussy : Agent
{
    public Perception flockPerception;
    public ObstaclePerception obstaclePerception;
    public AutonomousSussyData data;

	public float wanderAngle { get; set; } = 0;
	void Update()
    {
        var gameObjects = perception.GetGameObjects();
        foreach(var gameObject in gameObjects)
        {
            Debug.DrawLine(transform.position, gameObject.transform.position);
        }

        //seek/flee
        if (gameObjects.Length > 0) 
        {
            movement.ApplyForce(Steering.Seek(this, gameObjects[0]) * data.seekWeight);
            movement.ApplyForce(Steering.Flee(this, gameObjects[0]) * data.fleeWeight);
		}

        //flocking
        gameObjects = flockPerception.GetGameObjects();
        if (gameObjects.Length > 0)
        {
            foreach (var gameObject in gameObjects)
            {
                Debug.DrawLine(transform.position, gameObject.transform.position);
            }
            movement.ApplyForce(Steering.Cohesion(this, gameObjects) * data.cohesionWeight);
            movement.ApplyForce(Steering.Separation(this, gameObjects, data.separationRadius) * data.separationWeight);
            movement.ApplyForce(Steering.Alignment(this, gameObjects) * data.alignmentWeight);
        }

        // obstacle avoidance 
        if (obstaclePerception.IsObstacleInFront())
        {
            Vector3 direction = obstaclePerception.GetOpenDirection();
            movement.ApplyForce(Steering.CalculateSteering(this, direction) * data.obstacleWeight);
        }


        //wander
        if (movement.acceleration.sqrMagnitude <= movement.maxForce * 0.1f)
		{
			movement.ApplyForce(Steering.Wander(this));
		}

        transform.position = Utilities.Wrap(transform.position, new Vector3(-20, -20, -20), new Vector3(20, 20, 20));
    }
}   
