//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using EnsureThat;
using Microsoft.AspNetCore.Routing;
using Our.Umbraco.ScheduledContentDashboard.Contracts;
using Our.Umbraco.ScheduledContentDashboard.Controllers;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Infrastructure.WebAssets;
using Umbraco.Extensions;

namespace Our.Umbraco.ScheduledContentDashboard.Startup
{
    /// <summary>
    /// Implementation of <see cref="INotificationHandler<ServerVariablesParsingNotification >"/> for event handling
    /// </summary>
    public class ScheduledContentDashboardNotificationHandler : INotificationHandler<ServerVariablesParsingNotification>
    {
        /// <summary>
        /// Reference to the link generator
        /// </summary>
        private readonly LinkGenerator _linkGenerator;

        /// <summary>
        /// Initializes a new instance of the ScheduledContentDashboardNotificationHandler class
        /// </summary>
        /// <remarks>
        /// The default constructor initializes any fields to their default values.
        /// </remarks>
        /// <param name="linkGenerator">Reference to the link generator</param>
        public ScheduledContentDashboardNotificationHandler( LinkGenerator linkGenerator )
        {
            // Validate the request
            Ensure.Any.IsNotNull( linkGenerator, nameof( linkGenerator ) );

            // Store the provided references away
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Handles a notification
        /// </summary>
        /// <param name="notification">The notification</param>
        public void Handle( ServerVariablesParsingNotification notification )
        {
            // Validate the request
            Ensure.Any.IsNotNull( notification, nameof( notification ) );

            // Build a dictionary of server variables for this package
            Dictionary<string, object> urlDictionary = new Dictionary<string, object>
            {
                {
                Char.ToLowerInvariant( nameof( ScheduledContentDashboardController.GetScheduledContent )[0] ) + nameof( ScheduledContentDashboardController.GetScheduledContent ).Substring( 1 ),
                _linkGenerator.GetUmbracoApiService<ScheduledContentDashboardController>( nameof( ScheduledContentDashboardController.GetScheduledContent ) )
                },
                {
                Char.ToLowerInvariant( nameof( ScheduledContentDashboardController.DeleteScheduleEntry )[0] ) + nameof( ScheduledContentDashboardController.DeleteScheduleEntry ).Substring( 1 ),
                _linkGenerator.GetUmbracoApiService<ScheduledContentDashboardController>( nameof( ScheduledContentDashboardController.DeleteScheduleEntry ) )
                }
            };

            // Add them to the main collection of variables
            notification.ServerVariables.Add( PackageConstants.PackageName, urlDictionary );
        }
    }
}