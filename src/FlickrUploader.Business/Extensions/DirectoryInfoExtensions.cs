using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FlickrUploader.Business.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static bool HasWritePermisssion(this DirectoryInfoBase dirInfo)
        {
            try
            {
                var currentUserSecurityIdsArray = GetCurrentUserSecurityIdentifierArray();

                var directorySecurity = dirInfo.GetAccessControl();

                foreach (AuthorizationRule rule in directorySecurity.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    if (currentUserSecurityIdsArray.Contains(rule.IdentityReference.Value))
                    {
                        FileSystemAccessRule rights = ((FileSystemAccessRule)rule);
                        if (rights.AccessControlType == AccessControlType.Allow)
                        {
                            if (rights.FileSystemRights == (rights.FileSystemRights | FileSystemRights.Modify))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // NOTE: copied and adjusted from here http://web3.codeproject.com/Articles/771385/How-to-find-a-users-effective-rights-on-a-file
        private static string[] GetCurrentUserSecurityIdentifierArray()
        {
            // use WindowsIdentity to get the user's groups
            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
            string[] sids = new string[currentUser.Groups.Count + 1];

            sids[0] = currentUser.User.Value;

            for (int index = 1, total = currentUser.Groups.Count; index < total; index++)
            {
                sids[index] = currentUser.Groups[index].Value;
            }

            return sids;
        }
    }
}
