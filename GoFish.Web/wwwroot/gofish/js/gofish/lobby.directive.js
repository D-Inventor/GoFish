(function (angular) {

    angular.module('ithotl.gofish')
        .directive('gamelobby', [function () {

            return {
                restrict: 'A',
                controller: 'ithotl.gamelobbycontroller',
                templateUrl: '/gofish/js/gofish/gamelobbyview.html'
            };

        }])

        .controller('ithotl.gamelobbycontroller', ['$scope', 'gameservice', function ($scope, gameservice) {

            var onChange = function (msg, newgame) {
                $scope.game = newgame;
                $scope.$apply();
            }

            var onReady = function (newgame) {
                $scope.game = newgame;
                $scope.$apply();
            }

            $scope.isThisPlayer = function (player) {
                return gameservice.getUserId() === player.id;
            }

            $scope.startGame = function () {
                gameservice.startGame();
            }

            gameservice.subscribeOnGameChange(onChange);
            gameservice.subscribeOnReady(onReady);
            $scope.game = gameservice.getGame();
        }]);

}) (angular);