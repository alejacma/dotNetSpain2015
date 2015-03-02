using MarvelBackendService.DataObjects;
using MarvelBackendService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace MarvelBackendService.Controllers
{
    public class FavoriteCreatorController : TableController<FavoriteCreator>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MarvelBackendContext context = new MarvelBackendContext();
            DomainManager = new EntityDomainManager<FavoriteCreator>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/FavoriteCreator
        public IQueryable<FavoriteCreator> GetAllFavoriteCreator()
        {
            var currentUser = User as ServiceUser;

            return Query().Where(favoriteCreator => favoriteCreator.UserId == currentUser.Id);
        }

        // GET tables/FavoriteCreator/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<FavoriteCreator> GetFavoriteCreator(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/FavoriteCreator/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<FavoriteCreator> PatchFavoriteCreator(string id, Delta<FavoriteCreator> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/FavoriteCreator
        public async Task<IHttpActionResult> PostFavoriteCreator(FavoriteCreator favoriteCreator)
        {
            var currentUser = User as ServiceUser;
            favoriteCreator.UserId = currentUser.Id;

            FavoriteCreator current = await InsertAsync(favoriteCreator);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/FavoriteCreator/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteFavoriteCreator(string id)
        {
            return DeleteAsync(id);
        }

    }
}