var os = require('os');
var redis = require("redis"),
    RDS_PORT = 6379,
    RDS_HOST = "192.168.1.70",
    RDS_PWD = '',
    PDS_OPTS = { auth_pass: RDS_PWD };




rule = new Promise(function (resolve, reject) {
    var client = redis.createClient(RDS_PORT, RDS_HOST);
    client.keys("status_*", function (err, res) {
        console.log(res);
        resolve({ type: false, msg: "内存警告" });
        client.end(true);
    });

 


})

exports.rule = rule;