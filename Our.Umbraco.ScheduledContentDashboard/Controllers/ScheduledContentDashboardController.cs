//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Our.Umbraco.ScheduledContentDashboard.Contracts;
using Our.Umbraco.ScheduledContentDashboard.Mappers;
using Our.Umbraco.ScheduledContentDashboard.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Our.Umbraco.ScheduledContentDashboard.Controllers
{
    /// <summary>
    /// Implementation of a <see cref="UmbracoAuthorizedJsonController"/> to provide content in support of the Scheduled Content Dashboard
    /// </summary>
    [IsBackOffice]
    [PluginController( PackageConstants.PackageName )]
    public class ScheduledContentDashboardController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Reference to the content service
        /// </summary>
        private readonly IContentService _contentService;

        /// <summary>
        /// Reference to the object mapper
        /// </summary>
        private readonly IObjectMapper<Tuple<string, IEnumerable<IContent>>, IEnumerable<ScheduledContentModel>> _mapper;

        /// <summary>
        /// Initializes a new instance of the ScheduledContentDashboardController class
        /// </summary>
        /// <remarks>
        /// The default constructor initializes any fields to their default values.
        /// </remarks>
        public ScheduledContentDashboardController()
        {
            // Store the provided references away
            _contentService = Services.ContentService;
            _mapper = new ContentToScheduledContentMapper();
        }

        /// <summary>
        /// Retrieve the content items that are scheduled for release
        /// </summary>
        /// <returns>Collection containing the content that is scheduled if any else an empty collection</returns>
        [HttpGet]
        public IHttpActionResult GetScheduledContent()
        {
            // Retrieve the content that is scheduled for release and map the results
            List<IContent> results = new List<IContent>();
            _contentService.GetRootContent().ToList().ForEach( i =>
            {
                if( i.ReleaseDate.HasValue )
                {
                    results.Add( i );
                }

                IEnumerable<IContent> items = _contentService.GetDescendants( i ).Where( x => x.ReleaseDate.HasValue );
                if( items.Any() )
                {
                    results.AddRange( items );
                }
            } );
            IEnumerable<ScheduledContentModel> model = _mapper.Map( new Tuple<string, IEnumerable<IContent>>( PackageConstants.Release, results ) );

            // Retrieve the content that is scheduled for expiration and add to the results
            results = new List<IContent>();
            _contentService.GetRootContent().ToList().ForEach( i =>
            {
                if( i.ExpireDate.HasValue )
                {
                    results.Add( i );
                }

                IEnumerable<IContent> items = _contentService.GetDescendants( i ).Where( x => x.ExpireDate.HasValue );
                if( items.Any() )
                {
                    results.AddRange( items );
                }
            } );
            model = model.Concat( _mapper.Map( new Tuple<string, IEnumerable<IContent>>( PackageConstants.Expire, results ) ) );

            // Return the requested data set
            return Ok( model );
        }
    }
}