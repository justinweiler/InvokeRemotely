using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace InvokeRemotely
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setting up remote powershell (all cmds as admin)
            // Enable-PSRemoting -Force -SkipNetworkCheck
            // Set-Item wsman:\localhost\client\trustedhosts COMPUTERZ
            // Restart-Service WinRM
            // Test-WsMan COMPUTERZ
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                // add a script that creates a new instance of an object from the caller's namespace
                PowerShellInstance.AddScript(@"Invoke-Command -ComputerName JRWMBP2 -ScriptBlock {Get-ChildItem C:\ > C:\list.txt}");
                
                // invoke execution on the pipeline (collecting output)
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                foreach (PSObject outputItem in PSOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null
                    // object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        Console.WriteLine(outputItem.ToString());
                    }
                }

                Console.ReadLine();
            }
        }
    }
}
