using System.Collections.Generic;

class QueueWayPointAgent : NodeGraphAgent
{
	protected List<Node> TargetQueue = new List<Node>();


	public QueueWayPointAgent(NodeGraph nodeGraph) : base(nodeGraph)
	{
		SetOrigin(width / 2, height / 2);

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
            if (TargetQueue.Count == 1)
                standingNode = TargetQueue[0];
			TargetQueue.RemoveAt(0);
            if (standingNode != null && TargetQueue.Count > 0) { standingNode = null; }
        }
	}

	/// <summary>
	/// Checks if the target is connected to the last in queue and if so adds it
	/// </summary>
	public virtual void AddToQueue(Node target)
	{
		// check if there is a queue
		if (TargetQueue.Count > 0)
		{
			Node lastInQueue = TargetQueue[TargetQueue.Count - 1];
			// check if the node is connected to the lastInQueue
			if (lastInQueue.IsConnectedTo(target))
			{
				TargetQueue.Add(target);
			}
			return;
		}
		// else check from the standingNode
		else if (standingNode.IsConnectedTo(target))
		{
			TargetQueue.Add(target);
		}
	}

    public override Node GoToRandomNode(int seed)
    {
		TargetQueue.Clear();
        return base.GoToRandomNode(seed);
    }
}
