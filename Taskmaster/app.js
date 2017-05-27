'use strict';
var rules = require('./rulesmanage').rules;
var redis = require("redis"),
    RDS_PORT = 6379,
    RDS_HOST = "192.168.1.70",
    RDS_PWD = '',
    PDS_OPTS = { auth_pass: RDS_PWD },
    client = redis.createClient(RDS_PORT, RDS_HOST);

var async = require('async');

console.log(rules);

var message = [];
var q = async.queue(function (task, callback) {
    console.log("worker is processing task:" + task.name);
    callback();
}, 2);


for (var rule in rules) {
    //console.log(rule);
    //runcount[rule] = 1;
    //rules[rule]((result, msg) => {

    //    if (result) {
    //        message.push("message", "504883942@qq.com," + rule + "," + msgs);
    //    }
      
    //});
    q.push({ name: rule }, function () {
        rules[rule](() => {
            console.log("finish");
        });
    })
}



//if (result) {
//    client.rpush("message", "504883942@qq.com," + rule + "," + msg, function () {
//        client.end(true);
//    });
//}
q.empty = function () {
    console.log("empty");
}
q.drain = function () {
    console.log("all task have been processed");
}