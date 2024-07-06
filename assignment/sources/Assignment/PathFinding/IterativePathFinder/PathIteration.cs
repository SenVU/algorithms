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

    public List<Node> GetNextNodes() { return node.GetConnections(); }

    public List<Node> GetPath() { return new List<Node>(path); }

    public bool IsNode(Node node) { return this.node == node; }
}
