
var app = angular.module("main-app", ['ngRoute']);


app.config(function ($routeProvider) {

    $routeProvider.when("/user-list", {
        controller: "userCtrl",
        templateUrl: "./security-console/user-list.html"
    });

    routeProvider.when("/main", {
        templateUrl: "./main.html"
    });

    $routeProvider.otherwise({ redirectTo: "/main" });

});


var serviceBase = 'http://localhost:8002/webapi/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'zwbApp'
});
