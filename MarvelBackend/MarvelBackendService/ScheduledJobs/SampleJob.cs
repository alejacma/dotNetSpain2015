using MarvelBackendService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.ScheduledJobs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace MarvelBackendService
{
    // A simple scheduled job which can be invoked manually by submitting an HTTP
    // POST request to the path "/jobs/sample".

    // The following scheduled job purges soft deleted records that are more than a month old
    public class SampleJob : ScheduledJob
    {
        private MarvelBackendContext context;

        protected override void Initialize(ScheduledJobDescriptor scheduledJobDescriptor,
            CancellationToken cancellationToken)
        {
            base.Initialize(scheduledJobDescriptor, cancellationToken);
            context = new MarvelBackendContext();
        }

        public override Task ExecuteAsync()
        {
            Services.Log.Info("Purging old records");

            var monthAgo = DateTimeOffset.UtcNow.AddDays(-30);

            var charactersToDelete = context.FavoriteCharacters.Where(x => x.Deleted == true && x.UpdatedAt <= monthAgo).ToArray();
            context.FavoriteCharacters.RemoveRange(charactersToDelete);

            var comicsToDelete = context.FavoriteComics.Where(x => x.Deleted == true && x.UpdatedAt <= monthAgo).ToArray();
            context.FavoriteComics.RemoveRange(comicsToDelete);

            var creatorsToDelete = context.FavoriteCreators.Where(x => x.Deleted == true && x.UpdatedAt <= monthAgo).ToArray();
            context.FavoriteCreators.RemoveRange(creatorsToDelete);

            context.SaveChanges();

            return Task.FromResult(true);
        }
    }
}