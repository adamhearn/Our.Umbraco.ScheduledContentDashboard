//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;

namespace Our.Umbraco.ScheduledContentDashboard.Models
{
    /// <summary>
    /// Declares the model for an individual scheduled content
    /// </summary>
    public class ScheduledContentModel
    {
        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        /// <remarks>
        /// Umbraco's unique id for the content
        /// </remarks>
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time at which the content action is scheduled
        /// </summary>
        public DateTime ScheduledDate { get; set; }

        /// <summary>
        /// Gets or sets the action type
        /// </summary>
        public string Action { get; set; }
    }
}