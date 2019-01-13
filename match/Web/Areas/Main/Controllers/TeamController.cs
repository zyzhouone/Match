using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model;
using Utls;
using BLL;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.SS.Util;
using Web.Areas.Main.Models;
using Web.Controls;
using System.Web.Script.Serialization;
using System.Threading;
using System.Web.Mvc.Async;

namespace Web.Areas.Main.Controllers
{
    public class TeamController : BaseController
    {
        //
        // GET: /Main/Team/  pangs d  

        public ActionResult Index(string optMatch, string optLine, string optLines, string teamno, string teamname, int? pageIndex)
        {
            var teams = new List<tblteamsVew>();
            string matchid = base.UserInfo.Matchid;
            tblmatch Match = new MatchBll().GetMatchById(matchid);
            try
            {
                teams = new TeamBll().GetTeams(matchid, optLine, optLines, teamno, teamname, pageIndex.GetValueOrDefault(1));
                List<SelectListItem> Status = new MemberBll().GetDict(5);
                ViewData["Status"] = Status;

                if (optMatch != null)
                {
                    //获取赛事列表

                    ViewBag.Match += "<option value='" + Match.Match_id + "' selected>" + Match.Match_name + "</option>";

                    //获取line列表
                    List<SelectListItem> line = new MatchBll().GetlineList(optMatch);

                    if (optLine != "")
                    {
                        foreach (SelectListItem r in line)
                        {
                            if (optLine == r.Value)
                                ViewBag.Line += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                            else
                                ViewBag.Line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                        }

                        //获取lines列表
                        List<SelectListItem> lines = new MatchBll().GetlinesList(optLine);
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
                    else
                    {

                        foreach (SelectListItem r in line)
                        {
                            ViewBag.Line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                        }
                    }
                }
                else
                {

                    //获取赛事列表
                    ViewBag.Match += "<option value='" + Match.Match_id + "'>" + Match.Match_name + "</option>";
                }



            }
            catch (Exception e)
            {

            }
            return View(teams);
        }
        /// <summary>
        /// zzy 218-12-30
        /// 队伍组合
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        public ActionResult TeamGroup(string optMatch, string optLine, string optLines, string optAge, string teamname, string optType = "未组合")
        {
            var teams = new List<tblteamsGroupVew>();
            string matchid = base.UserInfo.Matchid;
            tblmatch Match = new MatchBll().GetMatchById(matchid);
            try
            {
                if (!string.IsNullOrEmpty(optMatch))//默认不查询
                {
                    teams = new TeamBll().GetOnePeoperTeams(optType, matchid, optLines, optAge, teamname);
                }
                List<SelectListItem> Status = new MemberBll().GetDict(5);
                ViewData["Status"] = Status;

                List<SelectListItem> Sexy = new MemberBll().GetDict(1);
                ViewData["Sexy"] = Sexy;

                ViewData["TeamType"] = optType;

                if (optType == "未组合")
                {
                    ViewBag.Type += "<option value='未组合' selected>未组合</option>";
                    ViewBag.Type += "<option value='已组合'>已组合</option>";
                }
                else
                {
                    ViewBag.Type += "<option value='未组合'>未组合</option>";
                    ViewBag.Type += "<option value='已组合' selected>已组合</option>";
                }

                List<string> agelist = new List<string>();
                agelist.Add("00后");
                agelist.Add("90后");
                agelist.Add("80后");
                agelist.Add("70后");
                agelist.Add("60后");
                foreach (string age in agelist)
                {
                    if (optAge == age)
                        ViewBag.Age += "<option value='" + age + "' selected>" + age + "</option>";
                    else
                        ViewBag.Age += "<option value='" + age + "'>" + age + "</option>";
                }

                if (optMatch != null)
                {

                    //获取赛事列表

                    ViewBag.Match += "<option value='" + Match.Match_id + "' selected>" + Match.Match_name + "</option>";



                    //获取line列表
                    List<SelectListItem> line = new MatchBll().GetlineList(matchid);
                    //List<SelectListItem> line = new TeamBll().GetTeamlineList(optType, matchid, optLines, optAge, teamname);
                    if (!string.IsNullOrEmpty(optLine))
                    {
                        foreach (SelectListItem r in line)
                        {
                            if (optLine == r.Value)
                                ViewBag.Line += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                            else
                                ViewBag.Line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                        }
                        //获取lines列表
                        List<SelectListItem> lines = new MatchBll().GetlinesList(optLine);
                        if (!string.IsNullOrEmpty(optLines))
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
                    else
                    {

                        foreach (SelectListItem r in line)
                        {
                            ViewBag.Line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                        }
                    }
                }
                else
                {
                    ViewBag.Match += "<option value='" + Match.Match_id + "'>" + Match.Match_name + "</option>";

                }


                ViewBag.ComMatch += "<option value='" + Match.Match_id + "'>" + Match.Match_name + "</option>";
                //选择线路
                //List<SelectListItem> comline = new MatchBll().GetlineList(matchid);
                //foreach (SelectListItem r in comline)
                //{
                //    ViewBag.comLine += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                //}

                //List<SelectListItem> comLines = new MatchBll().GetlinesList(comline[0].Value);
                //foreach (SelectListItem r in comLines)
                //{
                //    ViewBag.comLines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                //}

            }
            catch (Exception e)
            {

            }
            return View(teams);
        }

        /// <summary>
        /// 获取年龄段
        /// zzy 2018-12-31
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCombineAge()
        {

            string matchid = base.UserInfo.Matchid;
            var teams = new TeamBll().GetOnePeoperTeams("未组合", matchid, null, null, null);



            JsonResult jr = new JsonResult();
            UtilsResponseModel response = new UtilsResponseModel();
            List<string> agelist = new List<string>();
            foreach (var combine in teams)
            {
                if (agelist.Where(m => m == combine.AgeBetween).Count() <= 0)
                {
                    agelist.Add(combine.AgeBetween);
                }
            }
            agelist = agelist.OrderBy(m => m).ToList();
            response.agelist = agelist;
            response.iResultCode = 0;
            jr.Data = response;
            return jr;
        }


        /// <summary>
        /// zzy 218-12-30
        /// 组合计算男女比例
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CombineUtils(string CombineListJson)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CombineListModel model = serializer.Deserialize<CombineListModel>(CombineListJson);


            JsonResult jr = new JsonResult();
            UtilsResponseModel response = new UtilsResponseModel();
            List<string> agelist = new List<string>();
            foreach (CombineModel combine in model.list)
            {
                if (agelist.Where(m => m == combine.age).Count() <= 0)
                {
                    agelist.Add(combine.age);
                }
            }
            agelist = agelist.OrderBy(m => m).ToList();
            response.agelist = agelist;

            Utils utils = new Utils();
            Dictionary<int, List<string>> teamResult = utils.CombineUtils(model);
            Dictionary<int, List<string>> result = utils.CombineUtils(model, false);
            List<TeamGroupModel> teamList = new List<TeamGroupModel>();
            for (int i = 0; i < result.Count; i++)
            {
                TeamGroupModel teamModel = new TeamGroupModel();
                foreach (var team in result[i])
                {
                    teamModel.teamids += team + ",";
                }
                teamModel.teamids = teamModel.teamids.Remove(teamModel.teamids.Length - 1, 1);
                teamList.Add(teamModel);
            }
            response.teamList = teamList;
            if (teamResult == null || teamResult.Count == 0)
            {
                response.iResultCode = 0;
            }
            else
            {              
                response.iResultCode = result.Count;
            }
            jr.Data = response;
            return jr;
        }


        /// <summary>
        /// zzy 218-12-31
        /// 自动组合计算男女比例
        /// </summary>
        [HttpPost]
        public JsonResult CombineAutoUtils(string age)
        {

            string matchid = base.UserInfo.Matchid;
            var teams = new TeamBll().GetOnePeoperTeams("未组合", matchid, "", age+"后", "");

            List<CombineModel> list = new List<CombineModel>();
            if (teams != null && teams.Count > 0)
            {
                foreach (var t in teams)
                {
                    //if (t.AgeBetween == age)
                   // {
                        CombineModel c = new CombineModel();
                        c.age = t.Age.ToString();
                        c.sexy = t.Sexy.ToString();
                        c.teamid = t.teamid;
                        list.Add(c);
                  //  }
                }
            }
            CombineListModel model = new CombineListModel();
            model.list = list;


            JsonResult jr = new JsonResult();
            UtilsResponseModel response = new UtilsResponseModel();
            List<string> agelist = new List<string>();
            foreach (CombineModel combine in model.list)
            {
                if (agelist.Where(m => m == combine.age).Count() <= 0)
                {
                    agelist.Add(combine.age);
                }
            }
            agelist = agelist.OrderBy(m => m).ToList();
            response.agelist = agelist;

            if (model.list.Count >= 5)
            {
                Utils utils = new Utils();
                Dictionary<int, List<string>> teamResult = utils.CombineUtils(model);
                if (teamResult == null || teamResult.Count == 0)
                {
                    response.iResultCode = 0;
                    response.leftNum = 0;
                }
                else
                {

                    Dictionary<int, List<string>> result = utils.CombineUtils(model, false);
                    List<TeamGroupModel> teamList = new List<TeamGroupModel>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        TeamGroupModel teamModel = new TeamGroupModel();
                        foreach (var team in result[i])
                        {
                            teamModel.teamids += team + ",";
                        }
                        teamModel.teamids = teamModel.teamids.Remove(teamModel.teamids.Length - 1, 1);
                        teamList.Add(teamModel);
                    }
                    response.iResultCode = result.Count;
                    response.teamList = teamList;
                    response.leftNum = model.list.Count % 5;

                }
            }
            else
            {
                //队伍数小于5
                response.iResultCode = -1;
                response.leftNum = 0;
            }
            jr.Data = response;
            return jr;
        }


        /// <summary>
        /// zzy 218-12-30
        /// 队伍组合
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TeamsConBine(string CombineListJson, string matchid, string lineid, string linesid, int count)
        {
            var bll = new TeamBll();
            JsonResult jr = new JsonResult();
            CombineResponseModel response = new CombineResponseModel();
            try
            {
                // JavaScriptSerializer serializer = new JavaScriptSerializer();
                // CombineListModel model = serializer.Deserialize<CombineListModel>(CombineListJson);


                // Utils utils = new Utils();
                // Dictionary<int, List<string>> result = utils.CombineUtils(model,false);
                // //组合数量
                //// int iNum = count > result.Count ? result.Count : count;

                // for (int i = 0; i < result.Count; i++)
                // {
                //     string teamids = "";
                //     foreach (var team in result[i])
                //     {
                //         teamids += team + ",";
                //     }
                //     teamids = teamids.Remove(teamids.Length - 1, 1);
                //     string msg = bll.TeamsConBine(teamids, linesid, lineid);
                //     if (!string.IsNullOrEmpty(msg))
                //     {
                //         strMsg += msg + "\r\n";
                //     }
                //     iExcuteNum += 1;
                // }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                TeamGroupModel model = serializer.Deserialize<TeamGroupModel>(CombineListJson);
               // string msg = bll.TeamsConBine(model.teamids, linesid, lineid);
                string[] teamids = model.teamids.Split(',');
                string msg = bll.TeamsConBine2(teamids[0],teamids[1],teamids[2],teamids[3],teamids[4], linesid, lineid);
                if (!string.IsNullOrEmpty(msg))
                {
                    response.iResultCode = -1;
                    response.strMsg = msg;
                }
                else
                {
                    response.iResultCode = 0;
                }
            }
            catch (ValidException ex)
            {
                response.iResultCode = -1;
                response.strMsg = ex.Message; ;
            }
            finally
            {

                response.iCount = count + 1;
            }
            jr.Data = response;
            return jr;
        }

        /// <summary>
        /// zzy 218-12-30
        /// 队伍解散
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        public string TeamsComBineUndo(string batchnos)
        {
            var bll = new TeamBll();
            var strMsg = "";
            try
            {
                string[] batchnoList = batchnos.Split(',');
                foreach (string batchno in batchnoList)
                {
                    if (!string.IsNullOrEmpty(batchno))
                    {
                        string msg = bll.TeamsComBineUndo(batchno);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            strMsg += msg + "\r\n";
                        }
                    }
                }

            }
            catch (ValidException ex)
            {
                strMsg = ex.Message;
            }

            return strMsg;
        }

        /// <summary>
        /// zzy 218-12-30
        /// 队伍队员列表
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        public ActionResult TeamCombineUsers(string matchid, string teamid)
        {
            if (teamid != null)
                teamid = teamid.Replace("?", "");
            var teamuser = new TeamBll().getTeamUsers(matchid, teamid);
            return View(teamuser);
        }

        /// <summary>
        /// zzy 218-12-30
        /// 替换队员
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        public ActionResult setplayer(string matchuid)
        {
            ViewBag.mid = matchuid;
            return View();
        }
        /// <summary>
        /// zzy 218-12-30
        /// 替换队员
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult setplayer(string mobile, string mid)
        {
            TeamBll bll = new TeamBll();
            //int res = bll.Replayer(mobile, mid);

            //if (res > 0)
            //    return this.RefreshParent();
            //else if (res == -1)
            //    ViewBag.err = "更换队员已经参加了比赛";
            //else if (res == -2)
            //    ViewBag.err = "你不是队长，没有权限更换";
            //else if (res == -3)
            //    ViewBag.err = "替换的队员不存在";
            //else if (res == -4)
            //    ViewBag.err = "输入的手机号没有注册或者信息不完善";
            //else if (res == -9)
            //    ViewBag.err = "年龄不在16~60之间";
            string res = bll.Replayer(mobile, mid);

            if (string.IsNullOrEmpty(res))
                return this.RefreshParent();
            else
                ViewBag.err = res;

            ViewBag.mid = mid;
            return View();
        }

        /// <summary>
        /// zzy 2018-12-31
        /// 替换队员根据手机获取名字
        /// </summary>
        /// <param name="Matchuserid"></param>
        /// <returns></returns>
        public string GetUserByMobile(string mobile)
        {
            var bll = new TeamBll();
            var user = bll.GetUserByMobile(mobile);

            if (user != null)
            {
                string sexy = user.Sexy == 1 ? "男" : "女";
                return user.Nickname + ";" + sexy + ";" + user.Age.ToString();
            }
            else
                return ";;";
        }

        /// <summary>
        /// zzy 218-12-30
        /// 设置队长
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        [HttpPost]
        public string setteamer(string matchuid)
        {
            var script = "";
            TeamBll bll = new TeamBll();
            int res = bll.ReLeader(matchuid);

            if (res == -1)
                script = "{\"msg\":\"已经是队长，不能设置\",\"code\":-1}";
            else
                script = "{\"msg\":\"\",\"code\":0}";

            return script;
        }

        /// <summary>
        /// zzy 218-12-30
        /// 编辑队员信息
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPlayer(string Matchuserid, FormCollection fc)
        {
            var bll = new MatchBll();
            var model = bll.getMatchUserByID(Matchuserid);
            model.Nickname = fc["Nickname"].ToString();
            model.Mobile = fc["Mobile"].ToString();

            try
            {
                bll.EditPlayer(model);
            }
            catch (ValidException ex)
            {
                this.ModelState.AddModelError(ex.Name, ex.Message);
                return View(model);
            }

            return this.RefreshParent();
        }
        /// <summary>
        /// zzy 2018-12-30
        /// </summary>
        /// <param name="matchuid"></param>
        /// <returns></returns>
        public ActionResult EditPlayer(string matchuid)
        {
            var matchuser = new MatchBll().getMatchUserByID(matchuid);
            return View(matchuser);
        }


        public ActionResult TeamDistribution(string optMatch, string optLine, string optLines, string teamno, string teamname, int? pageIndex)
        {
            var teams = new List<tblteamsVew>();
            string matchid = base.UserInfo.Matchid;
            tblmatch Match = new MatchBll().GetMatchById(matchid);
            try
            {
                teams = new TeamBll().GetTeams(matchid, optLine, optLines, teamno, teamname, pageIndex.GetValueOrDefault(1));
                List<SelectListItem> Status = new MemberBll().GetDict(5);
                ViewData["Status"] = Status;

                if (optMatch != null)
                {
                    //获取赛事列表

                    ViewBag.Match += "<option value='" + Match.Match_id + "' selected>" + Match.Match_name + "</option>";

                    //获取line列表
                    List<SelectListItem> line = new MatchBll().GetlineList(optMatch);

                    if (optLine != "")
                    {
                        foreach (SelectListItem r in line)
                        {
                            if (optLine == r.Value)
                                ViewBag.Line += "<option value='" + r.Value.ToString() + "' selected>" + r.Text.ToString() + "</option>";
                            else
                                ViewBag.Line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                        }

                        //获取lines列表
                        List<SelectListItem> lines = new MatchBll().GetlinesList(optLine);
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
                    else
                    {

                        foreach (SelectListItem r in line)
                        {
                            ViewBag.Line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                        }
                    }
                }
                else
                {

                    //获取赛事列表
                    ViewBag.Match += "<option value='" + Match.Match_id + "'>" + Match.Match_name + "</option>";
                }



            }
            catch (Exception e)
            {

            }
            return View(teams);
        }

        [HttpPost]
        public string ExpToExcel(string matchname, string teamname, string status)
        {
            List<tblteamsVew> o = new TeamBll().ExportTeams(matchname, teamname, status);
            string ExportField = "Matchname,Teamname,Mobile,Company,Linename,Status,Createtime";
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
                        case "Matchname":
                            dr[fields[j]] = o[i].matchname;
                            break;
                        case "Teamname":
                            dr[fields[j]] = o[i].Teamname;
                            break;
                        case "Mobile":
                            dr[fields[j]] = o[i].Moblie;
                            break;
                        case "Company":
                            dr[fields[j]] = o[i].Company;
                            break;
                        case "Linename":
                            dr[fields[j]] = o[i].Linename;
                            break;
                        case "Status":
                            foreach (SelectListItem r in Status)
                            {
                                if (o[i].Status.ToString() == r.Value)
                                    dr[fields[j]] = r.Text.ToString();
                            }
                            break;
                        case "Createtime":
                            dr[fields[j]] = o[i].Createtime;
                            break;
                        default:
                            break;
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.Columns["Matchname"].ColumnName = "赛事名称";
            dt.Columns["Teamname"].ColumnName = "队伍名称";
            dt.Columns["Mobile"].ColumnName = "联系电话";
            dt.Columns["Companyname"].ColumnName = "所属公司";
            dt.Columns["Linename"].ColumnName = "所属线路";
            dt.Columns["Status"].ColumnName = "状态";
            dt.Columns["Createtime"].ColumnName = "创建时间";

            string file = "";
            try
            {
                file = GridToExcelByNPOI(dt, DateTime.Now.ToString("yyyyMMddHHmmss") + "_队伍统计.xls");
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


        public ActionResult Edit(string id)
        {
            var bll = new TeamBll();
            var model = bll.GetTeamById(id);
            List<SelectListItem> line = new MatchBll().GetlineList(model.match_id);
            List<SelectListItem> lines = new MatchBll().GetlinesList(model.Lineid);
            List<SelectListItem> Status = new MemberBll().GetDict(5);

            foreach (SelectListItem r in line)
            {
                if (model.Lineid == r.Value)
                    ViewBag.line += "<option value='" + r.Value.ToString() + "'selected>" + r.Text.ToString() + "</option>";
                else
                    ViewBag.line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            if (!string.IsNullOrEmpty(model.Linesid))
            {
                foreach (SelectListItem r in lines)
                {
                    if (model.Linesid == r.Value)
                        ViewBag.lines += "<option value='" + r.Value.ToString() + "'selected>" + r.Text.ToString() + "</option>";
                    else
                        ViewBag.lines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }
            }
            else
            {
                ViewBag.lines += "<option value=''>请选择线路</option>";
                foreach (SelectListItem r in lines)
                {
                    ViewBag.lines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }
            }


            foreach (SelectListItem r in Status)
            {
                if (model.Status.ToString() == r.Value)
                    ViewBag.Status += "<option value='" + r.Value.ToString() + "'selected>" + r.Text.ToString() + "</option>";
                else
                    ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }


            return View(model);
        }


        public JsonResult EditCheckTeamNo(string teamno, string teamid)
        {
            string match_id = UserInfo.Matchid;
            bool isValidate = true;
            var teams = new TeamBll().CheckTeamsNo(match_id, teamno, teamid);

            foreach (var m in teams)
            {
                if (m.Teamno == teamno)
                {
                    isValidate = false;
                }
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Edit(string id, FormCollection fc)
        {
            var bll = new TeamBll();
            var model = bll.GetTeamById(id);
            model.Lineid = fc["optLine"].ToString();
            model.Linesid = fc["optLines"].ToString();
            model.Teamno = fc["Teamno"].ToString();
            model.Company = fc["Company"].ToString();

            try
            {
                bll.EditTeam(model);
            }
            catch (ValidException ex)
            {
                this.ModelState.AddModelError(ex.Name, ex.Message);
                List<SelectListItem> company = bll.GetCompany();
                List<SelectListItem> line = bll.GetLine();
                List<SelectListItem> Status = new MemberBll().GetDict(5);

                foreach (SelectListItem r in company)
                {
                    ViewBag.company += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

                foreach (SelectListItem r in line)
                {
                    ViewBag.line += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

                foreach (SelectListItem r in Status)
                {
                    ViewBag.Status += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
                }

                return View(model);
            }

            return this.RefreshParent();
        }



        public ActionResult AssignsTeam()
        {
            string matchid = UserInfo.Matchid.ToString();
            //获取赛事列表
            List<SelectListItem> Match = new MatchBll().GetMatchsList();
            foreach (SelectListItem r in Match)
            {
                if (r.Value == matchid)
                    ViewBag.Match += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            //目标线路列表
            List<SelectListItem> Lines = new MatchBll().GetlinesByMatchid(matchid);
            ViewBag.TargetLines += "<option value=''>请选择线路</option>";
            foreach (SelectListItem r in Lines)
            {
                ViewBag.TargetLines += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            return View();
        }


        public ActionResult ResetSerial()
        {
            string matchid = UserInfo.Matchid.ToString();
            //获取赛事列表
            List<SelectListItem> Match = new MatchBll().GetMatchsList();
            List<SelectListItem> Type = new MemberBll().GetDict(13);
            foreach (SelectListItem r in Type)
            {
                ViewBag.Type += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }
            foreach (SelectListItem r in Match)
            {
                if (r.Value == matchid)
                    ViewBag.Match += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            return View();
        }


        [HttpPost]
        public string ResetSerial(TeamDistribution td)
        {
            string ret = "";
            string matchid = td.matchid;
            string lineid_ = td.lineid;
            string linesid_ = td.linesid;
            try
            {
                //重置队伍编号为0
                ret = new TeamBll().ResetSerial(matchid, lineid_, linesid_);
            }
            catch
            {
                ret = "-1";
            }

            return ret; ;
        }
        public ActionResult BuildSerial()
        {
            string matchid = UserInfo.Matchid.ToString();
            //获取赛事列表
            List<SelectListItem> Match = new MatchBll().GetMatchsList();
            List<SelectListItem> Type = new MemberBll().GetDict(13);
            foreach (SelectListItem r in Type)
            {
                ViewBag.Type += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }
            foreach (SelectListItem r in Match)
            {
                if (r.Value == matchid)
                    ViewBag.Match += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
            }

            return View();
        }
        [HttpPost]
        public int GetLeftLinesCount(string linesid)
        {
            int ret = -1;
            int limitcount = new TeamBll().GetLeftLinesCount(linesid);
            int count = new TeamBll().GetTeamsBylinesid(UserInfo.Matchid.ToString(), linesid).Count;
            ret = limitcount - count;
            return ret;
        }


        [HttpPost]
        public int GetLimitLinesCount(string linesid)
        {
            int ret = -1;
            int limitcount = new TeamBll().GetLimitLinesCount(linesid);
            int count = new TeamBll().GetTeamsBylinesid(UserInfo.Matchid.ToString(), linesid).Count;
            ret = limitcount - count;
            return ret;
        }


        [HttpPost]
        public int GetLinesTeamCount(string linesid)
        {
            int ret = -1;
            ret = new TeamBll().GetTeamsBylinesid1(UserInfo.Matchid.ToString(), linesid).Count;

            return ret;
        }

        [HttpPost]
        public string BuildSerial(TeamDistribution td)
        {
            string ret = "";
            string matchid = td.matchid;
            string linesno_ = "";
            try
            {
                ret = new TeamBll().BuildSerial(matchid);
            }
            catch
            {
                ret = "-1";
            }

            return ret; ;
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
            List<SelectListItem> line = new MatchBll().GetlinesList(lineid);
            return Json(line, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// zzy 2019-01-03
        /// 获取playercount等于5的线路
        /// </summary>
        /// <param name="lineid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLinesByLine2(string lineid)
        {
            var bll = new MatchBll();
            List<SelectListItem> line = new MatchBll().GetlinesList2(lineid);
            return Json(line, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public int preview(TeamDistribution td)
        {
            int ret = -1;
            try
            {
                string matchid = td.matchid;
                string lineid = td.lineid;
                string linesid = td.linesid;
                string targetLines = td.targetLines;
                int count = td.count;
                var lines = new TeamBll().getLineBylinesid(matchid, targetLines);
                //源数据
                List<tblteams> source = new TeamBll().GetAssignsTeams(matchid, lineid, linesid);
                if (source.Count > 0)
                {
                    TeamDisEntiyList tmp = new TeamDisEntiyList();
                    tmp.matchid = matchid;
                    tmp.lineid = lines.Line_id;
                    tmp.linesid = lines.Lines_id;
                    tmp.teams = source.Take(count).ToList();
                    tmp.linesno = lines.Line_no;

                    //获取最大队伍编号
                    int max = new TeamBll().GetMaxTeamno(matchid, tmp.linesid);

                    if (max < 0)
                    {
                        ret = -1;
                        return ret;
                    }

                    if (max == 0)
                    {
                        max = Int32.Parse(lines.Line_no.PadLeft(2, '0') + "001");
                    }
                    else if (max > 0)
                    {
                        max = max + 1;
                    }

                    //处理队伍编号

                    for (int i = 0; i < tmp.teams.Count; i++)
                    {
                        tmp.teams[i].Teamno = (max + i).ToString();
                    }

                    //更新数据库
                    ret = new TeamBll().TeamDis(tmp);

                }



            }
            catch
            {
                ret = -1;
            }

            return ret;


            ////目标线路
            //string[] targetlines = targetLines_.Split(',');
            //int maxLinesTeamCout = 0;
            //int minFillCount= 0;
            //int totalCount = 0;
            //List<tbllines> lines_ = new MatchBll().GetLinesByMatchid1(matchid);
            //List<TeamDisEntiyList> tdel = new List<TeamDisEntiyList>();
            //if (targetlines.Length > 0)
            //{
            //    for (int i = 0; i < targetlines.Length; i++)
            //    {
            //        int limitteamcount = lines_.Where(x => x.Lines_id == targetlines[i]).FirstOrDefault().Pointscount.Value;
            //        if (targetlines[i] != linesid_)
            //        {
            //            TeamDisEntiyList tmp = new TeamDisEntiyList();
            //            var linesteams = new TeamBll().GetTeamsBylinesid(matchid, targetlines[i]);
            //            if (linesteams != null && linesteams.Count > 0)
            //            {
            //                if (linesteams.Count > maxLinesTeamCout)
            //                    maxLinesTeamCout = linesteams.Count;
            //            }
            //            var line = new TeamBll().getLineBylinesid(matchid, targetlines[i]);
            //            tmp.matchid = matchid;
            //            tmp.lineid = line.Line_id;
            //            tmp.linesid = targetlines[i];
            //            tmp.teams = linesteams;
            //            tmp.limtitteamcount = limitteamcount;
            //            tdel.Add(tmp);
            //        }
            //    }

            //    //计算最小补齐数量
            //    for (int i = 0; i < tdel.Count; i++)
            //    {
            //        if (maxLinesTeamCout > tdel[i].limtitteamcount)
            //        {
            //            minFillCount += tdel[i].limtitteamcount - tdel[i].teams.Count;
            //            tdel[i].needcount = tdel[i].limtitteamcount - tdel[i].teams.Count;
            //        }else
            //        {
            //            minFillCount += maxLinesTeamCout - tdel[i].teams.Count;
            //            tdel[i].needcount = maxLinesTeamCout - tdel[i].teams.Count;
            //        }                 
            //        totalCount += tdel[i].teams.Count;
            //    }


            //}

            ////源数据
            //List<tblteams>  source  = new TeamBll().GetAssignsTeams(matchid, lineid_, linesid_);

            //int sourceCount = source.Count;

            //List<TeamDisEntiyList> allot = new List<TeamDisEntiyList>();
            //if (sourceCount > 0 && sourceCount > minFillCount)
            //{
            //    for (int i = 0; i < tdel.Count; i++)
            //    {
            //        TeamDisEntiyList tmp = new TeamDisEntiyList();
            //        int _fillCount = tdel[i].needcount;
            //        tmp.guid = Guid.NewGuid().ToString();
            //        tmp.matchid = matchid;
            //        tmp.lineid = tdel[i].lineid;
            //        tmp.linesid = tdel[i].linesid;
            //        if (tdel[i].teams.Count < maxLinesTeamCout)
            //        {
            //            //先补齐不是最大数量线路的数量
            //            List<tblteams> temp = source.Take(_fillCount).ToList();
            //            tdel[i].teams.AddRange(temp);
            //            tmp.teams = tdel[i].teams;
            //            tmp.limtitteamcount = tdel[i].limtitteamcount;
            //            tmp.needcount = 0;
            //            source = source.Skip(_fillCount).ToList();
            //            allot.Add(tmp);
            //        }else
            //        {
            //            tmp.teams = tdel[i].teams;
            //            tmp.limtitteamcount = tdel[i].limtitteamcount;
            //            tmp.needcount = 0;
            //            allot.Add(tmp);
            //        }
            //    }

            //    //剩余数量处理 填充置最大数量
            //    for (int i = 0; i < allot.Count; i++)
            //    {
            //        if (allot[i].limtitteamcount > allot[i].teams.Count)
            //        {
            //            int _fillCount = allot[i].limtitteamcount - allot[i].teams.Count;
            //            List<tblteams> temp = source.Take(_fillCount).ToList();
            //            allot[i].teams.AddRange(temp);
            //        }               
            //    }

            //    //还有剩余 不在处理
            //    if (source.Count > 0)
            //    {

            //    }
            //}
            //else if (sourceCount > 0 && sourceCount <= minFillCount)
            //{
            //    //平均数和队伍上限 取消补齐
            //    int surplusCount = source.Count;               
            //    int baseCount = totalCount + surplusCount;
            //    int linesteamcount = baseCount / targetlines.Length;

            //    if (targetlines.Length > 1)
            //    {
            //        for (int j = 0; j < targetlines.Length -1; j++)
            //        {
            //            int limitteamcount = lines_.Where(x => x.Lines_id == targetlines[j]).FirstOrDefault().Pointscount.Value;

            //            if (j < targetlines.Length - 1)
            //            {
            //                var linesteams = new TeamBll().GetTeamsBylinesid(matchid, targetlines[j]);
            //                if (linesteams.Count < linesteamcount)
            //                {
            //                    int needcount = 0; 
            //                    if (linesteamcount > limitteamcount)
            //                    {
            //                        needcount = limitteamcount - linesteams.Count;
            //                    }else
            //                    {
            //                        needcount = linesteamcount - linesteams.Count;
            //                    }
            //                    var line = new TeamBll().getLineBylinesid(matchid, targetlines[j]);
            //                    TeamDisEntiyList tmp = new TeamDisEntiyList();
            //                    tmp.guid = Guid.NewGuid().ToString();
            //                    tmp.matchid = matchid;
            //                    tmp.lineid = line.Line_id;
            //                    tmp.linesid = line.Lines_id;
            //                    tmp.teams = source.Take(needcount).ToList();
            //                    source = source.Skip(needcount).ToList();
            //                    allot.Add(tmp);
            //                }

            //            }

            //            //剩余数量处理 填充置最大数量
            //            for (int i = 0; i < allot.Count; i++)
            //            {
            //                if (allot[i].limtitteamcount > allot[i].teams.Count)
            //                {
            //                    int _fillCount = allot[i].limtitteamcount - allot[i].teams.Count;
            //                    List<tblteams> temp = source.Take(_fillCount).ToList();
            //                    allot[i].teams.AddRange(temp);
            //                }
            //            }

            //            //还有剩余 不在处理
            //            if (source.Count > 0)
            //            {

            //            }

            //        }

            //    }
            //}
            //else
            //{
            //  return null;
            //}


            ////去除相同线路
            //var TeamList = allot.Distinct(new List_Team_DistinctBy_Linesid()).ToList();

            ////合并
            //for (int i = 0; i < TeamList.Count; i++)
            //{
            //    var lines = new TeamBll().getLineBylinesid(matchid, TeamList[i].linesid);
            //    TeamList[i].linesname = lines.Linename;
            //    for (int j = 0; j < allot.Count; j++)
            //    {
            //        if (TeamList[i].guid != allot[j].guid && TeamList[i].linesid == allot[j].linesid)
            //        {
            //            TeamList[i].teams = TeamList[i].teams.Union(allot[j].teams).ToList();
            //        }
            //    }
            //    TeamList[i].count = TeamList[i].teams.Count;
            //}

            ////更新数据库
            //int ret = new TeamBll().TeamDis(TeamList);
            //if (ret == 0)
            //{
            //    return Json(TeamList, JsonRequestBehavior.AllowGet);
            //}else
            //{
            //    return null;
            //}

        }

        //生成编号
        //[HttpPost]
        //public int BuildSerial1(string matchid)
        //{
        //    int ret = -1;

        //    try
        //    {
        //        var match = new MatchBll().GetMatchById(matchid);
        //        //获取赛事各线路基础信息
        //        List<GroupLines> grouplines = new TeamBll().GetGroupLines(matchid).OrderBy(x=>x.linesno).ToList();
        //        List<tblteams> groupteam = new TeamBll().GetTeamsBymatchid(matchid);
        //        List<tblmatchusers> groupuser = new TeamBll().GetMatchUserBymatchid(matchid);

        //        List<tblmatchusers> finishedUser = new List<tblmatchusers>();

        //        for (int i = 0; i < grouplines.Count; i++)
        //        {
        //            string linesno = grouplines[i].linesno;
        //            int teamno = 0;
        //            List<tblteams> groupteam_ = groupteam.Where(x => x.Linesid == grouplines[i].linesid).ToList();

        //            for (int j = 0; j < groupteam_.Count; j++)
        //            {
        //                groupteam_[j].Teamno = linesno + (j+1).ToString().PadLeft(3, '0');

        //                List<tblmatchusers> groupuser_ = groupuser.Where(x => x.Teamid == groupteam_[j].teamid).ToList().OrderByDescending(x => x.Leader).ToList();
        //                for (int k = 0; k < groupuser_.Count; k++)
        //                {
        //                    groupuser_[k].Mono = linesno + (j+1).ToString().PadLeft(3, '0') +(k + 1).ToString();
        //                }
        //                finishedUser.AddRange(groupuser_);
        //            }  
        //        }


        //        ret = new TeamBll().BuildSerial(groupteam, finishedUser);

        //    }
        //    catch 
        //    {
        //        ret = 99;
        //    }

        //    return ret;
        //}

        //导出分组
        [HttpPost]
        public string ExportGroup(string matchid)
        {
            int err = 0;
            string filename = "";
            try
            {

                var match = new MatchBll().GetMatchById(matchid);
                //获取赛事各线路基础信息
                List<GroupLines> grouplines = new TeamBll().GetGroupLines(matchid);
                List<GroupTeam> groupteam = new TeamBll().GetGroupTeams(matchid);
                List<GroupUser> groupuser = new TeamBll().GetGrouUser(matchid);

                for (int i = 0; i < grouplines.Count; i++)
                {
                    List<GroupTeam> gtlst = groupteam.Where(x => x.linesid == grouplines[i].linesid).OrderBy(x => x.teamno).ToList();
                    for (int j = 0; j < gtlst.Count; j++)
                    {
                        List<GroupUser> gulst = groupuser.Where(x => x.teamid == gtlst[j].teamid).OrderBy(x => x.userno).ToList();
                        gtlst[j].groupuser = gulst;
                    }
                    grouplines[i].groupteam = gtlst;
                }

                //1个线路1个sheet
                HSSFWorkbook workbook = new HSSFWorkbook();
                for (int i = 0; i < grouplines.Count; i++)
                {

                    string sheetname = grouplines[i].linesname;
                    ISheet sheet = workbook.CreateSheet(sheetname);
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));
                    int icolIndex = 0;
                    IRow headerRow = sheet.CreateRow(0);
                    headerRow.Height = 1000;
                    ICellStyle style = workbook.CreateCellStyle();
                    //设置单元格的样式：水平对齐居中
                    style.Alignment = HorizontalAlignment.CENTER;
                    style.WrapText = true;
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

                    cell.SetCellValue(new HSSFRichTextString(match.Match_name + Convert.ToChar(10) + "参赛队名及队员名单"));
                    cell.CellStyle = style;


                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));
                    icolIndex = 0;
                    headerRow = sheet.CreateRow(1);
                    ICellStyle style1 = workbook.CreateCellStyle();
                    //设置单元格的样式：水平对齐居中
                    style1.Alignment = HorizontalAlignment.LEFT;
                    //新建一个字体样式对象
                    font = workbook.CreateFont();
                    //设置字体加粗样式
                    font.Boldweight = short.MinValue;
                    font.FontName = "微软雅黑";
                    font.FontHeightInPoints = 10;
                    //使用SetFont方法将字体样式添加到单元格样式中 
                    style1.SetFont(font);
                    //将新的样式赋给单元格

                    cell = headerRow.CreateCell(icolIndex);
                    cell.SetCellValue(grouplines[i].linesname + "（队员编号尾号为1的是参赛队长）");
                    cell.CellStyle = style1;

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
                    headerRow = sheet.CreateRow(2);
                    cell = headerRow.CreateCell(0);
                    cell.SetCellValue("队列号");
                    cell.CellStyle = HeadercellStyle;
                    cell = headerRow.CreateCell(1);
                    cell.SetCellValue("队名");
                    cell.CellStyle = HeadercellStyle;
                    cell = headerRow.CreateCell(2);
                    cell.SetCellValue("队员");
                    cell.CellStyle = HeadercellStyle;
                    cell = headerRow.CreateCell(3);
                    cell.SetCellValue("队员编号");
                    cell.CellStyle = HeadercellStyle;
                    cell = headerRow.CreateCell(4);
                    cell.SetCellValue("");
                    cell.CellStyle = HeadercellStyle;
                    cell = headerRow.CreateCell(5);
                    cell.SetCellValue("队列号");
                    cell.CellStyle = HeadercellStyle;
                    cell = headerRow.CreateCell(6);
                    cell.SetCellValue("队名");
                    cell.CellStyle = HeadercellStyle;
                    cell = headerRow.CreateCell(7);
                    cell.SetCellValue("队员");
                    cell.CellStyle = HeadercellStyle;
                    cell = headerRow.CreateCell(8);
                    cell.SetCellValue("队员编号");
                    cell.CellStyle = HeadercellStyle;

                    sheet.SetColumnWidth(0, 10 * 256);
                    sheet.SetColumnWidth(1, 20 * 256);
                    sheet.SetColumnWidth(2, 15 * 256);
                    sheet.SetColumnWidth(3, 10 * 256);
                    sheet.SetColumnWidth(4, 5 * 256);
                    sheet.SetColumnWidth(5, 10 * 256);
                    sheet.SetColumnWidth(6, 20 * 256);
                    sheet.SetColumnWidth(7, 15 * 256);
                    sheet.SetColumnWidth(8, 10 * 256);

                    ICellStyle cellStyle = workbook.CreateCellStyle();

                    //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                    cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.NONE;
                    NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
                    cellfont.Boldweight = (short)FontBoldWeight.NORMAL;
                    cellfont.FontName = "宋体";
                    cellStyle.VerticalAlignment = VerticalAlignment.TOP;
                    cellStyle.Alignment = HorizontalAlignment.CENTER;
                    cellStyle.SetFont(cellfont);


                    ICellStyle cellStyle1 = workbook.CreateCellStyle();

                    //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                    cellStyle1.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                    cellStyle1.BorderBottom = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle1.BorderLeft = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle1.BorderRight = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle1.BorderTop = NPOI.SS.UserModel.BorderStyle.NONE;

                    //cellStyle1.FillPattern = FillPatternType.SOLID_FOREGROUND;
                    //cellStyle1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
                    NPOI.SS.UserModel.IFont cellfont1 = workbook.CreateFont();
                    cellfont1.Boldweight = (short)FontBoldWeight.NORMAL;
                    cellfont1.FontName = "宋体";
                    cellStyle.Alignment = HorizontalAlignment.CENTER;
                    cellStyle1.SetFont(cellfont1);

                    //冻结行
                    sheet.CreateFreezePane(0, 3, 0, 3); //冻结的列数，行数

                    int rows = 0;
                    if (grouplines[i].groupteam.Count % 2 == 0)
                    {
                        rows = grouplines[i].groupteam.Count / 2;
                    }
                    else
                    {
                        rows = grouplines[i].groupteam.Count / 2 + 1;
                    }
                    int iRowIndex = 3;
                    int iCellIndex = 0;
                    for (int j = 0; j < rows; j++)
                    {
                        //if (j == rows - 1)
                        //{

                        //}
                        //err = j;
                        GroupTeam gt1 = grouplines[i].groupteam[j * 2];
                        GroupTeam gt2 = new GroupTeam();
                        if (j * 2 + 1 < grouplines[i].groupteam.Count)
                        {
                            gt2 = grouplines[i].groupteam[j * 2 + 1];
                        }

                        List<GroupUser> gu1 = groupuser.Where(x => x.teamid == gt1.teamid).OrderBy(x => x.userno).ToList();
                        List<GroupUser> gu2 = groupuser.Where(x => x.teamid == gt2.teamid).OrderBy(x => x.userno).ToList();
                        int gu1_count = gu1.Count;
                        int gu2_count = gu2.Count;
                        for (int c1 = 0; c1 < 5; c1++)
                        {
                            int row_ = iRowIndex + (j * 5) + c1;
                            IRow DataRow = sheet.CreateRow(row_);
                            iCellIndex = 0;
                            for (int c = 0; c < 9; c++)
                            {
                                ICell cell_ = DataRow.CreateCell(c);
                                switch (c)
                                {
                                    case 0:
                                        if (c1 == 0)
                                        {
                                            cell_.SetCellValue(gt1.teamno);
                                            cell_.CellStyle = cellStyle;
                                        }
                                        break;
                                    case 1:
                                        if (c1 == 0)
                                        {
                                            cell_.SetCellValue(gt1.teamname);
                                            cell_.CellStyle = cellStyle;
                                        }

                                        break;
                                    case 2:
                                        if (gu1_count > c1)
                                        {
                                            cell_.SetCellValue(gu1[c1].nickname);
                                            cell_.CellStyle = cellStyle1;
                                        }
                                        break;
                                    case 3:
                                        if (gu1_count > c1)
                                        {
                                            cell_.SetCellValue(gu1[c1].userno);
                                            cell_.CellStyle = cellStyle1;
                                        }
                                        break;
                                    case 4:
                                        break;
                                    case 5:
                                        if (c1 == 0)
                                        {
                                            cell_.SetCellValue(gt2.teamno);
                                            cell_.CellStyle = cellStyle;
                                        }
                                        break;
                                    case 6:
                                        if (c1 == 0)
                                        {
                                            cell_.SetCellValue(gt2.teamname);
                                            cell_.CellStyle = cellStyle;
                                        }
                                        break;
                                    case 7:
                                        if (gu2_count > c1)
                                        {
                                            cell_.SetCellValue(gu2[c1].nickname);
                                            cell_.CellStyle = cellStyle1;
                                        }
                                        break;
                                    case 8:
                                        if (gu2_count > c1)
                                        {
                                            cell_.SetCellValue(gu2[c1].userno);
                                            cell_.CellStyle = cellStyle1;
                                        }
                                        break;

                                }
                                icolIndex++;
                            }

                            //合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress((3 + 5 * j), (3 + 5 * j + 4), 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress((3 + 5 * j), (3 + 5 * j + 4), 1, 1));
                            sheet.AddMergedRegion(new CellRangeAddress((3 + 5 * j), (3 + 5 * j + 4), 5, 5));
                            sheet.AddMergedRegion(new CellRangeAddress((3 + 5 * j), (3 + 5 * j + 4), 6, 6));
                        }

                    }
                }

                //写Excel
                filename = match.Match_name + "_分组_ " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string filepath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Data\\" + filename;
                FileStream file = new FileStream(filepath, FileMode.OpenOrCreate);
                workbook.Write(file);
                file.Flush();
                file.Close();

            }
            catch (Exception ex)
            {
                int d = err;
            }
            return filename;
        }

        public class List_Team_DistinctBy_Linesid : IEqualityComparer<TeamDisEntiyList>
        {
            public bool Equals(TeamDisEntiyList x, TeamDisEntiyList y)
            {
                if (x.linesid == y.linesid && x.lineid == y.lineid && x.matchid == y.matchid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(TeamDisEntiyList obj)
            {
                return 0;
            }
        }

        [HttpPost]
        public String assigns(string matchid)
        {
            List<SelectListItem> line = new MatchBll().GetlineList(matchid);

            List<AssignsTeam> AssignsTeam = new List<AssignsTeam>();
            for (int i = 0; i < line.Count; i++)
            {
                Assigns Assigns_ = new Assigns();

                var lineid = line[i].Value;
                var linename = line[i].Text;
                var teams = new TeamBll().GetAssignsTeams(matchid, lineid, null);
                List<SelectListItem> lines = new MatchBll().GetlinesList(lineid);
                int linesteamcount = teams.Count / lines.Count;
                if (lines != null && lines.Count > 0)
                {
                    for (int j = 0; j < lines.Count; j++)
                    {

                        if (j < lines.Count - 1)
                        {
                            for (int k = linesteamcount * j; k < linesteamcount * (j + 1); k++)
                            {
                                AssignsTeam at = new AssignsTeam();
                                at.linesid = lines[j].Value;
                                at.linesname = lines[j].Text;
                                at.teamid = teams[k].teamid;
                                at.teamname = teams[k].Teamname;
                                AssignsTeam.Add(at);
                            }
                        }
                        else
                        {
                            for (int k = linesteamcount * (lines.Count - 1); k < teams.Count; k++)
                            {
                                AssignsTeam at = new AssignsTeam();
                                at.linesid = lines[j].Value;
                                at.teamid = teams[k].teamid;
                                at.teamid = teams[k].teamid;
                                at.teamname = teams[k].Teamname;
                                AssignsTeam.Add(at);
                            }
                        }

                    }
                }
            }

            //批量更新线路id
            int ret = new TeamBll().AssignsTeams(AssignsTeam);

            string msg = "";

            if (ret == 0)
            {
                msg = "OK";
            }
            else
            {
                msg = "ERROR";
            }

            return msg;
        }

        public ActionResult linedown(string teamname, string teamno, string dstatus, int? pageIndex)
        {
            TeamBll bll = new TeamBll();
            var lst = bll.GetTeamsLinedown(base.UserInfo.Matchid, teamname, teamno, dstatus, pageIndex.GetValueOrDefault(1));

            List<SelectListItem> Status = new MemberBll().GetDict(5);
            ViewData["Status"] = Status;

            return View(lst);
        }

        public ActionResult TeamUsers(string matchid, string teamid)
        {
            if (teamid != null)
                teamid = teamid.Replace("?", "");
            var teamuser = new TeamBll().getTeamUsers(matchid, teamid);
            return View(teamuser);
        }

        [HttpPost]
        public ActionResult Delete(List<string> ids)
        {
            var bll = new TeamBll();

            try
            {
                bll.TeamDown(ids);
            }
            catch (ValidException ex)
            {
                return Alert(ex.Message);
            }

            return RedirectToAction("linedown");
        }


        [HttpPost]
        public int AllTeamDown()
        {
            var bll = new TeamBll();
            try
            {
                string matchid = base.UserInfo.Matchid;
                if (!string.IsNullOrEmpty(matchid))
                {
                    bll.AllTeamDown(matchid);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 1;
            }

        }
    }
}