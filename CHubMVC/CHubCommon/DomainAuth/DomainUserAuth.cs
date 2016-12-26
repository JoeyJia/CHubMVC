using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    public class DomainUserAuth
    {
        public static bool IsAuthenticated(string domain, string username, string pwd)
        {
            string pathCur = "LDAP://RootDSE";

            DirectoryEntry ent = new DirectoryEntry(pathCur);
            String str = ent.Properties["defaultNamingContext"][0].ToString();


            string strPath = "LDAP://" + str;
            String domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(strPath, domainAndUsername, pwd);

            try
            {    //Bind to the native AdsObject to force authentication.            
                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                //DirectoryEntry deUser = result.GetDirectoryEntry();
                if (null == result)
                {
                    return false;
                }

                //strPath = result.Path;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


    }
}
