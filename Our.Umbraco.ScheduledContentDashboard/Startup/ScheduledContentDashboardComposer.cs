//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using EnsureThat;
using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.ScheduledContentDashboard.Contracts;
using Our.Umbraco.ScheduledContentDashboard.Mappers;
using Our.Umbraco.ScheduledContentDashboard.Models;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Infrastructure.WebAssets;

namespace Our.Umbraco.ScheduledContentDashboard.Startup
{
    /// <summary>
    /// Implementation of <see cref="IUserComposer"/> to support the package
    /// </summary>
    public class ScheduledContentDashboardComposer : IUserComposer
    {
        /// <summary>
        /// Compose callback for the user composer
        /// </summary>
        /// <param name="composition">Composition engine</param>
        public void Compose( IUmbracoBuilder builder )
        {
            // Validate the request
            Ensure.Any.IsNotNull( builder, nameof( builder ) );

            // Mappers
            builder.Services.AddScoped<IObjectMapper<Tuple<ContentScheduleAction, IEnumerable<IContent>>, IEnumerable<ScheduledContentModel>>, ContentToScheduledContentMapper>();
            builder.AddNotificationHandler<ServerVariablesParsingNotification, ScheduledContentDashboardNotificationHandler>();
        }
    }
}