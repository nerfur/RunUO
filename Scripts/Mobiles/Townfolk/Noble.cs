using System;
using Server;
using Server.Items;
using EDI = Server.Mobiles.EscortDestinationInfo;

namespace Server.Mobiles
{
	public class Noble : BaseEscortable
	{
		[Constructable]
		public Noble()
		{
			Title = "the noble";

			SetSkill( SkillName.Parry, 80.0, 100.0 );
			SetSkill( SkillName.Swords, 80.0, 100.0 );
			SetSkill( SkillName.Tactics, 80.0, 100.0 );
		}

		public override bool CanTeach{ get{ return true; } }
		public override bool ClickTitle{ get{ return false; } } // Do not display 'the noble' when single-clicking

		private static int GetRandomHue()
		{
			switch ( Utility.Random( 6 ) )
			{
				default:
				case 0: return 0;
				case 1: return Utility.RandomBlueHue();
				case 2: return Utility.RandomGreenHue();
				case 3: return Utility.RandomRedHue();
				case 4: return Utility.RandomYellowHue();
				case 5: return Utility.RandomNeutralHue();
			}
		}

		public override void InitOutfit()
		{
            if (Female)
                AddItem(new FancyDress(Utility.RandomNeutralHue()));
            else
            {
                AddItem(new FancyShirt(Utility.RandomAllColors()));
                AddItem(new LongPants(Utility.RandomAllColors()));
            }

            AddItem(RandomBoots());

		    AddItem( new BodySash( Utility.RandomAllColors() ) );

            AddItem(new Cloak(Utility.RandomAllColors()));

			AddItem( new Longsword() );

			Utility.AssignRandomHair( this );
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
            AddLootPouch(LootPack.PoorPouch);
            AddLoot(LootPack.AveragePile);
        }

		public Noble( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}