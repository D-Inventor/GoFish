(function (angular) {

    angular.module('ithotl.gofish')
        .directive('gamecreate', [function () {

            return {
                restrict: 'A',
                controller: 'ithotl.gamecreatecontroller',
                templateUrl: '/gofish/js/gofish/gamecreateview.html'
            };

        }])

        .controller('ithotl.gamecreatecontroller', ['$scope', 'gameservice', function ($scope, gameservice) {

            $scope.username = '';

            $scope.createGame = function () {

                gameservice.createGame($scope.username);
            }
        }]);

}) (angular);