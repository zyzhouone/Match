using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace Model
{
    /// <summary>
    /// zzy add
    /// </summary>
    public class tblteamsGroupVew
    {
        public string checkid { get; set; }
        public string teamid
        { get; set; }

        public string match_id
        { get; set; }

        public string matchname
        { get; set; }

        public string matchUserId
        { get; set; }

        public string groupType
        { get; set; }

        public string Teamno
        { get; set; }

        public string Teamname
        { get; set; }

        public string Userid
        { get; set; }

        public string Mobile
        { get; set; }

        public string Sexystring
        { get; set; }
        public int Sexy
        { get; set; }

        public int Age
        { get; set; }

        public string AgeBetween { get; set; }
        public string Username
        { get; set; }

        public string Nickname
        { get; set; }

        public string oprdatetime { get; set; }
        public string Company
        { get; set; }

        public string Lineid
        { get; set; }

        public string Linename
        { get; set; }

        public string Linesid
        { get; set; }

        public string line_no
        { get; set; }

        public string Linesname
        { get; set; }

        public DateTime? Createtime
        { get; set; }

        public int? Eventid
        { get; set; }

        public string Eventname
        { get; set; }

        public int? Status
        { get; set; }



    }
}
