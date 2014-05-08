using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Spoon.Standalone.Connector
{
    public static class SnapshotManager
    {
        public static async Task<ContentResult> GetSnapshotAsync(string escapedFragment, string azureStorageAccount, string azureStorageContainer)
        {
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
