using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DAL;
using Model;
using Utls;
using System.Web.Mvc;

namespace BLL
{
    public class MatchBll : BaseBll
    {
        /// <summary>
        /// 查询赛事信息
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="tel"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblmatch> GetMatchs(string matchname,string area2, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.* FROM tbl_match a WHERE 1=1");

                if (!string.IsNullOrEmpty(matchname))
                    sql.AppendFormat(" AND a.match_name  like '%{0}%'", matchname);

                if (!string.IsNullOrEmpty(area2))
                    sql.AppendFormat(" AND a.area2 like '%{0}%'", area2);


                return db.SqlQuery<tblmatch, DateTime?>(sql.ToString(), pageindex, p => p.Date3);
            }
        }

        /// <summary>
        /// 根据matchid查询赛事
        /// </summary>
        /// <param name="playerid"></param>
        /// <returns></returns>
        public tblmatch GetMatchById(string id)
        {
            using (var db = new BFdbContext())
            {
                return db.tblmatch.FirstOrDefault(p => p.Match_id == id);
            }
        }

        /// <summary>
        /// 根据matchid查询赛事其他图片
        /// </summary>
        /// <param name="Teamname"></param>
        /// <returns></returns>
        public List<tblmatchpics> GetOtherPics(string matchid)
        {
            using (var db = new BFdbContext())
            {
                 return db.FindAll<tblmatchpics>(p=>p.Match_id==matchid).ToList();
            }
        }

        /// <summary>
        /// 根据tel查询会员
        /// </summary>
        /// <param name="playerid"></param>
        /// <returns></returns>
        public tblusers GetMemberByTel(string tel)
        {
            using (var db = new BFdbContext())
            {
                return db.tblusers.FirstOrDefault(p => p.Mobile == tel);
            }
        }


        /// <summary>
        /// 新增赛事
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int AddMatch(tblmatch ent,string[] otherpics)
        {
            using (var db = new BFdbContext())
            {
                try
                {
                    db.TInsert<tblmatch>(ent);
                    if (otherpics != null && otherpics.Length > 0)
                    {
                        for (int i = 0; i < otherpics.Length; i++)
                        {
                            var pic = new tblmatchpics();
                            pic.Id = Guid.NewGuid().ToString();
                            pic.Match_id = ent.Match_id;
                            pic.Picture = otherpics[i].ToString();
                            pic.Createtime = DateTime.Now;
                            db.TInsert<tblmatchpics>(pic);
                        }
                    }

                    db.SaveChanges();
                    return 0;

                }catch
                {
                    return 99;
                }
               
            }
        }

        /// <summary>
        /// 更新赛事
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int EditMatch(tblmatch ent,string[] otherpics)
        {
            using (var db = new BFdbContext())
            {
                try
                {
                    tblmatch match = db.tblmatch.FirstOrDefault(p => p.Match_id == ent.Match_id);
                    match.Match_name = ent.Match_name;
                    match.Content = ent.Content;
                    match.Area1 = ent.Area1;
                    match.Area2 = ent.Area2;
                    match.Date1 = ent.Date1;
                    match.Date2 = ent.Date2;
                    match.Date3 = ent.Date3;
                    match.Date4 = ent.Date4;
                    match.Pic1 = ent.Pic1;
                    match.Pic2 = ent.Pic2;
                    match.Status = ent.Status;
                    db.TUpdate<tblmatch>(match);

                    if (otherpics != null && otherpics.Length > 0)
                    {
                        for (int i = 0; i < otherpics.Length; i++)
                        {
                            if (otherpics[i] != null)
                            {
                                var pic = new tblmatchpics();
                                pic.Id = Guid.NewGuid().ToString();
                                pic.Match_id = ent.Match_id;
                                pic.Picture = otherpics[i].ToString();
                                pic.Createtime = DateTime.Now;
                                db.TInsert<tblmatchpics>(pic);
                            }

                        }
                    }

                    db.SaveChanges();

                    return 0;
                }catch
                {
                    return 99;
                }
            }
        }

        /// <summary>
        /// 删除赛事
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteMatch(List<string> ids)
        {
            using (var db = new BFdbContext())
            {
                int res = 0;

                using (var tx = db.BeginTransaction())
                {
                    try
                    {
                        foreach (string id in ids)
                        {
                            tblmatch ent = db.tblmatch.FirstOrDefault(p => p.Match_id == id);
                            ent.Status = "2";
                            db.TUpdate<tblmatch>(ent);
                        }
                        db.SaveChanges();
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }

                return res;
            }
        }

        /// <summary>
        /// 查询线路类型信息
        /// </summary>
        /// <param name="matchname"></param>
        /// <param name="linename"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tbllineView> GetLine(string matchname,string name, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.*,m.match_name as Matchname FROM tbl_line a left join tbl_match m on m.match_id = a.match_id WHERE 1=1");

                if (!string.IsNullOrEmpty(matchname))
                    sql.AppendFormat(" AND m.match_name  like '%{0}%'", matchname);

                if (!string.IsNullOrEmpty(name))
                    sql.AppendFormat(" AND a.name  like '%{0}%'", name);

                return db.SqlQuery<tbllineView, DateTime?>(sql.ToString(), pageindex, p => p.Createtime);
            }
        }
        /// <summary>
        /// 获取赛事列表
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public List<SelectListItem> GetMatchsList()
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.match_id as value,a.match_name as text FROM tbl_match a ");
                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 获取赛事线路列表
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public List<tbllines> GetLinesByMatchid1(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM tbl_lines  ");
                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" where match_id = '{0}'", matchid);
                return db.SqlQuery<tbllines>(sql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 更新赛事用户
        /// zzy 2018-12-30
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int EditPlayer(tblmatchusers ent)
        {
            using (var db = new BFdbContext())
            {
                try
                {
                    return db.Update<tblmatchusers>(ent);
                }
                catch
                {
                    return 99;
                }
            }
        }

        /// <summary>
        /// 根据matchuserid查询赛事成员
        /// zzy 2018-12-30
        /// </summary>
        /// <param name="Teamname"></param>
        /// <returns></returns>
        public tblmatchusers getMatchUserByID(string matchuserid)
        {
            using (var db = new BFdbContext())
            {
                return db.tblmatchusers.FirstOrDefault(p => p.Matchuserid == matchuserid);
            }
        }

        /// <summary>
        /// 获取赛事线路列表
        /// zzy 2018-12-18
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLinesByMatchid2(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.lines_id as value,a.linename as text FROM tbl_lines  ");
                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" where match_id = '{0}' and  playercount=1 ", matchid);
                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }
        
        /// <summary>
        /// 获取线路类型列表
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public List<SelectListItem> GetlineList(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.lineid as value,a.name as text FROM tbl_line a  where 1=1 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND a.match_id = '{0}'", matchid);

                sql.Append(" order by status ");
                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 获取线路列表
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public List<SelectListItem> GetlinesList(string lineid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.lines_id as value,a.linename as text FROM tbl_lines a  where 1=1 ");

                if (!string.IsNullOrEmpty(lineid))
                    sql.AppendFormat(" AND a.line_id = '{0}'", lineid);

                sql.Append(" order by a.line_no ");

                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }

        public List<SelectListItem> GetlinesList2(string lineid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.lines_id as value,a.linename as text FROM tbl_lines a  where 1=1 and playercount=5 ");

                if (!string.IsNullOrEmpty(lineid))
                    sql.AppendFormat(" AND a.line_id = '{0}'", lineid);

                sql.Append(" order by a.line_no ");

                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }

        


        /// <summary>
        /// 新增线路类型
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int AddLine(tblline ent)
        {
            using (var db = new BFdbContext())
            {
                return db.Insert<tblline>(ent);
            }
        }

        /// <summary>
        /// 新增线路
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int AddLines(tbllines ent)
        {
            using (var db = new BFdbContext())
            {
                return db.Insert<tbllines>(ent);
            }
        }

        /// <summary>
        /// 根据lineid查询线路类型
        /// </summary>
        /// <param name="lineid"></param>
        /// <returns></returns>
        public tblline GetLineById(string id)
        {
            using (var db = new BFdbContext())
            {
                return db.tblline.FirstOrDefault(p => p.Lineid == id);
            }
        }

        /// <summary>
        /// 根据lineid查询线路
        /// </summary>
        /// <param name="lineid"></param>
        /// <returns></returns>
        public tbllines GetLinesById(string id)
        {
            using (var db = new BFdbContext())
            {
                return db.tbllines.FirstOrDefault(p => p.Lines_id == id);
            }
        }

        /// <summary>
        /// 根据matchid,teamid查询成绩
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        public tblresult GetScore(string matchid,string teamid)
        {
            using (var db = new BFdbContext())
            {
                return db.tblresult.FirstOrDefault(p => p.match_id == matchid && p.teamid == teamid);
            }
        }

        /// <summary>
        /// 更新成绩
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int EditScore(tblresult ent)
        {
            int ret = -1;
            using (var db = new BFdbContext())
            {
                try
                {

                    db.TUpdate<tblresult>(ent);

                    tblresultdetail d = new tblresultdetail();
                    d.detail_id = Guid.NewGuid().ToString();
                    d.teamid = ent.teamid;
                    d.match_id = ent.match_id;
                    d.teamno = ent.teamno;
                    d.userno = ent.userno;
                    d.starttime = ent.starttime;
                    d.settime = ent.settime;
                    d.timeline = ent.timeline;
                    d.createtime = ent.Createtime;

                    db.TInsert<tblresultdetail>(d);

                    db.SaveChanges();

                    ret = 0;
                }catch
                {
                    ret = 99;
                }  
            }

            return ret;
        }

        /// <summary>
        /// 更新线路类型
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int EditLine(tblline ent)
        {
            using (var db = new BFdbContext())
            {
                tblline line = db.tblline.FirstOrDefault(p => p.Lineid == ent.Lineid);
                line.Name = ent.Name;
                line.Match_id = ent.Match_id;
                line.Content = ent.Content;
                line.Players = ent.Players;
                line.teamprice = ent.teamprice;
                line.personprice = ent.personprice;
                line.Count = ent.Count;
                line.Conditions = ent.Conditions;
                line.Status = ent.Status;
                return db.Update<tblline>(line);
            }
        }

        /// <summary>
        /// 更新线路
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int EditLines(tbllines ent)
        {
            using (var db = new BFdbContext())
            {
                return db.Update<tbllines>(ent);
            }
        }

        /// <summary>
        /// 删除线路类型
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteLine(List<string> ids)
        {
            using (var db = new BFdbContext())
            {
                int res = 0;

                using (var tx = db.BeginTransaction())
                {
                    try
                    {
                        foreach (string id in ids)
                        {
                            tblline ent = db.tblline.FirstOrDefault(p => p.Lineid == id);
                            ent.Status = 2;
                            db.TUpdate<tblline>(ent);
                        }
                        db.SaveChanges();
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }

                return res;
            }
        }

        /// <summary>
        /// 删除线路
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteLines(List<string> ids)
        {
            using (var db = new BFdbContext())
            {
                int res = 0;

                using (var tx = db.BeginTransaction())
                {
                    try
                    {
                        foreach (string id in ids)
                        {
                            tbllines ent = db.tbllines.FirstOrDefault(p => p.Lines_id == id);
                            ent.Status = 2;
                            db.TUpdate<tbllines>(ent);
                        }
                        db.SaveChanges();
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }

                return res;
            }
        }

        /// <summary>
        /// 查询线路信息
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="lineid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tbllinesView> GetLines(string matchid, string lineid, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.*,m.match_name as Matchname,l.name as Line_name FROM tbl_lines a left join tbl_match m on m.match_id = a.match_id left join tbl_line l on l.lineid = a.line_id  WHERE 1=1");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND m.match_id  = '{0}'", matchid);

                if (!string.IsNullOrEmpty(lineid))
                    sql.AppendFormat(" AND a.line_id = '{0}' ", lineid);

                return db.SqlQuery1<tbllinesView, string>(sql.ToString(), pageindex, p => p.Line_no);
            }
        }

        public PagedList<tbllinesView> GetLinesOnly(string matchid, string linesid, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.*,m.match_name as Matchname,l.name as Line_name FROM tbl_lines a left join tbl_match m on m.match_id = a.match_id left join tbl_line l on l.lineid = a.line_id  WHERE 1=1");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND m.match_id  = '{0}'", matchid);

                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" AND a.lines_id = '{0}' ", linesid);

                return db.SqlQuery1<tbllinesView, string>(sql.ToString(), pageindex, p => p.Line_no);
            }
        }


        /// <summary>
        /// 查询标识点
        /// </summary>
        /// <param name="pointid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblpointsView> GetPoints(string linesid, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT d.name as pointtypename,a.*,l.linename as Linename FROM tbl_points a left join tbl_lines l on l.lines_id = a.lineguid  left join tbl_dict d on d.code = a.pointtype and d.dictid= 8 WHERE 1=1 ");

                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" AND a.lineguid = '{0}'", linesid);

                sql.Append(" order by a.sort ");
                return db.SqlQuery<tblpointsView, int?>(sql.ToString(), pageindex, p => p.Sort);
            }
        }



        /// <summary>
        /// 查询标识点
        /// </summary>
        /// <param name="pointid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<SelectListItem> GetPointsByLinesid(string linesid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.pointname as text,a.pointid as value FROM tbl_points a  ");
                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" where a.lineguid = '{0}'", linesid);

                sql.Append(" order by a.sort ");
                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 查询标识点
        /// </summary>
        /// <param name="pointid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblpointsView> GetPointsList(string linesid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT d.name as pointtypename,a.*,l.linename as Linename FROM tbl_points a left join tbl_lines l on l.lines_id = a.lineguid  left join tbl_dict d on d.code = a.pointtype and d.dictid= 8 WHERE 1=1 ");

                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" AND a.lineguid = '{0}'", linesid);

                sql.Append(" order by a.sort ");
                return db.SqlQuery<tblpointsView>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 查询标识点
        /// </summary>
        /// <param name="pointid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblpoints> GetPoints(string linesid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM tbl_points a  ");
                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" where a.lineguid = '{0}'", linesid);

                sql.Append(" order by a.sort ");
                return db.SqlQuery<tblpoints>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 根据pointid查询标识
        /// </summary>
        /// <param name="lineid"></param>
        /// <returns></returns>
        public tblpointsView GetPointById(string id)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.*,l.linename as Linename FROM tbl_points a left join tbl_lines l on l.lines_id = a.lineguid  WHERE 1=1 ");

                if (!string.IsNullOrEmpty(id))
                    sql.AppendFormat(" AND a.pointid = '{0}'", id);


                return db.SqlQuery<tblpointsView>(sql.ToString()).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据pointid查询标识
        /// </summary>
        /// <param name="lineid"></param>
        /// <returns></returns>
        public tblpointsView GetPointBySort(string linesid, string pointid, int sort)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                //sql.Append("SELECT a.*,l.linename as Linename FROM tbl_points a left join tbl_lines l on l.lines_id = a.lineguid  WHERE 1=1 ");
                sql.Append("SELECT a.pointid FROM tbl_points a  WHERE 1=1 ");
                sql.AppendFormat(" AND a.sort = {0}", sort);

                sql.AppendFormat(" AND a.Lineguid = '{0}'", linesid);

                if (!string.IsNullOrEmpty(pointid))
                    sql.AppendFormat(" AND a.pointid <> '{0}'", pointid);

                return db.SqlQuery<tblpointsView>(sql.ToString()).FirstOrDefault();
            }
        }

        /// <summary>
        /// 更新线路标识
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int EditPoints(tblpointsView ent)
        {
            using (var db = new BFdbContext())
            {
                try
                {
                    tblpoints point = db.tblpoints.FirstOrDefault(p => p.Pointid == ent.Pointid);
                    point.Pointname = ent.Pointname;
                    point.Pointno = ent.Pointno;
                    point.Pointout = ent.Pointout;
                    point.Pointtask = ent.Pointtask;
                    point.Sketchmap = ent.Sketchmap;
                    point.Sort = ent.Sort;
                    point.Pointtype = ent.Pointtype;
                    point.Pointaddress = ent.Pointaddress;
                    point.SketchVoice = ent.SketchVoice;
                    point.Linkno = ent.Linkno;
                    db.Update<tblpoints>(point);
                    return 0;

                }
                catch
                {
                    return 99;
                }
            }
        }

        /// <summary>
        /// 新增标识
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int AddPoints(tblpoints ent)
        {
            using (var db = new BFdbContext())
            {
                return db.Insert<tblpoints>(ent);
            }
        }

        /// <summary>
        /// 删除标识
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeletePoint(List<string> ids)
        {
            using (var db = new BFdbContext())
            {
                int res = 0;

                using (var tx = db.BeginTransaction())
                {
                    try
                    {
                        foreach (string id in ids)
                        {
                            tblpoints ent = db.tblpoints.FirstOrDefault(p => p.Pointid == id);
                            db.TDelete<tblpoints>(ent);
                        }
                        db.SaveChanges();
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }

                return res;
            }
        }

        /// <summary>
        /// 查询赛事队伍信息
        /// </summary>
        /// <param name="linesid"></param>
        /// <param name="teamname"></param>
        /// <param name="company"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblteamsVew> GetMatchTeams(string matchid, string linesid, string teamname, string company, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                //edit by pangsd 20190511
                //sql.Append("SELECT ls.line_no,ls.linename as linesname,u.name as nickname,t.*,u.mobile as Moblie,l.name as Linename,m.match_name as matchname FROM tbl_teams t  left join tbl_line l on l.lineid = t.lineid left join tbl_users u on u.userid = t.userid  left join tbl_match m on m.match_id = t.match_id  left join tbl_lines ls on ls.lines_id = t.linesid    where t.status = 0 ");
                sql.Append(@"SELECT ls.line_no,ls.linename as linesname,u.name as nickname,
                                    t.teamname,case when isNULL(tc.team_combine_id)=1 then t.company else '【合并组队】' end as company,t.Teamno,
                                    u.mobile as Moblie,l.name as Linename,m.match_name as matchname 
                              FROM tbl_teams t  
                                left join tbl_line l on l.lineid = t.lineid 
                                left join tbl_users u on u.userid = t.userid  
                                left join tbl_match m on m.match_id = t.match_id  
                                left join tbl_lines ls on ls.lines_id = t.linesid  
                                left join tbl_teams_combine tc on tc.team_id=t.teamid  
                              where t.status = 0 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id  =  '{0}'", matchid);

                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" AND t.linesid  =  '{0}'", linesid);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND t.teamname  like '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(company))
                    sql.AppendFormat(" AND t.company  like '%{0}%'", company);

                //sql.Append(" order by t.teamno desc ");
                return db.SqlQuery1<tblteamsVew, string>(sql.ToString(), pageindex, p => p.Teamno);
            }
        }

        /// <summary>
        /// 获取线路列表
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public List<SelectListItem> GetlinesByMatchid(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.lines_id as value,a.linename as text FROM tbl_lines a  where 1=1 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND a.match_id = '{0}'", matchid);

                sql.Append(" order by a.line_no");

                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 查询扫码信息
        /// </summary>
        /// <param name="linesid"></param>
        /// <param name="teamname"></param>
        /// <param name="company"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblmatchrecordView> GetScanInfo(string matchid,string nickname, string teamname, string pointname, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT mu.nickname,tm.teamname,p.pointname,t.pointtime,t.typ,t.status,t.createtime FROM tbl_match_record t left join tbl_match_users mu on mu.matchuserid = t.matchuserid left join tbl_teams tm on tm.teamid = t.teamid left join tbl_points p on p.pointid = t.pointid  left join tbl_teams a on a.teamid = t.teamid where 1=1");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND a.match_id =  '{0}'", matchid);

                if (!string.IsNullOrEmpty(nickname))
                    sql.AppendFormat(" AND mu.nickname  like  '%{0}%'", nickname);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND tm.teamname  like  '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(pointname))
                    sql.AppendFormat(" AND p.pointname like '%{0}%'", pointname);

                return db.SqlQuery<tblmatchrecordView, DateTime?>(sql.ToString(), pageindex, p => p.pointtime);
            }
        }


        /// <summary>
        /// 获取赛事队伍
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public List<teamview> GetMatchTeams(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select l.line_no,l.linename,t.teamno,t.teamname,t.company,t.teamid from tbl_teams t left join tbl_lines l on l.lines_id = t.linesid and l.match_id = t.match_id where t.status = 0 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id = '{0}'", matchid);

                return db.SqlQuery<teamview>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 获取队伍成员
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public List<TeamUser> GetTeamsUsers(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"select * from (
                               select case when u.leader= 1 then '队长' else '' end as user_no,u.nickname,u.age, case when u.sexy = 1 then '男' else '女' end as sex, 
                                    u.cardno,u.mobile,'是' as isgood,u.mono as memo,l.line_no,l.linename,t.teamno,t.teamname,t.company,t.teamid 
                                from tbl_teams t left join tbl_lines l on l.lines_id = t.linesid and l.match_id = t.match_id 
                                right join tbl_match_users u on u.teamid = t.teamid where  t.status = 0 and u.status = '1' ");
                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id = '{0}'", matchid);
                
                sql.Append(@"union 		 
                                select '' as user_no,  e1.info1 as nickname  ,'0' as age,case when e1.sexy = 1 then '男' else '女' end as sex,e1.info2  as cardno  ,'' as mobile,'是' as isgood
                                    ,'' AS memo,  s1.line_no,  s1.linename,  t1.teamno,  t1.teamname,  t1.company,  t1.teamid
                                from tbl_match_extra e1,tbl_teams t1,tbl_lines  s1,tbl_line l1 					
                                    where e1.teamid=t1.teamid and t1.linesid= s1.lines_id and s1.line_id=l1.lineid 
                                    and t1.status='0' and playercount=2");
                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t1.match_id = '{0}'", matchid);
                            

                sql.Append(") a order by teamno asc,user_no desc ");

                return db.SqlQuery<TeamUser>(sql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 查询赛事用户下载信息
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="status"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblmatchusersView> GetMatchusers(string matchid, string isdown, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT  m.match_name,t.teamname,mu.nickname,mu.mobile,mu.cardno,mu.age,mu.leader,mu.isdown FROM tbl_match_users mu ");
                sql.Append("inner join tbl_match m on m.match_id = mu.match_id ");
                sql.Append("inner join tbl_teams t on t.teamid = mu.teamid ");
                sql.Append("where mu.isdown <> '0'" );

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND mu.match_id  = '{0}'", matchid);

                if (!string.IsNullOrEmpty(isdown))
                    sql.AppendFormat(" AND mu.isdown = '{0}' ", isdown);

                return db.SqlQuery<tblmatchusersView, string>(sql.ToString(), pageindex, p => p.isdown);
            }
        }

        /// <summary>
        /// 赛事用户下载信息
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="status"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblmatchusersView> ExportUsers(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT  m.match_name,t.teamname,mu.nickname,mu.mobile,mu.cardno,mu.age,mu.leader,mu.isdown FROM tbl_match_users mu ");
                sql.Append("left join tbl_match m on m.match_id = mu.match_id ");
                sql.Append("left join tbl_teams t on t.teamid = mu.teamid ");
                sql.Append("where mu.isdown <> '0'");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND mu.match_id  = '{0}'", matchid);

                sql.Append(" order by mu.teamid ");

                return db.SqlQuery<tblmatchusersView>(sql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 查询队伍扫码信息
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="status"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<TeamScan> GetTeamScan(string matchid,string teamno,string teamname, string status, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("SELECT t.teamno, m.match_name ,t.teamname,t.company,l.name as linename,ls.linename as linesname,p.pointname,t.eventid,t.match_id,t.teamid  from tbl_teams t ");
                sql.Append("left join tbl_match m on m.match_id = t.match_id ");
                sql.Append("left join tbl_line l on l.lineid = t.lineid ");
                sql.Append("left join tbl_lines ls on ls.lines_id = t.linesid ");
                sql.Append("left join tbl_points p on p.pointid = t.record  ");
                sql.Append("where t.status = 0 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id  = '{0}'", matchid);

                if (!string.IsNullOrEmpty(teamno))
                    sql.AppendFormat(" AND t.teamno = '{0}' ", teamno);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND t.teamname like '%{0}%' ", teamname);

                if (!string.IsNullOrEmpty(status))
                    sql.AppendFormat(" AND t.eventid = '{0}' ", status);

                return db.SqlQuery<TeamScan, int?>(sql.ToString(), pageindex, p => p.eventid);
            }
        }


        /// <summary>
        /// 查询队伍扫码信息
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblmatchrecordView> GetTeamUserScan(string teamid, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT mu.nickname,tm.teamname,p.pointname,t.pointtime,t.typ,t.status,t.createtime FROM tbl_match_record t left join tbl_match_users mu on mu.matchuserid = t.matchuserid left join tbl_teams tm on tm.teamid = t.teamid left join tbl_points p on p.pointid = t.pointid  where 1=1");

                if (!string.IsNullOrEmpty(teamid))
                    sql.AppendFormat(" AND t.teamid ='{0}'", teamid);

                return db.SqlQuery<tblmatchrecordView, DateTime?>(sql.ToString(), pageindex, p => p.pointtime);
            }
        }

        /// <summary>
        /// 查询赛事成绩
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblresultView> GetResult(string matchid, string teamno ,string teamname, string lineno, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT t.*,m.teamname,s.match_name,tb.line_no FROM tbl_result t inner join tbl_teams m on m.teamid = t.teamid inner join tbl_match s on s.match_id = t.match_id inner join tbl_lines tb on m.linesid=tb.lines_id WHERE 1=1");

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND m.teamname  like '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id = '{0}'", matchid);

                if (!string.IsNullOrEmpty(teamno))
                    sql.AppendFormat(" AND t.teamno like '%{0}%'", teamno);

                if (!string.IsNullOrEmpty(lineno))
                    sql.AppendFormat(" AND tb.line_no = '{0}'", lineno);

                sql.Append(" order by t.timeline desc");

                return db.SqlQuery<tblresultView, string>(sql.ToString(), pageindex, p => p.teamno);
            }
        }

        public PagedList<tblresultDetailView> GetResultDetail(string matchid, string teamid,string teamname, string teamno,string lineno, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT t.*,m.teamname,s.match_name,tb.line_no FROM tbl_resultdetail t inner join tbl_teams m on m.teamid = t.teamid inner join tbl_match s on s.match_id = t.match_id inner join tbl_lines tb on m.linesid=tb.lines_id WHERE 1=1");

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND m.teamname  like '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id = '{0}'", matchid);

                if (!string.IsNullOrEmpty(teamid))
                    sql.AppendFormat(" AND t.teamid = '{0}'", teamid);

                if (!string.IsNullOrEmpty(teamno))
                    sql.AppendFormat(" AND t.teamno like '%{0}%'", teamno);

                if (!string.IsNullOrEmpty(lineno))
                    sql.AppendFormat(" AND tb.line_no = '{0}'", lineno);

                sql.Append(" order by t.teamid,t.starttime");

                return db.SqlQuery<tblresultDetailView, string>(sql.ToString(), pageindex, p => p.teamno);
            }
        }


        public List<tblresultView> GetResult(string matchid, string teamname, string lineno)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT t.*,m.teamname,s.match_name,tb.line_no FROM tbl_result t inner join tbl_teams m on m.teamid = t.teamid inner join tbl_match s on s.match_id = t.match_id inner join tbl_lines tb on m.linesid=tb.lines_id WHERE 1=1");

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND m.teamname  like '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id = '{0}'", matchid);

                if (!string.IsNullOrEmpty(lineno))
                    sql.AppendFormat(" AND tb.line_no = '{0}'", lineno);

                sql.Append(" order by t.timeline asc");

                return db.SqlQuery<tblresultView>(sql.ToString()).ToList();
            }
        }
        /// <summary>
        /// 赛事统计
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public Statistics GetStatistics(string matchid)
        {
            using (var db = new BFdbContext())
            {
                Statistics st = new Statistics();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM tbl_teams where status=0  ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND match_id = '{0}'", matchid);

                StringBuilder sql1 = new StringBuilder();
                sql1.Append("SELECT * FROM tbl_teams where  status=0  and record<>'0'  ");

                if (!string.IsNullOrEmpty(matchid))
                    sql1.AppendFormat(" AND match_id = '{0}'", matchid);

                StringBuilder sql2 = new StringBuilder();
                sql2.Append("select a.linesid,b.linename as linesname,cast(b.line_no as SIGNED INTEGER) as line_no,count(a.teamid) as count from tbl_teams a ,tbl_lines b where a.linesid=b.lines_id and a.eventid=2 and (a.record='0' or  a.record is null) ");
                if (!string.IsNullOrEmpty(matchid))
                    sql2.AppendFormat(" AND a.match_id = '{0}'", matchid);
                sql2.Append(" group by a.linesid");

                List<linesfinish> nocheckedteam = db.SqlQuery<linesfinish>(sql2.ToString()).OrderBy(o => o.line_no).ToList();

                StringBuilder sql3 = new StringBuilder();
                sql3.Append("select a.linesid,b.linename as linesname,cast(b.line_no as SIGNED INTEGER) as line_no,count(a.teamid) as count from tbl_teams a ,tbl_lines b where a.linesid=b.lines_id and a.eventid=3  ");
                if (!string.IsNullOrEmpty(matchid))
                    sql3.AppendFormat(" AND a.match_id = '{0}'", matchid);
                sql3.Append(" group by a.linesid");

                List<linesfinish> linesfinishteam = db.SqlQuery<linesfinish>(sql3.ToString()).OrderBy(o=>o.line_no).ToList();

                st.totalteam = db.SqlQuery<tblteams>(sql.ToString()).ToList().Count;
                st.checkedteam = db.SqlQuery<tblteams>(sql1.ToString()).ToList().Count;
                st.nocheckedteam = nocheckedteam;
                st.linesfinishteam = linesfinishteam;
                
                return st;
            }
           
        }

        /// <summary>
        /// 查询队伍排名
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<ranking> GetRanking(string matchid,string linesid, string teamname, string nickname, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"select b.teamname,b.teamno, u.nickname,u.mobile, '1' as valid,DATE_FORMAT(a.starttime,'%Y-%m-%d %H:%i:%s:%f') as maxtime,
                                DATE_FORMAT(a.settime,'%Y-%m-%d %H:%i:%s:%f') as mintime,timediff(a.settime,a.starttime) as total
                                from tbl_result a ");
                 sql.Append("left join tbl_teams b on a.teamid=b.teamid  "); 
                 //sql.Append("left join tbl_points c on c.lineguid = b.linesid ");
                 //sql.Append("left join tbl_match_record d on d.teamid = a.teamid and d.status = '0' ");
                 sql.Append("left join tbl_match_users u on u.teamid = a.teamid  ");
                 sql.Append("where a.starttime != '' and a.settime != '' ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND a.match_id  = '{0}'", matchid);

                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" AND b.linesid  = '{0}'", linesid);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND b.teamname  like '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(nickname))
                    sql.AppendFormat(" AND u.nickname  like '%{0}%'", nickname);

                sql.Append(" group by b.teamno,b.teamname ");
                return db.SqlQuery1<ranking, TimeSpan?>(sql.ToString(), pageindex, p => p.total);
            }
        }

        /// <summary>
        /// 未检录的队伍列表查询
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblteamsVew> GetNoCheckTeam(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select a.linesid,b.linename as linesname,a.teamid,a.teamname,a.teamno from tbl_teams a ,tbl_lines b where a.linesid=b.lines_id and a.eventid=2 and (record='0' or record is null)  ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND a.match_id = '{0}'", matchid);
                sql.AppendFormat(" group by a.linesid,a.teamid ");

                return db.SqlQuery<tblteamsVew>(sql.ToString()).ToList(); 
            }
        }

        /// <summary>
        /// 已完成队伍列表查询
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblteamsVew> GetFinishTeam(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select a.linesid,b.linename as linesname,a.teamid,a.teamname,a.teamno from tbl_teams a ,tbl_lines b where a.linesid=b.lines_id and a.eventid=3   ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND a.match_id = '{0}'", matchid);
                sql.AppendFormat(" group by a.linesid,a.teamid ");

                return db.SqlQuery<tblteamsVew>(sql.ToString()).ToList();
            }
        }

    }
}
