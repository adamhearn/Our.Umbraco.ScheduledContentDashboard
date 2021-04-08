//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EnsureThat;
using Our.Umbraco.ScheduledContentDashboard.Contracts;
using Our.Umbraco.ScheduledContentDashboard.Controllers;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.JavaScript;

namespace Our.Umbraco.ScheduledContentDashboard.Composers
{
    /// <summary>
    /// Implementation of <see cref="IComponent"/> for general application configuration
    /// </summary>
    public class ScheduledContentDashboardComponent : IComponent
    {
        /// <summary>
        /// Initializes the component
        /// </summary>
        /// <remarks>
        /// Configures the routing variables for the feature
        /// </remarks>
        public void Initialize()
        {
            // Wire up the parsing event
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        /// <summary>
        /// Server Variables parsing event handler
        /// </summary>
        /// <remarks>
        /// Adds the package's server variables
        /// </remarks>
        /// <param name="sender">Object that generated the event</param>
        /// <param name="e">Argument data for the event</param>
        private void ServerVariablesParser_Parsing( object sender, Dictionary<string, object> e )
        {
            // Validate the request
            Ensure.Any.IsNotNull( HttpContext.Current, nameof( HttpContext.Current ) );
            Ensure.Any.IsNotNull( e, nameof( e ) );

            // Add to the server variables dictionary
            UrlHelper urlHelper = new UrlHelper( new RequestContext( new HttpContextWrapper( HttpContext.Current ), new RouteData() ) );
            Dictionary<string, object> urlDictionary = new Dictionary<string, object>
                {
                    {
                        Char.ToLowerInvariant( nameof( ScheduledContentDashboardController.GetScheduledContent )[0] ) + nameof( ScheduledContentDashboardController.GetScheduledContent ).Substring( 1 ),
                        urlHelper.GetUmbracoApiService<ScheduledContentDashboardController>( nameof( ScheduledContentDashboardController.GetScheduledContent ), (RouteValueDictionary) null )
                    }
                };
            e.Add( PackageConstants.PackageName, urlDictionary );
        }

        /// <summary>
        /// Terminates the component
        /// </summary>
        /// <remarks>
        /// No specific implementation required for this component
        /// </remarks>
        public void Terminate()
        {
        }
    }
}