using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model;
using Utls;
using BLL;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Drawing;

namespace Web.Areas.Main.Controllers
{
    public class MatchController : BaseController
    {
       
        public ActionResult lines(string optMatch, string optLine, int? pageIndex)
        {
            string matchid = base.UserInfo.Matchid;
            string linesid = base.UserInfo.Linesid;
            tblmatch Match = new MatchBll().GetMatchById(matchid);

            //List<SelectListItem> Status = new MemberBll().GetDict(3);
            //ViewData["Status"] = Status;

            var lines = new List<tbllinesView>();
            try
            {
                if (!string.IsNullOrEmpty(linesid))
                {
                    lines = new MatchBll().GetLinesOnly(matchid, linesid, pageIndex.GetValueOrDefault(1));
                }
                else
                {
                    lines = new MatchBll().GetLines(matchid, optLine, pageIndex.GetValueOrDefault(1));
                }
               


                if (optMatch != null)
                {
                   
                    ViewBag.Match += "<option value='" + Match.Match_id + "' selected>" + Match.Match_name + "</option>";
                }
                else
                {
                    ViewBag.Match += "<option value='" + Match.Match_id + "'>" + Match.Match_name + "</option>";
                }

                if (optLine != "")
                {
                    List<SelectListItem> line = new MatchBll().GetlineList(optMatch);
                    foreach (SelectListItem r in line)
                    {
                        if (optLine == r.Value)
                            ViewBag.Line += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                        else
                            ViewBag.Line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                    }
                }

            }
            catch (Exception e)
            {

            }
            return View(lines);
        }

        [HttpPost]
        public ActionResult GetLineByMatch(string matchid)
        {
            var bll = new MatchBll();
            List<SelectListItem> line = new MatchBll().GetlineList(matchid);
            return Json(line, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLinesByLine(string lineid)
        {
            var bll = new MatchBll();
            List<SelectListItem> lines = new MatchBll().GetlinesList(lineid);
            return Json(lines, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LinesEdit(string id)
        {
            var bll = new MatchBll();
            var model = bll.GetLinesById(id);
            if (model != null)
            {
                List<SelectListItem> Status = new MemberBll().GetDict(3);
                foreach (SelectListItem r in Status)
                {
                    if (model.Status.ToString() == r.Value)
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "'selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

                string matchid = base.UserInfo.Matchid;
                tblmatch Match = new MatchBll().GetMatchById(matchid);
                ViewBag.Match += "<option value='" + Match.Match_id + "' selected>" + Match.Match_name + "</option>";


                List<SelectListItem> line = new MatchBll().GetlineList(model.Match_id);
                foreach (SelectListItem r in line)
                {
                    if (model.Line_id == r.Value)
                        ViewBag.Line += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.Line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult LinesEdit(string id, FormCollection fc)
        {
            var bll = new MatchBll();
            var model = bll.GetLinesById(id);
            model.Match_id = fc["optMatch"].ToString();
            model.Line_id = fc["optLine"].ToString();
            model.Line_no = fc["Line_no"].ToString();
            model.Linename = fc["Linename"].ToString().Trim();
            model.Content = fc["Content"].ToString().Trim();
            if (fc["Playercount"] != "")
            {
                model.Playercount = Int32.Parse(fc["Playercount"].ToString().Trim());
            }
            if (fc["Pointscount"]!="")
            {
                model.Pointscount = Int32.Parse(fc["Pointscount"].ToString().Trim());
            }
            
           // model.Condition_Sex = Int32.Parse(fc["Condition_Sex"].ToString().Trim());
            //model.Condition_Age = Int32.Parse(fc["Condition_Age"].ToString().Trim());
            //model.Condition_Subline = Int32.Parse(fc["Condition_Subline"].ToString().Trim());
            model.Url = fc["Url"].ToString();
            model.Summary = fc["Summary"].ToString();
            model.Status = Int32.Parse(fc["optStatus"].ToString());
            try
            {
                bll.EditLines(model);
            }
            catch (ValidException ex)
            {
                this.ModelState.AddModelError(ex.Name, ex.Message);
                return View(model);
            }

            return this.RefreshParent();
        }

        public ActionResult LinesCreate()
        {
            List<SelectListItem> Status = new MemberBll().GetDict(3);
            foreach (SelectListItem r in Status)
            {
                ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }


            string matchid = base.UserInfo.Matchid;
            tblmatch Match = new MatchBll().GetMatchById(matchid);
            ViewBag.Match += "<option value='" + Match.Match_id + "'>" + Match.Match_name + "</option>";


            return View();
        }

        [HttpPost]
        public ActionResult LinesCreate(string id, FormCollection fc)
        {
            var bll = new MatchBll();
            var model = new tbllines();
            model.Lines_id = Guid.NewGuid().ToString();
            model.Match_id = fc["optMatch"].ToString();
            model.Line_id = fc["optLine"].ToString();
            model.Line_no = fc["Lineno"].ToString();
            model.Linename = fc["Linename"].ToString().Trim();
            model.Content = fc["Content"].ToString().Trim();
            if (fc["Playercount"] != "")
            {
                model.Playercount = Int32.Parse(fc["Playercount"].ToString().Trim());
            }
            if (fc["Pointscount"] != "")
            {
                model.Pointscount = Int32.Parse(fc["Pointscount"].ToString().Trim());
            }
            // model.Condition_Sex = Int32.Parse(fc["Condition_Sex"].ToString().Trim());
            //model.Condition_Age = Int32.Parse(fc["Condition_Age"].ToString().Trim());
            //model.Condition_Subline = Int32.Parse(fc["Condition_Subline"].ToString().Trim());
            model.Url = fc["Url"].ToString();
            model.Summary = fc["Summary"].ToString();
            model.Status = Int32.Parse(fc["optStatus"].ToString());
            try
            {
                bll.AddLines(model);
            }
            catch (ValidException ex)
            {
                this.ModelState.AddModelError(ex.Name, ex.Message);
                return View(model);
            }

            return this.RefreshParent();
        }

        public ActionResult points(string id,int? pageIndex)
        {
            var points = new List<tblpointsView>();
            try
            {
                points = new MatchBll().GetPoints(id, pageIndex.GetValueOrDefault(1));

                ViewBag.linesid = id;
            }
            catch (Exception e)
            {

            }
            return View(points);
        }

         public ActionResult showcontent(string id){
            var bll = new MatchBll();
            var model = bll.GetPointById(id);
            return View(model);
         }

         public ActionResult QRcodePrint()
         {
            string matchid = UserInfo.Matchid;
            var bll = new MatchBll();
            var lines = bll.GetlinesByMatchid(matchid);
            foreach (SelectListItem r in lines)
            {
                ViewBag.optLines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }
            return View();
         }
        
        [HttpPost]
         public ActionResult GetPoints(string linesid)
        {
            var bll = new MatchBll();
            List<tblpoints> points = new MatchBll().GetPoints(linesid);
            return Json(points, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult PointsEdit(string id)
        {
            var bll = new MatchBll();
            var model = bll.GetPointById(id);
            ViewBag.code = MD5Encrypt(id + "dx");

            if (model.Pointtype == 1)
                ViewBag.code = "{\"linesid\":\"" + model.Lineguid + "\"}";
            else if (model.Pointtype == 2)
                ViewBag.code = MD5Encrypt("csdxsuccessdx");

            List<SelectListItem> pointtype = new MemberBll().GetDict(8);
            List<SelectListItem> Status = new MemberBll().GetDict(14);
            foreach (SelectListItem r in Status)
            {
                if(model.Status .ToString() == r.Value)
                    ViewBag.Status += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                else
                    ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            foreach (SelectListItem r in pointtype)
            {
                if (model.Pointtype.ToString() == r.Value)
                    ViewBag.pointtype += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                else
                    ViewBag.pointtype += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            //链接编号，目前写死0~10
            List<SelectListItem> linkno = new List<SelectListItem>();
            for (int i = 0; i < 11; i++)
            {
                SelectListItem tmp = new SelectListItem();
                tmp.Value = i.ToString();
                tmp.Text = i.ToString();
                linkno.Add(tmp);
            }

            foreach (SelectListItem r in linkno)
            {
                if (model.Linkno == r.Value)
                    ViewBag.optLinkno += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                else
                    ViewBag.optLinkno += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PointsEdit(string id, FormCollection fc)
        {
            var bll = new MatchBll();
            var model = bll.GetPointById(id);
            model.Pointname = fc["Pointname"].ToString();
            //model.Content = fc["Content"].ToString().Trim();
            model.Pointno = fc["Pointno"].ToString();
            model.Pointaddress = fc["Pointaddress"].ToString();
            model.Pointtask = fc["Pointtask"].ToString();
            model.Pointout = fc["Pointout"].ToString();

            string filename_img = "";
            string filename_voice = "";
            string SketchmapFlag = fc["SketchmapFlag"].ToString();
            string SketchvoiceFlag = fc["SketchvoiceFlag"].ToString();

            if (fc["optLinkno"] != "")
            {
                model.Linkno = fc["optLinkno"].ToString();
                //链接编号，目前写死0~10
                List<SelectListItem> linkno = new List<SelectListItem>();
                for (int i = 0; i < 11; i++)
                {
                    SelectListItem tmp = new SelectListItem();
                    tmp.Value = i.ToString();
                    tmp.Text = i.ToString();
                    linkno.Add(tmp);
                }

                foreach (SelectListItem r in linkno)
                {
                    if (model.Linkno == r.Value)
                        ViewBag.optLinkno += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.optLinkno += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

            }

            if (fc["optPointtype"] != "")
            {
                model.Pointtype = Int32.Parse(fc["optPointtype"].ToString());
                List<SelectListItem> pointtype = new MemberBll().GetDict(8);
                foreach (SelectListItem r in pointtype)
                {
                    if (model.Pointtype.ToString() == r.Value)
                        ViewBag.pointtype += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.pointtype += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

            }

            if (fc["optStatus"] != "")
            {
                model.Status = Int32.Parse(fc["optStatus"].ToString());
                List<SelectListItem> Status = new MemberBll().GetDict(14);
                foreach (SelectListItem r in Status)
                {
                    if (model.Status.ToString() == r.Value)
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }
            }


            HttpFileCollectionBase files = Request.Files;
            if (SketchmapFlag == "1")
            {
                model.Sketchmap = "";
            }else
            {
                HttpPostedFileBase file = files["sketchmap"];
                if (file != null)
                {
                    if (file.FileName != "")
                    {
                        filename_img = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                        if ((filename_img.ToUpper() == "PNG" || filename_img.ToUpper() == "JPG") && file.ContentLength / 1024 < 2048)
                        {
                            string path = Server.MapPath("~/UploadFiles/");
                            filename_img = model.Pointno + "_image_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + filename_img;
                            file.SaveAs(path + filename_img);
                            model.Sketchmap = filename_img;
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "示意图文件不合法！\r\n png,jpg文件,小于2m";
                            return View(model);
                        }
                    }
                }
            }

            if (SketchvoiceFlag == "1")
            {
                model.SketchVoice = "";
            }else
            {
                HttpPostedFileBase file1 = files["sketchvoice"];
                if (file1 != null)
                {
                    if (file1.FileName != "")
                    {
                        filename_voice = file1.FileName.Substring(file1.FileName.LastIndexOf(".") + 1);
                        if ((filename_voice.ToUpper() == "MP3") || (filename_voice.ToUpper() == "WAV") && file1.ContentLength / 1024 < 5120)
                        {
                            string path = Server.MapPath("~/UploadFiles/");
                            filename_voice = model.Pointno + "_voice_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + filename_voice;
                            file1.SaveAs(path + filename_voice);
                            model.SketchVoice = filename_voice;
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "语音文件不合法！\r\n mp3文件,小于5m";
                            return View(model);
                        }
                    }

                }
            }

            if (fc["Sort"] != "")
            {
                model.Sort = Int32.Parse(fc["Sort"].ToString().Trim());
                var m = bll.GetPointBySort(model.Lineguid,id,Int32.Parse(fc["Sort"].ToString().Trim()));
                if (m != null)
                {
                    ViewBag.ErrorMsg = "序号冲突！";
                    return View(model);
                }
            }

            if (fc["optPointtype"] != "")
            {
                if (model.Pointtype == 1&& model.Sort !=1)
                {
                    ViewBag.ErrorMsg = "[检录点]类型序号必须为1！";
                    return View(model);
                }
                if (model.Pointtype == 1 && model.Status != 0)
                {
                    ViewBag.ErrorMsg = "[检录点]类型的状态必须选择[正常]！";
                    return View(model);
                }
            }


            try
            {
                bll.EditPoints(model);
                return RedirectToAction("points", new { id = model.Lineguid });
            }
            catch (ValidException ex)
            {
                this.ModelState.AddModelError(ex.Name, ex.Message);
                ViewBag.code = MD5Encrypt(id + "dx");

                if (model.Pointtype == 1)
                    ViewBag.code = "{\"linesid\":\"" + model.Lineguid + "\"}";
                else if (model.Pointtype == 2)
                    ViewBag.code = MD5Encrypt("csdxsuccessdx");

                List<SelectListItem> pointtype = new MemberBll().GetDict(8);
                List<SelectListItem> Status = new MemberBll().GetDict(14);
                foreach (SelectListItem r in Status)
                {
                    if (model.Status.ToString() == r.Value)
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

                foreach (SelectListItem r in pointtype)
                {
                    if (model.Pointtype.ToString() == r.Value)
                        ViewBag.pointtype += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.pointtype += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }
                return View(model);
            }

        }

        [HttpPost]
         public ActionResult ckeditorUpload(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string filename = "";
            string path = Server.MapPath("~/UploadFiles/");
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["upload"];
            if (file != null)
            {
                filename = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                if ((filename.ToUpper() == "PNG" || filename.ToUpper() == "JPG") && file.ContentLength / 1024 < 2000)
                {
                   
                    filename = Guid.NewGuid().ToString() + "." + filename;
                    file.SaveAs(path + filename);
                }
            }

            var imageUrl = Url.Content("~/UploadFiles/" + filename);
            var vMessage = string.Empty;

            var result = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + imageUrl + "\", \"" + vMessage + "\");</script></body></html>";
 


            return Content(result);


        }
        

        public ActionResult PointsCreate(string id)
        {
            var bll = new MatchBll();
            ViewBag.linesid = id;
            var lines = bll.GetLinesById(id);
            ViewBag.linesname = lines.Linename;
            List<SelectListItem> pointtype = new MemberBll().GetDict(8);
            List<SelectListItem> Status = new MemberBll().GetDict(14);
            foreach (SelectListItem r in Status)
            {
                ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }
            foreach (SelectListItem r in pointtype)
            {
                ViewBag.pointtype += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            //链接编号，目前写死0~10
            List<SelectListItem> linkno = new List<SelectListItem>();
            for (int i = 0; i < 11; i++)
            {
                SelectListItem tmp = new SelectListItem();
                tmp.Value = i.ToString();
                tmp.Text = i.ToString();
                linkno.Add(tmp);
            }
            foreach (SelectListItem r in linkno)
            {
                    ViewBag.optLinkno += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }
            return View();
        }


        private void SetBaseDataE(tblpointsView model)
        {         
            List<SelectListItem> pointtype = new MemberBll().GetDict(8);
            List<SelectListItem> Status = new MemberBll().GetDict(14);
            foreach (SelectListItem r in Status)
            {
                if (model.Status != null)
                {
                    if (model.Status.ToString() == r.Value.ToString())
                    {
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    }else
                    {
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                    }
                }
                
            }
            foreach (SelectListItem r in pointtype)
            {
                if (model.Pointtype != null)
                {
                    if (model.Pointtype.ToString() == r.Value.ToString())
                    {
                        ViewBag.pointtype += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    }
                    else
                    {
                        ViewBag.pointtype += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                    }
                }
                
            }
        }
 

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PointsCreate(string id, FormCollection fc)
        {
            var bll = new MatchBll();
            var model = new tblpoints();
            model.Pointid = Guid.NewGuid().ToString();
            model.Lineguid = fc["linesid"].ToString();
            model.Pointname = fc["Pointname"].ToString();
            //model.Content = fc["Content"].ToString().Trim();
            model.Pointno = fc["Pointno"].ToString();
            model.Pointaddress = fc["Pointaddress"].ToString();
            model.Pointtask = fc["Pointtask"].ToString();
            model.Pointout = fc["Pointout"].ToString();
            //model.Status = Int32.Parse(fc["optStatus"].ToString());
            //model.Pointtype = Int32.Parse(fc["optPointtype"].ToString());


            if (fc["optLinkno"] != "")
            {
                model.Linkno = fc["optLinkno"].ToString();
                //链接编号，目前写死0~10
                List<SelectListItem> linkno = new List<SelectListItem>();
                for (int i = 0; i < 11; i++)
                {
                    SelectListItem tmp = new SelectListItem();
                    tmp.Value = i.ToString();
                    tmp.Text = i.ToString();
                    linkno.Add(tmp);
                }

                foreach (SelectListItem r in linkno)
                {
                    if (model.Linkno == r.Value)
                        ViewBag.optLinkno += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.optLinkno += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

            }


            if (fc["optStatus"] != "")
            {
                model.Status = Int32.Parse(fc["optStatus"].ToString());
                List<SelectListItem> Status = new MemberBll().GetDict(14);
                foreach (SelectListItem r in Status)
                {
                    if (model.Status.ToString() == r.Value)
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }
            }

            if (fc["optPointtype"] != "")
            {
                    model.Pointtype = Int32.Parse(fc["optPointtype"].ToString().Trim());
                    List<SelectListItem> pointtype = new MemberBll().GetDict(8);
                    foreach (SelectListItem r in pointtype)
                    {
                        if (model.Pointtype.ToString() == r.Value)
                            ViewBag.pointtype += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                        else
                            ViewBag.pointtype += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                    }
             }

            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMsg = "线路编号丢失，请重新开始！";
                ViewBag.linesid = id;
                return View(model);
            }
                
            string filename_img = "";
            string filename_voice = "";
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["sketchmap"];
            if (file != null)
            {
                if (file.FileName != "")
                {
                    filename_img = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                    if ((filename_img.ToUpper() == "PNG" || filename_img.ToUpper() == "JPG") && file.ContentLength / 1024 < 2048)
                    {
                        string path = Server.MapPath("~/UploadFiles/");
                        filename_img = model.Pointno + "_image_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + filename_img;
                        file.SaveAs(path + filename_img);
                        model.Sketchmap = filename_img;
                    }
                    else
                    {
                        
                        ViewBag.ErrorMsg = "示意图文件不合法！\r\n png,jpg文件,小于2m";
                        ViewBag.linesid = id;
                        return View(model);
                    }
                }

            }

            HttpPostedFileBase file1 = files["sketchvoice"];
            if (file1 != null)
            {
                if (file1.FileName != "")
                {
                    filename_voice = file1.FileName.Substring(file1.FileName.LastIndexOf(".") + 1);
                    if ((filename_voice.ToUpper() == "MP3") && file1.ContentLength / 1024 < 5120)
                    {
                        string path = Server.MapPath("~/UploadFiles/");
                        filename_voice = model.Pointno + "_voice_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + filename_voice;
                        file1.SaveAs(path + filename_voice);
                        model.SketchVoice = filename_voice;
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "语音文件不合法！\r\n mp3文件,小于5m";
                        ViewBag.linesid = id;
                        return View(model);
                    }
                }

            }

            if (fc["Sort"] != "")
            {
                model.Sort = Int32.Parse(fc["Sort"].ToString().Trim());
                var m = bll.GetPointBySort(model.Lineguid, "", Int32.Parse(fc["Sort"].ToString().Trim()));
                if (m != null)
                {
                    ViewBag.ErrorMsg = "序号冲突！";
                    ViewBag.linesid = id;
                    return View(model);
                }
            }
            if (fc["optPointtype"] != "")
            {
                if (model.Pointtype == 1 && model.Sort != 1)
                {
                    ViewBag.ErrorMsg = "[检录点]类型序号必须为1！";
                    ViewBag.linesid = id;
                    return View(model);
                }
                if (model.Pointtype == 1 && model.Status != 0)
                {
                    ViewBag.ErrorMsg = "[检录点]类型的状态必须选择[正常]！";
                    ViewBag.linesid = id;
                    return View(model);
                }
            }
            try
            {
                bll.AddPoints(model);

                return RedirectToAction("points", new { id = id });
            }
            catch (ValidException ex)
            {
                this.ModelState.AddModelError(ex.Name, ex.Message);
                ViewBag.linesid = id;
                return View(model);
            }

            //return this.RefreshParent();
        }


        [HttpPost]
        public ActionResult PointDelete(List<string> ids, string linesid)
        {
            var bll = new MatchBll();

            try
            {
                bll.DeletePoint(ids);
            }
            catch (ValidException ex)
            {
                return Alert(ex.Message);
            }

            return RedirectToAction("points", new { id = linesid });
        }

        [HttpPost]
        public ActionResult LinesDelete(List<string> ids)
        {
            var bll = new MatchBll();

            try
            {
                bll.DeleteLines(ids);
            }
            catch (ValidException ex)
            {
                return Alert(ex.Message);
            }

            return RedirectToAction("lines");
        }

        public String MD5Encrypt(string code)
        {

            byte[] result = Encoding.Default.GetBytes(code.Trim());
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }

        public ActionResult Team(string optLines,string teamname,string company, int? pageIndex)
        {
            var teams = new List<tblteamsVew>();
            string matchid = base.UserInfo.Matchid;
            tblmatch Match = new MatchBll().GetMatchById(matchid);
            try
            {
                teams = new MatchBll().GetMatchTeams(matchid,optLines, teamname, company, pageIndex.GetValueOrDefault(1));
                List<SelectListItem> Status = new MemberBll().GetDict(5);
                ViewData["Status"] = Status;

                List<SelectListItem> lines = new MatchBll().GetlinesByMatchid(matchid);
                if (optLines != "")
                {
                    foreach (SelectListItem r in lines)
                    {
                        if (optLines == r.Value)
                            ViewBag.Lines += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                        else
                            ViewBag.Lines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                    }
                }
                else
                {
                    foreach (SelectListItem r in lines)
                    {
                        ViewBag.Lines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                    }
                }

            }
            catch (Exception e)
            {

            }
            return View(teams);
        }

        public ActionResult ScanInfo(string nickname, string teamname, string pointname, int? pageIndex)
        {
            string matchid = base.UserInfo.Matchid;
            tblmatch Match = new MatchBll().GetMatchById(matchid);

            var record = new MatchBll().GetScanInfo(matchid,nickname, teamname, pointname, pageIndex.GetValueOrDefault(1));
            return View(record);
        }

        [HttpPost]
        public string ExportExcel()
        {
            string filename = "";
            try
            {
                string matchid = base.UserInfo.Matchid;
                List<TeamUser> teamuser = new MatchBll().GetTeamsUsers(matchid);

                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");

                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 13));
                int icolIndex = 0;
                IRow headerRow = sheet.CreateRow(0);
                ICellStyle style = workbook.CreateCellStyle();
                //设置单元格的样式：水平对齐居中
                style.Alignment = HorizontalAlignment.CENTER;
                //新建一个字体样式对象
                IFont font = workbook.CreateFont();
                //设置字体加粗样式
                font.Boldweight = short.MinValue;
                font.FontName = "微软雅黑";
                font.FontHeightInPoints = 18;
                //使用SetFont方法将字体样式添加到单元格样式中 
                style.SetFont(font);
                //将新的样式赋给单元格

                ICell cell = headerRow.CreateCell(icolIndex);
                cell.SetCellValue("中国坐标·城市定向户外挑战赛队伍团体报名表");
                cell.CellStyle = style;


                ICellStyle HeadercellStyle = workbook.CreateCellStyle();
                HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                //字体
                NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
                headerfont.Boldweight = (short)FontBoldWeight.BOLD;
                HeadercellStyle.SetFont(headerfont);
                //用column name 作为列名
                headerRow = sheet.CreateRow(1);
                cell = headerRow.CreateCell(0);
                cell.SetCellValue("序号");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(1);
                cell.SetCellValue("线路号");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(2);
                cell.SetCellValue("队列号");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(3);
                cell.SetCellValue("队名");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(4);
                cell.SetCellValue("单位名称");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(5);
                cell.SetCellValue("队员编号");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(6);
                cell.SetCellValue("队员姓名");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(7);
                cell.SetCellValue("年龄");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(8);
                cell.SetCellValue("性别");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(9);
                cell.SetCellValue("身份证/护照");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(10);
                cell.SetCellValue("手机号码");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(11);
                cell.SetCellValue("是否健康");
                cell.CellStyle = HeadercellStyle;
                cell = headerRow.CreateCell(12);
                cell.SetCellValue("备注");
                cell.CellStyle = HeadercellStyle;

                sheet.SetColumnWidth(0, 4 * 256);
                sheet.SetColumnWidth(1, 6 * 256);
                sheet.SetColumnWidth(2, 6 * 256);
                sheet.SetColumnWidth(3, 23 * 256);
                sheet.SetColumnWidth(4, 23 * 256);
                sheet.SetColumnWidth(5, 8 * 256);
                sheet.SetColumnWidth(6, 11 * 256);
                sheet.SetColumnWidth(7, 6 * 256);
                sheet.SetColumnWidth(8, 6 * 256);
                sheet.SetColumnWidth(9, 20 * 256);
                sheet.SetColumnWidth(10, 20 * 256);
                sheet.SetColumnWidth(11, 8 * 256);
                sheet.SetColumnWidth(12, 15 * 256);

                ICellStyle cellStyle = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
                cellfont.Boldweight = (short)FontBoldWeight.NORMAL;
                cellStyle.SetFont(cellfont);


                ICellStyle cellStyle1 = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                cellStyle1.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                cellStyle1.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle1.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle1.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle1.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
               
                cellStyle1.FillPattern = FillPatternType.SOLID_FOREGROUND;
                cellStyle1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
                NPOI.SS.UserModel.IFont cellfont1 = workbook.CreateFont();
                cellfont1.Boldweight = (short)FontBoldWeight.NORMAL;
                cellStyle1.SetFont(cellfont1);

                //建立内容行
                int iRowIndex = 2;
                int iCellIndex = 0;
                int memberIndex = 1;
                int s = 0;
                try
                {
                    foreach (var data in teamuser)
                    {
                        IRow DataRow = sheet.CreateRow(iRowIndex);
                        iCellIndex = 0;
                        for (int i = 0; i < 13; i++)
                        {
                            ICell cell_ = DataRow.CreateCell(iCellIndex);
                           
                            switch (i)
                            {
                                case 0:
                                    cell_.SetCellValue(s.ToString());
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 1:
                                    cell_.SetCellValue(data.line_no);
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 2:
                                    cell_.SetCellValue(data.teamno);
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 3:
                                    cell_.SetCellValue(data.teamname);
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 4:
                                    cell_.SetCellValue(data.company);
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 5:
                                    if (memberIndex > 5) { memberIndex = 1; }
                                    cell_.SetCellValue(data.teamno + memberIndex.ToString());
                                    memberIndex++;
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 6:
                                    cell_.SetCellValue(data.nickname);
                                    if (data.user_no == "队长")
                                    {
                                        cell_.CellStyle = cellStyle1;
                                    }
                                    else
                                    {
                                        cell_.CellStyle = cellStyle;
                                    }
                                    break;
                                case 7:
                                    if (data.age!=null)
                                    cell_.SetCellValue(data.age.ToString());
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 8:
                                    cell_.SetCellValue(data.sex);
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 9:
                                    cell_.SetCellValue(data.cardno);
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 10:
                                    cell_.SetCellValue(data.mobile);
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 11:
                                    cell_.SetCellValue(data.isgood);
                                    cell_.CellStyle = cellStyle;
                                    break;
                                case 12:
                                    if (data.memo != null)
                                    cell_.SetCellValue(data.memo.ToString());
                                    cell_.CellStyle = cellStyle;
                                    break;
                                   
                            }
                            iCellIndex++;

                        }
                        s++;
                        iRowIndex++;

                    }
                }
                catch (Exception e)
                {

                }
               

                //自适应列宽度
                for (int i = 0; i < icolIndex; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                //写Excel
                filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "报名表.xls";
                string filepath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Data\\" + filename;
                FileStream file = new FileStream(filepath, FileMode.OpenOrCreate);
                workbook.Write(file);
                file.Flush();
                file.Close();
                
            }
            catch (Exception ex)
            {
            }
            return filename;
        }

        public ActionResult Download(string optIsdown, int? pageIndex)
        {
            string matchid = base.UserInfo.Matchid;
            List<SelectListItem> isdown_ = new List<SelectListItem>();
            isdown_.Add(new SelectListItem { Text = "已发放", Value = "1", Selected = true });
            isdown_.Add(new SelectListItem { Text = "已下载", Value = "2" });
            isdown_.Add(new SelectListItem { Text = "已删除", Value = "3" });
            ViewBag.Isdown = isdown_;

            if (optIsdown != "")
            {
                foreach (SelectListItem r in isdown_)
                {
                    if (optIsdown == r.Value)
                        ViewBag.Isdown += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.Isdown += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

            }
            var record = new MatchBll().GetMatchusers(matchid, optIsdown, pageIndex.GetValueOrDefault(1));
            return View(record);
        }

        [HttpPost]
        public string Download()
        {
            string matchid = base.UserInfo.Matchid;

            List<tblmatchusersView> o = new MatchBll().ExportUsers(matchid);
            string ExportField = "teamname,nickname,mobile,isdown";
            string[] fields = ExportField.Split(',');
            DataTable dt = new DataTable();
            List<SelectListItem> Status = new MemberBll().GetDict(5);

            for (int j = 0; j <= fields.Length - 1; j++)
            {
                dt.Columns.Add(fields[j]);
            }

            for (int i = 0; i < o.Count; i++)
            {

                DataRow dr = dt.NewRow();
                for (int j = 0; j <= fields.Length - 1; j++)
                {
                    switch (fields[j])
                    {
                        case "teamname":
                            dr[fields[j]] = o[i].teamname;
                            break;
                        case "nickname":
                            dr[fields[j]] = o[i].nickname;
                            break;
                        case "mobile":
                            dr[fields[j]] = o[i].mobile;
                            break;
                        case "isdown":
                            if (o[i].isdown == "1")
                            {
                                dr[fields[j]] = "已发放";
                            }
                            else if (o[i].isdown == "2")
                            {
                                dr[fields[j]] = "已下载";
                            }
                            else if (o[i].isdown == "3")
                            {
                                dr[fields[j]] = "已删除";
                            }
                            else
                            {
                                dr[fields[j]] = "未知";
                            }
                           
                            break;
                        default:
                            break;
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.Columns["teamname"].ColumnName = "队伍名称";
            dt.Columns["nickname"].ColumnName = "昵称";
            dt.Columns["mobile"].ColumnName = "电话";
            dt.Columns["isdown"].ColumnName = "状态";

            string file = "";
            try
            {
                file = GridToExcelByNPOI(dt, DateTime.Now.ToString("yyyyMMddHHmmss") + "_下载统计.xls");
            }
            catch
            {
                file = "";
            }

            return file;
        }

        private static string GridToExcelByNPOI(DataTable dt, string strExcelFileName)
        {
            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");

                ICellStyle HeadercellStyle = workbook.CreateCellStyle();
                HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                //字体
                NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
                headerfont.Boldweight = (short)FontBoldWeight.BOLD;
                HeadercellStyle.SetFont(headerfont);


                //用column name 作为列名
                int icolIndex = 0;
                IRow headerRow = sheet.CreateRow(0);
                foreach (DataColumn item in dt.Columns)
                {
                    ICell cell = headerRow.CreateCell(icolIndex);
                    cell.SetCellValue(item.ColumnName);
                    cell.CellStyle = HeadercellStyle;
                    icolIndex++;
                }

                ICellStyle cellStyle = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;


                NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
                cellfont.Boldweight = (short)FontBoldWeight.NORMAL;
                cellStyle.SetFont(cellfont);

                //建立内容行
                int iRowIndex = 1;
                int iCellIndex = 0;
                foreach (DataRow Rowitem in dt.Rows)
                {
                    IRow DataRow = sheet.CreateRow(iRowIndex);
                    foreach (DataColumn Colitem in dt.Columns)
                    {

                        ICell cell = DataRow.CreateCell(iCellIndex);
                        cell.SetCellValue(Rowitem[Colitem].ToString());
                        cell.CellStyle = cellStyle;
                        iCellIndex++;
                    }
                    iCellIndex = 0;
                    iRowIndex++;
                }

                //自适应列宽度
                for (int i = 0; i < icolIndex; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                //写Excel
                string filepath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Data\\" + strExcelFileName;
                FileStream file = new FileStream(filepath, FileMode.OpenOrCreate);
                workbook.Write(file);
                file.Flush();
                file.Close();
                return strExcelFileName;
            }
            catch (Exception ex)
            {
                return "";
            }

        }


        public ActionResult ScanTeam(string teamno,string teamname,string optEvent, int? pageIndex)
        {
            string matchid = base.UserInfo.Matchid;

            List<SelectListItem> event_ = new List<SelectListItem>();
            event_.Add(new SelectListItem { Text = "已参赛", Value = "1", Selected = true });
            event_.Add(new SelectListItem { Text = "已推送", Value = "2" });
            event_.Add(new SelectListItem { Text = "已完赛", Value = "3" });

            if (!string.IsNullOrEmpty(optEvent))
            {
                foreach (SelectListItem r in event_)
                {
                    if (optEvent == r.Value)
                        ViewBag.Event += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.Event += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

            }
            else
            {
                foreach (SelectListItem r in event_)
                {
                        ViewBag.Event += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }
            }
            var record = new MatchBll().GetTeamScan(matchid,teamno,teamname, optEvent, pageIndex.GetValueOrDefault(1));
            return View(record);
        }

        public ActionResult TeamUserScan(string teamid, int? pageIndex)
        {
            var teamuser = new MatchBll().GetTeamUserScan(teamid, pageIndex.GetValueOrDefault(1));
            return View(teamuser);
        }

        public ActionResult RecordEdit(string id)
        {
            var model = new TeamBll().GetTeamById(id);
            List<SelectListItem> points = new MatchBll().GetPointsByLinesid(model.Linesid);
            ViewBag.points += "<option value='' selected></option>";
            foreach (SelectListItem r in points)
            {
                if (model.record == r.Value)
                    ViewBag.points += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                else
                    ViewBag.points += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RecordEdit(string id, FormCollection fc)
        {
            var model = new TeamBll().GetTeamById(id);
            string record = fc["optPoint"];
            if (string.IsNullOrEmpty(record))
            {
                model.record = "0";
            }
            else
            {
                model.record = record;
            }
            try
            {
                var ret = new TeamBll().EditTeam(model);
            }
            catch (ValidException ex)
            {
                List<SelectListItem> points = new MatchBll().GetPointsByLinesid(model.Linesid);
                ViewBag.points += "<option value='' selected></option>";
                foreach (SelectListItem r in points)
                {
                    if (model.record == r.Value)
                        ViewBag.points += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.points += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }
                return View(model);
            }

            return this.RefreshParent();

        }

        public ActionResult PScoreDetail(string teamid,string teamname,string teamno, string lineno, int? pageIndex)
        {
            string matchid = base.UserInfo.Matchid;
            var results = new List<tblresultDetailView>();
            try
            {
                //List<SelectListItem> Status = new MemberBll().GetDict(3);
                //ViewData["PStatus"] = Status;
                results = new MatchBll().GetResultDetail(matchid, teamid, teamname, teamno,lineno, pageIndex.GetValueOrDefault(1));

                foreach (var item in results)
                {
                    if (string.IsNullOrEmpty(item.timeline))
                        item.timeline = "0秒";
                    else
                    {
                        TimeSpan ts = TimeSpan.FromHours(double.Parse(item.timeline));
                        item.timeline = string.Format("{0}时{1}分{2}秒{3}毫秒", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                    }
                }
            }
            catch (Exception e)
            {

            }
            return View(results);
        }

        public ActionResult Score(string teamno,string teamname, string lineno, int? pageIndex)
        {
            string matchid = base.UserInfo.Matchid;
            var results = new List<tblresultView>();
            try
            {
                List<SelectListItem> Status = new MemberBll().GetDict(3);
                ViewData["Status"] = Status;
                results = new MatchBll().GetResult(matchid, teamno,teamname, lineno, pageIndex.GetValueOrDefault(1));

                foreach (var item in results)
                {
                    if (string.IsNullOrEmpty(item.timeline))
                        item.timeline = "0秒";
                    else
                    {
                        TimeSpan ts = TimeSpan.FromHours(double.Parse(item.timeline));
                        item.timeline = string.Format("{0}时{1}分{2}秒{3}毫秒", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                    }
                }
            }
            catch (Exception e)
            {

            }
            return View(results);
        }

        public ActionResult ScoreEdit(string matchid, string teamid)
        {
            tblresult1 result = new tblresult1();
            var bll = new MatchBll();
            var model = bll.GetScore(matchid, teamid.Replace("?", ""));
            if (model != null)
            {
                result.match_id = model.match_id;
                result.teamid = model.teamid;
                if (model.starttime != null)
                    result.starttime = model.starttime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                if (model.settime != null)
                    result.settime = model.settime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            return View(result);
        }

        [HttpPost]
        public ActionResult ScoreEdit(FormCollection fc)
        {
            tblresult model = null;
            tblresult1 result = new tblresult1();

            try
            {
                var bll = new MatchBll();

                model = bll.GetScore(fc["matchid"], fc["teamid"]);
                if (model != null)
                {
                    result.match_id = model.match_id;
                    result.teamid = model.teamid;
                    if (model.starttime != null)
                        result.starttime = model.starttime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    if (model.settime != null)
                        result.settime = model.settime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    DateTime starttime;
                    DateTime settime;
                    if (fc["starttime"] != "")
                    {
                        if (DateTime.TryParse(fc["starttime"].ToString(), out starttime))
                        {
                            model.starttime = Convert.ToDateTime(fc["starttime"].ToString());
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "开始时间不合法！";
                            return View(result);
                        }
                    }
                    if (fc["settime"] != "")
                    {
                        if (DateTime.TryParse(fc["settime"].ToString(), out settime))
                        {
                            model.settime = Convert.ToDateTime(fc["settime"].ToString());
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "结束时间不合法！";
                            return View(result);
                        }
                    }

                    if (model.settime.Value.CompareTo(model.starttime.Value) < 0)
                    {
                        ViewBag.ErrorMsg = "结束时间不能早于开始时间！";
                        return View(result);
                    }

                    model.timeline = DateDiff(model.settime.Value, model.starttime.Value).ToString();
                    model.userno = "match";
                    model.Createtime = DateTime.Now;
                    bll.EditScore(model);

                }

            }
            catch (ValidException ex)
            {
                ViewBag.ErrorMsg = "成绩编辑异常！";
                this.ModelState.AddModelError(ex.Name, ex.Message);
                return View(result);
            }

            return this.RefreshParent();
        }



        private string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts = DateTime1 - DateTime2;

            string dDays = ts.TotalHours.ToString("f9");

            return dDays;
        }

        //public FileResult ScoreExp1(string teamname, string lineno)
        //{
        //    string matchid = base.UserInfo.Matchid;
        //    var results = new List<tblresultView>();
        //    try
        //    {
        //        results = new MatchBll().GetResult(matchid, teamname, lineno).OrderBy(p => p.line_no).ThenBy(p => p.timeline).ToList();


        //        HSSFWorkbook workbook = new HSSFWorkbook();
        //        ISheet sheet = workbook.CreateSheet("Sheet1");

        //        sheet.SetColumnWidth(0, 100 * 256);

        //        ICellStyle cellStyle = workbook.CreateCellStyle();
        //        //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
        //        cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
        //        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.NONE;
        //        cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.NONE;
        //        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.NONE;
        //        cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.NONE;
        //        cellStyle.Alignment = HorizontalAlignment.LEFT;

        //        ICellStyle cellStyle1 = workbook.CreateCellStyle();
        //        //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
        //        cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
        //        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.NONE;
        //        cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.NONE;
        //        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.NONE;
        //        cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.NONE;
        //        cellStyle.Alignment = HorizontalAlignment.LEFT;


        //        NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
        //        cellfont.Boldweight = (short)FontBoldWeight.NORMAL;
        //        cellStyle.SetFont(cellfont);

        //        string score = "";
        //        string lineno_ = "";
        //        int i = 0;
        //        if (lineno != "")
        //        {
        //            foreach (var data in results)
        //            {
        //                if (data.timeline != null)
        //                {
        //                    IRow DataRow = sheet.CreateRow(i);
        //                    ICell cell_ = DataRow.CreateCell(0);
        //                    TimeSpan ts = TimeSpan.FromHours(double.Parse(data.timeline));
        //                    score = string.Format("{0}:{1}:{2}:{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        //                    string value = i.ToString() + ".   " + data.teamno + Convert.ToChar(10) + "             " + score;
        //                    cell_.SetCellValue(value);
        //                    cell_.CellStyle = cellStyle;
        //                    i++;
        //                }
        //            }
        //        }else
        //        {
        //            foreach (var data in results)
        //            {
        //                if (data.line_no != lineno_)
        //                    i = 0;
        //                IRow DataRow = sheet.CreateRow(i);
        //                ICell cell_ = DataRow.CreateCell(0);
        //                if (data.timeline != null)
        //                {
        //                    TimeSpan ts = TimeSpan.FromHours(double.Parse(data.timeline));
        //                    score = string.Format("{0}:{1}:{2}:{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        //                }
        //                string value = i.ToString() + ".   " + data.teamno + Convert.ToChar(10) + "             " + score;
        //                cell_.SetCellValue(value);
        //                cell_.CellStyle = cellStyle;
        //                lineno_ = data.line_no;
        //                i++;
        //            }

        //        }


        //        //写Excel
        //        string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "成绩统计.xls";
        //        string filepath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Data\\" + filename;
        //        FileStream file = new FileStream(filepath, FileMode.OpenOrCreate);
        //        workbook.Write(file);
        //        file.Flush();
        //        file.Close();

        //        var fileName = Server.MapPath("~/Data/" + filename);
        //        return File(fileName, "application/ms-excel", fileName);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        public FileResult ScoreExp(string teamname, string lineno)
        {
            string matchid = base.UserInfo.Matchid;
            var results = new List<tblresultView>();
            try
            {
                results = new MatchBll().GetResult(matchid, teamname, lineno);

                DataTable dt = new DataTable();
                dt.Columns.Add("名次");
                dt.Columns.Add("赛事名称");
                dt.Columns.Add("路线");
                dt.Columns.Add("队伍编号");
                dt.Columns.Add("队伍名称");               
                dt.Columns.Add("出发时间");
                dt.Columns.Add("到达时间");
                dt.Columns.Add("时间间隔");
                dt.Columns.Add("操作人");
                //dt.Columns.Add("状态");
                int i = 1;
                foreach (var item in results)
                {
                    if (item.starttime != null && item.settime != null)
                    {
                        DataRow d = dt.NewRow();
                        d["名次"] = i.ToString();
                        d["赛事名称"] = item.match_name;
                        d["路线"] = item.line_no;
                        d["队伍名称"] = item.teamname;
                        d["队伍编号"] = item.teamno;
                        if (item.starttime != null)
                        {
                            d["出发时间"] = item.starttime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        }
                        if (item.settime != null)
                        {
                            d["到达时间"] = item.settime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        }

                        d["操作人"] = item.userno;

                        if (string.IsNullOrEmpty(item.timeline))

                            d["时间间隔"] = "";
                        else
                        {
                            TimeSpan ts = TimeSpan.FromHours(double.Parse(item.timeline));
                            d["时间间隔"] = string.Format("{0}:{1}:{2}.{3}", ts.Hours.ToString().PadLeft(2, '0'), ts.Minutes.ToString().PadLeft(2, '0'), ts.Seconds.ToString().PadLeft(2,'0'), ts.Milliseconds.ToString().PadRight(3,'0'));
                        }

                        dt.Rows.Add(d);
                        i++;
                    }                 
                }

                foreach (var item in results)
                {
                    if (item.starttime == null || item.settime == null)
                    {
                        DataRow d = dt.NewRow();
                        d["名次"] = i.ToString();
                        d["赛事名称"] = item.match_name;
                        d["路线"] = item.line_no;
                        d["队伍名称"] = item.teamname;
                        d["队伍编号"] = item.teamno;
                        if (item.starttime != null)
                        {
                            d["出发时间"] = item.starttime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        }
                        if (item.settime != null)
                        {
                            d["到达时间"] = item.settime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        }

                        d["操作人"] = item.userno;

                        if (string.IsNullOrEmpty(item.timeline))

                            d["时间间隔"] = "";
                        else
                        {
                            TimeSpan ts = TimeSpan.FromHours(double.Parse(item.timeline));
                            d["时间间隔"] = string.Format("{0}:{1}:{2}.{3}", ts.Hours.ToString().PadLeft(2, '0'), ts.Minutes.ToString().PadLeft(2, '0'), ts.Seconds.ToString().PadLeft(2, '0'), ts.Milliseconds.ToString().PadRight(3, '0'));
                        }

                        dt.Rows.Add(d);
                        i++;
                    }
                }

                string xls = GridToExcelByNPOI(dt, Guid.NewGuid().ToString() + ".xls");
                var fileName = Server.MapPath("~/Data/" + xls);
                return File(fileName, "application/ms-excel", fileName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult Statistics()
        {
            string matchid = base.UserInfo.Matchid;
            Statistics Statistics = null; 
           
            try
            {
                Statistics = new MatchBll().GetStatistics(matchid);
            }
            catch (Exception e)
            {

            }
            return View(Statistics);
        }


        public ActionResult Ranking(string optLines, string teamname, string nickname, int? pageIndex)
        {
            string matchid = base.UserInfo.Matchid;
            var rank = new List<ranking>();
            try
            {
                var lines = new MatchBll().GetlinesByMatchid(matchid);
                if (string.IsNullOrEmpty(optLines))
                {
                    foreach (SelectListItem r in lines)
                    {
                        ViewBag.optLines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                    }
                }
                else
                {
                    foreach (SelectListItem r in lines)
                    {
                        if (optLines == r.Value)
                            ViewBag.optLines += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                        else
                            ViewBag.optLines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                    }

                }
                rank = new MatchBll().GetRanking(matchid, optLines, teamname, nickname, pageIndex.GetValueOrDefault(1));
            }
            catch (Exception e)
            {

            }
            return View(rank);
        }


        public ActionResult PointsView(string id)
        {
            ViewBag.linesid = id;
            return View();
        }

        public ActionResult PointsDownload(string id)
        {
            ViewBag.linesid = id;
            return View();
        }

        [HttpPost]
        public ActionResult GetPointsList(string linesid)
        {
            var bll = new MatchBll();
            List<tblpointsView> points = new MatchBll().GetPointsList(linesid);
            return Json(points, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPointsListMD5(string linesid)
        {
            var bll = new MatchBll();
            List<tblpointsView> points = new MatchBll().GetPointsList(linesid);
            foreach(var p in points)
            {
                if (p.Pointtype == 1)
                    p.Code = "{\"linesid\":\"" + p.Lineguid + "\"}";
                else if (p.Pointtype == 2)
                    p.Code = MD5Encrypt("csdxsuccessdx");
                else
                    p.Code = MD5Encrypt(p.Pointid + "dx");
            }
            return Json(points, JsonRequestBehavior.AllowGet);
        }

        public FileResult GetNoCheckTeamResult()
        {
            string matchid = base.UserInfo.Matchid;
            var results = new List<tblteamsVew>();
            try
            {
                results = new MatchBll().GetNoCheckTeam(matchid);

                DataTable dt = new DataTable();
                dt.Columns.Add("线路名称");
                dt.Columns.Add("队伍名称");
                dt.Columns.Add("队伍编号");
                int i = 1;
                foreach (var item in results)
                {
                    DataRow d = dt.NewRow();
                    d["线路名称"] = item.Linesname;
                    d["队伍名称"] = item.Teamname;
                    d["队伍编号"] = item.Teamno;
                    dt.Rows.Add(d);
                }

                string xls = GridToExcelByNPOI(dt, Guid.NewGuid().ToString() + "未检录队伍详情.xls");
                var fileName = Server.MapPath("~/Data/" + xls);
                return File(fileName, "application/ms-excel", fileName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public FileResult GetFinishTeamResult()
        {
            string matchid = base.UserInfo.Matchid;
            //var results = new List<tblteamsVew>();
            try
            {
                var results = new MatchBll().GetFinishTeam(matchid);

                DataTable dt = new DataTable();
                dt.Columns.Add("线路名称");
                dt.Columns.Add("队伍名称");
                dt.Columns.Add("队伍编号");
                int i = 1;
                foreach (var item in results)
                {
                    DataRow d = dt.NewRow();
                    d["线路名称"] = item.Linesname;
                    d["队伍名称"] = item.Teamname;
                    d["队伍编号"] = item.Teamno;
                    dt.Rows.Add(d);
                }

                string xls = GridToExcelByNPOI(dt, Guid.NewGuid().ToString() + "已完成队伍详情.xls");
                var fileName = Server.MapPath("~/Data/" + xls);
                return File(fileName, "application/ms-excel", fileName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
