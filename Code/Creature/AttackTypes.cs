using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOiD
{
    struct Attack
    {
        public string Name;
        public float Damage;

        public Attack(string Name, float Damage)
        {
            this.Name = Name;
            this.Damage = Damage;
        }
    }

    struct AttackTypes
    {
        private List<Attack> Attacks;

        public AttackTypes(Traits traits)
        {
            Attacks = new List<Attack>();

            if (traits.TailColumns.Level>=0)
            {
                AvailableAttacks.Add(new Attack("Whip", ((traits.Dexterity.Level*traits.Agressiveness.Level*traits.Strength.Level)/3)/6));
            }
        }

        public List<Attack> AvailableAttacks
        {
            get { return Attacks; }
        }
    }
}