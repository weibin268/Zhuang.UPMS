app.controller('userCtrl', ['$scope', 'userService', function ($scope, userService) {

    $scope.users = [];

    userService.getUser().then(function (results) {

        $scope.user = results.data;

    }, function (error) {
        //alert(error.data.message);
    });

}]);