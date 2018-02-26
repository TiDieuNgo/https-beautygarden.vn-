'use strict';

angular.module('myApp', [
        'ngAnimate', 'ngSanitize', 'ui.bootstrap', "angularFileUpload", 'infinite-scroll'
    ])
    .config([
        '$httpProvider', function($httpProvider) {
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];
        }
    ])
    .directive('ngEnter', function() {
        return function(scope, element, attrs) {
            element.bind("keydown keypress", function(event) {
                if (event.which === 13) {
                    scope.$apply(function() {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .directive('datetime', function() {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, element, attrs, ngModelCtrl) {
                element.datetimepicker({
                    format: 'DD/MM/YYYY HH:mm',
                    minDate: moment(),
                    sideBySide: true,

                    // language: 'vi'
                }).on('dp.change', function(e) {
                    ngModelCtrl.$setViewValue(moment(e.date).format("DD/MM/YYYY HH:mm"));
                    scope.$apply();
                });
            }
        };
    })
    .filter('region', function() {
        return function(value) {
            if (value === "" || angular.isUndefined(value)) return "";

            switch (value) {
            case "All":
                return "Toàn bộ khu vực";
            case "Bac":
                return "Miền Bắc";
            case "Trung":
                return "Miền Trung";
            case "Name":
                return "Miền Nam";


            }
        };
    });
