using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class CommerceContextExtensions
    {
        public static bool AddPolicyKeys(this CommerceContext commerceContext, string[] policyKeys)
        {
            if (commerceContext.Headers == null || policyKeys == null || !policyKeys.Any<string>())
                return false;
            StringValues source1;
            if (commerceContext.Headers.TryGetValue("PolicyKeys", out source1))
            {
                string str = source1.FirstOrDefault<string>();
                List<string> stringList;
                if (str == null)
                    stringList = (List<string>)null;
                else
                    stringList = ((IEnumerable<string>)str.Split('|')).ToList<string>();
                if (stringList == null)
                    stringList = new List<string>();
                List<string> source2 = stringList;
                source2.AddRange((IEnumerable<string>)policyKeys);
                commerceContext.Headers["PolicyKeys"] = (StringValues)string.Join("|", source2.Distinct<string>());
            }
            else
                commerceContext.Headers["PolicyKeys"] = (StringValues)string.Join("|", ((IEnumerable<string>)policyKeys).Distinct<string>());
            return true;
        }
    }
}