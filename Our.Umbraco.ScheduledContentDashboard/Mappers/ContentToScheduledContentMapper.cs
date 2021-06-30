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
    public class ContentToScheduledContentMapper : IObjectMapper<Tuple<string, IEnumerable<IContent>>, IEnumerable<ScheduledContentModel>>
    {
        /// <summary>
        /// Map from one instance of an object to another
        /// </summary>
        /// <param name="from">Object instance to convert from</param>
        /// <returns>Mapped object</returns>
        public IEnumerable<ScheduledContentModel> Map( Tuple<string, IEnumerable<IContent>> from )
        {
            // Validate the request
            Ensure.Any.IsNotNull( from, nameof( from ) );

            // Project the results based on the request into the required model
            return from.Item2.Where( x => from.Item1 == PackageConstants.Release ? x.ReleaseDate.HasValue : x.ExpireDate.HasValue ).Select( s => new ScheduledContentModel()
            {
                Id = s.Key,
                ContentId = s.Id,
                Name = s.Name,
                Action = from.Item1.ToString(),
                ScheduledDate = DateTime.SpecifyKind( from.Item1 == PackageConstants.Release ? s.ReleaseDate.Value: s.ExpireDate.Value, DateTimeKind.Local )
            } );
        }
    }
}