//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EnsureThat;
using Our.Umbraco.ScheduledContentDashboard.Contracts;
using Our.Umbraco.ScheduledContentDashboard.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
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
        /// Reference to the logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Reference to the content service
        /// </summary>
        private readonly IContentService _contentService;

        /// <summary>
        /// Reference to the object mapper
        /// </summary>
        private readonly IObjectMapper<Tuple<ContentScheduleAction, IEnumerable<IContent>>, IEnumerable<ScheduledContentModel>> _mapper;

        /// <summary>
        /// Initializes a new instance of the ScheduledContentDashboardController class
        /// </summary>
        /// <remarks>
        /// The default constructor initializes any fields to their default values.
        /// </remarks>
        /// <param name="logger">Reference to the logger</param>
        /// <param name="contentService">Reference to the content service</param>
        /// <param name="mapper">Reference to the object mapper</param>
        public ScheduledContentDashboardController( ILogger logger, IContentService contentService, IObjectMapper<Tuple<ContentScheduleAction, IEnumerable<IContent>>, IEnumerable<ScheduledContentModel>> mapper )
        {
            // Validate the request
            Ensure.Any.IsNotNull( logger, nameof( logger ) );
            Ensure.Any.IsNotNull( contentService, nameof( contentService ) );
            Ensure.Any.IsNotNull( mapper, nameof( mapper ) );

            // Store the provided references away
            _logger = logger;
            _contentService = contentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve the content items that are scheduled for release
        /// </summary>
        /// <param name="sortAscending">Sort direction</param>
        /// <returns>Collection containing the content that is scheduled if any else an empty collection</returns>
        [HttpGet]
        public IHttpActionResult GetScheduledContent( string orderBy, string orderDirection )
        {
            Ensure.Any.HasValue<string>( orderBy, nameof( orderBy ) );
            Ensure.Any.HasValue<string>( orderDirection, nameof( orderDirection ) );

            _logger.Info<ScheduledContentDashboardController>( $"Scheduled content requested, sort column: {orderBy}, direction: {orderDirection}" );

            // Retrieve the content that is scheduled for release and map the results
            IEnumerable<IContent> results = _contentService.GetContentForRelease( DateTime.MaxValue );
            IEnumerable<ScheduledContentModel> model = _mapper.Map( new Tuple<ContentScheduleAction, IEnumerable<IContent>>( ContentScheduleAction.Release, results ) );

            // Retrieve the content that is scheduled for expiration and add to the results
            results = _contentService.GetContentForExpiration( DateTime.MaxValue );
            model = model.Concat( _mapper.Map( new Tuple<ContentScheduleAction, IEnumerable<IContent>>( ContentScheduleAction.Expire, results ) ) );

            // Order the results
            // TODO - apply the column selection via reflective lookup on the class against PropertyName of JsonProperty attribute
            model = orderDirection == "asc" ? model.OrderBy( x => x.ScheduledDate ) : model.OrderByDescending( x => x.ScheduledDate );

            // Return the requested data set
            return Ok( model );
        }

        /// <summary>
        /// Retrieve the content items that are scheduled for release
        /// </summary>
        /// <param name="sortAscending">Sort direction</param>
        /// <returns>Collection containing the content that is scheduled if any else an empty collection</returns>
        [HttpGet]
        public IHttpActionResult DeleteScheduleEntry( int contentId, ContentScheduleAction scheduleAction, DateTime scheduleEntryDate )
        {
            Ensure.Any.HasValue<int>( contentId, nameof( contentId ) );
            Ensure.Any.HasValue<DateTime>( scheduleEntryDate, nameof( scheduleEntryDate ) );

            _logger.Info<ScheduledContentDashboardController>( $"Schedule entry removal requested, Content Id: {contentId}, Action {scheduleAction}, Date: {scheduleEntryDate}" );

            // Retrieve the content that is scheduled for release
            IContent content = _contentService.GetById( contentId );

            // Clear the specific schedule entry and persist the change
            content.ContentSchedule.Clear( scheduleAction, scheduleEntryDate );
            _contentService.Save( content );

            return Ok();
        }
    }
}