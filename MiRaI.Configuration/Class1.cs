using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MiRaI.Configuration {
	public class ConfigKeyValuePairs {
		Dictionary<string, string> _kvps;
		public string this[string key] {
			get {
				if (_kvps.ContainsKey(key)) return _kvps[key];
				return null;
			}
		}
	}
	public static class MiRaIConfigManager {
		ConfigKeyValuePairs _appSettings = null;
		ConfigKeyValuePairs _connectionStrings = null;


	}


}
