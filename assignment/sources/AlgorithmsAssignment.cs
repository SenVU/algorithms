using System;
using System.Drawing;
using GXPEngine;
using GXPEngine.OpenGL;

class AlgorithmsAssignment : Game
{
	public static AlgorithmsAssignment instance = null;

	Grid grid = null;

    Dungeon dungeon = null;
	NodeGraph nodeGraph = null;
	TiledView tiledView = null;
	NodeGraphAgent agent = null;
	PathFinder pathFinder = null;

    //common dungeon settings
    private static int? generationSeed = null;	//null for random

    public const int SCREEN_WIDTH = 1920;				//the width of the progam window
    public const int SCREEN_HEIGHT = 1080;				//the height of the progam window

    public const int DUNGEON_SCALE = 20;                // MIN of 2
	public const int MIN_ROOM_SIZE = 14;

	public const int MAX_ROOM_SHRINK = 3;				// MAX of (MIN_ROOM_SIZE/2)-1 (inclusive) _ Min of MIN_ROOM_SHRINK
    public const int MIN_ROOM_SHRINK = 1;				// MAX of MAX_ROOM_SHRINK _ MIN of 0

    public const int MAX_GENERATION_LOOPS = -1;			// MAX any positive int _ MIN of -1 _ -1 for infinite
    public const float FINALIZE_ROOM_CHANSE = .005f;	// RANGE of 0 to 1 _ chanse of a room not splitting further
    public const bool GENERATE_ALL_DOORS = false;		// generates a door between all rooms

    public const bool TILED_VIEW = true;                //fancy mode


    // common node graph settings
    public const bool NODEGRAPH_HIGH_QUALITY = true;
	public const bool NODEGRAPH_HIGH_QUALITY_DIAGONALS = true	// only works if NODEGRAPH_HIGH_QUALITY is true
        && NODEGRAPH_HIGH_QUALITY;


	// common agent settings
	public const NodeGraphAgent.AgentType AGENT_TYPE = NodeGraphAgent.AgentType.PATHFIND;
    public const int AGENT_SPEED = 2;
    public const int AGENT_RUN_SPEED = 10;
    public const int AGENT_RUN_KEY = Key.LEFT_CTRL;

    public const PathFinder.PathFindType PATHFIND_TYPE = PathFinder.PathFindType.ITERATIVE;     // only works if AGENT_TYPE is PATHFIND
    public const bool PATH_FIND_TRUE_DISTANCE = true;                                           // makes the pathfinder search for shortest path based on actual distance instead of nodecount

    public AlgorithmsAssignment() : base(SCREEN_WIDTH, SCREEN_HEIGHT, false, false, -1, -1, false)
	{
		// set the instance so it can be referanced later
		instance = this;

		//set our default background color and title
		GL.ClearColor(0, 0, 0, 1);
		GL.glfwSetWindowTitle("Algorithms Game");

		// set all variables
		CreateVariables();

		// generate everything
		if (generationSeed!=null)
			Generate(generationSeed.Value);
		else
			Generate();

		// add everyling to the engine
		if (grid != null) AddChild(grid);
		if (dungeon != null) AddChild(dungeon);
		if (tiledView != null) AddChild(tiledView);
		if (nodeGraph != null) AddChild(nodeGraph);
		if (pathFinder != null) AddChild(pathFinder);             //pathfinder on top of that
		if (nodeGraph != null) AddChild(new NodeLabelDrawer(nodeGraph));  //node label display on top of that
		if (agent != null) AddChild(agent);                       //and last but not least the agent itself
	}

    void CreateVariables()
	{
        grid = new Grid(width, height, DUNGEON_SCALE);

        Size size = new Size(width / DUNGEON_SCALE, height / DUNGEON_SCALE);

        dungeon = new GeneratedDungeon(size);
        dungeon?.SetScale(DUNGEON_SCALE);

        nodeGraph = NODEGRAPH_HIGH_QUALITY ?
            new HighQualityDungeonNodeGraph(dungeon) :
            new DungeonNodeGraph(dungeon) as NodeGraph;

        switch (PATHFIND_TYPE)
        {
            case PathFinder.PathFindType.RECURSIVE:
                pathFinder = new RecursivePathFinder(nodeGraph);
                break;
            case PathFinder.PathFindType.ITERATIVE:
                pathFinder = new IterativePathFinder(nodeGraph);
                break;
        }
        
        switch (AGENT_TYPE)
        {
            case NodeGraphAgent.AgentType.RANDOM:
                agent = new RandomPathFindWayPointAgent(nodeGraph);
                break;
            case NodeGraphAgent.AgentType.QEUEU:
                agent = new QueueWayPointAgent(nodeGraph);
                break;
            case NodeGraphAgent.AgentType.PATHFIND:
                agent = new PathFindingAgent(nodeGraph, pathFinder);
                break;
        }

        if (TILED_VIEW) tiledView = new TiledDungeonView(dungeon);
    }

    void Generate(int seed)
	{
        dungeon?.StartGeneration(seed);
        nodeGraph?.StartGeneration();
        tiledView?.StartGeneration();
        agent?.GoToRandomNode(seed);
        pathFinder.DrawClear();
    }

    void Generate()
    {
        Generate(Environment.TickCount);
    }

    public void Update()
    {
        if (Input.GetKey(Key.R))
        {
            if (generationSeed != null)
                Generate(generationSeed.Value);
            else
                Generate();
        }
    }

    public NodeGraphAgent GetAgent() { return agent; }
}


