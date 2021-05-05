//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using Our.Umbraco.ScheduledContentDashboard.Contracts;
using Our.Umbraco.ScheduledContentDashboard.Models;
using Umbraco.Core.Models;

namespace Our.Umbraco.ScheduledContentDashboard.Mappers
{
    /// <summary>
    /// Implementation of an <see cref="IObjectMapper{TFrom, TTo}"/> in support of mapping between Umbraco content and Scheduled Results
    /// </summary>
    public class ContentToScheduledContentMapper : IObjectMapper<Tuple<ContentScheduleAction, IEnumerable<IContent>>, IEnumerable<ScheduledContentModel>>
    {
        /// <summary>
        /// Map from one instance of an object to another
        /// </summary>
        /// <param name="from">Object instance to convert from</param>
        /// <returns>Mapped object</returns>
        public IEnumerable<ScheduledContentModel> Map( Tuple<ContentScheduleAction, IEnumerable<IContent>> from )
        {
            // Validate the request
            Ensure.Any.IsNotNull( from, nameof( from ) );
            
            // Project the results based on the request into the required model
            return from.Item2.SelectMany( x => x.ContentSchedule.FullSchedule.Where( s => s.Action == from.Item1 ).Select( s => new ScheduledContentModel()
            {
                Id = s.Id,
                ContentId = x.Id,
                Name = x.Name,
                Action = from.Item1.ToString(),
                ScheduledDate = DateTime.SpecifyKind( s?.Date ?? default, DateTimeKind.Local ),
                Culture = s.Culture
            } ) );
        }
    }
}