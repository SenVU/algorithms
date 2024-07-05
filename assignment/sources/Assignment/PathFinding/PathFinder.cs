using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;

/**
 * This class is the base class for your pathfinder, you 'only' have to override generate so that it returns
 * the requested path and then it will handle the visualization part for you. This class can be used in two ways:
 * 1. By setting the start and end node by left/right shift-clicking and then pressing G (for Generate)
 * 2. By calling Generate directly with the given start and end node
 * 
 * TODO:
 * - create a subclass for this class and override the generate method (See SamplePathFinder for an example)
 */
abstract class PathFinder : Canvas
{
	protected Node startNode;							
	protected Node endNode;
	protected List<Node> lastCalculatedPath = null;

	protected NodeGraph nodeGraph;

	//some values for drawing the path
	private Pen outlinePen = new Pen(Color.Black, 4);
	private Pen connectionPen1 = new Pen(Color.Black, 10);
	private Pen connectionPen2 = new Pen(Color.Yellow, 3);

	private Brush startNodeColor = Brushes.Green;
	private Brush endNodeColor = Brushes.Red;
	private Brush pathNodeColor = Brushes.Yellow;

	public PathFinder (NodeGraph pGraph) : base (pGraph.width, pGraph.height)
	{
		nodeGraph = pGraph;
		nodeGraph.OnNodeShiftLeftClicked += (node) => { startNode = node; Draw(); };
		nodeGraph.OnNodeShiftRightClicked += (node) => { endNode = node; Draw(); };

		Console.WriteLine("\n-----------------------------------------------------------------------------");
		Console.WriteLine(this.GetType().Name + " created.");
		Console.WriteLine("* Shift-LeftClick to set the starting node.");
		Console.WriteLine("* Shift-RightClick to set the target node.");
		Console.WriteLine("* G to generate the Path.");
		Console.WriteLine("* C to clear the Path.");
		Console.WriteLine("-----------------------------------------------------------------------------");
	}

	/////////////////////////////////////////////////////////////////////////////////////////
	/// Core PathFinding methods

	public List<Node> GeneratePath(Node from, Node to)
	{
		Console.WriteLine(this.GetType().Name + ".Generate: Generating path...");

		lastCalculatedPath = null;
		startNode = from;
		endNode = to;

		if (startNode == null || endNode == null)
		{
			Console.WriteLine("Please specify start and end node before trying to generate a path.");
		}
		else
		{
			lastCalculatedPath = Generate(from, to);
		}

		Draw();

		System.Console.WriteLine(this.GetType().Name + ".Generate: Path generated.");
		return lastCalculatedPath;
	}

	/**
	 * @return the last found path. 
	 *	-> 'null'		means	'Not completed.'
	 *	-> Count == 0	means	'Completed but empty (no path found).'
	 *	-> Count > 0	means	'Yolo let's go!'
	 */
	protected abstract List<Node> Generate(Node from, Node to);

	/////////////////////////////////////////////////////////////////////////////////////////
	/// PathFinder visualization helpers method
	///	As you can see this looks a lot like the code in NodeGraph, but that is just coincidence
	///	By not reusing any of that code you are free to tweak the visualization anyway you want

	protected virtual void Draw()
	{
		//to keep things simple we redraw all debug info every frame
		graphics.Clear(Color.Transparent);

		//draw path if we have one
		if (lastCalculatedPath != null) DrawPath();

		//draw start and end if we have one
		if (startNode != null) DrawNode(startNode, startNodeColor);
		if (endNode != null) DrawNode(endNode, endNodeColor);

		//TODO: you could override this method and draw your own additional stuff for debugging
	}

	protected virtual void DrawPath()
	{
		//draw all lines
		for (int i = 0; i < lastCalculatedPath.Count - 1; i++)
		{
			DrawConnection(lastCalculatedPath[i], lastCalculatedPath[i + 1]);
		}

		//draw all nodes between start and end
		for (int i = 1; i < lastCalculatedPath.Count - 1; i++)
		{
			DrawNode(lastCalculatedPath[i], pathNodeColor);
		}
	}

	protected virtual void DrawNodes (IEnumerable<Node> nodes, Brush color)
	{
		foreach (Node node in nodes) DrawNode(node, color);
	}

	protected virtual void DrawNode(Node node, Brush color)
	{
		int nodeSize = nodeGraph.GetNodeSize()+2;

		//colored fill
		graphics.FillEllipse(
			color,
			node.location.X - nodeSize,
			node.location.Y - nodeSize,
			2 * nodeSize,
			2 * nodeSize
		);

		//black outline
		graphics.DrawEllipse(
			outlinePen,
			node.location.X - nodeSize - 1,
			node.location.Y - nodeSize - 1,
			2 * nodeSize + 1,
			2 * nodeSize + 1
		);
	}

	protected virtual void DrawConnection(Node startNode, Node endNode)
	{
		//draw a thick black line with yellow core
		graphics.DrawLine(connectionPen1,	startNode.location,endNode.location);
		graphics.DrawLine(connectionPen2,	startNode.location,endNode.location);
	}

	/////////////////////////////////////////////////////////////////////////////////////////
	///							Keypress handling
	///							

	public void Update()
	{
		HandleInput();
	}

	protected virtual void HandleInput()
	{
		if (Input.GetKeyDown(Key.C))
		{
			//clear everything
			graphics.Clear(Color.Transparent);
			startNode = endNode = null;
			lastCalculatedPath = null;
		}

		if (Input.GetKeyDown(Key.G))
		{
			if (startNode != null && endNode != null)
			{
				GeneratePath(startNode, endNode);
			}
		}
	}

	public void ClearLastPath()
	{
		lastCalculatedPath = null;
	}


}
