using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOiD
{
    /// <summary>
    /// Variable statistics 
    /// </summary>
    public struct StatsUShort
    {
        /// <summary>
        /// Battle Data. how many times skill has been used or required.
        /// </summary>
        public int Used;

        /// <summary>
        /// Maximum value for this statistic.
        /// </summary>
        public ushort Maximum;

        /// <summary>
        /// Current level of skill. use items or level creature.
        /// </summary>
        public ushort Level;
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

    struct Traits
    {
        // Physical
        public StatsBool Head, Legs, Arms, Wings, Claws;
        public StatsUShort SpinalColumns;
        public StatsUShort TailColumns;

        // Surival - TWO-PART-SPLIT
        public StatsUShort Health;
        public StatsUShort Weight;
        public StatsUShort Size;
        public StatsUShort Strength;
        public StatsUShort Dexterity;
        public StatsUShort Endurance;
        public StatsUShort Speed;

        // Personality - (Battle Traits) VOLATILE
        public StatsUShort Agressiveness;
        public StatsUShort Obedience;
        public StatsUShort Focus;
    }
}