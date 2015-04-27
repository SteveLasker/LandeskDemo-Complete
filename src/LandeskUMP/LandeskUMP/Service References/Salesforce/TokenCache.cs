using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using static LandeskUMP.Salesforce.SalesforceService;

namespace LandeskUMP.Salesforce
{
    class TokenEntry : TableEntity
    {
        public TokenEntry(string cacheKey, string nameIdentifier)
        {
            this.PartitionKey = cacheKey;
            this.RowKey = nameIdentifier;
        }

        public TokenEntry()
        { }

        /// <summary>
        /// Gets or sets the access token that can be used to make authenticated Salesforce service requests.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token that can be used to obtain a new access token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the Salesforce Service Url that was used to obtain this token.
        /// </summary>
        public string InstanceUrl { get; set; }

        /// <summary>
        /// Gets or sets the Salesforce API version that this token is valid for.
        /// </summary>
        public string ApiVersion { get; set; }
    }

    public class TokenCache
    {
        CloudTable cacheTable;
        string cacheKey;
        const string cacheTableName = "cacheTable";

        public TokenCache(string cacheKey)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["landeskdemotokenstore_AzureStorageConnectionString"]);
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist.
            this.cacheTable = tableClient.GetTableReference(cacheTableName);
            this.cacheTable.CreateIfNotExists();
            this.cacheKey = cacheKey;
        }

        private static string GetNameIdentifier(IPrincipal user)
        {
            string nameIdentifier = null;
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            ClaimsPrincipal claimsPrincipal = user as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                // Prefer to use the NameIdentifier as a user key
                Claim claim = claimsPrincipal.FindFirst(System.IdentityModel.Claims.ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    nameIdentifier = claim.Value;
                }
            }
            // If claim cannot be found, use the identity name instead
            if (nameIdentifier == null)
            {
                nameIdentifier = user.Identity.Name;
            }
            return nameIdentifier;
        }

        private TokenEntry GetEntry(IPrincipal user)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<TokenEntry>(this.cacheKey, GetNameIdentifier(user));
            TableResult retrievedResult = cacheTable.Execute(retrieveOperation);

            TokenEntry tokenEntry = (TokenEntry)retrievedResult.Result;
            return tokenEntry;
        }
        public void AddToCache(IPrincipal user, SalesforceToken token)
        {
            if (GetEntry(user) != null)
            {
                RemoveFromCache(user);
            }
            TokenEntry entry = new TokenEntry(this.cacheKey, GetNameIdentifier(user));
            entry.AccessToken = TokenEncryption.Encrypt(token.AccessToken);
            entry.RefreshToken = TokenEncryption.Encrypt(token.RefreshToken);
            entry.InstanceUrl = token.InstanceUrl;
            entry.ApiVersion = token.ApiVersion;
            TableOperation insertOperation = TableOperation.Insert(entry);
            cacheTable.Execute(insertOperation);
        }

        public SalesforceToken GetFromCache(IPrincipal user)
        {
            SalesforceToken token = null;
            TokenEntry entry = this.GetEntry(user);
            if (entry != null)
            {
                string accessToken = TokenEncryption.Decrypt(entry.AccessToken);
                string refreshToken = TokenEncryption.Decrypt(entry.RefreshToken);
                token = new SalesforceToken(accessToken, refreshToken, entry.InstanceUrl, entry.ApiVersion);
            }
            return token;
        }

        public void RemoveFromCache(IPrincipal user)
        {
            TokenEntry entry = this.GetEntry(user);
            if (entry != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(entry);
                cacheTable.Execute(deleteOperation);
            }
        }
    }

}