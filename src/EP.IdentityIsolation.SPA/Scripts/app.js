﻿function ViewModel() {
    var self = this,
        tokenKey = 'accessToken',
        apiPrefix = 'https://localhost:44300',
        requestTime;

    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();

    self.latency = ko.observable();
    self.latency(-1);

    function showError(jqXHR) {
        self.result(jqXHR.status + ': ' + jqXHR.statusText);
    }

    self.callApi = function() {
        self.result('');

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: apiPrefix + '/api/values',
            headers: headers,
            beforeSend: function() { requestTime = new Date(); },
            complete: function() { self.latency((new Date()) - requestTime); }
        }).done(function(data) {
            self.result(data);
        }).fail(showError);
    }

    self.register = function() {
        self.result('');

        var data = {
            Email: self.registerEmail(),
            Password: self.registerPassword(),
            ConfirmPassword: self.registerPassword2()
        };

        $.ajax({
            type: 'POST',
            url: apiPrefix + '/api/Account/Register',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            beforeSend: function() { requestTime = new Date(); },
            complete: function() { self.latency((new Date()) - requestTime); }
        }).done(function(data) {
            self.result("Done!");
        }).fail(showError);
    }

    self.login = function() {
        self.result('');

        var loginData = {
            grant_type: 'password',
            username: self.loginEmail(),
            password: self.loginPassword()
        };

        $.ajax({
            type: 'POST',
            url: apiPrefix + '/Token',
            data: loginData,
            beforeSend: function() { requestTime = new Date(); },
            complete: function() { self.latency((new Date()) - requestTime); }
        }).done(function(data) {
            self.user(data.userName);
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, data.access_token);
        }).fail(showError);
    }

    self.logout = function() {
        self.user('');
        sessionStorage.removeItem(tokenKey);
    }
}

var app = new ViewModel();
ko.applyBindings(app);