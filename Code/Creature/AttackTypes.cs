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
                AvailableAttacks.Add(new Attack("Whip", ((traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 3)/3));

            if(traits.Wings.Active)
                AvailableAttacks.Add(new Attack("Gust", ((traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 3) / 3));

            if (traits.Head.Active)
                AvailableAttacks.Add(new Attack("Headbutt", ((traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 3) / 3));

            if (traits.Claws.Active&&traits.Arms.Active)
                AvailableAttacks.Add(new Attack("Scratch", ((traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 3) / 3));

            if (traits.Arms.Active)
                AvailableAttacks.Add(new Attack("Punch", ((traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 3) / 3));

            if (traits.Legs.Active)
                AvailableAttacks.Add(new Attack("Kick", ((traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 3) / 3));

            if (traits.Arms.Active&&traits.Weight.Level<=32)
                AvailableAttacks.Add(new Attack("Bludgeon", ((traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 3) / 3));
        }

        public List<Attack> AvailableAttacks
        {
            get { return Attacks; }
        }
    }
}