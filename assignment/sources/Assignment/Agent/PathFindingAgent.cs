using System.Collections.Generic;

class PathFindingAgent : QueueWayPointAgent
{
    PathFinder pathFinder = null;

    public PathFindingAgent(NodeGraph nodeGraph, PathFinder pathFinder) : base(nodeGraph)
    {
        this.pathFinder = pathFinder;
    }

    public override void AddToQueue(Node target)
    {
        // check if there is a queue
        if (TargetQueue.Count == 0)
        {
            List<Node> generatedPath = GeneratePath(target);
            if (generatedPath != null)
                TargetQueue.AddRange(generatedPath);
        }
    }

    protected List<Node> GeneratePath(Node target) {
        return pathFinder.GeneratePath(standingNode, target);
    }
}
