using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

class IterativePathFinder : PathFinder
{

	List<Node> lockedNodes = new List<Node>();
	Brush lockedNodeColor = Brushes.Orange;
	private Pen avaliblePathPen = new Pen(Color.Green, 3);

	Node parent = null;
	List<PathIteration> iterations = null;


	public IterativePathFinder(NodeGraph pGraph) : base(pGraph)
	{
		nodeGraph.OnNodeRightClicked += (node) => { LockOrUnlockNode(node); SetParentToStanding(); GenerateIterations(parent, lockedNodes); Draw(); };
	}

	/// <summary>
	/// Generate a path between 2 nodes using itteration
	/// </summary>
	protected override List<Node> Generate(Node from, Node to)
	{
		Console.WriteLine("Starting Path Generation");

		GenerateIterations(from, lockedNodes);

		List<Node> toReturn = GetPath(to, iterations);


		Console.WriteLine("Finished Path Generation");
		if (toReturn != null)
		{
			Console.WriteLine($"Path Found With Length {toReturn.Count}");
		}
		else
		{
			Console.WriteLine("No Path Found");
		}

		return toReturn;
	}

	/// <summary>
	/// Renerates a map of paths steming from the parent
	/// </summary>
	void GenerateIterations(Node parent, List<Node> nodeBlackList)
	{
		this.parent = parent;
		List<PathIteration> DoneList = new List<PathIteration>();
		List<PathIteration> TodoList = new List<PathIteration>();

		List<Node> startingPath = new List<Node> { parent };
		TodoList.Add(new PathIteration(parent, startingPath));

		List<Node> path = null;
		while (TodoList.Count > 0)
		{
			PathIteration currentIteration = GetShortestIteration(TodoList);
			List<Node> connections = currentIteration.GetNextNodes();
			foreach (Node node in connections)
			{
				if (nodeBlackList.Contains(node)) continue;
				if (!PathExists(node, TodoList) && !PathExists(node, DoneList))
				{
					List<Node> newPath = currentIteration.GetPath();
					newPath.Add(node);
					TodoList.Add(new PathIteration(node, newPath));
				}
			}
			TodoList.Remove(currentIteration);
			DoneList.Add(currentIteration);
		}
		iterations = DoneList;
	}

	/// <summary>
	/// find if there is a path to a given node from the parent
	/// </summary>
	bool PathExists(Node node, List<PathIteration> iterations)
	{
		foreach (PathIteration iteration in iterations)
		{
			if (iteration.IsNode(node)) return true;
		}
		return false;
	}

	/// <summary>
	/// finds the curent shortest path in a list of itterations
	/// </summary>
	PathIteration GetShortestIteration(List<PathIteration> iterations)
	{
		PathIteration shortest = null;
		foreach (PathIteration iteration in iterations)
		{
			if (shortest == null || isShorterPath(iteration.GetPath(), shortest.GetPath()))
			{
				shortest = iteration;
			}
		}
		return shortest;
	}

	/// <summary>
	/// returns the path to a given node, if it does not exist return null
	/// </summary>
	List<Node> GetPath(Node target, List<PathIteration> iterations)
	{
		foreach (PathIteration iteration in iterations)
		{
			if (iteration.IsNode(target)) return iteration.GetPath();
		}
		return null;
	}

	/// <summary>
	/// toggles the lock on a node
	/// </summary>
	void LockOrUnlockNode(Node node)
	{
		if (lockedNodes.Contains(node)) lockedNodes.Remove(node);
		else lockedNodes.Add(node);
	}

	protected override void Draw()
	{
		DrawClear();
		DrawLockedNodes();
		DrawAvaliblePaths();
		base.Draw(false);
	}

	void DrawLockedNodes()
	{
		foreach (Node lockedNode in lockedNodes)
		{
			DrawNode(lockedNode, lockedNodeColor);
		}
	}

	void DrawAvaliblePaths()
	{
		if (iterations != null)
			foreach (PathIteration iteration in iterations)
			{
				List<Node> path = iteration.GetPath();
				for (int i = 0; i < path.Count - 1; i++)
				{
					graphics.DrawLine(avaliblePathPen, path[i].location, path[i + 1].location);
				}
			}
	}

	void SetParentToStanding()
	{
		NodeGraphAgent agent = AlgorithmsAssignment.instance.GetAgent();
		if (agent == null) { return; }
		if (agent.IsIdle()) parent = agent.GetStandingNode();
	}
}

