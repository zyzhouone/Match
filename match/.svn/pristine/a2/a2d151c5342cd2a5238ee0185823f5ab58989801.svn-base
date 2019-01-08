using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/********************************************
 * sys_match_user实体类
 * 
 * *****************************************/
namespace Model
{
    [Table("sys_match_user")]
    public class sysmatchuser
    {
        [Key]
        [Column("id", Order = 1)]
        public string Id
        { get; set; }

        [Column("`account`")]
        public string Account
        { get; set; }

        [Column("`name`")]
        public string Name
        { get; set; }

        [Column("`pwd`")]
        public string Pwd
        { get; set; }

        [Column("`match_id`")]
        public string Matchid
        { get; set; }

        [Column("`role`")]
        public string Role
        { get; set; }

        [Column("`linesid`")]
        public string Linesid
        { get; set; }

        [Column("`status`")]
        public int? Status
        { get; set; }

        [NotMapped]
        public string NewPassword
        { get; set; }

        [NotMapped]
        public string ConfirmPassword
        { get; set; }

    }
}
