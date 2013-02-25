using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOiD
{
    /// <summary>
    /// Variable statistics 
    /// </summary>
    public struct StatsInt
    {
        /// <summary>
        /// Battle Data. how many times skill has been used or required.
        /// </summary>
        int Used;

        /// <summary>
        /// Maximum value for this statistic.
        /// </summary>
        int Maximum;

        /// <summary>
        /// Current level of skill. use items or level creature.
        /// </summary>
        int Level;

        /// <summary>
        /// Current value.
        /// </summary>
        int Current;
    }

    /// <summary>
    /// fixed statistics 
    /// </summary>
    public struct StatsBool
    {
        public bool Active;

        /// <summary>
        /// Battle Data. how many times skill has been used or required.
        /// </summary>
        public int Used;
    }

    public class fuck
    {
        public StatsBool[] BoolMethod(StatsBool AD, StatsBool BD, StatsBool AR, StatsBool BR)
        {
            StatsBool[] output = new StatsBool[2];

            if (AD.Active && BD.Active)
            {
                output[0].Active = true;
                output[0].Used = 0;
                float num = (AD.Used + BD.Used) / 2;
                if (num < 4.5f)
                    output[1].Active = false;
                else
                    output[1].Active = true;

                output[1].Used = (int)num;
            }
            else if (AD.Active && BD.Active)
            {
                output[0].Active = true;
                output[0].Used = 0;
                float num = (AR.Used + BR.Used) / 2;
                if (num < 4.5f)
                    output[1].Active = false;
                else
                    output[1].Active = true;

                output[1].Used = (int)num;
            }
            else
            {
                output[0].Active = false;
                output[0].Used = (AD.Used + BD.Used) / 2;

                float num = (AR.Used + BR.Used) / 2;
                if (num < 4.5f)
                    output[1].Active = false;
                else
                    output[1].Active = true;

                output[1].Used = (int)num;
            }

            return output;
        }

        public StatsInt[] IntMethod(StatsInt AD, StatsInt BD, StatsInt AR, StatsInt BR)
        {
            StatsInt[] output = new StatsInt[2];

            //float recadv = AR.

            return output;
        }
    }

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