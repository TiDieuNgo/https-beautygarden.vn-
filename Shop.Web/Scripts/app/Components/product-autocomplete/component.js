(function () {
    'use strict';

    angular
        .module('myApp')
        .component('cpProductAutocomplete', {
            templateUrl: '/Scripts/app/Components/product-autocomplete/view.html',
            controller: cpProductAutocompleteComponent,
            controllerAs: "vm",
            bindings: {
                onSelected:"&"
            }
        });

    /** @ngInject */
    function cpProductAutocompleteComponent($scope,$element) {
        var vm = this;


        vm.$onInit = function () {
            vm.search = "";
            var offset = angular.element($element).offset();
            var top = offset.top;
            var bottom = $(window).height() - top - angular.element($element).height();
            var xHeight = bottom - 50;
            var w = angular.element($element).width();

           

            angular.element($element.find("input")).autocomplete('/admin/SanPham/ProductAutocomplete',
            {
                dataType: 'json',
                scroll: true,
                scrollHeight: 200,
                parse: function (data) {
                    var rows = new Array();
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            rows[i] = { data: data[i], value: data[i].Name, result: data[i].Name };
                        }
                    }
                    return rows;
                },
                formatItem: function (row, i, n) {
                    return '<table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-top:dashed #ccc 1px">' +
                        '<tr style="cursor:pointer">' +
                        '<td style="border:none !important;padding-left:3px">' + row.Name + '</td>' +
                        '<td style="border:none !important" align="left" width="50">' + parseInt(row.Price).toFixed(0) + '</td>' +
                        '</tr>' +
                        '</table>';
                },
                width: w,
                mustMatch: true,
                autoFill: false,
                delay: 50,
                highlight: function (row, term) {
                    return row.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + term.replace(/([\^\$\(\)\[\]\{\}\*\.\+\?\|\\])/gi, "\\$1") + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong class='hightlight-word'>$1</strong>");
                }
            }).setOptions({
                matchContains: true,
                scrollHeight: xHeight,
                scroll: true,
                max: 20,
                width: w,
                containerDOM: "body"
            }).result(function (event, item) {
                if (item != 'undefined' && item) {
                    $scope.$applyAsync(function() {
                        vm.onSelected({ product: item });
                    });
                    
                }
            });
        };

        vm.$onDestroy = function () {
            angular.element($element).unautocomplete();
            vm = null;
        };



    }
})();