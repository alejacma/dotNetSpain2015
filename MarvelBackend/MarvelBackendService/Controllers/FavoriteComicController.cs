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
    public class FavoriteComicController : TableController<FavoriteComic>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MarvelBackendContext context = new MarvelBackendContext();
            DomainManager = new EntityDomainManager<FavoriteComic>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/FavoriteComic
        public IQueryable<FavoriteComic> GetAllFavoriteComic()
        {
            var currentUser = User as ServiceUser;

            return Query().Where(favoriteComic => favoriteComic.UserId == currentUser.Id);
        }

        // GET tables/FavoriteComic/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<FavoriteComic> GetFavoriteComic(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/FavoriteComic/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<FavoriteComic> PatchFavoriteComic(string id, Delta<FavoriteComic> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/FavoriteComic
        public async Task<IHttpActionResult> PostFavoriteComic(FavoriteComic favoriteComic)
        {
            var currentUser = User as ServiceUser;
            favoriteComic.UserId = currentUser.Id;

            FavoriteComic current = await InsertAsync(favoriteComic);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/FavoriteComic/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteFavoriteComic(string id)
        {
            return DeleteAsync(id);
        }

    }
}