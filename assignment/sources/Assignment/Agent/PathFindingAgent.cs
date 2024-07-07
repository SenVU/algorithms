using System.Collections.Generic;

class PathFindingAgent : QueueWayPointAgent
{
    PathFinder pathFinder = null;

    public PathFindingAgent(NodeGraph nodeGraph, PathFinder pathFinder) : base(nodeGraph)
    {
        this.pathFinder = pathFinder;
    }

    /// <summary>
    /// Generates a path from the last to the target queue
    /// </summary>
    public override void AddToQueue(Node target)
    {
        // check if there is a queue
        if (TargetQueue.Count == 0)
        {
            List<Node> generatedPath = pathFinder.GeneratePath(standingNode, target);
            if (generatedPath != null)
                TargetQueue.AddRange(generatedPath);
        } else
        {
            List<Node> generatedPath = pathFinder.GeneratePath(TargetQueue[TargetQueue.Count-1], target);
            if (generatedPath != null)
                TargetQueue.AddRange(generatedPath);
        }
    }
}
