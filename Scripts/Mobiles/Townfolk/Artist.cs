using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	public class Artist : BaseCreature
	{
		public override bool CanTeach { get { return true; } }

		[Constructable]
		public Artist()
			: base( AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4 )
		{
			InitStats( 31, 41, 51 );

			SetSkill( SkillName.Healing, 36, 68 );


			SpeechHue = Utility.RandomDyedHue();
			Title = "the artist";
			Hue = Utility.RandomSkinHue();


			if( this.Female = Utility.RandomBool() )
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName( "female" );

                AddItem(Skirt(Utility.RandomAllColors()));

			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName( "male" );

                AddItem(PlainPants(Utility.RandomAllColors()));
			}

            AddItem(PlainShirt(Utility.RandomAllColors()));
			AddItem( RandomShoes( Utility.RandomNeutralHue() ) );
			AddItem( new HalfApron( 2301 ) );

			Utility.AssignRandomHair( this );

			Container pack = new Backpack();

			pack.Movable = false;

			AddItem( pack );
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
            AddLootPouch(LootPack.PoorPouch);
            AddLoot(LootPack.PoorPile);
        }

		public override bool ClickTitle { get { return false; } }


		public Artist( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
