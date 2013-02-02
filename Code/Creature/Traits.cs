using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOiD
{
    public struct Traits
    {
	    // Visual
	    public bool Pelvis, Spine, Neck, Calfs, Feet, Thighs, Tail, Arms, Ears, Eyes, Nose;
        public ushort SpinalColumns;
        public ushort TailColumns;
        public ushort AmountofLegs;
        public ushort AmountofArms;
        //public unsafe fixed float BaseColor[3];
        //public unsafe fixed float LegColor[3];
        //public unsafe fixed float ArmColor[3];
        public ushort EyeType;
        public ushort EarType;
        public ushort NoseType;

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