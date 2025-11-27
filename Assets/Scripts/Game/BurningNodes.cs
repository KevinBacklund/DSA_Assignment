using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /*
    I chose to use a list to store burning node for alot of the same reasons as the DOT effects on units. 
    Also because it is the data structure I am most familiar with and comfortable using.
    */
    public class BurningNodes : MonoBehaviour
    {
        private class BurningNode
        {
            public Cave.Node   Node;
            public float       Duration;
            public float applyCooldown = 0;
            public BurningNode(Cave.Node node, float duration)
            {
                Node = node;
                Duration = duration;
            }
        }
        List<BurningNode> burningNodes = new List<BurningNode>();
        public void AddBurningNode(Cave.Node node, float duration)
        {
            burningNodes.Add(new BurningNode(node, duration));
        }

        private void Update()
        {
            for (int i = 0; i < burningNodes.Count; ++i)
            {
                BurningNode bn = burningNodes[i];
                bn.applyCooldown -= Time.deltaTime;
                bn.Duration -= Time.deltaTime;
                if (bn.Duration <= 0.0f)
                {
                    burningNodes.RemoveAt(i);
                }
                else
                {
                    if (bn.Node.Unit != null && bn.applyCooldown <= 0)
                    {
                        int duration = Random.Range(4, 6);
                        Unit unit = bn.Node.Unit;
                        unit.damageOverTimeEffects.Add(new Burning(duration));
                        Effects.CreateFire(unit.transform, new Vector3(Random.Range(0,0.2f),Random.Range(0,0.5f)), 0.5f, duration);
                        bn.applyCooldown = 0.5f;
                    }
                }
            }
        }
    }
}
