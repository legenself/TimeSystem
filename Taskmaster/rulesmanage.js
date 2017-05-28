var fs = require('fs');
var path = require('path');
var files = fs.readdirSync(__dirname + "/rules");
var rules =[];
var pattern = /[rR]ule.js$/;

//动态加载controller模块
for (var i in files) {
    var r = files[i].match(pattern);
    if (r !== null) {
        //var a = files[i].replace(pattern, "");

        rules.push(require('./rules/' + files[i]).rule);
    }
}

exports.rules = rules;
