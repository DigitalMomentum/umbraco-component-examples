/**
 * @ngdoc controller
 * @name Umbraco.Editors.DocumentType.EditController
 * @function
 *
 * @description
 * The controller for the content type editor
 * 
 * Umbraco TV:- https://umbraco.tv/videos/umbraco-v7/developer/extending/custom-listview/
 * Docs:- https://our.umbraco.com/apidocs/v8/ui/#/api
 * 
 */
(function () {
	"use strict";

	function ListViewProductsController($scope, mediaHelper, $location, listViewHelper, mediaTypeHelper, $filter, mediaResource) {
		var vm = this;

		console.log($scope.items)

		angular.forEach($scope.items, function (value, key) {
			if (value.photos.length > 0) {
				updateMediaUrl(value.photos[0]);
			}
		});

		vm.goToItem = function(item, $event, $index) {
			listViewHelper.editItem(item, $scope);
		}

		vm.toggleItem = function (item) {
			if (item.selected) {
				listViewHelper.deselectItem(item, $scope.selection);
			} else {
				listViewHelper.selectItem(item, $scope.selection);
			}
		}


		function updateMediaUrl(photoObj) {
			mediaResource.getById(photoObj.mediaKey)
				.then(function (media) {
					photoObj.src = media.mediaLink;
				});

		}
	}

	angular.module("umbraco").controller("DM.Listview.ProductsController", ListViewProductsController);

})();