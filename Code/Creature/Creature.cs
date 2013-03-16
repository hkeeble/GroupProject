using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VOiD.Components;

namespace VOiD
{
    class Creature : Entity
    {
        private CreatureModel creatureModel;

        // TEMP
        private short _ID;
        public short ID { get { return _ID; } }

        // Variables
        #region DNA
        public Traits Dominant;//A dominant allele always shows, even if the individual only has one copy of the allele.
        protected Traits Recessive;//A recessive allele only shows if the individual has two copies of the recessive allele.
        #endregion

        #region Statistics
        private int _Health = 60;
        private ushort _Strength = 0;
        private ushort _Dexterity = 0;
        private ushort _Endurance = 50;
        private ushort _Speed = 0;
        public int Health
        {
            get { return _Health; }
            set
            {
                /*
                if (value > 0)
                {
                    if (value <= Dominant.Health.Level)
                        _Health = value;
                    else
                    {
                        if (value < Dominant.Health.Maximum - Dominant.Health.Level)
                        {
                            _Health += value;
                            if (_Health > Dominant.Health.Level)
                            {
                                Dominant.Health.Level = (ushort)_Health;
                            }
                        }
                    }
                }
                else*/
                    _Health = value;
            }
        }
        public ushort Strength = 0;
        public ushort Dexterity = 0;
        public ushort Endurance = 0;
        public ushort Speed = 0;
        #endregion

        #region Behaviour
        public ushort Aggressiveness = 0;
        public ushort Focus = 0;
        public ushort Obedience = 0;
        #endregion

        #region Abilities
        public bool canFly
        {
            get
            {
                return Dominant.Wings.Active && Dominant.Weight.Level <= 4 && Endurance >= 4;
            }
        }
        public bool canSwim
        {
            get
            {
                return Dominant.Weight.Level <= 4 && Endurance >= 4;
            }
        }
        public bool canClimb
        {
            get
            {
                return Dominant.Claws.Active && Dominant.Arms.Active && Endurance >= 2;
            }
        }
        #endregion

        protected UInt16 BattlesLost;
        private AttackTypes avTacks;

        #region Constructors

        public Creature(int Seed)
            : this(Seed, new Texture2D(Configuration.GraphicsDevice,1,1), Vector2.Zero, 1f, 32, 32, 100)
        {

        }

        /// <summary>
        /// Generates a creature based on seed value.
        /// </summary>
        /// <param name="Seed">Seed value.</param>
        public Creature(int Seed, Texture2D texture, Vector2 position2D, float moveSpeed, int frameWidth, int frameHeight, int millisecondsBetweenFrame)
            : base(texture, position2D, moveSpeed, frameWidth, frameWidth, millisecondsBetweenFrame)
        {
            int[] Points = new int[2];
            int[] BDMax = new int[2];
            int[] TUMax = new int[2];

            ushort[,] Stats = new ushort[2, 7];
            ushort[,] BD = new ushort[2, 7];

            StatsBool[,] Toggles = new StatsBool[2, 5];

            Random rand = new Random(Seed);
            for (int y = 0; y < 2; y++)
            {
                Points[y] = 800;
                BDMax[y] = 32;
                TUMax[y] = 32;

                //stats
                for (int i = 0; i < 7; i++)
                {
                    Stats[y, i] = (ushort)rand.Next(Points[y] / (7 - (i)));
                    Points[y] -= Stats[y, i];

                    BD[y, i] = (ushort)rand.Next(BDMax[y] / (7 - (i)));
                    BDMax[y] -= BD[y, i];
                }

                //left over points get allocated
                int looper = 0;
                while (Points[y] != 0)
                {
                    int value = (ushort)rand.Next(1, Points[y]);
                    Stats[y, looper] += (ushort)value;
                    Points[y] -= value;
                    looper = (looper + 1) % 7;
                }

                while (BDMax[y] != 0)
                {
                    int value = (ushort)rand.Next(1, BDMax[y]);
                    BD[y, looper] += (ushort)value;
                    BDMax[y] -= value;
                    looper = (looper + 1) % 7;
                }

                for (int i = 0; i < 5; i++)
                {

                    Toggles[y, i].Active = rand.Next(0, 2) == 0;
                    Toggles[y, i].Used = (ushort)rand.Next(TUMax[y] / (5 - (i)));
                    TUMax[y] -= BD[y, i];
                }


                while (TUMax[y] != 0)
                {
                    looper = (looper + 1) % 5;
                    int value = (ushort)rand.Next(1, TUMax[y]);
                    Toggles[y, looper].Used += value;
                    TUMax[y] -= value;
                }
            }

            StatsUShort[] Health = new StatsUShort[2];
            Health[0].Maximum = Stats[0, 0];
            Health[0].Used = BD[0, 0];
            Health[1].Maximum = Stats[1, 0];
            Health[1].Used = BD[1, 0];
            InitialValues(ref Health);
            StatsUShort[] Weight = new StatsUShort[2];
            Weight[0].Maximum = Stats[0, 1];
            Weight[0].Used = BD[0, 1];
            Weight[1].Maximum = Stats[1, 1];
            Weight[1].Used = BD[1, 1];
            InitialValues(ref Weight);
            StatsUShort[] Size = new StatsUShort[2];
            Size[0].Maximum = Stats[0, 2];
            Size[0].Used = BD[0, 2];
            Size[1].Maximum = Stats[1, 2];
            Size[1].Used = BD[1, 2];
            InitialValues(ref Size);
            StatsUShort[] Strength = new StatsUShort[2];
            Strength[0].Maximum = Stats[0, 3];
            Strength[0].Used = BD[0, 3];
            Strength[1].Maximum = Stats[1, 3];
            Strength[1].Used = BD[1, 3];
            InitialValues(ref Strength);
            StatsUShort[] Dexterity = new StatsUShort[2];
            Dexterity[0].Maximum = Stats[0, 4];
            Dexterity[0].Used = BD[0, 4];
            Dexterity[1].Maximum = Stats[1, 4];
            Dexterity[1].Used = BD[1, 4];
            InitialValues(ref Dexterity);
            StatsUShort[] Endurance = new StatsUShort[2];
            Endurance[0].Maximum = Stats[0, 5];
            Endurance[0].Used = BD[0, 5];
            Endurance[1].Maximum = Stats[1, 5];
            Endurance[1].Used = BD[1, 5];
            InitialValues(ref Endurance);
            StatsUShort[] Speed = new StatsUShort[2];
            Speed[0].Maximum = Stats[0, 6];
            Speed[0].Used = BD[0, 6];
            Speed[1].Maximum = Stats[1, 6];
            Speed[1].Used = BD[1, 6];
            InitialValues(ref Speed);

            Dominant.Health = Health[0];
            Recessive.Health = Health[1];
            Dominant.Weight = Weight[0];
            Recessive.Weight = Weight[1];
            Dominant.Size = Size[0];
            Recessive.Size = Size[1];
            Dominant.Strength = Strength[0];
            Recessive.Strength = Strength[1];
            Dominant.Dexterity = Dexterity[0];
            Recessive.Dexterity = Dexterity[1];
            Dominant.Endurance = Endurance[0];
            Recessive.Endurance = Endurance[1];
            Dominant.Speed = Speed[0];
            Recessive.Speed = Speed[1];

            Dominant.Head = Toggles[0, 0];
            Recessive.Head = Toggles[1, 0];
            Dominant.Legs = Toggles[0, 1];
            Recessive.Legs = Toggles[1, 1];
            Dominant.Arms = Toggles[0, 2];
            Recessive.Arms = Toggles[1, 2];
            Dominant.Wings = Toggles[0, 3];
            Recessive.Wings = Toggles[1, 3];
            Dominant.Claws = Toggles[0, 4];
            Recessive.Claws = Toggles[1, 4];



            //Not Yet Implimented
            Dominant.SpinalColumns.Maximum = 8;
            Dominant.TailColumns.Maximum = 4;

            avTacks = new AttackTypes(Dominant);
            CreateModel();
        }

        /// <summary>
        /// Breeds two creatures together.
        /// </summary>
        /// <param name="A">Parent No1</param>
        /// <param name="B">Parent No2</param>
        public Creature(Creature A, Creature B, Texture2D texture, Vector2 position2D, float moveSpeed, int frameWidth, int frameHeight, int millisecondsBetweenFrame)
            : base(texture, position2D, moveSpeed, frameWidth, frameWidth, millisecondsBetweenFrame)
        {
            StatsUShort[] Health = IntMethod(A.Dominant.Health, B.Dominant.Health, A.Recessive.Health, B.Recessive.Health);
            Dominant.Health = Health[0];
            Recessive.Health = Health[1];

            StatsUShort[] Weight = IntMethod(A.Dominant.Weight, B.Dominant.Weight, A.Recessive.Weight, B.Recessive.Weight);
            Dominant.Weight = Weight[0];
            Recessive.Weight = Weight[1];

            StatsUShort[] Size = IntMethod(A.Dominant.Size, B.Dominant.Size, A.Recessive.Size, B.Recessive.Size);
            Dominant.Size = Size[0];
            Recessive.Size = Size[1];

            StatsUShort[] Strength = IntMethod(A.Dominant.Strength, B.Dominant.Strength, A.Recessive.Strength, B.Recessive.Strength);
            Dominant.Strength = Strength[0];
            Recessive.Strength = Strength[1];

            StatsUShort[] Dexterity = IntMethod(A.Dominant.Dexterity, B.Dominant.Dexterity, A.Recessive.Dexterity, B.Recessive.Dexterity);
            Dominant.Dexterity = Dexterity[0];
            Recessive.Dexterity = Dexterity[1];

            StatsUShort[] Endurance = IntMethod(A.Dominant.Endurance, B.Dominant.Endurance, A.Recessive.Endurance, B.Recessive.Endurance);
            Dominant.Endurance = Endurance[0];
            Recessive.Endurance = Endurance[1];

            StatsUShort[] Speed = IntMethod(A.Dominant.Speed, B.Dominant.Speed, A.Recessive.Speed, B.Recessive.Speed);
            Dominant.Speed = Speed[0];
            Recessive.Speed = Speed[1];

            float AvgRecUsed = (A.RecessiveGenesBoolUsedAvg + B.RecessiveGenesBoolUsedAvg) / 2;

            StatsBool[] Head = BoolMethod(A.Dominant.Head, B.Dominant.Head, A.Recessive.Head, B.Recessive.Head, AvgRecUsed);
            Dominant.Head = Head[0];
            Recessive.Head = Head[1];
            StatsBool[] Legs = BoolMethod(A.Dominant.Legs, B.Dominant.Legs, A.Recessive.Legs, B.Recessive.Legs, AvgRecUsed);
            Dominant.Legs = Legs[0];
            Recessive.Legs = Legs[1];
            StatsBool[] Arms = BoolMethod(A.Dominant.Arms, B.Dominant.Arms, A.Recessive.Arms, B.Recessive.Arms, AvgRecUsed);
            Dominant.Arms = Arms[0];
            Recessive.Arms = Arms[1];
            StatsBool[] Wings = BoolMethod(A.Dominant.Wings, B.Dominant.Wings, A.Recessive.Wings, B.Recessive.Wings, AvgRecUsed);
            Dominant.Wings = Wings[0];
            Recessive.Wings = Wings[1];
            StatsBool[] Claws = BoolMethod(A.Dominant.Claws, B.Dominant.Claws, A.Recessive.Claws, B.Recessive.Claws, AvgRecUsed);
            Dominant.Claws = Claws[0];
            Recessive.Claws = Claws[1];

            // Reset Used on breed creature.
            if (Dominant.Head.Active)
                Dominant.Head.Used = 0;
            if (Dominant.Legs.Active)
                Dominant.Legs.Used = 0;
            if (Dominant.Arms.Active)
                Dominant.Arms.Used = 0;
            if (Dominant.Wings.Active)
                Dominant.Wings.Used = 0;
            if (Dominant.Claws.Active)
                Dominant.Claws.Used = 0;


            avTacks = new AttackTypes(Dominant);
            CreateModel();
        }
        #endregion

        #region CalculateUtilities
        private StatsBool[] BoolMethod(StatsBool AD, StatsBool BD, StatsBool AR, StatsBool BR, float UsedRAvg)
        {
            StatsBool[] output = new StatsBool[2];

            if (AD.Active && BD.Active)
            {
                output[0].Active = true;
                output[0].Used = 0;
                float num = (AD.Used + BD.Used) / 2;
                if (num < UsedRAvg)
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
                if (num < UsedRAvg)
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
                if (num < UsedRAvg)
                    output[1].Active = false;
                else
                    output[1].Active = true;

                output[1].Used = (int)num;
            }

            return output;
        }

        private StatsUShort[] IntMethod(StatsUShort AD, StatsUShort BD, StatsUShort AR, StatsUShort BR)
        {
            StatsUShort[] output = new StatsUShort[2];

            //float recadv = AR.
            int traitD = (AR.Used + BR.Used) / 2;
            int traitR = (AR.Used + BR.Used) / 2;

            if ((traitD > 0.2) && (traitR > traitD))
            {
                output[0].Maximum = (ushort)((AR.Maximum + BR.Maximum) / 2);
                output[0].Used = 0;

                output[1].Maximum = (ushort)((AD.Maximum + BD.Maximum) / 2);
                output[1].Used = (AD.Used + BD.Used) / 2;
            }
            else
            {
                output[0].Maximum = (ushort)((AD.Maximum + BD.Maximum) / 2);
                output[0].Used = 0;

                output[1].Maximum = (ushort)((AR.Maximum + BR.Maximum) / 2);
                output[1].Used = (AR.Used + BR.Used) / 2;
            }


            InitialValues(ref output);

            return output;
        }

        /// <summary>
        /// Set initial values/levels
        /// </summary>
        /// <param name="input">array size should be 2 for dominant and recessive</param>
        protected void InitialValues(ref StatsUShort[] input)
        {
            int value = (800 / 7);//Total Points avalible divided by amount of statistics

            if (input[0].Maximum <= value)
                input[0].Level = (ushort)(input[0].Maximum * 0.80);
            else
                input[0].Level = (ushort)(input[0].Maximum * 0.70);

            if (input[1].Maximum <= value)
                input[1].Level = (ushort)(input[1].Maximum * 0.80);
            else
                input[1].Level = (ushort)(input[1].Maximum * 0.70);
        }

        protected float RecessiveGenesBoolUsedAvg
        {
            get
            {
                int value = 0;
                value += Recessive.Head.Used;
                value += Recessive.Legs.Used;
                value += Recessive.Arms.Used;
                value += Recessive.Wings.Used;
                value += Recessive.Claws.Used;
                return value / 5;
            }
        }
        #endregion

        // Methods
        void CreateModel()
        {
            // Make the model from the set up Dominant traits
            creatureModel = new CreatureModel(new CubePrimitive(Configuration.GraphicsDevice, 0.2f), Vector3.Zero, Vector3.Zero);
            CreatureModel tmp = creatureModel;
            if (Dominant.SpinalColumns.Maximum > 0)
            {
                for (int i = 0; i <= Dominant.SpinalColumns.Maximum; i++)
                {
                    if ((i != Dominant.SpinalColumns.Maximum))
                    {
                        tmp.children.Add(new CreatureModel(new SpherePrimitive(Configuration.GraphicsDevice, 0.3f, 8), Vector3.Up / Dominant.SpinalColumns.Maximum, Vector3.Zero));
                        tmp = tmp.children[0];
                    }
                    else
                    {
                        if (Dominant.Arms.Active)
                        {
                            tmp.children.Add(new CreatureModel(new CubePrimitive(Configuration.GraphicsDevice, .4f), Vector3.Left * 0.6f, Vector3.Zero));
                            tmp.children.Add(new CreatureModel(new CubePrimitive(Configuration.GraphicsDevice, .4f), Vector3.Right * 0.6f, Vector3.Zero));
                        }
                        if (Dominant.Head.Active)
                            tmp.children.Add(new CreatureModel(new CubePrimitive(Configuration.GraphicsDevice, .3f), Vector3.Up / Dominant.SpinalColumns.Maximum, Vector3.Zero));
                    }

                }
            }

            if (Dominant.TailColumns.Maximum > 0)
            {
                tmp = creatureModel;
                for (int i = 0; i < Dominant.TailColumns.Maximum; i++)
                {
                    tmp.children.Add(new CreatureModel(new SpherePrimitive(Configuration.GraphicsDevice, .2f, 8), Vector3.Forward / Dominant.TailColumns.Maximum, new Vector3(0, 0, 0.2f)));
                    tmp = tmp.children[tmp.children.Count - 1];
                }
            }
        }

        void CreateAttacks()
        {
            avTacks = new AttackTypes(this.Dominant);
        }

        public List<Attack> AvailableAttacks
        {
            get { return avTacks.AvailableAttacks; }
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix world)
        {
            if (creatureModel == null)
            {
                CreateModel();
            }
            else
                creatureModel.Draw(world);
        }
    }
}