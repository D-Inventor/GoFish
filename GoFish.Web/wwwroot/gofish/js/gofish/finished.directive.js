(function (angular) {

    angular.module('ithotl.gofish')
        .directive('gamefinished', [function () {

            return {
                restrict: 'A',
                controller: 'ithotl.gamefinishedcontroller',
                templateUrl: '/gofish/js/gofish/gamefinishedview.html'
            };

        }])

        .controller('ithotl.gamefinishedcontroller', ['$scope', 'gameservice', function ($scope, gameservice) {

            $scope.winners = [];

            var getWinner = function () {
                if (game === null) {
                    $scope.winners = [];
                    return;
                }

                $scope.winners = game.players.map(function (p) { return { name: p.username, sets: Object.keys(p.finishedCollections) }; });
                $scope.winners.sort(function (a, b) { return b.sets.length - a.sets.length; });
            }

            var onChange = function (msg, newgame) {

                game = newgame;
                getWinner();
                $scope.$apply();
            }

            var onReady = function (newgame) {
                game = newgame;
                getWinner();
                $scope.$apply();
            }

            gameservice.subscribeOnGameChange(onChange);
            gameservice.subscribeOnReady(onReady);

            var game = gameservice.getGame();
            getWinner();
        }]);

}) (angular);