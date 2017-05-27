var os = require('os');


exports.rule = function (callback) {
    var totalmem = os.totalmem();
    var freemem = os.freemem();
    var memper = freemem / totalmem;

    if (memper < 0.5) {
        callback(true, "内存警告" + memper);
    }
    callback(false);

}