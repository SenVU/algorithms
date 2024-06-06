using GXPEngine;
using System.Collections.Generic;

/**
 * Very simple example of a nodegraphagent that walks directly to the node you clicked on,
 * ignoring walls, connections etc.
 */
class OnGraphWayPointAgent : NodeGraphAgent
{
	//Current target to move towards
	//Node previousTarget = null;
	List<Node> TargetQueue = new List<Node>();


    public OnGraphWayPointAgent(NodeGraph nodeGraph) : base(nodeGraph)
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
		AddToQueue(node);
	}

	protected override void Update()
	{
		WalkStep();
	}

	protected void WalkStep()
	{
		if (TargetQueue.Count==0) return;

		//Move towards the target node, if we reached it, clear the target
		if (moveTowardsNode(TargetQueue[0]))
		{
			standingNode = TargetQueue[0];
            TargetQueue.RemoveAt(0);
		}
	}

    protected void AddToQueue(Node node)
	{
		if (TargetQueue.Count > 0)
		{
			Node lastInQueue = TargetQueue[TargetQueue.Count - 1];
			if (lastInQueue.IsConnectedTo(node))
			{
				TargetQueue.Add(node);
			}
			return;
		}
        if (standingNode.IsConnectedTo(node))
        {
            TargetQueue.Add(node);
        }
    }
}
