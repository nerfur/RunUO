using System;
using Server;
using Server.Items;
using EDI = Server.Mobiles.EscortDestinationInfo;

namespace Server.Mobiles
{
	public class Peasant : BaseEscortable
	{
		[Constructable]
		public Peasant()
		{
			Title = "the peasant";

		}

		public override bool CanTeach{ get{ return true; } }
		public override bool ClickTitle{ get{ return false; } } // Do not display 'the peasant' when single-clicking

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
            AddItem(PlainShirt(Utility.RandomAllColors()));

            if (Female)
                AddItem(Skirt(Utility.RandomAllColors()));
            else
                AddItem(PlainPants(Utility.RandomAllColors()));

			Utility.AssignRandomHair( this );
		}

		public Peasant( Serial serial ) : base( serial )
		{
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
            AddLootPouch(LootPack.PoorPouch);
            AddLoot(LootPack.PoorPile);
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