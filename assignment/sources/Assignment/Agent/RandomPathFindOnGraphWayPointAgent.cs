using GXPEngine;
using System.Collections.Generic;
using System.Diagnostics;

/**
 * Very simple example of a nodegraphagent that walks directly to the node you clicked on,
 * ignoring walls, connections etc.
 */
class RandomPathFindOnGraphWayPointAgent : NodeGraphAgent
{
	//Current target to move towards
	//Node previousTarget = null;
	Node target = null;
	Node previosTarget = null;

	Node finalTarget = null;


	public RandomPathFindOnGraphWayPointAgent(NodeGraph nodeGraph) : base(nodeGraph)
	{
		SetOrigin(width / 2, height / 2);
        //position ourselves on a random node
        if (nodeGraph.GetNodes().Count > 0)
		{
			GotoRandomNode();
		}

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
		if (standingNode == finalTarget)
		{
			finalTarget = null;
			target = null;
			previosTarget = null;
		}
		if (finalTarget != null && standingNode != null && target==null)
		{
			if (standingNode.IsConnectedTo(finalTarget))
			{
				SetTarget(finalTarget);
			}
			else
			{
				//Debug.Assert(standingNode.GetConnections().Count == 0, "current node has no connections");

				if (standingNode.GetConnections().Count == 1)
				{
					SetTarget(standingNode.GetConnections()[0]);
				}
				else
				{
					List<Node> possibleTargets = new List<Node>();
					foreach (Node node in standingNode.GetConnections())
					{
						if (node != previosTarget)
						{
							possibleTargets.Add(node);
						}
					}
					SetTarget(possibleTargets[Utils.Random(0, possibleTargets.Count)]);
				}
			}
		}
	}

	protected void WalkStep()
	{
		if (target == null) return;

		//Move towards the target node, if we reached it, clear the target
		if (moveTowardsNode(target))
		{
			standingNode = target;
			target = null;
		}
	}

	protected void SetTarget(Node node)
	{
        target = node;
        previosTarget = standingNode;
        standingNode = null;
    }
}
