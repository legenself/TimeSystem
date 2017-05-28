'use strict';
var rules = require('./rulesmanage').rules;
var linq = require('linq');
var redis = require("redis"),
    RDS_PORT = 6379,
    RDS_HOST = "192.168.1.70",
    RDS_PWD = '',
    PDS_OPTS = { auth_pass: RDS_PWD };

var nodemailer = require('nodemailer');
var transporter = nodemailer.createTransport({
    service: 'qq',
    port: 465, // SMTP 端口
    secureConnection: true, // 使用 SSL
    auth: {
        user: 'lab2401@qq.com',
        //这里密码不是qq密码，是你设置的smtp密码
        pass: '*****'
    }
});
var admin = "504883942@qq.com";
var mailOptions = {
    from: '768065158@qq.com', // 发件地址
    to: '528779822@qq.com', // 收件列表
    subject: 'Hello sir', // 标题
    //text和html两者只支持一种
    text: 'Hello world ?', // 标题
    html: '<b>Hello world ?</b>' // html 内容
};
Promise.all(rules).then(values => {
    var client = redis.createClient(RDS_PORT, RDS_HOST);

    var list = linq.from(values)
        .where(p => p.type == true)
        .select(p =>
            new Promise((resolve, reject) => {
                client.rpush("message", admin + "," + p.msg, function (err) {
                    if (!err) {
                        resolve(true);
                    }
                });
            }));
    Promise.all(list).then(values => {
        client.end(true);
    });

     


    console.log(values);
});

