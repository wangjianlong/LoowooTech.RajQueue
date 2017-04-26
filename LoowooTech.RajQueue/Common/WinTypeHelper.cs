using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace LoowooTech.RajQueue.Common
{
    public static class WinTypeHelper
    {
        private static XmlDocument configXml { get; set; }
        public static Dictionary<string,string> Dict { get; private set; }
        static WinTypeHelper()
        {
            configXml = new XmlDocument();
            configXml.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["TABLE_WINTYPE_FILE"]));
            Dict = GetWinTypes();
        }

        private static Dictionary<string,string> GetWinTypes()
        {
            var dict = new Dictionary<string, string>();
            var nodes = configXml.SelectNodes("/WinTypes/WinType");
            if (nodes != null)
            {
                for(var i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    dict.Add(node.Attributes["Value"].Value, node.Attributes["Title"].Value);
                }
            }
            return dict;
        }
    }
}