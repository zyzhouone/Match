using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DAL;
using Model;
using Utls;
using System.Web.Mvc;
using System.Data;

namespace BLL
{
    public class TeamBll : BaseBll
    {
        /// <summary>
        /// 查询队伍信息
        /// </summary>
        /// <param name="teamname"></param>
        /// <param name="company"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblteamsVew> GetTeams(string optMatch, string optLine, string optLines, string teamno, string teamname, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT case when isNULL(tc.team_combine_id)=1 then t.company else '【合并组队】' end as 'Company',ls.linename as linesname,u.name as nickname,t.*,u.mobile as Moblie,l.name as Linename,m.match_name as matchname FROM tbl_teams t  left join tbl_line l on l.lineid = t.lineid left join tbl_users u on u.userid = t.userid  left join tbl_match m on m.match_id = t.match_id  left join tbl_lines ls on ls.lines_id = t.linesid  left join tbl_teams_combine tc on tc.team_id=t.teamid  where t.status = 0 ");

                if (!string.IsNullOrEmpty(optMatch))
                    sql.AppendFormat(" AND t.match_id  =  '{0}'", optMatch);

                if (!string.IsNullOrEmpty(optLine))
                    sql.AppendFormat(" AND t.lineid  = '{0}'", optLine);

                if (!string.IsNullOrEmpty(optLines))
                    sql.AppendFormat(" AND t.linesid  = '{0}'", optLines);

                if (!string.IsNullOrEmpty(teamno))
                    sql.AppendFormat(" AND t.teamno  = '{0}'", teamno);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND t.teamname  like '%{0}%'", teamname);


                return db.SqlQuery1<tblteamsVew, string>(sql.ToString(), pageindex, p => p.Teamno);
            }
        }

        /// <summary>
        /// 查询未组合队伍信息
        /// zzy 2018-12-18
        /// </summary>
        /// <param name="teamname"></param>
        /// <param name="company"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblteamsGroupVew> GetOnePeoperTeams(string opttype, string optMatch, string optLines, string optAge, string teamname)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                if (opttype == "未组合")
                {
                    sql.Append(@"select *,case when (age >(YEAR(CURDATE()) -2019) and age<=(YEAR(CURDATE())-2000)) then '00' 
                            when (age >(YEAR(CURDATE()) -1999) and age<=(YEAR(CURDATE())-1990)) then '90' 
                            when (age >(YEAR(CURDATE()) -1989) and age<=(YEAR(CURDATE())-1980)) then '80' 
                            when (age >(YEAR(CURDATE()) -1979) and age<=(YEAR(CURDATE())-1970)) then '70' 
                            when (age >(YEAR(CURDATE()) -1969) and age<=(YEAR(CURDATE())-1940)) then '60' 
                            else '60' end ageBetween from (
                            select a.*,b.line_no,b.linename as linesname ,l.name as linename,
                            (select count(*) from tbl_match_users s where s.teamid=a.teamid) as peoplenum,
                            (select max(sexy) from tbl_match_users u where u.teamid=a.teamid) as sexy ,
                            (select max(age) from tbl_match_users u where u.teamid=a.teamid) as age ,
                            (select max(mobile) from tbl_match_users u where u.teamid=a.teamid) as mobile ,
                            (select max(nickname) from tbl_match_users u where u.teamid=a.teamid) as nickname ,
                            (select max(matchuserid) from tbl_match_users u where u.teamid=a.teamid) as matchuserid ,
                            '未组合' groupType,
                            '' as oprdatetime,
                            a.teamid as checkid
                            from  tbl_teams a   
                            left join tbl_line l on l.lineid = a.lineid 
                            inner join tbl_lines b on a.linesid=b.lines_id 
                            where a.status='0'  and playercount=1");
                }
                else if (opttype == "已组合")
                {
                    sql.Append(@"select *,case when (age >(YEAR(CURDATE()) -2019) and age<=(YEAR(CURDATE())-2000)) then '00' 
                            when (age >(YEAR(CURDATE()) -1999) and age<=(YEAR(CURDATE())-1990)) then '90' 
                            when (age >(YEAR(CURDATE()) -1989) and age<=(YEAR(CURDATE())-1980)) then '80' 
                            when (age >(YEAR(CURDATE()) -1979) and age<=(YEAR(CURDATE())-1970)) then '70' 
                            when (age >(YEAR(CURDATE()) -1969) and age<=(YEAR(CURDATE())-1940)) then '60' 
                            else '60' end ageBetween,
                            CONCAT(cast(man as char),'男',cast(wuman as char),'女') as Sexystring from (
                            select a.*,ls.linename as linesname ,l.name as linename,
                            (select count(*) from tbl_match_users s where s.teamid=a.teamid) as peoplenum,
                            (select count(sexy) from tbl_match_users u where u.teamid=a.teamid and sexy=1) as man ,
                            (select count(sexy) from tbl_match_users u where u.teamid=a.teamid and sexy=2) as wuman ,
                            (select max(age) from tbl_match_users u where u.teamid=a.teamid  and leader=1) as age ,
                            (select max(mobile) from tbl_match_users u where u.teamid=a.teamid  and leader=1) as mobile ,
                            (select max(nickname) from tbl_match_users u where u.teamid=a.teamid  and leader=1) as nickname ,
                            (select max(matchuserid) from tbl_match_users u where u.teamid=a.teamid) as matchuserid ,
                            '已组合' groupType,
                            date_format(oprdatetime, '%Y-%m-%d %H:%i:%s') as oprdatetime,
                            team_combine_batchno as checkid
                            from  tbl_teams a   
                            left join tbl_line l on l.lineid = a.lineid 
                            left join tbl_lines ls on ls.lines_id = a.linesid 
                            inner join tbl_teams_combine b on a.teamid=b.team_id 
                            where a.status='0' ");

                }

                if (!string.IsNullOrEmpty(optMatch))
                    sql.AppendFormat(" AND a.match_id  =  '{0}'", optMatch);

                if (!string.IsNullOrEmpty(optLines))
                    sql.AppendFormat(" AND a.linesid  = '{0}'", optLines);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND a.teamname  like '%{0}%'", teamname);

                if (opttype == "已组合")
                {
                    sql.Append(" order by oprdatetime desc");
                }

                sql.Append(@") c
                            where age is not null");


                if (!string.IsNullOrEmpty(optAge) && opttype == "未组合")
                {
                    if (optAge == "00后")
                        sql.Append(" and age >(YEAR(CURDATE()) -2019) and age<=(YEAR(CURDATE())-2000) ");
                    else if(optAge=="90后")
                        sql.Append(" and age >(YEAR(CURDATE()) -1999) and age<=(YEAR(CURDATE())-1990) ");
                    else if (optAge == "80后")
                        sql.Append(" and age >(YEAR(CURDATE()) -1989) and age<=(YEAR(CURDATE())-1980) ");
                    else if (optAge == "70后")
                        sql.Append(" and age >(YEAR(CURDATE()) -1979) and age<=(YEAR(CURDATE())-1970) ");
                    else if (optAge == "60后")
                        sql.Append(" and age >(YEAR(CURDATE()) -1969) and age<=(YEAR(CURDATE())-1940) ");

                }

                return db.SqlQuery<tblteamsGroupVew>(sql.ToString()).ToList();
                
            }
        }

        /// <summary>
        /// zzy
        /// </summary>
        /// <param name="opttype"></param>
        /// <param name="optMatch"></param>
        /// <param name="optLines"></param>
        /// <param name="optAge"></param>
        /// <param name="teamname"></param>
        /// <returns></returns>
        public List<SelectListItem> GetTeamlineList(string opttype, string optMatch, string optLines, string optAge, string teamname)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                if (opttype == "未组合")
                {
                    sql.Append(@"select lineid as value,linename as text,count(*) from (
                            select a.*,b.line_no,b.linename as linesname ,l.name as linename,
                            (select count(*) from tbl_match_users s where s.teamid=a.teamid) as peoplenum,
                            (select max(sexy) from tbl_match_users u where u.teamid=a.teamid) as sexy ,
                            (select max(age) from tbl_match_users u where u.teamid=a.teamid) as age ,
                            (select max(mobile) from tbl_match_users u where u.teamid=a.teamid) as mobile ,
                            (select max(nickname) from tbl_match_users u where u.teamid=a.teamid) as nickname ,
                            '未组合' groupType,
                            a.teamid as checkid
                            from  tbl_teams a   
                            left join tbl_line l on l.lineid = a.lineid 
                            inner join tbl_lines b on a.linesid=b.lines_id 
                            where a.status='0'  and playercount=1");
                }
                else if (opttype == "已组合")
                {
                    sql.Append(@"select lineid as value,linename as text,count(*) from (
                            select a.*,ls.linename as linesname ,l.name as linename,
                            (select count(*) from tbl_match_users s where s.teamid=a.teamid) as peoplenum,
                            (select max(sexy) from tbl_match_users u where u.teamid=a.teamid  and leader=1) as sexy ,
                            (select max(age) from tbl_match_users u where u.teamid=a.teamid  and leader=1) as age ,
                            (select max(mobile) from tbl_match_users u where u.teamid=a.teamid  and leader=1) as mobile ,
                            (select max(nickname) from tbl_match_users u where u.teamid=a.teamid  and leader=1) as nickname ,
                            '已组合' groupType,
                            team_combine_batchno as checkid
                            from  tbl_teams a   
                            left join tbl_line l on l.lineid = a.lineid 
                            left join tbl_lines ls on ls.lines_id = a.linesid 
                            inner join tbl_teams_combine b on a.teamid=b.team_id 
                            where a.status='0' ");

                }

                if (!string.IsNullOrEmpty(optMatch))
                    sql.AppendFormat(" AND a.match_id  =  '{0}'", optMatch);

                if (!string.IsNullOrEmpty(optLines))
                    sql.AppendFormat(" AND a.linesid  = '{0}'", optLines);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND a.teamname  like '%{0}%'", teamname);

                sql.Append(@") c
                            where age is not null
                            and sexy is not null");


                if (!string.IsNullOrEmpty(optAge) && opttype == "未组合")
                {
                    if (optAge == "00后")
                        sql.Append(" and age >(YEAR(CURDATE()) -2019) and age<=(YEAR(CURDATE())-2000) ");
                    else if (optAge == "90后")
                        sql.Append(" and age >(YEAR(CURDATE()) -1999) and age<=(YEAR(CURDATE())-1990) ");
                    else if (optAge == "80后")
                        sql.Append(" and age >(YEAR(CURDATE()) -1989) and age<=(YEAR(CURDATE())-1980) ");
                    else if (optAge == "70后")
                        sql.Append(" and age >(YEAR(CURDATE()) -1979) and age<=(YEAR(CURDATE())-1970) ");
                    else if (optAge == "60后")
                        sql.Append(" and age >(YEAR(CURDATE()) -1969) and age<=(YEAR(CURDATE())-1940) ");

                }
                sql.Append(" order by value ");

                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 队伍组合
        /// zzy 2018-12-19
        /// </summary>
        /// <param name="teamids"></param>
        /// <param name="optLines"></param>
        /// <param name="optLine"></param>
        /// <returns></returns>
        public string TeamsConBine(string teamids, string optLines, string optLine)
        {
            using (var db = new BFdbContext())
            {

                BFParameters bf = new BFParameters();
                bf.Add("@from_teamids", teamids);
                bf.Add("@to_team_id", "");
                bf.Add("@to_linesid", optLines);
                bf.Add("@to_lineid", optLine);
                bf.Add("@msg", null, DbType.String, ParameterDirection.Output);
                db.MysqlExecuteProcedure("sp_teamcombine", bf);
                return bf.GetOutParameter("@msg").ToString();
            }
        }

        public string TeamsConBine2(string teamid1, string teamid2, string teamid3, string teamid4, string teamid5, string optLines, string optLine)
        {
            using (var db = new BFdbContext())
            {

                BFParameters bf = new BFParameters();
                bf.Add("@from_teamid1", teamid1); 
                bf.Add("@from_teamid2", teamid2); 
                bf.Add("@from_teamid3", teamid3); 
                bf.Add("@from_teamid4", teamid4); 
                bf.Add("@from_teamid5", teamid5);
                bf.Add("@to_team_id", "");
                bf.Add("@to_linesid", optLines);
                bf.Add("@to_lineid", optLine);
                bf.Add("@msg", null, DbType.String, ParameterDirection.Output);
                db.MysqlExecuteProcedure("sp_teamcombine_ex", bf);
                return bf.GetOutParameter("@msg").ToString();
            }
        }

        /// <summary>
        /// zzy
        /// </summary>
        /// <param name="batchno"></param>
        /// <returns></returns>
        public string TeamsComBineUndo(string batchno)
        {
            using (var db = new BFdbContext())
            {

                BFParameters bf = new BFParameters();
                bf.Add("@in_batchno", batchno);
                bf.Add("@msg", null, DbType.String, ParameterDirection.Output);
                db.MysqlExecuteProcedure("sp_teamcombine_undo", bf);
                return bf.GetOutParameter("@msg").ToString();
            }
        }

        /// <summary>
        /// zzy 2018-12-31
        /// 根据手机号获取名字
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public tblmatchusers GetUserByMobile(string mobile)
        {
            using (var db = new BFdbContext())
            {
                var usr = db.tblmatchusers.FirstOrDefault(p => p.Mobile == mobile);
                if (usr == null) return null;
                else return usr;
                

            }
        }

        /// <summary>
        /// zzy 2018-12-30
        /// 替换队员
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        public string Replayer(string mobile, string mid)
        {
            using (var db = new BFdbContext())
            {
                BFParameters bf = new BFParameters();
                bf.Add("@new_mobile", mobile);
                bf.Add("@org_matchuserid", mid);
                bf.Add("@msg", null, DbType.String, ParameterDirection.Output);
                db.MysqlExecuteProcedure("sp_teamcombine_setplayer", bf);
                return bf.GetOutParameter("@msg").ToString();

                //var usr = db.tblusers.FirstOrDefault(p => p.Mobile == mobile && p.Status == 0);
                ////if (usr == null || usr.Isupt == "0")
                ////    return -4;

                //var musr = db.tblmatchusers.FirstOrDefault(p => p.Matchuserid == mid);
                //if (musr == null || (musr.Status != "1" && musr.Status != "8"))
                //    return -3;

                ////if (db.tblmatchusers.Any(p => p.Match_Id == musr.Match_Id && p.use == usr.userid && p.Status == "1"))
                ////    return -1;

                //tblmatchusers tm = new tblmatchusers();
                //tm.birthday = usr.birthday;
                //tm.Cardno = usr.cardno;
                //tm.Cardtype = usr.cardtype;
                //tm.Createtime = DateTime.Now;
                //tm.Leader = 0;
                //tm.Match_Id = musr.Match_Id;
                //tm.Matchuserid = Guid.NewGuid().ToString();
                //tm.Mobile = usr.Mobile;
                //tm.Nickname = usr.Name;
                //tm.Pay = 0;
                //tm.Sexy = int.Parse(usr.sexy);
                //tm.Status = "1";
                //tm.Teamid = musr.Teamid;
                //tm.Teamname = musr.Teamname;
                //tm.Userid = usr.userid;
                //SetYearOld(tm);

                //if (tm.Age < 16 || tm.Age > 60)
                //    return -9;

                //db.TInsert<tblmatchusers>(tm);
                //db.TDelete<tblmatchusers>(musr);

                //tblreplace tbl = new tblreplace();
                //tbl.Createtime = DateTime.Now;
                //tbl.D_Age = musr.Age;
                //tbl.D_Birthday = musr.birthday;
                //tbl.D_Cardno = musr.Cardno;
                //tbl.D_Cardtype = musr.Cardtype;
                //tbl.D_Flag = "1";
                //tbl.D_Agreetime = DateTime.Now;
                //tbl.D_Matchuserid = musr.Matchuserid;
                //tbl.D_Mobile = musr.Mobile;
                //tbl.D_Nickname = musr.Nickname;
                //tbl.D_Sexy = musr.Sexy;
                //tbl.D_Userid = musr.Userid;
                //tbl.Id = Guid.NewGuid().ToString();
                //tbl.Match_Id = musr.Match_Id;
                //tbl.S_Flag = "1";
                //tbl.S_Userid = usr.userid;
                //tbl.S_Matchuserid = tm.Matchuserid;
                //tbl.S_Agreetime = DateTime.Now;
                //tbl.Teamid = musr.Teamid;

                //db.TInsert<tblreplace>(tbl);

                //return db.SaveChanges();
            }
        }
        /// <summary>
        /// zzy 2018-12-30
        /// </summary>
        /// <param name="item"></param>
        private void SetYearOld(tblmatchusers item)
        {
            try
            {
                //if (item.Cardtype == "1")
                //{
                if (item.birthday.HasValue)
                {
                    string dy = item.birthday.Value.ToString("yyyyMMdd");
                    string nw = DateTime.Now.ToString("yyyyMMdd");
                    string m = (int.Parse(nw) - int.Parse(dy) + 1).ToString();

                    if (m.Length > 4)
                        item.Age = int.Parse(m.Substring(0, m.Length - 4));
                    else
                        item.Age = 0;
                }
                else
                    item.Age = 0;
                //}
            }
            catch (Exception ex)
            {
                item.Age = 0;
            }
        }

        /// <summary>
        /// zzy 2018-12-30
        /// 设置队长
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public int ReLeader(string mid)
        {
            using (var db = new BFdbContext())
            {
                var player = db.tblmatchusers.FirstOrDefault(p => p.Matchuserid == mid);

                var leader = db.tblmatchusers.FirstOrDefault(p => p.Leader == 1 && p.Teamid == player.Teamid);
                if (leader.Matchuserid == player.Matchuserid)
                    return -1;

                var team = db.tblteams.FirstOrDefault(p => p.teamid == leader.Teamid);

                leader.Leader = 0;
                leader.Mono = DateTime.Now.ToString("yyMMddHHmmss");

                player.Leader = 1;
                team.Userid = player.Userid;

                db.TUpdate<tblmatchusers>(leader);
                db.TUpdate<tblmatchusers>(player);
                db.TUpdate<tblteams>(team);

                return db.SaveChanges();
            }
        }


        /// <summary>
        /// 查询team
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public List<tblteams> GetTeamsBymatchid(string matchid, string linesid)
        {
            using (var db = new BFdbContext())
            {
                return db.FindAll<tblteams>(p => p.match_id == matchid && p.Linesid == linesid && p.Status == 0).ToList();
            }
        }

        /// <summary>
        /// 查询team
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public List<tblteams> CheckTeamsNo(string matchid, string teamno, string teamid)
        {
            using (var db = new BFdbContext())
            {
                return db.FindAll<tblteams>(p => p.match_id == matchid && p.Teamno == teamno && p.Status == 0 && p.teamid != teamid && p.Teamno != "0").ToList();
            }
        }


        /// <summary>
        /// 查询matchuser
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public List<tblmatchusers> GetMatchUserBymatchid(string matchid)
        {
            using (var db = new BFdbContext())
            {
                return db.FindAll<tblmatchusers>(p => p.Match_Id == matchid && p.Status == "1").ToList();
            }
        }

        /// <summary>
        /// 查询赛事分组
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public List<GroupLines> GetGroupLines(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select m.match_id as matchid,m.match_name as matchname,n.lineid,n.name as linename,l.lines_id as linesid,linename as linesname,l.line_no as linesno from tbl_lines l left join tbl_match m on m.match_id = l.match_id left join tbl_line n on n.lineid = l.line_id where 1=1  ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND l.match_id  =  '{0}'", matchid);
                return db.SqlQuery<GroupLines>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 查询队伍信息
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public List<GroupTeam> GetGroupTeams(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT t.teamid,t.teamname,t.teamno,t.lineid,t.linesid from tbl_teams t  where t.status=0  ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id  =  '{0}'", matchid);

                sql.Append(" order by t.teamno desc  ");
                return db.SqlQuery<GroupTeam>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 查询队伍信息
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public List<GroupUser> GetGrouUser(string matchid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT t.matchuserid, t.userid,t.nickname,t.mono as userno,t.leader,t.teamid from tbl_match_users t  where t.status = 1  ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id  =  '{0}'", matchid);
                sql.Append(" order by t.mono desc  ");
                return db.SqlQuery<GroupUser>(sql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 导出队伍信息
        /// </summary>
        /// <param name="teamname"></param>
        /// <param name="company"></param>
        /// <param name="status"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblteamsVew> ExportTeams(string matchname, string teamname, string status)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT t.*,u.mobile as Moblie,c.name as Companyname,l.name as Linename,m.match_name as Matchname FROM tbl_teams t left join tbl_company c on c.companyid = t.company left join tbl_line l on l.lineid = t.lineid left join tbl_users u on u.userid = t.userid  left join tbl_match m on m.match_id = t.match_id where 1=1 ");

                if (!string.IsNullOrEmpty(matchname))
                    sql.AppendFormat(" AND m.match_name like '%{0}%'", matchname);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND t.teamname like '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(status))
                    sql.AppendFormat(" AND t.status  = '{0}'", status);


                return db.SqlQuery<tblteamsVew>(sql.ToString()).ToList();
            }
        }


        //读取线路待分配队伍
        public List<tblteams> GetAssignsTeams(string matchid, string lineid, string linesid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * from tbl_teams where status=0 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND  match_id = '{0}'", matchid);

                if (!string.IsNullOrEmpty(lineid))
                    sql.AppendFormat(" AND  lineid = '{0}'", lineid);

                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" AND  linesid = '{0}'", linesid);

                sql.AppendFormat(" AND teamno not like  '_____'  order by rand() ");

                return db.SqlQuery<tblteams>(sql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 队伍队员信息
        /// </summary>
        /// <param name="teamname"></param>
        /// <param name="company"></param>
        /// <param name="status"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblmatchusers> getTeamUsers(string matchid, string teamid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM tbl_match_users  where status = '1' ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND match_id = '{0}'", matchid);

                if (!string.IsNullOrEmpty(teamid))
                    sql.AppendFormat(" AND teamid = '{0}'", teamid);

                return db.SqlQuery<tblmatchusers>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 根据线路赛事查询队伍
        /// </summary>
        /// <param name="Teamname"></param>
        /// <returns></returns>
        public List<tblteams> GetTeamsBylinesid(string matchid, string linesid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM tbl_teams  where 1=1 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND match_id = '{0}'", matchid);

                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" AND Linesid = '{0}'", linesid);

                sql.Append(" AND teamno  like '_____' AND status = '0' ");

                return db.SqlQuery<tblteams>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 根据线路赛事查询队伍
        /// </summary>
        /// <param name="Teamname"></param>
        /// <returns></returns>
        public List<tblteams> GetTeamsBylinesid1(string matchid, string linesid)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM tbl_teams  where 1=1 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND match_id = '{0}'", matchid);

                if (!string.IsNullOrEmpty(linesid))
                    sql.AppendFormat(" AND Linesid = '{0}'", linesid);

                sql.Append(" AND teamno not like '_____' AND status = '0' ");

                return db.SqlQuery<tblteams>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 根据线路赛事查询线路类型
        /// </summary>
        /// <param name="Teamname"></param>
        /// <returns></returns>
        public tbllines getLineBylinesid(string matchid, string linesid)
        {
            using (var db = new BFdbContext())
            {
                return db.tbllines.FirstOrDefault(p => p.Match_id == matchid && p.Lines_id == linesid);
            }
        }

        /// <summary>
        /// 查询公司信息
        /// </summary>
        public List<SelectListItem> GetCompany()
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT  a.companyid  as value,a.name as text FROM tbl_company a order by a.name");

                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 查询线路信息
        /// </summary>
        public List<SelectListItem> GetLine()
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.lineid as value,a.name as text FROM tbl_line a order by a.name");

                return db.SqlQuery<SelectListItem>(sql.ToString()).ToList();
            }
        }

        /// <summary>
        /// 根据teamname查询队伍
        /// </summary>
        /// <param name="Teamname"></param>
        /// <returns></returns>
        public tblteams GetTeamByName(string Teamname)
        {
            using (var db = new BFdbContext())
            {
                return db.tblteams.FirstOrDefault(p => p.Teamname == Teamname);
            }
        }

        /// <summary>
        /// 根据id查询队伍
        /// </summary>
        /// <param name="Teamname"></param>
        /// <returns></returns>
        public tblteams GetTeamById(string teamid)
        {
            using (var db = new BFdbContext())
            {
                return db.tblteams.FirstOrDefault(p => p.teamid == teamid);
            }
        }
        /// <summary>
        /// 新增队伍
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int AddTeam(tblteams ent)
        {
            using (var db = new BFdbContext())
            {
                if (db.tblteams.Any(u => u.Teamname == ent.Teamname))
                    throw new ValidException("Mobile", "此队伍名称已经存在！");
                return db.Insert<tblteams>(ent);
            }
        }

        /// <summary>
        /// 更新队伍
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int EditTeam(tblteams ent)
        {
            using (var db = new BFdbContext())
            {
                return db.Update<tblteams>(ent);
            }
        }

        /// <summary>
        /// 删除队伍
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteTeam(List<string> ids)
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
                            tblteams ent = db.tblteams.FirstOrDefault(p => p.teamid == id);
                            ent.Status = 2;
                            db.TUpdate<tblteams>(ent);
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
        /// 更新队伍线路
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public int AssignsTeams(List<AssignsTeam> AssignsTeam)
        {

            int ret = -1;
            try
            {
                using (var db = new BFdbContext())
                {
                    for (int i = 0; i < AssignsTeam.Count; i++)
                    {
                        tblteams ent = GetTeamById(AssignsTeam[i].teamid);
                        ent.Linesid = AssignsTeam[i].linesid;
                        db.TUpdate<tblteams>(ent);

                    }
                    db.SaveChanges();
                }

                ret = 0;
            }
            catch
            {
                ret = 99;
            }

            return ret;
        }

        public PagedList<tblteamsVew> GetTeamsLinedown(string matchid, string teamname, string teamno, string dstatus, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT ls.linename as linesname,u.name as nickname,t.*,u.mobile as Moblie,l.name as Linename,m.match_name as matchname FROM tbl_teams t  left join tbl_line l on l.lineid = t.lineid left join tbl_users u on u.userid = t.userid  left join tbl_match m on m.match_id = t.match_id left join tbl_lines ls on ls.lines_id = t.linesid  where t.status = 0 ");

                if (!string.IsNullOrEmpty(matchid))
                    sql.AppendFormat(" AND t.match_id  =  '{0}'", matchid);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND t.teamname  like  '{0}%'", teamname);

                if (!string.IsNullOrEmpty(teamno))
                    sql.AppendFormat(" AND t.teamno  = '{0}'", teamno);

                if (!string.IsNullOrEmpty(dstatus))
                    sql.AppendFormat(" AND t.eventid  = {0}", dstatus);


                return db.SqlQuery<tblteamsVew, DateTime?>(sql.ToString(), pageindex, p => p.Createtime);
            }
        }

        public int TeamDown(List<string> ids)
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
                            string sqlteam = string.Format("update tbl_teams set eventid=2 where teamid='{0}'", id);
                            db.ExecuteSqlCommand(sqlteam);

                            string sqlusers = string.Format("update tbl_match_users set isdown='1' where teamid='{0}'", id);
                            db.ExecuteSqlCommand(sqlusers);
                        }
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

        public int AllTeamDown(string matchid)
        {
            using (var db = new BFdbContext())
            {
                int res = 0;

                using (var tx = db.BeginTransaction())
                {
                    try
                    {
                        string sqlteam = string.Format("update tbl_teams set eventid=2 where status=0  and match_id='{0}'", matchid);
                        db.ExecuteSqlCommand(sqlteam);

                        string sqlusers = string.Format("update tbl_match_users set isdown='1' where status=1 and match_id='{0}'", matchid);
                        db.ExecuteSqlCommand(sqlusers);

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

        public string BuildSerial(string matchid)
        {
            using (var db = new BFdbContext())
            {
                BFParameters bf = new BFParameters();
                bf.Add("@i_matchid", matchid);
                bf.Add("@msg", null, DbType.String, ParameterDirection.Output);
                db.MysqlExecuteProcedure("sp_buildserial", bf);
                return bf.GetOutParameter("@msg").ToString();
            }
        }


        public string BuildSerial1(string teamid, string teamno)
        {
            using (var db = new BFdbContext())
            {
                BFParameters bf = new BFParameters();
                bf.Add("@i_teamid", teamid);
                bf.Add("@i_teamno", teamno);
                bf.Add("@msg", null, DbType.String, ParameterDirection.Output);
                db.MysqlExecuteProcedure("sp_buildserial1", bf);
                return bf.GetOutParameter("@msg").ToString();
            }
        }

        //public int BuildSerial(List<tblteams> gt, List<tblmatchusers> gu)
        //{
        //    using (var db = new BFdbContext())
        //    {
        //        int res = -1;
        //        string id = "";
        //        try
        //        {
        //            for (int i = 0; i < gu.Count; i++)
        //            {
        //                id = gu[i].Matchuserid;
        //                tblmatchusers tu = db.tblmatchusers.FirstOrDefault(p => p.Matchuserid == id);
        //                tu.Mono = gu[i].Mono;
        //                db.TUpdate<tblmatchusers>(tu);
        //            }

        //            for(int i=0;i<gt.Count;i++)
        //            {
        //                id = gt[i].teamid;
        //                tblteams tm = db.tblteams.FirstOrDefault(p => p.teamid == id);
        //                tm.Teamno = gt[i].Teamno;
        //                db.TUpdate<tblteams>(tm);
        //            }

        //            db.SaveChanges();
        //            res = 0;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //        return res;
        //    }
        //}

        public int GetLeftLinesCount(string linesid)
        {
            using (var db = new BFdbContext())
            {
                return db.tbllines.FirstOrDefault(p => p.Lines_id == linesid).Paycount.Value;
            }
        }

        public int GetLimitLinesCount(string linesid)
        {
            using (var db = new BFdbContext())
            {
                return db.tbllines.FirstOrDefault(p => p.Lines_id == linesid).Pointscount.Value;
            }
        }

        public int GetLinesTeamCount(string linesid)
        {
            using (var db = new BFdbContext())
            {
                return db.FindAll<tblteams>(p => p.Linesid == linesid && p.Status == 0).ToList().Count;
            }
        }

        public int TeamDis(TeamDisEntiyList r)
        {
            using (var db = new BFdbContext())
            {
                int res = -1;

                using (var tx = db.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < r.teams.Count; i++)
                        {
                            string id = r.teams[i].teamid;
                            tblteams team = db.tblteams.FirstOrDefault(p => p.teamid == id);
                            team.Lineid = r.lineid;
                            team.Linesid = r.linesid;
                            team.Teamno = r.teams[i].Teamno.PadLeft(5, '0');
                            db.TUpdate<tblteams>(team);
                            //BuildSerial1(id,team.Teamno);
                        }
                        db.SaveChanges();
                        tx.Commit();
                        res = 0;
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

        public string ResetSerial(string matchid, string lineid, string linesid)
        {
            string ret = "-1";

            using (var db = new BFdbContext())
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("update tbl_teams set teamno = '0' where 1=1 ");

                    if (!string.IsNullOrEmpty(matchid))
                        sql.AppendFormat(" AND match_id  =  '{0}'", matchid);

                    if (!string.IsNullOrEmpty(lineid))
                        sql.AppendFormat(" AND lineid  =  '{0}'", lineid);

                    if (!string.IsNullOrEmpty(linesid))
                        sql.AppendFormat(" AND linesid  =  '{0}'", linesid);

                    db.ExecuteSqlCommand(sql.ToString());
                    ret = "0";
                }
                catch { }

            }

            return ret;
        }

        public int GetMaxTeamno(string matchid, string linesid)
        {
            int ret = -1;
            try
            {
                using (var db = new BFdbContext())
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("SELECT max(t.teamno) as maxteamno  from tbl_teams t  where t.status=0  ");

                    if (!string.IsNullOrEmpty(matchid))
                        sql.AppendFormat(" AND t.match_id  =  '{0}'", matchid);

                    if (!string.IsNullOrEmpty(matchid))
                        sql.AppendFormat(" AND t.linesid  =  '{0}'", linesid);

                    var no = db.SqlQuery<MaxTeamnoEntity>(sql.ToString()).FirstOrDefault().maxteamno;

                    if (no == null || no == "" || no == "0")
                    {
                        ret = 0;
                    }
                    else
                    {
                        ret = Int32.Parse(no);
                    }
                }

            }
            catch
            {
                ret = -2;
            }

            return ret;
        }

        //public int TeamDis(List<TeamDisEntiyList> td)
        //{
        //    using (var db = new BFdbContext())
        //    {
        //        int res = -1;

        //        using (var tx = db.BeginTransaction())
        //        {
        //            try
        //            {
        //                foreach (TeamDisEntiyList r in td)
        //                {
        //                    for (int i = 0; i < r.teams.Count; i++)
        //                    {
        //                        string id = r.teams[i].teamid;
        //                        tblteams team = db.tblteams.FirstOrDefault(p => p.teamid == id);
        //                        team.Lineid = r.lineid;
        //                        team.Linesid = r.linesid;
        //                        db.TUpdate<tblteams>(team);
        //                    }
        //                }
        //                db.SaveChanges();
        //                tx.Commit();
        //                res = 0;
        //            }
        //            catch (Exception ex)
        //            {
        //                tx.Rollback();
        //                throw ex;
        //            }
        //        }

        //        return res;
        //    }
        //}
    }
}