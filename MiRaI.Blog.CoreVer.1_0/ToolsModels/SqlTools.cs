using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using MiRaI.Blog.CoreVer.Models;

namespace MiRaI.Blog.CoreVer.ToolsModels {
	public static class SqlTools {
		/// <summary>
		/// 获取一个ConnectionString
		/// </summary>
		/// <param name="key">name</param>
		/// <returns>value，若不存在则返回null</returns>
		public static string ConnectionString(string key) {
			//value = ConfigurationManager.ConnectionStrings[key]?.ConnectionString;
			string value = null;
			if (string.IsNullOrEmpty(key)) return null;
			try {
				value = ConfigurationManager.ConnectionStrings[key].ConnectionString;
			} catch {
				return null;
			}
			return value;
		}

		#region 用户相关
		private static string _usermanageconnstr = null;
		/// <summary>
		/// 用于用户验证的连接字符串
		/// </summary>
		public static string Usermanageconnstr {
			get {
				if (_usermanageconnstr == null) {
					_usermanageconnstr = ConnectionString("usercheckconnstr");
				}
				return _usermanageconnstr;
			}
		}

		/// <summary>
		/// 验证用户
		/// </summary>
		/// <param name="account">用户名</param>
		/// <param name="userpwd">密码</param>
		/// <returns>
		/// 用户id
		/// -1:验证未通过
		/// -2:配置文件出错
		/// -3:数据库读取出错
		/// -4:其他错误
		/// </returns>
		public static int UCheckUser(string account, string userpwd) {
			if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(userpwd)) return -1;
			if (Usermanageconnstr == null) return -2;
			SqlConnection conn = null;
			int re = -1;
			try {
				conn = new SqlConnection(Usermanageconnstr);
				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = "select [ID] from [dbo].[User] where [Account] = @uname and [Password] = @pwd";
				cmd.Parameters.AddWithValue("@uname", account);
				cmd.Parameters.AddWithValue("@pwd", userpwd);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read()) {
					if (re != -1) { re = -1; break; }
					re = int.Parse(reader["ID"].ToString());
				}
				reader.Close();
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return -3;
			}
			finally {
				if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
			}

			return re;
		}

		/// <summary>
		/// 检查帐号是否可用【与目前帐号和邮箱地址及手机号冲突】
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public static bool UCheckAAccount(string account) {
			if (string.IsNullOrWhiteSpace(account)) return false;

			if (Usermanageconnstr == null) return false;
			SqlConnection conn = null;
			int count = -1;
			try {
				conn = new SqlConnection(Usermanageconnstr);
				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = "select count(*) from [dbo].[User] where [Account] = @account or [Email] = @account or [Phone] = @account;";
				cmd.Parameters.AddWithValue("@account", account);

				conn.Open();
				string restr = cmd.ExecuteScalar().ToString();
				count = int.Parse(restr);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return false;
			}
			finally {
				if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
			}

			return count == 0;
		}

		/// <summary>
		/// 检查email是否可用
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public static bool UCheckAEmail(string email) {
			if (string.IsNullOrWhiteSpace(email)) return false;

			if (Usermanageconnstr == null) return false;
			SqlConnection conn = null;
			int count = -1;
			try {
				conn = new SqlConnection(Usermanageconnstr);
				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = "select count(*) from [dbo].[User] where [Email] = @account";
				cmd.Parameters.AddWithValue("@account", email);

				conn.Open();
				string restr = cmd.ExecuteScalar().ToString();
				count = int.Parse(restr);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return false;
			}
			finally {
				if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
			}

			return count == 0;
		}

		/// <summary>
		/// 检查手机号是否重复
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		public static bool UCheckAPhone(string phone) {
			if (string.IsNullOrWhiteSpace(phone)) return false;

			if (Usermanageconnstr == null) return false;
			SqlConnection conn = null;
			int count = -1;
			try {
				conn = new SqlConnection(Usermanageconnstr);
				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = "select count(*) from [dbo].[User] where [Phone] = @account";
				cmd.Parameters.AddWithValue("@account", phone);

				conn.Open();
				string restr = cmd.ExecuteScalar().ToString();
				count = int.Parse(restr);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return false;
			}
			finally {
				if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
			}

			return count == 0;
		}
		#endregion

		private static string _blogmanageconnstr = null;
		/// <summary>
		/// 用于博客管理的连接字符串
		/// </summary>
		public static string Blogmanageconnstr {
			get {
				if (_blogmanageconnstr == null) {
					_blogmanageconnstr = ConnectionString("blogconnstr");
				}
				return _blogmanageconnstr;
			}
		}

		#region 文章相关
		/// <summary>
		/// 初始化一个文章内容块
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static bool CInitAContent(ArticleContent content) {
			if (content == null || content.ContentID < 1) return false;
			if (Blogmanageconnstr == null) return false;
			SqlConnection conn = null;
			conn = new SqlConnection(Blogmanageconnstr);

			using (SqlCommand cmd = conn.CreateCommand()) {
				cmd.CommandText = "select [ArtID], [Title], [FileName], [CreateDate], [CreateRea], [State] from [dbo].[ContentSet] where [ContentID] = @contid and [Delflag] = 0";
				cmd.Parameters.AddWithValue("@contid", content.ContentID);
				conn.Open();
				using (SqlDataReader reader = cmd.ExecuteReader()) {
					if (!reader.HasRows) return false;

					reader.Read();
					int aid = int.Parse(reader["ArtID"].ToString());
					string tit = reader["Title"].ToString();
					string fname = reader["FileName"].ToString();
					DateTime cdate = DateTime.Parse(reader["CreateDate"].ToString());
					Int16 crea = Int16.Parse(reader["CreateRea"].ToString());
					Int16 state = Int16.Parse(reader["State"].ToString());

					return content.Init(aid, tit, fname, crea, cdate, state);
				}
			}
		}
		#endregion

		#region 评论相关

		#endregion
	}
}
