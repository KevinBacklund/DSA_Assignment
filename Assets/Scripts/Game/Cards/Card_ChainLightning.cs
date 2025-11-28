using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

namespace Game
{
    public class Card_ChainLightning : Cards.Card
    {
        private static Color32 sm_electrictyColor = new Color32(10, 92, 255, 64);

        public class Tree
        {
            public class Node
            {
                public Cave.Node value;
                public List<Node> children;
            }
            public Node root;
        }


        #region Properties

        public override string Name => "Chain\nLightning";

        public override int ManaCost => 15;

        public override string IconName => "Lightning";

        #endregion

        protected Queue<Cave.Node> CalculateNodes(Vector2Int vMouseCoord)
        {
            Queue<Cave.Node> result = new();
            Tree chainlightning = new();
            chainlightning.root.value = Cave.Instance[vMouseCoord];
            FindChildren(chainlightning.root);

            return result;
        }

        protected Tree.Node FindChildren(Tree.Node node) 
        {
            Queue<Cave.Node> open = new();
            HashSet<Cave.Node> closed = new();
            open.Enqueue(node.value);
            while (open.Count > 0) 
            {
                Cave.Node current = open.Dequeue();
                closed.Add(current);
                foreach (Cave.Node neighbour in Cave.Instance.GetNeighbours(current)) 
                {
                    if (!closed.Contains(neighbour) && !open.Contains(neighbour))
                    {
                        if (neighbour.Coord.x < current.Coord.x + 2 && neighbour.Coord.y < current.Coord.y + 2)
                        {
                            if (neighbour.Unit != null)
                            {
                                Tree.Node childNode = new Tree.Node();
                                childNode.value = neighbour;
                                node.children.Add(childNode);
                            }
                            open.Enqueue(neighbour);
                        }
                    }
                }    
            }
            if (node.children.Count > 0)
            {
                foreach (Tree.Node child in node.children)
                {
                    return FindChildren(child);
                }
            }
            return node;
        }

        public override List<(Vector2Int, Color32)> GetPreviewEffect(Vector2Int vMouseCoord) 
        {
            return new List<(Vector2Int, Color32)>()
            {
                (vMouseCoord, sm_electrictyColor)
            };
        }

        public override void Perform(Vector2Int vMouseCoord)
        {

        }
    }
}
