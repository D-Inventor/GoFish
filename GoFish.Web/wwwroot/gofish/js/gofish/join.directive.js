(function (angular) {

    angular.module('ithotl.gofish')
        .directive('gamejoin', [function () {

            return {
                restrict: 'A',
                controller: 'ithotl.gamejoincontroller',
                templateUrl: '/gofish/js/gofish/gamejoinview.html'
            };

        }])

        .controller('ithotl.gamejoincontroller', ['$scope', 'gameservice', function ($scope, gameservice) {

            $scope.username = '';

            $scope.joinGame = function () {

                gameservice.joinGame($scope.username);
            }
        }]);

}) (angular);