(function () {
    'use strict';

    angular.module("umbraco").controller("ScheduledContentDashboardController",
        function (userService, listViewHelper, dateHelper, notificationsService, scheduledContentDashboardResources, $q) {

            var vm = this;

            vm.options = {
                filter: '',
                orderBy: "scheduledDate",
                orderDirection: "asc",
                includeProperties: [
                    { alias: "id", header: "Node" },
                    { alias: "action", header: "Action" },
                    { alias: "scheduledDate", header: "Scheduled Date" }
                ],
                allowBulkDelete: true
            };

            vm.buttonState = "init";
            vm.actionInProgress = false;
            vm.selection = [];
            vm.selectItem = selectItem;
            vm.clickItem = clickItem;
            vm.selectAll = selectAll;
            vm.isSelectedAll = isSelectedAll;
            vm.isSortDirection = isSortDirection;
            vm.sort = sort;
            vm.clearSelection = clearSelection;
            vm.getContent = getContent;

            function selectAll() {
                listViewHelper.selectAllItemsToggle(vm.items, vm.selection);
            }

            function clearSelection() {
                listViewHelper.clearSelection(vm.items, null, vm.selection);
            }

            function isSelectedAll() {
                return listViewHelper.isSelectedAll(vm.items, vm.selection);
            }

            function clickItem(item) {
                listViewHelper.editItem(item);
            }

            function selectItem(item, $index, $event) {
                listViewHelper.selectHandler(item, $index, vm.items, vm.selection, $event);
            }

            function isSortDirection(col, direction) {
                // Is the specified field the current sort pattern
                return vm.options.orderBy === col && vm.options.orderDirection === direction;
            }

            function sort(field, allow, isSystem) {
                // Allow sorting on all fields
                listViewHelper.setSorting(field, true, vm.options);

                // Need to retrieve the content again as the sort order has changed
                getContent();
            }

            // Fetch the data from the API endpoint
            function getContent() {
                // Default values
                vm.items = null;
                vm.selection = [];
                vm.buttonState = "busy";

                scheduledContentDashboardResources.getScheduledContent(vm.options.orderBy, vm.options.orderDirection)
                    .then(function (response) {
                        if (response.length > 0) {

                            // Add the custom object type's properties and adjust the date to the current user's preference
                            userService.getCurrentUser().then(function (currentUser) {
                                angular.forEach(response, function (item) {
                                    item.editPath = "content/content/edit/" + item.id;
                                    item.icon = "icon-calendar";
                                    item.selected = false;
                                    item.scheduledDate = dateHelper.getLocalDate(item.scheduledDate, currentUser.locale, 'YYYY-MM-DD HH:mm');
                                });
                            });
                            vm.items = response;

                            // Apply any filter
                            if (vm.options.filter !== '') {
                                vm.items = vm.items.filter(x => ~x.name.toLowerCase().indexOf(vm.options.filter.toLowerCase()));
                            }
                        }
                        vm.buttonState = "init";

                    }, function (response) {
                        notificationsService.error("Error", "Could not retrieve scheduled content");
                    });
            }

            // Remove content item schedule entries
            vm.remove = function () {
                // Ensure UI shows activity
                vm.actionInProgress = true;

                // Prepare an array of selected items
                const selectedItems = vm.items.filter(function (item) {
                    return item.selected;
                });

                // Build a set of promises so that we can run post execution commands
                let promises = [];
                angular.forEach(selectedItems, function (item) {
                    promises.push(
                        scheduledContentDashboardResources.deleteScheduleEntry(item.id, item.action, item.scheduledDate)
                            .then(function (response) {

                                const index = vm.items.indexOf(item);
                                if (index > -1) {
                                    vm.items.splice(index, 1);
                                }

                            }, function (response) {
                                notificationsService.error("Error", "Could not remove scheduled content");
                            }));
                });

                // Run all promises and then update the UI
                $q.all(promises)
                    .then(function () {
                        vm.selection = [];
                        vm.actionInProgress = false;
                    });
            };

            // Run
            getContent();
        });
})();