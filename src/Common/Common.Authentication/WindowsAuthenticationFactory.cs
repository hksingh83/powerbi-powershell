﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class WindowsAuthenticationFactory : IAuthenticationFactory
    {
        private static bool authenticatedOnce = false;
        private static object tokenCacheLock = new object();

        private static TokenCache Cache { get; set;}

        public bool AuthenticatedOnce { get => authenticatedOnce; }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new NotSupportedException("Authenticator only works on Windows");
            }

#if !DEBUG
            LoggerCallbackHandler.UseDefaultLogging = false;
#endif
            string queryParamString = queryParameters.ToQueryParameterString();
            if(Cache == null)
            {
                lock(tokenCacheLock)
                {
                    if(Cache == null)
                    {
                        Cache = InitializeCache(environment, queryParamString);
                    }
                }
            }

            if(!this.AuthenticatedOnce)
            {
                throw new AuthenticationException("Failed to authenticate once");
            }

            var context = new AuthenticationContext(environment.AzureADAuthority, Cache);
            AuthenticationResult token = null;
            try
            {
                token = context.AcquireTokenSilentAsync(environment.AzureADResource, environment.AzureADClientId).Result;
            }
            catch(AdalSilentTokenAcquisitionException)
            {
                // ignore and try one more time by getting a new cache and let the exception fly if it fails
                lock (tokenCacheLock)
                {
                    Cache = InitializeCache(environment, queryParamString);
                }

                context = new AuthenticationContext(environment.AzureADAuthority, Cache);
                token = context.AcquireTokenSilentAsync(environment.AzureADResource, environment.AzureADClientId).Result;
            }

            return token.ToIAccessToken();
        }

        private static TokenCache InitializeCache(IPowerBIEnvironment environment, string queryParams)
        {
            using (var windowAuthProcess = new Process())
            {
                var executingDirectory = GetExecutingDirectory();

                windowAuthProcess.StartInfo.FileName = Path.Combine(executingDirectory, "WindowsAuthenticator", "AzureADWindowsAuthenticator.exe");
                windowAuthProcess.StartInfo.Arguments = $"-Authority:\"{environment.AzureADAuthority}\" -Resource:\"{environment.AzureADResource}\" -ID:\"{environment.AzureADClientId}\" -Redirect:\"{environment.AzureADRedirectAddress}\" -Query:\"{queryParams}\"";
                windowAuthProcess.StartInfo.UseShellExecute = false;
                windowAuthProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                windowAuthProcess.StartInfo.RedirectStandardOutput = true;

                windowAuthProcess.Start();
                var result = windowAuthProcess.StandardOutput.ReadToEnd();
                windowAuthProcess.WaitForExit();

                if (windowAuthProcess.ExitCode != 0)
                {
                    throw new AdalException("0", "Failed to get ADAL token"); // TODO beter message and read from output of console Error stream
                }

                authenticatedOnce = true;
                var tokeCacheBytes = Convert.FromBase64String(result);
                return new TokenCache(tokeCacheBytes);
            }
        }

        private static string GetExecutingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var fileUri = new UriBuilder(codeBase);
            var directory = Uri.UnescapeDataString(fileUri.Path);
            directory = Path.GetDirectoryName(directory);
            return directory;
        }

        public void Challenge()
        {
            if (Cache != null)
            {
                lock (tokenCacheLock)
                {
                    if (Cache != null)
                    {
                        authenticatedOnce = false;
                        Cache = null;
                    }
                }
            }
        }
    }
}