using GXPEngine;
using System.Diagnostics;
using System.Drawing.Printing;

/**
 * NodeGraphAgent provides a starting point for your own agents that would like to navigate the nodegraph.
 * It provides convenience methods such as moveTowardsNode & jumpToNode.
 * 
 * Create a subclass of this class, override Update and call these methods as required for your specific assignment.
 * See SampleNodeGraphAgent for an example.
 */
abstract class NodeGraphAgent : AnimationSprite
{
	public enum AgentType {
		RANDOM,
		QEUEU,
		PATHFIND,
	}

	NodeGraph nodeGraph = null;
	protected Node standingNode = null;

	public NodeGraphAgent(NodeGraph nodeGraph) : base("assets/orc.png", 4, 2, 7)
	{
		this.nodeGraph = nodeGraph;
		Debug.Assert(nodeGraph != null, "Please pass in a node graph.");

		SetOrigin(width / 2, height / 2);
		System.Console.WriteLine(this.GetType().Name + " created.");
	}

	//override in subclass to implement any functionality
	protected abstract void Update();


	/// <summary>
	/// moves towards the given node
	/// returns true if the node is reached
	/// </summary>
	protected virtual bool MoveTowardsNode(Node target)
	{
		float speed = Input.GetKey(AlgorithmsAssignment.AGENT_RUN_KEY) ? AlgorithmsAssignment.AGENT_RUN_SPEED : AlgorithmsAssignment.AGENT_SPEED;
		//increase our current frame based on time passed and current speed
		SetFrame((int)(speed * (Time.time / 100)) % frameCount);

		//standard vector math as you had during the Physics course
		Vec2 targetPosition = new Vec2(target.location.X, target.location.Y);
		Vec2 currentPosition = new Vec2(x, y);
		Vec2 delta = targetPosition.Sub(currentPosition);

		if (delta.Length() < speed)
		{
			JumpToNode(target);
			return true;
		}
		else
		{
			Vec2 velocity = delta.Normalize().Scale(speed);
			x += velocity.x;
			y += velocity.y;

			scaleX = (velocity.x >= 0) ? 1 : -1;

			return false;
		}
	}

	/// <summary>
	/// Jumps towards the given node immediately
	/// </summary>
	protected virtual void JumpToNode(Node node)
	{
		x = node.location.X;
		y = node.location.Y;
	}


	/// <summary>
	/// selects a random node and jumps to it immediately
	/// </summary>
	public virtual Node GotoRandomNode(int seed)
	{
		if (nodeGraph.GetNodes().Length > 0)
		{
			//Node randomNode = nodeGraph.GetNodes()[Utils.Random(0, nodeGraph.GetNodes().Count)];
			Node randomNode = Array2D<Node>.GetRandomForm2DArray(nodeGraph.GetNodes(), seed);
			JumpToNode(randomNode);
			standingNode = randomNode;
			return randomNode;
		}
		return null;
	}

	public bool IsIdle()
	{
		return standingNode != null;
	}

	public Node GetStandingNode() {  return standingNode; }
}

