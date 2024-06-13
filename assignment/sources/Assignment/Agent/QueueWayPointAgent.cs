using System.Collections.Generic;

class QueueWayPointAgent : NodeGraphAgent
{
	//Current target to move towards
	//Node previousTarget = null;
	List<Node> TargetQueue = new List<Node>();


	public QueueWayPointAgent(NodeGraph nodeGraph) : base(nodeGraph)
	{
		SetOrigin(width / 2, height / 2);

		//position ourselves on a random node
		GotoRandomNode();

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
		// if the queue is empty return
		if (TargetQueue.Count == 0) return;

		//Move towards the first node in the queue, if we reached it, clear it
		if (MoveTowardsNode(TargetQueue[0]))
		{
			standingNode = TargetQueue[0];
			TargetQueue.RemoveAt(0);

			if (TargetQueue.Count>0) { standingNode = null; }
		}
	}

	public void AddToQueue(Node node)
	{
		// check if there is a queue
		if (TargetQueue.Count > 0)
		{
			Node lastInQueue = TargetQueue[TargetQueue.Count - 1];
			// check if the node is connected to the lastInQueue
			if (lastInQueue.IsConnectedTo(node))
			{
				TargetQueue.Add(node);
			}
			return;
		}
		// else check from the standingNode
		else if (standingNode.IsConnectedTo(node))
		{
			TargetQueue.Add(node);
		}
	}
}
