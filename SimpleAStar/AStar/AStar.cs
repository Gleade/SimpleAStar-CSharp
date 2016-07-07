using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    class AStar
    {

        /// <summary>
        /// Map width.
        /// </summary>
        private uint Width { get; set; }

        /// <summary>
        /// Map height.
        /// </summary>
        private uint Height { get; set; }

        /// <summary>
        /// The cell size of the map.
        /// </summary>
        private uint MapCellSize { get; set; }

        /// <summary>
        /// The cost of moving non-diagonals.
        /// </summary>
        private uint HValuePrecision = 4;

        /// <summary>
        /// The diagonal cost of moving.
        /// </summary>
        private uint DiagonalCost = 6;

        /// <summary>
        /// The parent node.
        /// </summary>
        private Node ParentNode { get; set; }

        /// <summary>
        /// A list of nodes that represent the map.
        /// </summary>
        private List<Node> Map { get; set; }

        /// <summary>
        /// The list of nodes that are currently being looked at.
        /// </summary>
        private List<Node> OpenList { get; set; }

        /// <summary>
        /// The list of nodes that have already been looked at.
        /// </summary>
        private List<Node> ClosedList { get; set; }

        /// <summary>
        /// Static closed nodes (e.g a wall)
        /// </summary>
        private List<Node> StaticClosedList { get; set; }

        /// <summary>
        /// The final solved path.
        /// </summary>
        private List<Node> SolvedPath { get; set; }


        private bool DiagonalSearchEnabled { get; set; }

        public AStar(uint Width, uint Height, bool DiagonalSearchEnabled, uint MapCellSize)
        {
            this.Width = Width;
            this.Height = Height;
            this.DiagonalSearchEnabled = DiagonalSearchEnabled;
            this.MapCellSize = MapCellSize;

            // Create a new map
            Map = new List<Node>();

            // Create our new lists
            SolvedPath = new List<Node>();
            OpenList = new List<Node>();
            ClosedList = new List<Node>();
            StaticClosedList = new List<Node>();

            for (uint y = 0; y < Height; y++)
            {
                for(uint x = 0; x < Width; x++)
                {
                    // Add a new node to our map
                    Map.Add(new Node(x * MapCellSize, y * MapCellSize, MapCellSize));

                    // Possibly add any closed nodes here (Will add later)
                }
            }

        }

        /// <summary>
        /// Add a closed node to the specified position based on X/Y co-ords.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddClosedNode(uint x, uint y)
        {
            StaticClosedList.Add(new Node(x, y, MapCellSize));
        }

        /// <summary>
        /// Add a closed node.
        /// </summary>
        /// <param name="closedNode"></param>
        public void AddClosedNode(Node closedNode)
        {
            StaticClosedList.Add(closedNode);
        }

        private bool CheckClosed(Node aNode)
        {
            return CheckClosedList(aNode);
        }

        /// <summary>
        /// Loop through each node and check if it's on our closed list.
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        private bool CheckClosedList(Node aNode)
        {
            foreach(Node node in ClosedList)
            {
                // Check if the node is on the closed list
                if(node.X == aNode.X && node.Y == aNode.Y)
                {
                    return true;
                }
            }

            // Node isn't on the closed list
            return false;
        }

        /// <summary>
        /// Loop through each node and check if it's on our old closed list.
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        private bool CheckStaticClosedList(Node aNode)
        {
            foreach (Node node in StaticClosedList)
            {
                // Check if the node is on the closed list
                if (node.X == aNode.X && node.Y == aNode.Y)
                {
                    return true;
                }
            }

            // Node isn't on the closed list
            return false;
        }

        private bool CheckOpen(Node aNode)
        {
            foreach (Node node in OpenList)
            {
                // Check if the node is on the open list
                if (node.X == aNode.X && node.Y == aNode.Y)
                {
                    return true;
                }
            }

            // Node isn't on the open list
            return false;
        }

        public List<Node> FindPath(Node Start, Node Finish)
        {
            // Clear our solved path list
            SolvedPath.Clear();

            // Clear our open list
            OpenList.Clear();

            // Clear our closed list
            ClosedList.Clear();

            Console.WriteLine("A");

            // Populate our closed list with our static closed list nodes
            foreach(Node node in StaticClosedList)
            {
                ClosedList.Add(node);
            }

            // Rest all of our nodes on the map
            foreach (Node node in Map)
            {
                node.GValue = HValuePrecision;
                node.HValue = 0;
                node.SearchX = 0;
                node.SearchY = 0;
            }

            // Check if our finish node isn't on the closed list
            if(CheckClosedList(Finish))
            {
                // Return null because there isn't a way to get to a closed node
                return null;
            }

            // Set our parent node to the start of the search area
            ParentNode = Start;

            // Add our parent node to the open list to begin searching for surrounding nodes
            OpenList.Add(ParentNode);

            while(OpenList.Count != 0)
            {
                // Get the node at the top of the list
                ParentNode = OpenList.Last();

                // Add the parent node to our closed list
                ClosedList.Add(ParentNode);

                // Check if we've found the finish
                if(ParentNode.X == Finish.X && ParentNode.Y == Finish.Y)
                {
                    //  We've found the node so finish the loop here
                    Console.WriteLine("Found the finish.");
                    break;
                }

                foreach (Node node in Map)
                {

                    // Find the node north of us
                    if (node.X == ParentNode.X && node.Y == ParentNode.Y - MapCellSize)
                    {
                        if (!CheckClosed(node) && !CheckOpen(node))
                        {
                            // Add to the GValue
                            node.GValue += ParentNode.GValue;
                            // Set our HValue to the shortest path
                            node.SetShortestPath(Finish);
                            // Calculate our FValue (F = G + H)
                            node.CalculateFValue();
                            // Set the node's parent node to the current ParentNode
                            node.Parent = ParentNode;
                            // Add this new node to the open list
                            OpenList.Add(node);
                        }
                    }

                    // Find the node south of us
                    if (node.X == ParentNode.X && node.Y == ParentNode.Y + MapCellSize)
                    {
                        if (!CheckClosed(node) && !CheckOpen(node))
                        {
                            // Add to the GValue
                            node.GValue += ParentNode.GValue;
                            // Set our HValue to the shortest path
                            node.SetShortestPath(Finish);
                            // Calculate our FValue (F = G + H)
                            node.CalculateFValue();
                            // Set the node's parent node to the current ParentNode
                            node.Parent = ParentNode;
                            // Add this new node to the open list
                            OpenList.Add(node);
                        }
                    }

                    // Find node to the east of us
                    if (node.X == ParentNode.X + MapCellSize && node.Y == ParentNode.Y)
                    {
                        if (!CheckClosed(node) && !CheckOpen(node))
                        {
                            // Add to the GValue
                            node.GValue += ParentNode.GValue;
                            // Set our HValue to the shortest path
                            node.SetShortestPath(Finish);
                            // Calculate our FValue (F = G + H)
                            node.CalculateFValue();
                            // Set the node's parent node to the current ParentNode
                            node.Parent = ParentNode;
                            // Add this new node to the open list
                            OpenList.Add(node);
                        }
                    }



                    // Find the node to the west of us
                    if (node.X == ParentNode.X - MapCellSize && node.Y == ParentNode.Y)
                    {
                        if (!CheckClosed(node) && !CheckOpen(node))
                        {
                            // Add to the GValue
                            node.GValue += ParentNode.GValue;
                            // Set our HValue to the shortest path
                            node.SetShortestPath(Finish);
                            // Calculate our FValue (F = G + H)
                            node.CalculateFValue();
                            // Set the node's parent node to the current ParentNode
                            node.Parent = ParentNode;
                            // Add this new node to the open list
                            OpenList.Add(node);
                        }
                    }




                    if (DiagonalSearchEnabled)
                    {
                        // Find the node to the north-west of us
                        if (node.X == ParentNode.X - MapCellSize && node.Y == ParentNode.Y - MapCellSize)
                        {
                            if (!CheckClosed(node) && !CheckOpen(node))
                            {
                                // Add to the GValue
                                node.GValue += ParentNode.GValue + DiagonalCost;
                                // Set our HValue to the shortest path
                                node.SetShortestPath(Finish);
                                // Calculate our FValue (F = G + H)
                                node.CalculateFValue();
                                // Set the node's parent node to the current ParentNode
                                node.Parent = ParentNode;
                                // Add this new node to the open list
                                OpenList.Add(node);
                            }
                        }


                        // Find the node to the north-east of us
                        if (node.X == ParentNode.X + MapCellSize && node.Y == ParentNode.Y - MapCellSize)
                        {
                            if (!CheckClosed(node) && !CheckOpen(node))
                            {
                                // Add to the GValue
                                node.GValue += ParentNode.GValue + DiagonalCost;
                                // Set our HValue to the shortest path
                                node.SetShortestPath(Finish);
                                // Calculate our FValue (F = G + H)
                                node.CalculateFValue();
                                // Set the node's parent node to the current ParentNode
                                node.Parent = ParentNode;
                                // Add this new node to the open list
                                OpenList.Add(node);
                            }
                        }

                        // Find the node to the south-east of us
                        if (node.X == ParentNode.X + MapCellSize && node.Y == ParentNode.Y + MapCellSize)
                        {
                            if (!CheckClosed(node) && !CheckOpen(node))
                            {
                                // Add to the GValue
                                node.GValue += ParentNode.GValue + DiagonalCost;
                                // Set our HValue to the shortest path
                                node.SetShortestPath(Finish);
                                // Calculate our FValue (F = G + H)
                                node.CalculateFValue();
                                // Set the node's parent node to the current ParentNode
                                node.Parent = ParentNode;
                                // Add this new node to the open list
                                OpenList.Add(node);
                            }
                        }

                        // Find the node to the north-east of us
                        if (node.X == ParentNode.X + MapCellSize && node.Y == ParentNode.Y - MapCellSize)
                        {
                            if (!CheckClosed(node) && !CheckOpen(node))
                            {
                                // Add to the GValue
                                node.GValue += ParentNode.GValue + DiagonalCost;
                                // Set our HValue to the shortest path
                                node.SetShortestPath(Finish);
                                // Calculate our FValue (F = G + H)
                                node.CalculateFValue();
                                // Set the node's parent node to the current ParentNode
                                node.Parent = ParentNode;
                                // Add this new node to the open list
                                OpenList.Add(node);
                            }
                        }

                        // Console.WriteLine("AZ: " + OpenList.Count)

                    }
                }

                // Remove the parent node from the open list
                if (OpenList.Count > 0)
                {
                    OpenList.Remove(ParentNode);
                }

                // Sort the list by FValue
                OpenList = OpenList.OrderBy(order => order.FValue).ToList();
                OpenList.Reverse();



            }

            // Find the finish node and its parent
            foreach(Node node in ClosedList)
            {
                if(node.CompareOtherNode(Finish))
                {
                    // Add the finished node to the solved path
                    SolvedPath.Add(node);

                    // Get the finished node's parent node
                    ParentNode = node.Parent;

                    // Add the finish node's parent to the Solved List
                    SolvedPath.Add(ParentNode);

                    foreach(Node closedNode in ClosedList)
                    {
                        if(ParentNode.CompareOtherNode(Start) == false)
                        {
                            // Trace back trough each parent node
                            ParentNode = ParentNode.Parent;
                            SolvedPath.Add(ParentNode);
                        }
                        else
                        {
                            // We've found the start node & have completed the path
   
                            return SolvedPath;
                        }
                    }

                }
            }



            return SolvedPath;
        }

        
    }


}
