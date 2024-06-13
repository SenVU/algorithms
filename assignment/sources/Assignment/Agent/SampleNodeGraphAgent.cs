using GXPEngine;

/**
 * Very simple example of a nodegraphagent that walks directly to the node you clicked on,
 * ignoring walls, connections etc.
 */
class SampleNodeGraphAgent : NodeGraphAgent
{
	//Current target to move towards
	private Node _target = null;

	public SampleNodeGraphAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
	{
		SetOrigin(width / 2, height / 2);

		GotoRandomNode();

		//listen to nodeclicks
		pNodeGraph.OnNodeLeftClicked += OnNodeClickHandler;
	}

    protected virtual void OnNodeClickHandler(Node pNode)
	{
		_target = pNode;
	}

	protected override void Update()
	{
		//no target? Don't walk
		if (_target == null) return;

		//Move towards the target node, if we reached it, clear the target
		if (MoveTowardsNode(_target))
		{
			_target = null;
		}
	}
}
