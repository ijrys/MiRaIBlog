using System;
using System.Collections;
using System.Collections.Generic;

namespace MiRaI.Configuration {
	public class KeyValuePairs {
		Dictionary<string, string> _kvps;
		public string this[string key] {
			get {
				if (_kvps.ContainsKey(key)) return _kvps[key];
				return null;
			}
		}
	}
	public class ConfigurationManager {
		private string _filePath;
		private ConfigurationManager _appsettings = null;

		public ConfigurationManager LocalSettings {
			get {
				if (_appsettings == null) {
					_appsettings = new ConfigurationManager("MiRaIApp.config");
				}

				return _appsettings;
			}
		}
		

		KeyValuePair _appSettings {

		}

		public ConfigurationManager (string filepath) {
			_filePath = filepath;
		}
	}
}
