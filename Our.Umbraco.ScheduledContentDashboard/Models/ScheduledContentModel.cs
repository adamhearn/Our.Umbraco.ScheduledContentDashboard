//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using Newtonsoft.Json;

namespace Our.Umbraco.ScheduledContentDashboard.Models
{
    /// <summary>
    /// Declares the model for an individual scheduled content
    /// </summary>
    public class ScheduledContentModel
    {
        /// <summary>
        /// Gets or sets the content schedule id
        /// </summary>
        /// <remarks>
        /// Umbraco's unique id for the schedule entry
        /// </remarks>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        /// <remarks>
        /// Umbraco's unique id for the content
        /// </remarks>
        [JsonProperty( PropertyName = "contentId" )]
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the item
        /// </summary>
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time at which the content action is scheduled
        /// </summary>
        [JsonProperty( PropertyName = "scheduledDate" )]
        public DateTime ScheduledDate { get; set; }

        /// <summary>
        /// Gets or sets the action type
        /// </summary>
        [JsonProperty( PropertyName = "action" )]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the culture type
        /// </summary>
        [JsonProperty( PropertyName = "culture" )]
        public string Culture { get; set; }
    }
}