using GXPEngine;
using System.Collections.Generic;

class RandomPathFindWayPointAgent : NodeGraphAgent
{
	//Current target to move towards
	//Node previousTarget = null;
	Node target = null;
	Node previousTarget = null;

	Node finalTarget = null;


	public RandomPathFindWayPointAgent(NodeGraph nodeGraph) : base(nodeGraph)
	{
		SetOrigin(width / 2, height / 2);

		//listen to nodeclicks
		nodeGraph.OnNodeLeftClicked += OnNodeClickHandler;
	}

	protected virtual void OnNodeClickHandler(Node node)
	{
		if (target == null && finalTarget == null && standingNode != null)
			finalTarget = node;
	}

	protected override void Update()
	{
		PathFind();
		WalkStep();
	}


	protected void PathFind()
	{
		// checks if we are where we should be
		if (standingNode == finalTarget)
		{
			finalTarget = null;
			target = null;
			previousTarget = null;
		}

		// checks if there is a target and idle (it is idle when standing node is not null))
		if (finalTarget != null && IsIdle())
		{
			// checks if the current node is directly connected to the target
			if (standingNode.IsConnectedTo(finalTarget))
			{
				SetTarget(finalTarget);
			}
			else
			{
				FindNewTarget();
			}
		}
	}

	protected void FindNewTarget()
	{
		// if there is only one connection set that as the target
		if (standingNode.GetConnections().Count == 1)
		{
			SetTarget(standingNode.GetConnections()[0]);
		}
		// select a random target excluding the previous target
		else
		{
			List<Node> possibleTargets = new List<Node>();
			foreach (Node node in standingNode.GetConnections())
			{
				if (node != previousTarget)
				{
					possibleTargets.Add(node);
				}
			}
			SetTarget(possibleTargets[Utils.Random(0, possibleTargets.Count)]);
		}
	}

	protected void WalkStep()
	{
		if (target == null) return;

		//Move towards the target node, if we reached it, clear the target
		if (MoveTowardsNode(target))
		{
			standingNode = target;
			target = null;
		}
	}

	/// <summary>
	/// sets a target to a new target
	/// sets the previosTarget to the current target
	/// sets standingNode to null
	/// </summary>
	protected void SetTarget(Node node)
	{
		target = node;
		previousTarget = standingNode;
		standingNode = null;
	}
}
