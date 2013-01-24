using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreatureGame
{
    class Creature : Entity
    {
        //private CreatureModel creatureModel;

	    // Variables
	    protected Traits Dominant;//A dominant allele always shows, even if the individual only has one copy of the allele.
	    protected Traits Recessive;//A recessive allele only shows if the individual has two copies of the recessive allele.
	    protected UInt16 BattlesLost;
        private AttackTypes avTacks;

	    //Constructors
        public Creature(short ID, Texture2D texture, Vector2 position2D, float moveSpeed)
            : base(texture, position2D, moveSpeed)
        {
            Traits temp = new Traits();
            temp.Pelvis = true;
            temp.Spine = true;
            temp.SpinalColumns = 4;
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
        void CreateAttacks()
        {
            avTacks = new AttackTypes(this.Dominant);
        }

        public List<Attack> AvalibleAttacks
        {
            get { return avTacks.AvalibleAttacks; }
        }

        public void Draw(GraphicsDevice graphicsDevice)
	    {
            //creatureModel.Draw(Matrix.Identity);
	    }
    }
}