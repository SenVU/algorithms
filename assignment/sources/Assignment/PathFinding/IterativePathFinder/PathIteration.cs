using System.Collections.Generic;

class PathIteration
{
    List<Node> path;
    Node node;

    public PathIteration(Node node, List<Node> path)
    {
        this.node = node;
        this.path = path;
    }

    /// <summary>
    /// returns all the nodes this path could continue to
    /// </summary>
    public List<Node> GetNextNodes() {
        List<Node> toReturn = node.GetConnections(); 
            if (toReturn.Contains(path[path.Count-1])) toReturn.Remove(path[path.Count-1]);
        return toReturn;
    }

    /// <summary>
    /// returns a list of nodes representing the path
    /// </summary>
    public List<Node> GetPath() { return new List<Node>(path); }

    /// <summary>
    /// does this itteration end at node
    /// </summary>
    public bool IsNode(Node node) { return this.node == node; }
}
