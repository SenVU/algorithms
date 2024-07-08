using System;
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


    /// <summary>
    /// Create a node
    /// </summary>
    /// <param name="location">location the position of this node</param>
    public Node(Point location)
	{
		this.location = location;

		//use an autoincrementing id as label
		id = "" + lastID++;
		System.Console.WriteLine($"New node Created ID:{id} (X:{location.X},Y:{location.Y})");
	}



    /// <summary>
    /// Create a node and add it to a nodegraph
    /// </summary>
    /// <param name="location">location the position of this node</param>
    /// <param name="nodeGraph">nodeGraph the graph this node is a part of</param></param>
	public Node(Point location, NodeGraph nodeGraph) : this(location)
	{
		nodeGraph.AddNode(this, location.X, location.Y);
	}

	public override string ToString()
	{
		return $"Node ID:{id}";
	}

	/// <summary>
	/// returns a copy of the list with all connections that go from this node
	/// </summary>
	public List<Node> GetConnections() { return new List<Node>(connections); }


	/// <summary>
	/// create a connection from this node to another one
	/// NOTE: when using this the connection is one way
	/// </summary>
	public void AddConnection(Node node) { connections.Add(node); }

	/// <summary>
	/// check if there is a connection between this node and another one
	/// </summary>
	public bool IsConnectedTo(Node node) { return connections.Contains(node); }

	/// <summary>
	/// returns the distance from this node to another one
	/// </summary>
	public float GetDistanceTo(Node otherNode) { return (float)Math.Sqrt((otherNode.location.X - location.X) * (otherNode.location.X - location.X) + (otherNode.location.Y - location.Y) * (otherNode.location.Y - location.Y)); }

	/// <summary>
	/// resets the static id counter to 0
	/// </summary>
	public static void ResetIDCounter() {lastID = 0;}
}

