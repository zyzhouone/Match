﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class tblpointsView
    {
        public string Pointid
        { get; set; }

        public string Code
        { get; set; }

        public string Lineguid
        { get; set; }

        public string Linename
        { get; set; }

        public int? Id
        { get; set; }

        public int? Eventid
        { get; set; }

        public int? Lineid
        { get; set; }


        [Required(ErrorMessage = "标识名称不能为空！")]
        public string Pointname
        { get; set; }

        public string Content
        { get; set; }

        [Required(ErrorMessage = "序号不能为空！")]
        public int? Sort
        { get; set; }

        public int? Pointtype
        { get; set; }

        public int? Status
        { get; set; }

        public int? Creater
        { get; set; }

        [Required(ErrorMessage = "标识编号不能为空！")]
        public string Pointno
        { get; set; }

        public string pointtypename
        { get; set; }

        [Required(ErrorMessage = "标识地址不能为空！")]
        public string Pointaddress
        { get; set; }

        [Required(ErrorMessage = "任务不能为空！")]
        public string Pointtask
        { get; set; }

        public string Pointout
        { get; set; }

        public string Linkno
        { get; set; }


        public string Sketchmap
        { get; set; }

        public string SketchVoice
        { get; set; }

    }
}
