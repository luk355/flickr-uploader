using System.IO.Abstractions;
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
                var security = dirInfo.GetAccessControl();
                SecurityIdentifier users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                foreach (AuthorizationRule rule in security.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    if (rule.IdentityReference == users)
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
    }
}
