using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.DataContext
{
    /// <summary>
    /// Ignora cualquier posible conflicto de sincronización con Azure Mobile Services
    /// </summary>
    public class MarvelBackendSyncHandler : MobileServiceSyncHandler
    {
        public async override Task<JObject> ExecuteTableOperationAsync(IMobileServiceTableOperation operation)
        {
            try
            {
                return await base.ExecuteTableOperationAsync(operation);
            }
            catch (MobileServiceConflictException ex)
            {
                Debug.WriteLine(string.Format("MarvelBackendSyncHandler.ExecuteTableOperationAsync error: {0}", ex.Message));
                throw;
            }
        }

        public override Task OnPushCompleteAsync(MobileServicePushCompletionResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.Status == HttpStatusCode.Conflict)
                {
                    error.CancelAndUpdateItemAsync(error.Result);
                    error.Handled = true;
                }
            }
            return base.OnPushCompleteAsync(result);
        }
    }
}

