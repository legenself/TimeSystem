var cmd = require('node-cmd');

 
exports.rule = function (callback) {
    cmd.get(
        'ping www.baidu.com',
        function (err, data, stderr) {
            callback(false)
        }
    );
}