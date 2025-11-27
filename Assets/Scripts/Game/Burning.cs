using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Burning : DamageOverTime
    {
        public Burning(float fDuration) : base(fDuration)
        {
           
        }

        protected override void ApplyEffect(Unit unit)
        {
            unit.TakeDamage(Random.Range(4,7));
        }
    }
}
