using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiRaI.Blog.CoreVer.Models {
	public class Article {
		#region 私有字段
		bool _needInit = true;
		int _articleID;
		int _ownerID = -1;
		DateTime _createDate = DateTime.MinValue;
		int _contentID = -1;
		Int16 _state = -1;

		int[] _history = null;

		ArticleContent _content = null;
		#endregion
		#region 属性
		public int ArticleID { get => _articleID; }
		public int OwnerID { get { if (_needInit) InitFromSQL(); return _ownerID; } }
		public DateTime CreateDate { get { if (_needInit) InitFromSQL(); return _createDate; } }
		public int ContentID { get { if (_needInit) InitFromSQL(); return _contentID; } }
		public short State { get { if (_needInit) InitFromSQL(); return _state; } }
		public int[] History {
			get {
				if (_history == null) {

				}
				return _history;
			}
		}

		/// <summary>
		/// 获取ArticleContent
		/// </summary>
		public ArticleContent Content {
			get {
				if (_content == null) {
					_content = new ArticleContent(ContentID);
				}
				return Content;
			}
		}
		#endregion

		#region 初始化相关
		public bool InitFromSQL() {
			return MiRaI.Blog.CoreVer.ToolsModels.SqlTools.AInitAArticle(this);
		}
		/// <summary>
		/// 使用现有数据初始化
		/// </summary>
		/// <param name="ownid">作者id</param>
		/// <param name="contentid">内容块id</param>
		/// <param name="state">状态码</param>
		/// <returns>若所有数据均有效，则初始化，返回true，否则返回false</returns>
		public bool Init(int ownid, int contentid, DateTime createdate, Int16 state) {
			if (ownid < 1 || contentid < 1 || createdate == DateTime.MinValue || state < 0) return false;
			_ownerID = ownid;
			_contentID = contentid;
			_createDate = createdate;
			_state = state;
			_needInit = false;
			return true;
		}
		/// <summary>
		/// 使用现有数据初始化
		/// </summary>
		/// <param name="ownid">作者id</param>
		/// <param name="contentid">内容块id</param>
		/// <param name="state">状态码</param>
		/// <returns>若所有数据均有效，则初始化，返回true，否则返回false</returns>
		public bool Init(int ownid, ArticleContent content, DateTime createdate, Int16 state) {
			if (ownid < 1 || content == null || createdate == DateTime.MinValue || state < 0) return false;
			_ownerID = ownid;
			_contentID = content.ContentID;
			_content = content;
			_createDate = createdate;
			_state = state;
			_needInit = false;
			return true;
		}

		public int[] GetHistoryID () {
			return MiRaI.Blog.CoreVer.ToolsModels.SqlTools.AGetHistoryID(this.ArticleID);
		}

		public ArticleContent[] GetHistory () {
			return MiRaI.Blog.CoreVer.ToolsModels.SqlTools.AGetHistory(this.ArticleID);
		}
		#endregion

		#region 构造函数
		public Article (int artid) {
			_articleID = artid;
		}

		public Article (int artid, int ownid, int contentid, DateTime createdate, Int16 state) : this(artid) {
			Init(ownid, contentid, createdate, state);
		}
		public Article(int artid, int ownid, ArticleContent content, DateTime createdate, Int16 state) : this(artid) {
			Init(ownid, content, createdate, state);
		}
		#endregion
	}
}
