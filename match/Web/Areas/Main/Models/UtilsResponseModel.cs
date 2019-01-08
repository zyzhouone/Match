using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Main.Models
{
    public class UtilsResponseModel
    {
        public int iResultCode { get; set; }
        public List<string> agelist { get; set; }
        public List<TeamGroupModel> teamList { get; set; }

        public int leftNum { get; set; }
    }
}