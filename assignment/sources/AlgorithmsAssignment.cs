using System.Drawing;
using GXPEngine;
using GXPEngine.OpenGL;

class AlgorithmsAssignment : Game
{

    Dungeon dungeon = null;

	NodeGraph nodeGraph = null;
	TiledView tiledView = null;
	NodeGraphAgent agent = null;

	PathFinder pathFinder = null;

	//common settings
	const int dungeonScale = 15;                // MIN OF 2
	const int minRoomSize = 18;

	public static int maxShrink = 3;            // MAX OF (MIN_ROOM_SIZE/2)-1 (inclusive)
	public static int minShrink = 1;

    public static int maxGenerationLoops = 100000;
    public static float finalizeRoomChanse = .005f;
    public static bool generateAllDoors = false;

	public static bool nodeGraphHighQuality = true;

    public AlgorithmsAssignment() : base(1920, 1080, false, false, -1, -1, false)
	{
		//set our default background color and title
		GL.ClearColor(0, 0, 0, 1);
		GL.glfwSetWindowTitle("Algorithms Game");

		Grid grid = new Grid(width, height, dungeonScale);

		Size size = new Size(width / dungeonScale, height / dungeonScale);

		dungeon = new GeneratedDungeon(size);
        //assign the SCALE we talked about above, so that it no longer looks like a tinietiny stamp:
        dungeon?.SetScale(dungeonScale);

		//Tell the dungeon to generate rooms and doors with the given MIN_ROOM_SIZE
		dungeon?.StartGeneration(minRoomSize);
		

		/////////////////////////////////////////////////////////////////////////////////////////
		/// ASSIGNMENT 2 : GRAPHS, AGENTS & TILES
		///							
		/// SKIP THIS BLOCK UNTIL YOU'VE FINISHED ASSIGNMENT 1 AND ASKED FOR TEACHER FEEDBACK !

		/////////////////////////////////////////////////////////////
		//Assignment 2.1 Sufficient (Mandatory) High Level NodeGraph
		//
		//TODO: Study assignment 2.1 on blackboard
		//TODO: Study the NodeGraph and Node classes
		//TODO: Study the SampleDungeonNodeGraph class and try it out below
		//TODO: Comment out the SampleDungeonNodeGraph again, implement a HighLevelDungeonNodeGraph class and uncomment it below

		//_graph = new SampleDungeonNodeGraph(_dungeon);
		nodeGraph = new HighLevelDungeonNodeGraph(dungeon);
        //_graph = new LowLevelDungeonNodeGraph(_dungeon);

        nodeGraph?.StartGeneration();

        /////////////////////////////////////////////////////////////
        //Assignment 2.1 Sufficient (Mandatory) OnGraphWayPointAgent
        //
        //TODO: Study the NodeGraphAgent class
        //TODO: Study the SampleNodeGraphAgent class and try it out below
        //TODO: Comment out the SampleNodeGraphAgent again, implement an OnGraphWayPointAgent class and uncomment it below

        //agent = new OnGraphWayPointAgent(nodeGraph);
        agent = new RandomPathFindOnGraphWayPointAgent(nodeGraph);

        ////////////////////////////////////////////////////////////
        //Assignment 2.2 Good (Optional) TiledView
        //
        //TODO: Study assignment 2.2 on blackboard
        //TODO: Study the TiledView and TileType classes
        //TODO: Study the SampleTileView class and try it out below
        //TODO: Comment out the SampleTiledView again, implement the TiledDungeonView and uncomment it below

        //tiledView = new SampleTiledView(dungeon, TileType.GROUND);
        tiledView = new TiledDungeonView(dungeon); 
        tiledView?.Generate();

		////////////////////////////////////////////////////////////
		//Assignment 2.2 Good (Optional) RandomWayPointAgent
		//
		//TODO: Comment out the OnGraphWayPointAgent above, implement a RandomWayPointAgent class and uncomment it below

		//_agent = new RandomWayPointAgent(_graph);	

		//////////////////////////////////////////////////////////////
		//Assignment 2.3 Excellent (Optional) LowLevelDungeonNodeGraph
		//
		//TODO: Comment out the HighLevelDungeonNodeGraph above, and implement the LowLevelDungeonNodeGraph 

		/////////////////////////////////////////////////////////////////////////////////////////
		/// ASSIGNMENT 3 : PathFinding and PathFindingAgents
		///							
		/// SKIP THIS BLOCK UNTIL YOU'VE FINISHED ASSIGNMENT 2 AND ASKED FOR TEACHER FEEDBACK !

		//////////////////////////////////////////////////////////////////////////
		//Assignment 3.1 Sufficient (Mandatory) - Recursive Pathfinding
		//
		//TODO: Study assignment 3.1 on blackboard
		//TODO: Study the PathFinder class
		//TODO: Study the SamplePathFinder class and try it out
		//TODO: Comment out the SamplePathFinder, implement a RecursivePathFinder and uncomment it below

		//_pathFinder = new SamplePathFinder(_graph);
		//_pathFinder = new RecursivePathFinder(_graph);

		//////////////////////////////////////////////////////////////////////////
		//Assignment 3.1 Sufficient (Mandatory) - BreadthFirst Pathfinding
		//
		//TODO: Comment out the RecursivePathFinder above, implement a BreadthFirstPathFinder and uncomment it below
		//_pathFinder = new BreadthFirstPathFinder(_graph);

		//TODO: Implement a PathFindingAgent that uses one of your pathfinder implementations (should work with any pathfinder implementation)
		//_agent = new PathFindingAgent(_graph, _pathFinder);

		/////////////////////////////////////////////////
		//Assignment 3.2 Good & 3.3 Excellent (Optional)
		//
		//There are no more explicit TODO's to guide you through these last two parts.
		//You are on your own. Good luck, make the best of it. Make sure your code is testable.
		//For example for A*, you must choose a setup in which it is possible to demonstrate your 
		//algorithm works. Find the best place to add your code, and don't forget to move the
		//PathFindingAgent below the creation of your PathFinder!

		//------------------------------------------------------------------------------------------
		/// REQUIRED BLOCK OF CODE TO ADD ALL OBJECTS YOU CREATED TO THE SCREEN IN THE CORRECT ORDER
		/// LOOK BUT DON'T TOUCH :)

		if (grid != null) AddChild(grid);
		if (dungeon != null) AddChild(dungeon);
		if (tiledView != null) AddChild(tiledView);
		if (nodeGraph != null) AddChild(nodeGraph);
		if (pathFinder != null) AddChild(pathFinder);             //pathfinder on top of that
		if (nodeGraph != null) AddChild(new NodeLabelDrawer(nodeGraph));  //node label display on top of that
		if (agent != null) AddChild(agent);                       //and last but not least the agent itself

		/////////////////////////////////////////////////
		//The end!
		////
	}

	public void Update()
	{
		while (false && dungeon.rooms.Count!=60)
		{
            dungeon.StartGeneration(minRoomSize);
            nodeGraph.StartGeneration();
            tiledView?.Generate();
            agent.GotoRandomNode();
        }

        if (Input.GetKey(Key.R))
		{
			dungeon.StartGeneration(minRoomSize);
			nodeGraph.StartGeneration();
            tiledView?.Generate();
			agent.GotoRandomNode();
        }
	}
}


