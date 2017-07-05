using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabelingVisualIdentification
{
    public class ConfigManager
    {
        private static UserPrograms _userPrograms;
        public static UserPrograms UserPrograms
        {
            get
            {
                if (_userPrograms == null)
                {
                    string configPath = string.Format(@"{0}UserPrograms.xml", AppDomain.CurrentDomain.BaseDirectory);
                    _userPrograms = ObjectXmlSerializer<UserPrograms>.Load(configPath);
                }

                return _userPrograms;
            }
            set { _userPrograms = value; }
        }
    }
}
