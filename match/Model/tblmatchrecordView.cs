﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class tblmatchrecordView
    {
        public string nickname
        { get; set; }

        public string teamname
        { get; set; }

        public string pointname
        { get; set; }

        public DateTime? pointtime
        { get; set; }

        public string typ
        { get; set; }

        public string status
        { get; set; }

        public DateTime? createtime
        { get; set; }
    }
}
