using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VOiD
{
    public class Creature : Entity
    {
        private CreatureModel creatureModel;

        // TEMP
        private short _ID;
        public short ID { get { return _ID; } }

	    // Variables
	    protected Traits Dominant;//A dominant allele always shows, even if the individual only has one copy of the allele.
	    protected Traits Recessive;//A recessive allele only shows if the individual has two copies of the recessive allele.
	    protected UInt16 BattlesLost;
        private AttackTypes avTacks;

	    //Constructors
        public Creature(short ID, Texture2D texture, Vector2 position2D, float moveSpeed)
            : base(texture, position2D, moveSpeed)
        {
            _ID = ID;
            Position = position2D;
            Traits temp = new Traits();
            temp.Spine = true;
            temp.SpinalColumns = 4;
            temp.Head = true;
            temp.Arms = true;
            temp.Tail = true;
            temp.TailColumns = 6;
            Dominant = temp;
            Recessive = temp;
            CreateAttacks();
            BattlesLost = 0;
        }

        public Creature(ref Creature a, ref Creature b, GraphicsDevice graphicsDevice)
            : base(null, Vector2.Zero, 1.0f)
            // Could Gene Samples be included here, and passed in as a parameter, with a null default value?
	    {                                                                              // If you don't want to implement them then I can do it, would just need another class and a few small modifications here.
		    if(a.Recessive.Tail&b.Recessive.Tail)
		    {
			    Dominant.Tail=true;
			    if (!(a.Dominant.Tail&b.Dominant.Tail))
				    Recessive.Tail=false;
			    else
                {
                    Recessive.Tail = new Random().Next(100) % 2 == 0;
                }
		    }
		    else if (a.Dominant.Tail|b.Dominant.Tail)
		    {
			    if (a.BattlesLost<b.BattlesLost)
			    {
				    Dominant.Tail=true;
				    Recessive.Tail=false;
			    }
			    else if (a.BattlesLost>b.BattlesLost)
			    {
				    Dominant.Tail=false;
				    Recessive.Tail=true;
			    }
			    else
			    {
				    Dominant.Tail  = new Random().Next(100) % 2 == 0;
				    Recessive.Tail = new Random().Next(100) % 2 == 0;
			    }
		    }
		    else
		    {
                Dominant.Tail  = new Random().Next(100) % 2 == 0;
                Recessive.Tail = new Random().Next(100) % 2 == 0;
		    }

		    if (b.Dominant.TailColumns!=0)
			    Dominant.TailColumns=(ushort)(a.Dominant.TailColumns/b.Dominant.TailColumns);
		    else
			    Dominant.TailColumns=a.Dominant.TailColumns;
            CreateAttacks();
            BattlesLost = 0;
	    }
        
	    // Methods
        void CreateModel(GraphicsDevice graphicsDevice)
        {
            // Make the model from the set up Dominant traits
            creatureModel = new CreatureModel(new CubePrimitive(graphicsDevice,0.2f), Vector3.Zero, Vector3.Zero);
            CreatureModel tmp =  creatureModel;
            if (Dominant.Spine)
            {
                for (int i = 0; i <= Dominant.SpinalColumns; i++)
                {
                    if ((i != Dominant.SpinalColumns))
                    {
                        tmp.children.Add(new CreatureModel(new SpherePrimitive(graphicsDevice, 0.6f, 8), Vector3.Up/Dominant.SpinalColumns, Vector3.Zero));
                        tmp = tmp.children[0];
                    }
                    else
                    {
                        if (Dominant.Arms)
                        {
                            tmp.children.Add(new CreatureModel(new CubePrimitive(graphicsDevice, .4f), Vector3.Left*0.6f, Vector3.Zero));
                            tmp.children.Add(new CreatureModel(new CubePrimitive(graphicsDevice, .4f), Vector3.Right*0.6f, Vector3.Zero));
                        }
                        if (Dominant.Head)
                            tmp.children.Add(new CreatureModel(new CubePrimitive(graphicsDevice, .3f), Vector3.Up / Dominant.SpinalColumns, Vector3.Zero));
                    }

                }
            }
            /*
            if (Dominant.Tail)
            {
                tmp = creatureModel;
                for (int i = 0; i < Dominant.TailColumns; i++)
                {
                    tmp.children.Add(new CreatureModel(new SpherePrimitive(graphicsDevice, .4f, 8), Vector3.Forward / Dominant.TailColumns, Vector3.Zero));
                    tmp = tmp.children[0];
                }
            }*/
        }

        void CreateAttacks()
        {
            avTacks = new AttackTypes(this.Dominant);
        }

        public List<Attack> AvalibleAttacks
        {
            get { return avTacks.AvalibleAttacks; }
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix world)
	    {
            if (creatureModel == null)
            {
                CreateModel(graphicsDevice);
            }
            else
                creatureModel.Draw(world);
	    }
    }
}