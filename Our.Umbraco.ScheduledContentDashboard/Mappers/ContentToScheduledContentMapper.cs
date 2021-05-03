//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Our.Umbraco.ScheduledContentDashboard.Contracts;
using Our.Umbraco.ScheduledContentDashboard.Models;
using Umbraco.Core.Models;

namespace Our.Umbraco.ScheduledContentDashboard.Mappers
{
    /// <summary>
    /// Implementation of an <see cref="IObjectMapper{TFrom, TTo}"/> in support of mapping between Umbraco content and Scheduled Results
    /// </summary>
    public class ContentToScheduledContentMapper : IObjectMapper<IEnumerable<IContent>, IEnumerable<ScheduledContentModel>>
    {
        /// <summary>
        /// Map from one instance of an object to another
        /// </summary>
        /// <param name="from">Object instance to convert from</param>
        /// <returns>Mapped object</returns>
        public IEnumerable<ScheduledContentModel> Map( IEnumerable<IContent> from )
        {
            // Project the results based on the request into the required model
            return from.SelectMany( x => x.ContentSchedule.FullSchedule.Select( s => new ScheduledContentModel()
            {
                Id = s.Id,
                ContentId = x.Id,
                Name = x.Name,
                Action = s.Action.ToString(),
                ScheduledDate = DateTime.SpecifyKind( s?.Date ?? default, DateTimeKind.Local ),
                Culture = s?.Culture
            } ) );
        }
    }
}