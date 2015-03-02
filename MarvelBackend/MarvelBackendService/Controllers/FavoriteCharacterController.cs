using MarvelBackendService.DataObjects;
using MarvelBackendService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Notifications;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace MarvelBackendService.Controllers
{
    public class FavoriteCharacterController : TableController<FavoriteCharacter>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MarvelBackendContext context = new MarvelBackendContext();
            DomainManager = new EntityDomainManager<FavoriteCharacter>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/FavoriteCharacter
        public IQueryable<FavoriteCharacter> GetAllFavoriteCharacter()
        {
            var currentUser = User as ServiceUser;

            return Query().Where(favoriteCharacter => favoriteCharacter.UserId == currentUser.Id);
        }

        // GET tables/FavoriteCharacter/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<FavoriteCharacter> GetFavoriteCharacter(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/FavoriteCharacter/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<FavoriteCharacter> PatchFavoriteCharacter(string id, Delta<FavoriteCharacter> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/FavoriteCharacter
        public async Task<IHttpActionResult> PostFavoriteCharacter(FavoriteCharacter favoriteCharacter)
        {
            var currentUser = User as ServiceUser;
            favoriteCharacter.UserId = currentUser.Id;

            FavoriteCharacter current = await InsertAsync(favoriteCharacter);

            SendPushAsync(string.Format("Character {0} inserted", favoriteCharacter.CharacterId), currentUser.Id);

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/FavoriteCharacter/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteFavoriteCharacter(string id)
        {
            return DeleteAsync(id);
        }

        private async void SendPushAsync(string message, string userId)
        {
            // http://azure.microsoft.com/en-us/documentation/articles/mobile-services-how-to-use-multiple-clients-single-service/#push            

            // Define una notificación push para WNS
            WindowsPushMessage wnsMessage = new WindowsPushMessage();
            wnsMessage.XmlPayload = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                                 @"<toast><visual><binding template=""ToastText02"">" +
                                 @"<text id=""1"">MarvelApp</text>" +
                                 @"<text id=""2"">" + message + @"</text>" +
                                 @"</binding></visual></toast>";

            // Define una notificación push para MPNS
            MpnsPushMessage mpnsMessage = new MpnsPushMessage(
                new Toast() { Text1 = "MarvelApp", Text2 = message });

            // Define una notificación push para GCM
            GooglePushMessage gcmMessage = new GooglePushMessage(
                new Dictionary<string, string>() { { "MarvelApp", message } }, TimeSpan.FromHours(1));

            // Envía todas las notificaciones
            try
            {
                var wnsResult = await Services.Push.SendAsync(wnsMessage, userId);
                var mpnsResult = await Services.Push.SendAsync(mpnsMessage, userId);
                var gcmResult = await Services.Push.SendAsync(gcmMessage, userId);

            }
            catch (System.Exception ex)
            {
                Services.Log.Error(ex.Message, null, "Push.SendAsync Error");
            }
        }
    }
}