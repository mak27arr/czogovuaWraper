using System;
using System.Collections.Generic;
using System.Text;

namespace czogovuaWraper
{
    interface ICzoGovUaWraper
    {
        public int Init(int time_out_ms, string server_name);
        public void ShutDown();
        public bool SingFile(string file_patch,string sing_patch,string sing_password,string sing_name);
        public IEnumerable<ISingInfo> CheakFileSing(string file_patch);
    }
}
