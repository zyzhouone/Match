using System;
using System.Linq;
using System.Collections.Generic;

using DAL;
using Model;

namespace BLL
{
    public class AuthBll : BaseBll
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public sysmatchuser Login(string name, string pwd)
        {
            using (BFdbContext db = new BFdbContext())
            {
                IEnumerable<sysmatchuser> users = db.FindAll<sysmatchuser>(p => p.Account == name && p.Pwd == pwd && p.Status == 0);
                if (users.Count() < 1)
                    return null;
                else
                {
                    sysmatchuser usr = users.First();
                    return usr;
                }
            }
        }
    }
}
