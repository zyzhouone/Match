using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class AssignsPreview
    {
        public string linesId
        { get; set; }

        public string linesName
        { get; set; }

        public int linesTeamCount
        { get; set; }

        public int totalTeamCount
        { get; set; }

        public List<AssignsTeam> linesTeams
        { get; set; }
    }
}
