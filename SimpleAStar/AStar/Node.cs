using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    class Node
    {
        /// <summary>
        /// Node X position.
        /// </summary>
        public uint X { get; set; }

        /// <summary>
        /// Node Y position.
        /// </summary>
        public uint Y { get; set; }

        /// <summary>
        /// Node SearchX position.
        /// </summary>
        public uint SearchX { get; set; }

        /// <summary>
        /// Node SearchY position.
        /// </summary>
        public uint SearchY { get; set; }

        /// <summary>
        /// The cell size of the node.
        /// </summary>
        private uint CellSize;

        /// <summary>
        /// Movement cost.
        /// </summary>
        public uint GValue { get; set; }

        /// <summary>
        /// Estimated movement cost.
        /// </summary>
        public uint HValue { get; set; }

        /// <summary>
        /// The score of GValue + HValue.
        /// </summary>
        public uint FValue { get; set; }

        // Parent node
        public Node Parent { get; set; }

        /// <summary>
        /// Create a node at position x, y.
        /// </summary>
        /// <param name="Xpos"></param>
        /// <param name="Ypos"></param>
        /// <param name="CellSize">Size of the cells e.g 32x32 </param>
        public Node(uint Xpos, uint Ypos, uint CellSize)
        {
            X = Xpos;
            Y = Ypos;
            this.CellSize = CellSize;
        }

        /// <summary>
        /// Calculate the FValue (GValue + HValue)
        /// </summary>
        public void CalculateFValue()
        {
            FValue = GValue + HValue;
        }

        /// <summary>
        /// Compare this nodes positon with another node.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        private bool CompareNode(Node Other)
        {
            // Compare this nodes SearchX & Y with the Other node
            if(Other.X == SearchX && Other.Y == SearchY)
            {
                return true;
            }

            // Return false otherwise
            return false;
        }

        /// <summary>
        /// Compares another node with this one.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool CompareOtherNode(Node Other)
        {
            if(Other.X == X && Other.Y == Y)
            {
                // We are the same node
                return true;
            }

            // We are not the same node
            return false;

        }


        /// <summary>
        /// Calculate the shortest path towards the Other node.
        /// </summary>
        /// <param name="Other"></param>
        public void SetShortestPath(Node Other)
        {
            // Set our search starting position
            SearchX = X;
            SearchY = Y;

            // While we aren't the finish node
            while(!CompareNode(Other))
            {
                if(SearchX > Other.X)
                {
                    SearchX -= CellSize;
                }
                else if (SearchX < Other.X)
                {
                    SearchX += CellSize;
                }
                else if (SearchY > Other.Y)
                {
                    SearchY -= CellSize;
                }
                else if (SearchY < Other.Y)
                {
                    SearchY += CellSize;
                }

                // Increment our HValue
                HValue++;
            }

        }



        


    }
}
