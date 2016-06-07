using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.BAL.Manuscript.Impersonation
{
    public class Impersonate
    {
         private int LOGON32_LOGON_INTERACTIVE = 2;

    private int LOGON32_PROVIDER_DEFAULT = 0;

    private WindowsImpersonationContext impersonationContext;
    [DllImport("advapi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    private static extern int LogonUserA(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    private static extern int DuplicateToken(IntPtr ExistingTokenHandle, int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    private static extern long RevertToSelf();
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    private static extern long CloseHandle(IntPtr handle);
        private bool _impersonationActive;
    /// <summary>
    /// Allows creation of a new instance without starting impersonation
    /// </summary>
    /// <remarks></remarks>
    public Impersonate()
    {
    }

    /// <summary>
    /// starts impersonation with specified parameters
    /// </summary>
    /// <param name="userName">username to impersonate</param>
    /// <param name="domain">domain for user</param>
    /// <param name="password">password for user</param>
    /// <remarks></remarks>
    public Impersonate(string userName, string domain, string password)
    {
        //StartImpersonation(userName, domain, password)
    }

    /// <summary>
    /// starts impersonation with specified parameters
    /// </summary>
    /// <param name="userName">username to impersonate</param>
    /// <param name="domain">domain for user</param>
    /// <param name="password">password for user</param>
    /// <remarks></remarks>
    public bool StartImpersonation(string userName, string domain, string password)
    {
        bool ReturnValue = false;
        WindowsIdentity tempWindowsIdentity = null;
        IntPtr token = IntPtr.Zero;
        IntPtr tokenDuplicate = IntPtr.Zero;
        if (Convert.ToBoolean(RevertToSelf()))
        {
            if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if ((impersonationContext != null))
                    {
                        ReturnValue = true;
                        _impersonationActive = true;
                    }
                }
            }
        }
        if (!tokenDuplicate.Equals(IntPtr.Zero))
        {
            CloseHandle(tokenDuplicate);
        }
        if (!token.Equals(IntPtr.Zero))
        {
            CloseHandle(token);
        }
        return ReturnValue;
    }

    /// <summary>
    /// Ends impersonation session and returns thread back to original user
    /// </summary>
    /// <remarks></remarks>
    public void EndImpersonation()
    {
        impersonationContext.Undo();
        _impersonationActive = false;
    }
    public bool ImpersonationActive
    {
        get { return _impersonationActive; }
    }

    }
}
