using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiRaI.Blog.CoreVer.ToolsModels;
using MiRaI.Blog.CoreVer.Models;

namespace MiRaI.Blog.CoreVer._1_0.Controllers {
	public class APIController: Controller {
		public string LogIn () {
			bool fromcookie = false;
			string uname = Request.Query["username"];
			string upwd = Request.Query["userpwd"];

			if (string.IsNullOrEmpty(uname) &&
				string.IsNullOrEmpty(upwd)) {
				uname = Request.Cookies["username"];
				upwd = Request.Cookies["userpwd"];
				fromcookie = true;
			}

			int uid = SqlTools.UCheckUser(uname, upwd);
			string restr;
			if (uid < 1) {
				if (fromcookie) return "NO cookie ";
				else return "NO query ";
			} else {
				if (fromcookie) {
					restr = "OK cookie ";
				} else {
					restr = "OK query ";
				}
			}
			restr += uid.ToString();
			return restr;
		}

		public string R_CheckAccount (string account) {
			if (SqlTools.UCheckAAccount(account)) return "OK";
			return "NO";
		}
		public string R_CheckEmail (string email) {
			if (!email.Contains('@')) return "NOT";
			if (SqlTools.UCheckAEmail(email)) return "OK";
			return "NO";
		}
		public string R_CheckPhone (string phone) {
			if (SqlTools.UCheckAPhone(phone)) return "OK";
			return "NO";
		}

		public string A_GetArticle (int artid) {
			/*
{
	"article": {
		"artid": 1,
		"contid": 2,
		"ownerid": 3,
		"state": 4
	}
}
*/
			Response.ContentType = "application/json";
			Article art = new Article()
		}
	}
}