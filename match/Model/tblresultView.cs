using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class tblresultView
    {
        public string teamid
        { get; set; }

        public string teamname
        { get; set; }

        public string match_id
        { get; set; }

        public string match_name
        { get; set; }

        public string userno
        { get; set; }

        public string teamno
        { get; set; }

        public DateTime? starttime
        { get; set; }

        public DateTime? settime
        { get; set; }

        public string timeline
        { get; set; }

        public DateTime? createtime
        { get; set; }

        public string Status
        { get; set; }

        public string line_no
        { get; set; }
    }
}
