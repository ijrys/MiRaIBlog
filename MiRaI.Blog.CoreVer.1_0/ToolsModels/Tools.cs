using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace MiRaI.Blog.CoreVer.ToolsModels {
	public static class Tools {
		/// <summary>
		/// 获取一个appsetting
		/// </summary>
		/// <param name="key">name</param>
		/// <returns>value，若不存在则返回null</returns>
		public static string AppSetting(string key) {
			//value = ConfigurationManager.ConnectionStrings[key]?.ConnectionString;
			string value = null;
			if (string.IsNullOrEmpty(key)) return null;
			try {
				value = ConfigurationManager.AppSettings[key];
			} catch {
				return null;
			}
			return value;
		}

	}
}
