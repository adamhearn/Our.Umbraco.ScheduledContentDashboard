(function () {
    "use strict";

    angular.module("umbraco").controller("ScheduledContentDashboardController",
        function (userService, dateHelper, notificationsService, scheduledContentDashboardResources, localizationService) {

            var vm = this;

            vm.options = {
                orderBy: "scheduledDate",
                orderDirection: "asc"
            };
            vm.items = [];
            vm.getContent = getContent;

            // Fetch the data from the API endpoint
            function getContent() {
                // Default values
                vm.items = null;
                vm.isLoading = true;

                scheduledContentDashboardResources.getScheduledContent(vm.options.orderBy, vm.options.orderDirection)
                    .then(function (response) {
                        if (response.length > 0) {

                            // Add the custom object type's properties and adjust the date to the current user's preference
                            userService.getCurrentUser().then(function (currentUser) {
                                angular.forEach(response, function (item) {
                                    item.editPath = "content/content/edit/" + item.contentId;
                                    item.icon = "icon-calendar";
                                    item.selected = false;
                                    item.scheduledDate = dateHelper.getLocalDate(item.scheduledDate, currentUser.locale, 'YYYY-MM-DD HH:mm');
                                });

                                vm.items = response;
                                vm.isLoading = false;
                            });
                        } else {
                            vm.isLoading = false;
                        }

                    }, function (response) {
                        localizationService.localize("scheduledContentDashboard_retrieveFailed").then(value => {
                            notificationsService.error("Error", value);
                        });
                    });
            }

            // Run
            getContent();
        });
})();