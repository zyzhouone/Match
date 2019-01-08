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
    public class OrderBll : BaseBll
    {
        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="matchname"></param>
        /// <param name="teamname"></param>
        /// <param name="status"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public PagedList<tblordersView> GetOrders(string teamname, string moblie, string status, int pageindex)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select m.match_name as Matchname,t.teamname as Teamname,o.mobile as Mobile,l.name as Linename,o.*  from(SELECT o.*,u.mobile  from tbl_orders o ,tbl_users u  where u.userid = o.userid) o left join tbl_match m on m.match_id = o.match_id left join tbl_teams t on t.teamid = o.teamid  left join tbl_line l on l.lineid = o.lineid  where 1=1 ");

                if (!string.IsNullOrEmpty(moblie))
                    sql.AppendFormat(" AND o.mobile like '%{0}%'", moblie);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND t.teamname like '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(status))
                    sql.AppendFormat(" AND o.status  = '{0}'", status);


                return db.SqlQuery<tblordersView, DateTime?>(sql.ToString(), pageindex, p => p.Createtime);
            }
        }

        /// <summary>
        /// 导出订单信息
        /// </summary>
        /// <param name="teamname"></param>
        /// <param name="company"></param>
        /// <param name="status"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<tblordersView> ExportOrders(string teamname, string moblie, string status)
        {
            using (var db = new BFdbContext())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT m.match_name as Matchname,t.teamname as Teamname,u.mobile as Mobile,l.name as Linename,o.* FROM tbl_orders o left join tbl_match m on m.match_id = o.match_id left join tbl_teams t on t.teamid = o.teamid left join tbl_users u on u.userid = o.userid left join tbl_line l on l.lineid = o.lineid where 1=1 ");

                if (!string.IsNullOrEmpty(moblie))
                    sql.AppendFormat(" AND u.mobile like '%{0}%'", moblie);

                if (!string.IsNullOrEmpty(teamname))
                    sql.AppendFormat(" AND t.teamname like '%{0}%'", teamname);

                if (!string.IsNullOrEmpty(status))
                    sql.AppendFormat(" AND o.status  = '{0}'", status);


                return db.SqlQuery<tblordersView>(sql.ToString()).ToList();
            }
        }

      

    }
}
