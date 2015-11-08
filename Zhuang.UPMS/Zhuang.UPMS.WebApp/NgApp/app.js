
var app = angular.module("main-app", ['ngRoute']);


app.config(function ($routeProvider) {

    $routeProvider.when("/user-list", {
        controller: "userCtrl",
        templateUrl: "./security-console/user-list.html"
    });

});


var serviceBase = 'http://localhost:8002/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'zwbApp'
});
