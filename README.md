# SimpleAStar

A simple C# implimentation of the A* Algorithm. 

Example:

```C#

class Program
{
	static void Main(string[] args)
	{
		AStar.AStar map = new AStar.AStar(10, 4, true, 32);
		map.AddClosedNode(64, 96);
		map.AddClosedNode(64, 64);
		map.AddClosedNode(128, 96);
		map.AddClosedNode(128, 64);
		map.AddClosedNode(128, 32);

		List<Node> Result = map.FindPath(new Node(192, 96, 32), new Node(0, 32, 32));

		foreach(Node a in Result)
		{
			Console.WriteLine(a.X + "," + a.Y);
		}

	}
}