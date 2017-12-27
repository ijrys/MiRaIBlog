using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo {
	class Program {
		static void DoFun (object oname) {
			string name = (string) oname;
			string connstr = "server=172.16.1.100,2356;uid=sa;pwd=1234567890hh;database=SQLDemo";
			SqlConnection conn = new SqlConnection(connstr);
			using (SqlCommand cmd = conn.CreateCommand()) {
				cmd.CommandText = $"EXECUTE [dbo].[Demo] @pname = N'{name}'";
				conn.Open();
				string restr = cmd.ExecuteScalar().ToString();
				Console.WriteLine(restr + " " + name);
			}
		}

		static void Main(string[] args) {
			Thread th1 = new Thread(new ParameterizedThreadStart(DoFun));
			Thread th2 = new Thread(new ParameterizedThreadStart(DoFun));
			th1.Start("张三");
			Thread.Sleep(20);
			th2.Start("李四");


			Console.ReadLine();
		}
	}
}
