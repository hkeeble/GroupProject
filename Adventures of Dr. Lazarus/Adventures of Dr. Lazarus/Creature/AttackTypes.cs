using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreatureGame
{
    public struct Attack
    {
        public string Name;
        public float Damage;

        public Attack(string Name, float Damage)
        {
            this.Name = Name;
            this.Damage = Damage;
        }
    }

    public struct AttackTypes
    {
        private List<Attack> Attacks;

        public AttackTypes(Traits traits)
        {
            Attacks = new List<Attack>();

            if (traits.Tail)
            {
                AvalibleAttacks.Add(new Attack("Whip", ((traits.Dexterity*traits.Agressiveness*traits.Strength)/3)/6));
            }
        }

        public List<Attack> AvalibleAttacks
        {
            get { return Attacks; }
        }
    }
}
