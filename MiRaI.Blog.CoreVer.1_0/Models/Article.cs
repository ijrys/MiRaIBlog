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
		int _contentID = -1;
		Int16 _state = -1;

		int[] _history = null;
		#endregion
		#region 属性
		public int ArticleID { get => _articleID; }
		public int OwnerID { get { if (_needInit) InitFromSQL(); return _ownerID; } }
		public int ContentID { get { if (_needInit) InitFromSQL(); return _contentID; } }
		public short State { get { if (_needInit) InitFromSQL(); return _state; } }
		public int[] History {
			get {
				if (_history == null) {

				}
				return _history;
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
		public bool Init(int ownid, int contentid, Int16 state) {
			if (ownid < 1 || contentid < 1 || state < 0) return false;
			_ownerID = ownid;
			_contentID = contentid;
			_state = state;
			_needInit = false;
			return true;
		}

		public int[] GetHistory () {
			return MiRaI.Blog.CoreVer.ToolsModels.SqlTools.AGetHistory(this.ArticleID);
		}
		#endregion
	}
}
