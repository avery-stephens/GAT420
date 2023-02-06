using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PatrolState : State
{
	private float timer;
	public PatrolState(StateAgent owner) : base(owner)
	{
	}

	public override void OnEnter()
	{
		timer = Random.Range(5, 10);
		owner.movement.Resume();
		owner.navigation.targetNode = owner.navigation.GetNearestNode();

	}

	public override void OnExit()
	{
		
	}

	public override void OnUpdate()
	{
		timer -= Time.deltaTime;
		if (timer < 0)
		{
			owner.stateMachine.StartState(nameof(WanderState));
		}

		if (owner.perceived.Length > 0)
		{
			owner.stateMachine.StartState(nameof(ChaseState));
		}
	}
}
