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
                AvailableAttacks.Add(new Attack("Whip", (traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 6));

            if(traits.Wings.Active)
                AvailableAttacks.Add(new Attack("Gust", (traits.Dexterity.Level + traits.Speed.Level) / 6));

            if (traits.Head.Active)
                AvailableAttacks.Add(new Attack("Headbutt", (traits.Strength.Level + traits.Agressiveness.Level) / 4));

            if (traits.Claws.Active&&traits.Arms.Active)
                AvailableAttacks.Add(new Attack("Scratch", ((((traits.Dexterity.Level + traits.Speed.Level)/2)) + traits.Agressiveness.Level) / 6));

            if (traits.Arms.Active)
                AvailableAttacks.Add(new Attack("Punch", (traits.Focus.Level + traits.Strength.Level + traits.Size.Level + ((traits.Agressiveness.Level + 1) * (traits.Focus.Level + 1))) / 10));

            if (traits.Legs.Active)
                AvailableAttacks.Add(new Attack("Kick", (traits.Dexterity.Level + traits.Strength.Level + traits.Agressiveness.Level) / 6));

            if (traits.Arms.Active&&traits.Weight.Level<=32)
                AvailableAttacks.Add(new Attack("Bludgeon", (traits.Weight.Level + traits.Strength.Level + traits.Agressiveness.Level) / 6));
        }

        public List<Attack> AvailableAttacks
        {
            get { return Attacks; }
        }
    }
}