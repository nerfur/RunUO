using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Items;
using Server.Guilds;

namespace Server.Mobiles
{
    public class SpawningGuard : BaseCreature
    {
        public override bool ClickTitle { get { return false; } }

        [Constructable]
        public SpawningGuard() : base(AIType.AI_Melee, FightMode.Aggressor, 14, 1, 0.8, 1.6)
        {
            Title = "the guard";

            SpeechHue = Utility.RandomDyedHue();

            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");

                switch (Utility.Random(2))
                {
                    case 0: AddItem(new LeatherSkirt()); break;
                    case 1: AddItem(new LeatherShorts()); break;
                }

                switch (Utility.Random(5))
                {
                    case 0: AddItem(new FemaleLeatherChest()); break;
                    case 1: AddItem(new FemaleStuddedChest()); break;
                    case 2: AddItem(new LeatherBustierArms()); break;
                    case 3: AddItem(new StuddedBustierArms()); break;
                    case 4: AddItem(new FemalePlateChest()); break;
                }
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");

                AddItem(new PlateChest());
                AddItem(new PlateArms());
                AddItem(new PlateLegs());

                switch (Utility.Random(3))
                {
                    case 0: AddItem(new Doublet(Utility.RandomNondyedHue())); break;
                    case 1: AddItem(new Tunic(Utility.RandomNondyedHue())); break;
                    case 2: AddItem(new BodySash(Utility.RandomNondyedHue())); break;
                }
            }
            Utility.AssignRandomHair(this);

            if (Utility.RandomBool())
                Utility.AssignRandomFacialHair(this, HairHue);

            Halberd weapon = new Halberd();

            weapon.Movable = false;

            AddItem(weapon);

            Container pack = new Backpack();

            pack.Movable = false;

            pack.DropItem(new Gold(10, 25));

            AddItem(pack);

            SetStr(81, 105);
            SetDex(91, 115);
            SetInt(96, 120);

            SetHits(81, 105);

            SetDamage(10, 25);
            Karma = 25;
            VirtualArmor = 16;

            Skills[SkillName.Tactics].Base = 100.0;
            Skills[SkillName.Swords].Base = 100.0;
            Skills[SkillName.MagicResist].Base = 100.0;
            Skills[SkillName.DetectHidden].Base = 100.0;

            //this.NextCombatTime = DateTime.Now + TimeSpan.FromSeconds(0.5);
        }

        public void GetSpeech()
        {
            if (Utility.Random(3) == 1)
            {
                switch (Utility.Random(15))
                {
                    case 0: Say(true, "Er... thanks."); break;
                    case 1: Say(true, "I really hope that wasn't intended as a bribe."); break;
                    case 2: Say(true, "How disgusting!  I'll dispose of this."); break;
                    case 3: Say(true, "Er... thanks."); break;
                    case 4: Say(true, "Er... thanks."); break;
                    case 5: Say(true, "If this were the head of a murderer, I would check for a bounty."); break;
                    case 6: Say(true, "I shall place this on my mantle!"); break;
                    case 7: Say(true, "This tasteth like chicken."); break;
                    case 8: Say(true, "This tasteth just like the juicy peach I just ate."); break;
                    case 9: Say(true, "Ahh!  That was the one piece I was missing!"); break;
                    case 10: Say(true, "Somehow, it reminds me of mother."); break;
                    case 11: Say(true, "It's a sign!  I can see Elvis in this!"); break;
                    case 12: Say(true, "Thanks, I was missing mine."); break;
                    case 13: Say(true, "I'll put this in the lost-and-found box."); break;
                    case 14: Say(true, "My family will eat well tonight!"); break;
                }
            }
            else
            {
                Say(true, "'Tis a decapitated head. How disgusting.");
            }
        }

        public class GiveBountyTimer : Timer
        {
            PlayerMobile pm;
            Mobile guard;
            Head head;
            Mobile from;

            public GiveBountyTimer(PlayerMobile b, Mobile g, Head h, Mobile f)
                : base(TimeSpan.FromSeconds(5))
            {
                pm = b;
                guard = g;
                head = h;
                from = f;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (pm.Bounty > 0)
                {
                    guard.Say(true, String.Format("The bounty on {0} was {1} gold, and has been credited to your account.", pm.Name, pm.Bounty));
                    pm.Bounty = 0;
                }
                /*else if (head.Killer != ((PlayerMobile)from))
                {
                    guard.Say(true, "I had heard that this scum was taken care of. But thou didst not do the deed, and thus shall not get the reward!");
                }*/
                else
                {
                    guard.Say(true, String.Format("There was no bounty on {0}.", pm.Name));
                }

                Stop();
                return;
            }
        }

        public void FindDeletePost(PlayerMobile from)
        {
            List<Item> list = new List<Item>();

            foreach (Item item in World.Items.Values)
            {
                if (item is BulletinBoardPost)
                {
                    BulletinBoardPost bm = item as BulletinBoardPost;

                    if (bm.Subject == String.Format("{0}: {1} gold pieces", from.Name, from.Bounty))
                        list.Add(bm);
                }
            }

            foreach (BulletinBoardPost post in list)
                post.Delete();

            list.Clear();
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Head)
            {
                Head ph = dropped as Head;

                Mobile p = World.FindMobile(ph.PlayerSerial);

                if (p != null && p is PlayerMobile && (ph.WhenKilled + TimeSpan.FromHours(24.0)) > DateTime.Now)
                {
                    PlayerMobile pm = p as PlayerMobile;
                    if (pm.Bounty > 0)
                    {
                        if (from.Karma < 1)
                        {
                            Say(true, "We only accept bounty hunting from honorable folk! Away with thee!");
                            return false;
                        }

                        Say(true, "Ah, a head!  Let me check to see if there is a bounty on this.");
                        if (Banker.Deposit(from, pm.Bounty))
                        {
                            FindDeletePost(pm);
                            Timer m_timer = new GiveBountyTimer(pm, this, ph, from);
                            m_timer.Start();
                            ph.Delete();
                            return true;
                        }
                        else
                        {
                            Say(true, String.Format("There is a bounty on {0}, but your bank box is full.", pm.Name));
                            return false;
                        }
                    }
                    else
                    {
                        if (from.Karma < 1)
                        {
                            Say(true, "We only accept bounty hunting from honorable folk! Away with thee!");
                            return false;
                        }
                        //GetSpeech();
                        Say(true, "Ah, a head!  Let me check to see if there is a bounty on this.");
                        Timer m_timer = new GiveBountyTimer(pm, this, ph, from);
                        m_timer.Start();
                        ph.Delete();
                        return true;
                    }
                }
                else
                {
                    GetSpeech();
                    ph.Delete();
                    return true;
                }
            }
            return base.OnDragDrop(from, dropped);
        }

        public SpawningGuard(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}