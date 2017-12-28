using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MiRaI.Blog.CoreVer._1_0.Controllers {
	public class APIController: Controller {
		public string LogIn () {
			bool fromcookie = false;
			string uname = Request.Query["username"];
			string upwd = Request.Query["userpwd"];

			if (string.IsNullOrEmpty(uname) ||
				string.IsNullOrEmpty(upwd)) {
				uname = Request.Cookies["username"];
				upwd = Request.Cookies["userpwd"];
				fromcookie = true;
			}
			
			
			return "OK";
		}
	}
}