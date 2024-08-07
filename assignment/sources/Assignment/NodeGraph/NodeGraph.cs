﻿using GXPEngine;
using System;
using System.Drawing;

abstract class NodeGraph : Canvas
{
	//references to all the nodes in our nodegraph
	//readonly List<Node> nodes = new List<Node>();
	Node[,] nodes = new Node[AlgorithmsAssignment.SCREEN_WIDTH, AlgorithmsAssignment.SCREEN_HEIGHT];

	//int ints = new int[2][3];

	//event handlers, register for any of these events if interested
	//see SampleNodeGraphAgent for an example of a LeftClick event handler.
	//see PathFinder for an example of a Shift-Left/Right Click event handler.
	public Action<Node> OnNodeLeftClicked = delegate { };
	public Action<Node> OnNodeRightClicked = delegate { };
	public Action<Node> OnNodeShiftLeftClicked = delegate { };
	public Action<Node> OnNodeShiftRightClicked = delegate { };

	//required for node highlighting on mouse over
	Node nodeUnderMouse = null;

	//some drawing settings
	int nodeSize;
	Pen connectionPen = new Pen(Color.Black, 2);
	Pen outlinePen = new Pen(Color.Black, 2.1f);
	Brush defaultNodeColor = Brushes.CornflowerBlue;
	Brush highlightedNodeColor = Brushes.Cyan;



	
	/// <summary>
	/// Construct a nodegraph with the given screen dimensions, eg 800x600
	/// </summary>
	
	public NodeGraph(int width, int height, int nodeSize) : base(width, height)
	{
		this.nodeSize = nodeSize;

		nodes = new Node[AlgorithmsAssignment.SCREEN_WIDTH, AlgorithmsAssignment.SCREEN_HEIGHT];

		Console.WriteLine("\n-----------------------------------------------------------------------------");
		Console.WriteLine(this.GetType().Name + " created.");
		Console.WriteLine("* (Shift) LeftClick/RightClick on nodes to trigger the corresponding events.");
		Console.WriteLine("-----------------------------------------------------------------------------");
	}

	/// <summary>
	/// Convenience method for adding a connection between two nodes in the nodegraph
	/// </summary>
	public void AddConnection(Node nodeA, Node nodeB)
	{
        Console.WriteLine($"Trying to connect ({nodeA},{nodeB})");
        if (nodeA != null && nodeB != null)
		{
			if (!nodeA.IsConnectedTo(nodeB)) nodeA.AddConnection(nodeB);
			if (!nodeB.IsConnectedTo(nodeA)) nodeB.AddConnection(nodeA);
			Console.WriteLine($"ConnectionCreated ({nodeA},{nodeB})");
		}
	}

	public void StartGeneration()
	{
		Console.WriteLine(this.GetType().Name + ".Generate: Generating graph...");

		//always remove all nodes before generating the graph, as it might have been generated previously
		//Array2D<Node>.Clear2DArray(nodes,AlgorithmsAssignment.screenWidth, AlgorithmsAssignment.screenHeight);
		nodes = new Node[AlgorithmsAssignment.SCREEN_WIDTH, AlgorithmsAssignment.SCREEN_HEIGHT];
		//nodes.Clear();
		Generate();
		Draw();

		System.Console.WriteLine(this.GetType().Name + ".Generate: Graph generated.");
	}

	protected abstract void Generate();

	/////////////////////////////////////////////////////////////////////////////////////////
	/// NodeGraph visualization helper methods
	///

	protected virtual void Draw()
	{
		graphics.Clear(Color.Transparent);
		DrawAllConnections();
		DrawNodes();
	}

	protected virtual void DrawNodes()
	{
		foreach (Node node in nodes)
		{
			DrawNode(node, defaultNodeColor);
		}
	}

	protected virtual void DrawNode(Node node, Brush color)
	{
		if (node == null) return;
		//colored node fill
		graphics.FillEllipse(
			color,
			node.location.X - nodeSize,
			node.location.Y - nodeSize,
			2 * nodeSize,
			2 * nodeSize
		);

		//black node outline
		graphics.DrawEllipse(
			outlinePen,
			node.location.X - nodeSize - 1,
			node.location.Y - nodeSize - 1,
			2 * nodeSize + 1,
			2 * nodeSize + 1
		);
	}

	protected virtual void DrawAllConnections()
	{
		//note that this means all connections are drawn twice, once from A->B and once from B->A
		//but since is only a debug view we don't care
		foreach (Node node in nodes)
		{
			DrawNodeConnections(node);
		}
	}

	protected virtual void DrawNodeConnections(Node node)
	{
		if (node == null) return;
		foreach (Node connection in node.GetConnections())
		{
			DrawConnection(node, connection);
		}
	}

	protected virtual void DrawConnection(Node pStartNode, Node pEndNode)
	{
		graphics.DrawLine(connectionPen, pStartNode.location, pEndNode.location);
	}

	/// <summary>
	/// Update loop
	/// </summary>
	protected virtual void Update()
	{
		HandleMouseInteraction();
	}

	/// <summary>
	/// Node click handling
	/// </summary>	
	protected virtual void HandleMouseInteraction()
	{
		//then check if one of the nodes is under the mouse and if so assign it to _nodeUnderMouse
		Node newNodeUnderMouse = null;
		foreach (Node node in nodes)
		{
			if (IsMouseOverNode(node))
			{
				newNodeUnderMouse = node;
				break;
			}
		}



		//do mouse node hightlighting
		if (newNodeUnderMouse != nodeUnderMouse)
		{
			if (nodeUnderMouse != null) DrawNode(nodeUnderMouse, defaultNodeColor);
			nodeUnderMouse = newNodeUnderMouse;
			if (nodeUnderMouse != null) DrawNode(nodeUnderMouse, highlightedNodeColor);
		}

		//if we are still not hovering over a node, we are done
		if (nodeUnderMouse == null) return;

		//If _nodeUnderMouse is not null, check if we released the mouse on it.
		//This is architecturally not the best way, but for this assignment 
		//it saves a lot of hassles and the trouble of building a complete event system

		if (Input.GetKey(Key.LEFT_SHIFT) || Input.GetKey(Key.RIGHT_SHIFT))
		{
			if (Input.GetMouseButtonUp(0)) OnNodeShiftLeftClicked(nodeUnderMouse);
			if (Input.GetMouseButtonUp(1)) OnNodeShiftRightClicked(nodeUnderMouse);
		}
		else
		{
			if (Input.GetMouseButtonUp(0)) OnNodeLeftClicked(nodeUnderMouse);
			if (Input.GetMouseButtonUp(1)) OnNodeRightClicked(nodeUnderMouse);
		}
	}

	/// <summary>
	/// Checks whether the mouse is over a Node.
	////This assumes local and global space are the same.
	/// </summary>
	public bool IsMouseOverNode(Node node)
	{
		if (node == null) return false;
		//ah life would be so much easier if we'd all just use Vec2's ;)
		float dx = node.location.X - Input.mouseX;
		float dy = node.location.Y - Input.mouseY;
		float mouseToNodeDistance = Mathf.Sqrt(dx * dx + dy * dy);

		return mouseToNodeDistance < nodeSize;
	}

	/// <summary>
	/// Checks what node is at (x,y)
	/// </summary>
	public Node GetNodeAt(int x, int y)
	{
		return nodes[x, y];
	}

	/// <summary>
	/// Checks what node is at a point
	/// </summary>
	public Node GetNodeAt(Point point)
	{
		return GetNodeAt(point.X, point.Y);
	}

	/// <summary>
	/// Adds a node to the graphs list
	/// </summary>
	public void AddNode(Node node, int x, int y) { nodes[x, y] = node; }

    /// <summary>
    /// Reruns the graphs node array
    /// </summary>
    /// <returns>
	/// A 2dArray nodegraph
	/// </returns>
    public Node[,] GetNodes() { return nodes; }

	public int GetNodeSize() { return nodeSize; }

	public Node TryPlaceNode(Point location)
	{
		Node  node = GetNodeAt(location) ??new Node(location, this);
		return node;
	}
}