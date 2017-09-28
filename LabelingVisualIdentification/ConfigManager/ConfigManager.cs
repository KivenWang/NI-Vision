using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabelingVisualIdentification
{
    public class ConfigManager
    {
        private static UserProgramConfig _userPrograms;
        public static UserProgramConfig UserPrograms
        {
            get
            {
                if (_userPrograms == null)
                {
                    string configPath = string.Format(@"{0}UserPrograms.xml", AppDomain.CurrentDomain.BaseDirectory);
                    _userPrograms = ObjectXmlSerializer<UserProgramConfig>.Load(configPath);
                }

                return _userPrograms;
            }
            set { _userPrograms = value; }
        }
        
    }
    
}
