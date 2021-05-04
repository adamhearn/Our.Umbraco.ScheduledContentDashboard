//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Our.Umbraco.ScheduledContentDashboard.Contracts;
using Our.Umbraco.ScheduledContentDashboard.Mappers;
using Our.Umbraco.ScheduledContentDashboard.Models;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;

namespace Our.Umbraco.ScheduledContentDashboard.Composers
{
    /// <summary>
    /// Implementation of <see cref="IUserComposer"/> to support the package
    /// </summary>
    [RuntimeLevel( MinLevel = RuntimeLevel.Run )]
    public class ScheduledContentDashboardComposer : IUserComposer
    {
        /// <summary>
        /// Compose callback for the user composer
        /// </summary>
        /// <param name="composition">Composition engine</param>
        public void Compose( Composition composition )
        {
            // Mappers
            composition.Register<IObjectMapper<Tuple<ContentScheduleAction, IEnumerable<IContent>>, IEnumerable<ScheduledContentModel>>, ContentToScheduledContentMapper>();

            // Ensure the component is added to the start up chain
            composition.Components().Append<ScheduledContentDashboardComponent>();
        }
    }
}