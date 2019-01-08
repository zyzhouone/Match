using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Assigns
    {
        public string matchid
        { get; set; }

        public string matchname
        { get; set; }

        public string lineid
        { get; set; }

        public string linename
        { get; set; }

        public List<AssignsPreview> AssignsPreview
        { get; set; }
    }
}
