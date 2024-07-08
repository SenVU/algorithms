using System;
using System.Collections.Generic;
using System.Drawing;


// DIAGRM LINK https://lucid.app/lucidchart/7906d8e5-e625-4d2b-b339-ebbd89b00312/edit?viewport_loc=244%2C468%2C3246%2C1602%2C0_0&invitationId=inv_07e88e37-e89d-4cb9-bc2d-be0485c20f96
class RecursivePathFinder : PathFinder	{

	List<Node> lockedNodes = new List<Node>();
	Brush lockedNodeColor = Brushes.Orange;

    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph) {
        nodeGraph.OnNodeRightClicked += (node) => { LockOrUnlockNode(node); Draw(); };
    }

    /// <summary>
    /// Generate a path between 2 nodes using recursion
    /// </summary>
    protected override List<Node> Generate(Node from, Node to)
	{
		Console.WriteLine("Starting Path Generation");

		List<Node> toReturn = RecursiveCall(from, to, lockedNodes, new List<Node>());

        Console.WriteLine("Finished Path Generation");
		if (toReturn!=null) {
            Console.WriteLine($"Path Found With Length {toReturn.Count}");
        } else
		{
			Console.WriteLine("No Path Found");
		}

        return toReturn;
	}

	List<Node> RecursiveCall(Node currentNode, Node targetNode, List<Node> nodeBlackList, List<Node> currentPath)
	{
		currentPath.Add(currentNode);
		if (currentNode.IsConnectedTo(targetNode))
		{
			currentPath.Add(targetNode);
            return currentPath;
		}

        List<Node> shortestPath = null;

		foreach (Node nextNode in currentNode.GetConnections()) 
		{
			if (nodeBlackList.Contains(nextNode)) { continue; }
			nodeBlackList.Add(nextNode);
			
			List<Node> returnPath = RecursiveCall(nextNode, targetNode, new List<Node>(nodeBlackList), new List<Node>(currentPath));
			
			if (isShorterPath(returnPath, shortestPath))
			{
				shortestPath = returnPath;
			}
        }
		
        return shortestPath;
	}

    /// <summary>
    /// toggles the lock on a node
    /// </summary>
    void LockOrUnlockNode(Node node)
	{
		if(lockedNodes.Contains(node)) lockedNodes.Remove(node);
		else lockedNodes.Add(node);
	}

    protected override void Draw()
    {
        DrawClear();
        DrawLockedNodes();
        base.Draw(false);
    }

    void DrawLockedNodes()
    {
        foreach (Node lockedNode in lockedNodes)
        {
            DrawNode(lockedNode, lockedNodeColor);
        }
    }
}

