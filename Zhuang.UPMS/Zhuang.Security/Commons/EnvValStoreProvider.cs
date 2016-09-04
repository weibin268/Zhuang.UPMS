using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Data.Common;

namespace Zhuang.Security.Commons
{
    public class EnvValStoreProvider : Zhuang.Data.EnvironmentVariable.IEnvValStoreProvider
    {
        public Dictionary<string, object> GetEnvironmentVariables()
        {
            var dicResult = new Dictionary<string, object>();

            var security = new MyEnvFunc((c) =>
            {

                var args = c.Split(',');

                var elementCode = args[0];

                var defaultValue = args.Length > 1 ? args[1] : "";

                object result =SecurityContext.Current.PermissionManager.GetElementRule(elementCode);

                if (result == null)
                {
                    result = defaultValue;
                }

                return result;
            });

            dicResult.Add("Security", security);
            dicResult.Add("security", security);

            return dicResult;

        }
    }
}
