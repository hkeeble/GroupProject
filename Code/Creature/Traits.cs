using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOiD
{
    public struct Traits
    {
	    // Visual
	    public bool Spine, Head, Legs, Arms, Tail, Wings;
        public ushort SpinalColumns;
        public ushort TailColumns;

	    // Surival
        public float Weight;
        public float Strength;
        public float Dexterity;
        public float Size;
        public float Endurence;//FP
        public float Health;//HP

	    // Personality (will influence survival)
        public float Agressiveness;
        public float Focus;// not too sure if this is personality / Maybe this contributes to how far they'll chase a creature on the map? - Henri

	    // Other
        public float Luck;
    };
}