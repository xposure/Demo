using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class CharacterGroup : IEnumerable<Character>
    {
        private Character[] partyMembers;
        private Character partyLeader;

        public Character PartyLeader
        {
            get
            {
                //System.Diagnostics.Debug.Assert(partyLeader != null, "party leader was null");
                //System.Diagnostics.Debug.Assert(partyLeader.IsAlive, "party leader was dead");

                if (partyLeader == null || !partyLeader.IsAlive)
                {
                    foreach (var member in this)
                    {
                        if (member.IsAlive)
                        {
                            partyLeader = member;
                            break;
                        }
                    }
                }

                return partyLeader;
            }
        }

        public CharacterGroup(Character[] _partyMembers)
        {
            partyMembers = _partyMembers;
        }

        public IEnumerator<Character> GetEnumerator()
        {
            return ((IEnumerable<Character>)partyMembers).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return partyMembers.GetEnumerator();
        }

        public override string ToString()
        {
            var sb = new StringBuilder(1024);
            foreach (var member in this)
                sb.AppendLine(member.ToString());

            return sb.ToString();
        }

    }
}

