using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/********************************************
 * tbl_orders实体类
 * 
 * *****************************************/
namespace Model
{
    [Table("tbl_orders")]
    public class tblorders
    {
        [Key]
        [Column("id",Order=1)]
        public int? Id
        { get;set; }

        [Column("`match_id`")]
        public string Match_Id
        { get;set; }

        [Column("`orderid`")]
        public string Orderid
        { get;set; }

        [Column("`teamid`")]
        public int? Teamid
        { get;set; }

        [Column("`lineid`")]
        public int? Lineid
        { get;set; }

        [Column("`userid`")]
        public int? Userid
        { get;set; }

        [Column("`ordertotal`")]
        public string Ordertotal
        { get;set; }

        [Column("`status`")]
        public int? Status
        { get;set; }

        [Column("`createtime`")]
        public string Createtime
        { get;set; }

    }
}
