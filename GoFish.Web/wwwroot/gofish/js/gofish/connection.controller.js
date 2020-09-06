(function (angular) {

    angular.module('ithotl.gofish')
        .controller('ithotl.connectioncontroller', ['$scope', 'gameservice', function ($scope, gameservice) {

            var $this = this;

            var onReady = function (newgame) {

                $this.ready = true;
                $scope.$apply();
            }

            gameservice.subscribeOnReady(onReady);
            $this.ready = gameservice.getReady();
        }]);

}) (angular);