(function () {
    'use strict';

    // Resources for the Scheduled Content Dashboard
    angular.module('umbraco.resources').factory('scheduledContentDashboardResources', function ($http, umbRequestHelper) {

        return {
            getScheduledContent: function (criteria) {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.ScheduledContentDashboard.getScheduledContent, {
                        params: {
                            sortAscending: criteria.ascending
                        }
                    })
                );
            },
            getContentUrl: function (item) {
                if (item === null || item.ContentId === 0) {
                    return null;
                }
                return "#/content/content/edit/" + item.ContentId;
            }
        };
    });
})();