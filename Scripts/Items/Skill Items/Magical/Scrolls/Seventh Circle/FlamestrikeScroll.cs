using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class FlamestrikeScroll : SpellScroll
	{
		[Constructable]
		public FlamestrikeScroll() : this( 1 )
		{
		}

		[Constructable]
		public FlamestrikeScroll( int amount ) : base( 50, 0x1F5F, amount )
		{
		}

		public FlamestrikeScroll( Serial serial ) : base( serial )
		{
		}
        
        public override void OnSingleClick(Mobile from)
        {
            if (this.Name != null)
            {
                if (Amount >= 2)
                {
                    from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", Amount + " " + this.Name));
                }
                else
                {
                    from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", this.Name));
                }
            }
            else
            {
                if (Amount >= 2)
                {
                    from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", Amount + " Flamestrike scrolls"));
                }
                else
                {
                    from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "a Flamestrike scroll"));
                }
            }
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