using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model;
using BLL;
using Utls;

namespace Web.Areas.Auth.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult ModifyPwd()
        {
            sysmatchuser usr = new sysmatchuser();
            return View(usr);
        }

        //
        // GET: /Auth/Account/
        [HttpPost]
        public ActionResult ModifyPwd(sysmatchuser model)
        {
            if (model.NewPassword == model.Pwd)
            {
                ModelState.AddModelError("NewPassword", "新密码与旧密码一致，修改失败");
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "密码输入不一致");
                return View(model);
            }

            string oldpwd = model.Pwd;
            string newpwd = model.NewPassword;

            var bll = new UserBll();
            model = bll.GetMatchUser(base.UserInfo.Id);

            if (model.Pwd != oldpwd)
            {
                ModelState.AddModelError("Password", "旧密码输入错误");
                return View(model);
            }

            model.Pwd = newpwd;

            try
            {
                bll.Update<sysmatchuser>(model);
            }
            catch (ValidException ex)
            {
                this.ModelState.AddModelError(ex.Name, ex.Message);
                return View(model);
            }

            return this.RefreshParent();
        }

        //
        // GET: /Auth/Account/Details/5
        [AllowAnonymous]
        public ActionResult Logout()
        {
            UserLoginInfo.Clear();
            return RedirectToAction("Login", "Login", new { Area = "Auth" });
        }

        [AllowAnonymous]
        public ActionResult ErrorA()
        {
            return View();
        }
    }
}
