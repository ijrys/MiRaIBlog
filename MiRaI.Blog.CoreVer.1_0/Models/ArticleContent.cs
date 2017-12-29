using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MiRaI.Blog.CoreVer.ToolsModels;

namespace MiRaI.Blog.CoreVer.Models {
	public class ArticleContent: IComparable<ArticleContent>, IEquatable<ArticleContent> {
		/// <summary>
		/// 存储content文件所在文件夹
		/// </summary>
		static string _contentFileDir = null;
		public static string ContentFileDir {
			get {
				if (_contentFileDir == null) {
					_contentFileDir = Tools.AppSetting("contentFilePath");
				}
				return _contentFileDir;
			}
		}

		#region 私有字段
		bool _needInit = true;
		int _contentID;
		int _articleID = -1;
		string _title = null;
		string _saveFile = null;
		Int16 _createRea = -1;
		DateTime _createDate = DateTime.MinValue;
		Int16 _state = -1;
		string _content = null;
		#endregion

		#region 属性
		/// <summary>
		/// 内容ID
		/// </summary>
		public int ContentID { get => _contentID; }
		/// <summary>
		/// 所属文章ID
		/// </summary>
		public int ArticleID { get { if (_needInit) InitFromSQL(); return _articleID; } }
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get { if (_needInit) InitFromSQL(); return _title; } }
		/// <summary>
		/// 存储文件路径
		/// </summary>
		public string SaveFile { get { if (_needInit) InitFromSQL(); return _saveFile; } }
		/// <summary>
		/// 创建原因
		/// </summary>
		public Int16 CreateRea { get { if (_needInit) InitFromSQL(); return _createRea; } }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateDate { get { if (_needInit) InitFromSQL(); return _createDate; } }
		/// <summary>
		/// 状态码
		/// </summary>
		public short State { get { if (_needInit) InitFromSQL(); return _state; } }
		/// <summary>
		/// 内容
		/// </summary>
		public string Content {
			get {
				if (_content == null) { //读取文件
					if (SaveFile == null) return null;
					if (ContentFileDir == null) return null;
					try {
						_content = File.ReadAllText(ContentFileDir + _saveFile);
					} catch (Exception ex) {
#if DEBUG
						Console.WriteLine(ex.Message);
#endif
						_content = null;
					}
				}
				return _content;
			}
		}
		#endregion

		#region 初始化相关
		/// <summary>
		/// 从数据库初始化数据
		/// </summary>
		/// <returns></returns>
		public bool InitFromSQL() {
			return SqlTools.CInitAContent(this);
		}

		/// <summary>
		/// 使用现有有效数据初始化
		/// </summary>
		/// <param name="articleid">所属文章id</param>
		/// <param name="title">标题</param>
		/// <param name="savefile">存储文件</param>
		/// <param name="createrea">创建原因码</param>
		/// <param name="createdate">创建时间</param>
		/// <returns>若所有数据均有效，则初始化，返回true，否则返回false</returns>
		public bool Init(int articleid, string title, string savefile, Int16 createrea, DateTime createdate, Int16 state) {
			if (articleid < 1 ||
				title == null ||
				savefile == null ||
				createdate == DateTime.MinValue ||
				state < 0) return false;
			_articleID = articleid;
			_title = title;
			_saveFile = savefile;
			_createRea = createrea;
			_createDate = createdate;
			_state = state;
			_needInit = false;
			return true;
		}
		#endregion

		#region 构造函数
		public ArticleContent(int countid) {
			_contentID = countid;
		}

		public ArticleContent(int countid, int articleid, string title, string savefile, Int16 createrea, DateTime createdate, Int16 state) : this(countid) {
			Init(articleid, title, savefile, createrea, createdate, state);
		}
		#endregion

		#region 功能相关
		public int CompareTo(ArticleContent other) {
			return ContentID - other.ContentID;
		}

		public bool Equals(ArticleContent other) {
			return (_contentID == other._contentID &&
					_articleID == other._articleID &&
					_title == other._title &&
					_saveFile == other._saveFile &&
					_createRea == other._createRea &&
					_createDate == other._createDate &&
					_state == other._state);
		}
		#endregion
	}
}
