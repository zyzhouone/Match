using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/********************************************
 * tbl_points实体类
 * 
 * *****************************************/
namespace Model
{
    [Table("tbl_points")]
    public class tblpoints
    {
        [Key]
        [Column("pointid",Order=1)]
        public string Pointid
        { get;set; }

        [Column("lineguid")]
        public string Lineguid
        { get; set; }

        [Column("id")]
        public string Id
        { get; set; }

        [Column("`eventid`")]
        public string Eventid
        { get;set; }

        [Column("`lineid`")]
        public string Lineid
        { get;set; }
        
        [Column("`pointname`")]
        public string Pointname
        { get;set; }

        [Column("`content`")]
        public string Content
        { get;set; }
        
        [Column("`sort`")]
        public int? Sort
        { get;set; }

        [Column("`pointtype`")]
        public int? Pointtype
        { get;set; }

        [Column("`linkno`")]
        public string Linkno
        { get; set; }

        [Column("`status`")]
        public int? Status
        { get;set; }

        [Column("`creater`")]
        public DateTime? Creater
        { get;set; }
        
        [Column("`pointno`")]
        public string Pointno
        { get;set; }
      
        [Column("`pointaddress`")]
        public string Pointaddress
        { get; set; }
        
        [Column("`pointtask`")]
        public string Pointtask
        { get; set; }

        [Column("`pointout`")]
        public string Pointout
        { get; set; }

        [Column("`sketchmap`")]
        public string Sketchmap
        { get; set; }

        [Column("`sketchvoice`")]
        public string SketchVoice
        { get; set; }

    }
}
