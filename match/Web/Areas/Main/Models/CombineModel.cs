using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Main.Models
{
    public class CombineModel
    {
        public string teamid { get; set; }
        public string sexy { get; set; }

        public string age { get; set; }
    }

    public class CombineListModel
    {
        public List<CombineModel> list { get; set; }
    }
}