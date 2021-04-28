(function () {
    'use strict';

    // Resources for the Scheduled Content Dashboard
    angular.module('umbraco.resources').factory('scheduledContentDashboardResources', function ($http, umbRequestHelper) {

        return {
            getScheduledContent: function (orderBy, orderDirection) {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.ScheduledContentDashboard.getScheduledContent, {
                        params: {
                            orderBy: orderBy,
                            orderDirection: orderDirection
                        }
                    })
                );
            },
            deleteScheduleEntry: function (id, action, scheduledDate) {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.ScheduledContentDashboard.deleteScheduleEntry, {
                        params: {
                            contentId: id,
                            scheduleAction: action,
                            scheduleEntryDate: scheduledDate
                        }
                    })
                );
            }
        };
    });
})();