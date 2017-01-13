using CHubDBEntity;
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
        public static APP_USERS IsAuthenticated(string domain, string username, string pwd)
        {
            #region for debug get domain path
            //string pathCur = "LDAP://RootDSE";
            //DirectoryEntry ent = new DirectoryEntry(pathCur);
            //String str = ent.Properties["defaultNamingContext"][0].ToString();

            //string strPathTmp = "LDAP://" + str;
            #endregion

            string strPath = @"LDAP://DC=ced,DC=corp,DC=cummins,DC=com";
            String domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(strPath, domainAndUsername, pwd);

            try
            {    //Bind to the native AdsObject to force authentication.            
                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return null;
                }

                APP_USERS user = new APP_USERS();
                DirectoryEntry deUser = result.GetDirectoryEntry();
                if (deUser == null)
                    return null;
                user.FIRST_NAME = (deUser.Properties["givenName"]).Value.ToString();
                user.EMAIL_ADDR = (deUser.Properties["mail"]).Value.ToString();

                return user;
            }
            catch
            {
                return null;
            }

            
        }


    }
}
