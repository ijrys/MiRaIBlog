using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiRaI.Blog.CoreVer._1_0.Models;

namespace MiRaI.Blog.CoreVer._1_0.Controllers {
	public class MPageController: Controller {
		public IActionResult Index() {
			string showstr = "[empty]";
			showstr = ConfigurationManager.ConnectionStrings["usercheckcr"]?.ConnectionString;
			ViewBag.constr = showstr;
			return View();
		}

		public IActionResult About() {
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact() {
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
