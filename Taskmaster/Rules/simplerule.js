var cmd = require('node-cmd');

 


rule = new Promise(function (resolve, reject) {
    cmd.get(
        'ping www.baidu.com',
        function (err, data, stderr) {


            resolve({ type: false, msg: "内存警告" + data });
        }
    );
})
exports.rule = rule;