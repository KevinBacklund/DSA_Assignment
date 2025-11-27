using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Card_Firewall : Cards.Card
    {
        private static Color32  sm_fireColor = new Color32(255, 92, 10, 64);

        #region Properties

        public override string Name => "Firewall";

        public override int ManaCost => 12;

        public override string IconName => "Fire";

        #endregion

        protected Queue<Cave.Node> CalculateNodes(Vector2Int vMouseCoord)
        {
            // TODO:             
            // 1. Get the Cave.Node at vMouseCoord (use Cave.Instance[vMouseCoord])
            // 2. If it is a valid Node, grow the result by alternating North and South in a growing pattern
            // 3. Only add Valid / Walkable nodes
            // 4. Store the result in a queue and return from this function
            Cave.Node targetPos = Cave.Instance[vMouseCoord];

            if (targetPos == null)
            {
                return null;
            }

            Queue<Cave.Node> result = new Queue<Cave.Node>();

            result.Enqueue(targetPos);

            Cave.Node northNode = Cave.Instance[targetPos.Coord.x, targetPos.Coord.y + 1];
            Cave.Node southNode = Cave.Instance[targetPos.Coord.x, targetPos.Coord.y - 1];

            while (northNode != null || southNode != null)
            {
                if (northNode != null)
                {
                    result.Enqueue(northNode);
                    northNode = Cave.Instance[northNode.Coord.x, northNode.Coord.y + 1];
                }
                if (southNode != null)
                {
                    result.Enqueue(southNode);
                    southNode = Cave.Instance[southNode.Coord.x, southNode.Coord.y - 1];
                }
            }

            return result;
        }

        public override List<(Vector2Int, Color32)> GetPreviewEffect(Vector2Int vMouseCoord)
        {
            // TODO: use CalculateNodes() to get the affected nodes from vMouseCoord
            // Create a result list of type List<(Vector2Int, Color32)>.
            // For each Node in the node queue add a tuple (Node.Coord, sm_fireColor) in your
            // result list and return this list

            Queue<Cave.Node> affectedNodes = CalculateNodes(vMouseCoord);
            List<(Vector2Int, Color32)> result = new List<(Vector2Int, Color32)>();
            foreach (Cave.Node node in affectedNodes)
            {
                result.Add((node.Coord, sm_fireColor));
            }
            return result;
        }

        public override void Perform(Vector2Int vMouseCoord)
        {
            // TODO: Use CalculateNodes() to get the affected node queue
            // Add a burning effect at this node (see the Effects.CreateFire() function)
            // Add this Node to a container/manager of 'Burning nodes' 

            // (Optional Bonus) Add a little delay between each Node so that
            // the firewall "grows" in the alternating North / South pattern over time
            float duration = 7.0f;
            Queue<Cave.Node> affectedNodes = CalculateNodes(vMouseCoord);

            Cave.Instance.StartCoroutine(AddBurningNodeWithDelay(affectedNodes, duration));

            IEnumerator AddBurningNodeWithDelay(Queue<Cave.Node> affectedNodes, float duration)
            {
                while (affectedNodes.Count > 0)
                {
                    Cave.Node node = affectedNodes.Dequeue();
                    Effects.CreateFire(node.Center, 0.7f, duration);
                    Cave.Instance.burningNodes.GetComponent<BurningNodes>().AddBurningNode(node, duration);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
}
