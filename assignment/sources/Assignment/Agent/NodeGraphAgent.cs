using GXPEngine;
using System.Diagnostics;

/**
 * NodeGraphAgent provides a starting point for your own agents that would like to navigate the nodegraph.
 * It provides convenience methods such as moveTowardsNode & jumpToNode.
 * 
 * Create a subclass of this class, override Update and call these methods as required for your specific assignment.
 * See SampleNodeGraphAgent for an example.
 */
abstract class NodeGraphAgent : AnimationSprite
{
	NodeGraph nodeGraph = null;
    protected Node standingNode = null;

    protected const int regularSpeed = 1;
	protected const int fastSpeed = 10;
	protected const int speedUpKey = Key.LEFT_CTRL;

	public NodeGraphAgent(NodeGraph nodeGraph) : base("assets/orc.png", 4, 2, 7)
	{
		this.nodeGraph = nodeGraph;
		Debug.Assert(nodeGraph != null, "Please pass in a node graph.");

		SetOrigin(width / 2, height / 2);
		System.Console.WriteLine(this.GetType().Name + " created.");
	}

	//override in subclass to implement any functionality
	protected abstract void Update();

	/////////////////////////////////////////////////////////////////////////////////////////
	///	Movement helper methods

	/**
	 * Moves towards the given node with either REGULAR_SPEED or FAST_TRAVEL_SPEED 
	 * based on whether the RIGHT_CTRL key is pressed.
	 */
	protected virtual bool moveTowardsNode(Node target)
	{
		float speed = Input.GetKey(speedUpKey) ? fastSpeed : regularSpeed;
		//increase our current frame based on time passed and current speed
		SetFrame((int)(speed * (Time.time / 100)) % frameCount);

		//standard vector math as you had during the Physics course
		Vec2 targetPosition = new Vec2(target.location.X, target.location.Y);
		Vec2 currentPosition = new Vec2(x, y);
		Vec2 delta = targetPosition.Sub(currentPosition);

		if (delta.Length() < speed)
		{
			jumpToNode(target);
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

	/**
	 * Jumps towards the given node immediately
	 */
	protected virtual void jumpToNode(Node node)
	{
		x = node.location.X;
		y = node.location.Y;
	}

	public Node GotoRandomNode()
	{
        Node randomNode = nodeGraph.GetNodes()[Utils.Random(0, nodeGraph.GetNodes().Count)];
        jumpToNode(randomNode);
		standingNode = randomNode;
		return randomNode;
    }

}

