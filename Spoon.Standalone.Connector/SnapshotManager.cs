﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Spoon.Standalone.Connector
{
    /// <summary>
    /// Serves page snapshots generated by Spoon Standalone.
    /// </summary>
    public static class SnapshotManager
    {
        /// <summary>
        /// Asynchronously retrieves a snapshot for a given _escaped_fragment_ value from an Azure storage container.
        /// </summary>
        /// <param name="escapedFragment">The _escaped_fragment_ value </param>
        /// <param name="azureStorageAccount">The name of the Azure storage account used to store the snapshots.</param>
        /// <param name="azureStorageContainer">The name of the Azure storage container used to store the snapshots.</param>
        /// <returns>A ContentResult object containing the HTML of the generated snapshot.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the _escaped_framgent_ is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the Azure storage account name or storage container name is null or whitespace.</exception>
        /// <exception cref="HttpRequestException">Thrown when no snapshot exists for the given _escaped_fragment_.</exception>
        public static async Task<ContentResult> GetSnapshotAsync(string escapedFragment, string azureStorageAccount, string azureStorageContainer)
        {
            if (escapedFragment == null)
                throw new ArgumentNullException("escapedFragment");
            if (string.IsNullOrWhiteSpace(azureStorageAccount))
                throw new ArgumentException("azureStorageAccount");
            if (string.IsNullOrWhiteSpace(azureStorageContainer))
                throw new ArgumentException("azureStorageContainer");

            if (escapedFragment == "/")
                escapedFragment = "";

            var blobUrl = GetSnapshotUrl(escapedFragment, azureStorageAccount, azureStorageContainer);
            using (var client = new HttpClient())
            {
                return new ContentResult
                {
                    Content = await client.GetStringAsync(blobUrl),
                    ContentType = "text/html"
                };
            }
        }

        static string GetSnapshotUrl(string escapedFragment, string azureStorageAccount, string azureStorageContainer)
        {
            return string.Format(
                "http://{0}.blob.core.windows.net/{1}/_escaped_fragment_={2}.html",
                azureStorageAccount,
                azureStorageContainer,
                escapedFragment);
        }
    }
}