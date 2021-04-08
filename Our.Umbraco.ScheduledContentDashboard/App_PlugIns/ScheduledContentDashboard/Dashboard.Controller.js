(function () {
    'use strict';

    angular.module("umbraco").controller("ScheduledContentDashboardController",
        function (notificationsService, scheduledContentDashboardResources) {
            var vm = this;
            vm.criteria = {
                ascending: true
            };

            // Fetch the data from the API endpoint
            function fetchData() {
                // Default values
                vm.isLoading = true;
                vm.items = null;

                scheduledContentDashboardResources.getScheduledContent(vm.criteria)
                    .then(function (response) {
                        if (response.length > 0) {
                            vm.items = response;
                        }
                        vm.isLoading = false;

                    }, function (response) {
                        notificationsService.error("Error", "Could not retrieve scheduled content");
                    });
            }

            // Sort order
            vm.order = function () {
                vm.criteria.ascending = !vm.criteria.ascending;
                fetchData();
            };

            // Gets the edit URL for the specified Node Id
            vm.getContentUrl = function (entry) {
                return scheduledContentDashboardResources.getContentUrl(entry);
            };

            // Reload
            vm.reload = function () {
                fetchData();
            };

            // Run
            fetchData();
        });
})();