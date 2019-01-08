using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class GroupTeam
    {
        public string teamid
        { get; set; }

        public string teamno
        { get; set; }

        public string teamname
        { get; set; }

        public string lineid
        { get; set; }

        public string matchid
        { get; set; }

        public string linesid
        { get; set; }

        public List<GroupUser> groupuser
        { get; set; }
    }
}
