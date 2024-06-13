using System.Collections.Generic;
using System.Drawing;

/**
 * This class represents a single node in a nodegraph.
 * Links between nodes are implemented in the Node itself (through a list of connections).
 * This means that if node A and B are connected, A will have B in its connections list and vice versa.
 * This is also called a bi-directional connection.
 * 
 * Some items are specific to this example, such as position since this node represents a node in a 
 * navigation graph. A node in a boardgame for example might represent completely different data, 
 * such as the current state of the board.
 */
class Node
{
	readonly List<Node> connections = new List<Node>();

	//node data
	public readonly Point location;

	//Every node has a id that we can display on screen for debugging
	public readonly string id;
	private static int lastID = 0;

	/**
	 * Create a node.
	 * @param pLocation the position of this node
	 * @param pLabel a label for the node, if null a unique id is assigned.
	 */
	public Node(Point location)
	{
		this.location = location;

		//use an autoincrementing id as label
		id = ""+lastID++;
		System.Console.WriteLine($"New node Created ID:{id} (X:{location.X},Y:{location.Y})");
	}

    public Node(Point location, NodeGraph nodeGraph) : this(location)
    {
		nodeGraph.AddNode(this, location.X, location.Y);
    }

    public override string ToString()
	{
		return $"Node ID:{id}";
	}

	public List<Node> GetConnections() { return connections; }

	public void AddConnection(Node node) { connections.Add(node); }

	public bool IsConnectedTo(Node node) {  return connections.Contains(node); }
}

