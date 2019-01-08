using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Model
{
    [Table("tbl_result")]
    public class tblresult
    {
        [Key]
        [Column("match_id",Order=1)]
        public string match_id
        { get; set; }

        [Key]
        [Column("teamid", Order = 2)]
        public string teamid
        { get; set; }

        [Column("`userno`")]
        public string userno
        { get; set; }

        [Column("`teamno`")]
        public string teamno
        { get; set; }

        [Column("`starttime`")]
        public DateTime? starttime
        { get; set; }

        [Column("`settime`")]
        public DateTime? settime
        { get; set; }

        [Column("`timeline`")]
        public string timeline
        { get; set; }

        [Column("`createtime`")]
        public DateTime? Createtime
        { get; set; }

        [Column("`status`")]
        public string status
        { get; set; }

    }
}
