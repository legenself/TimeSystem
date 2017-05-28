var os = require('os');




  rule = new Promise(function (resolve, reject) {
    var totalmem = os.totalmem();
    var freemem = os.freemem();
    var memper = freemem / totalmem;

    if (memper < 0.5) {
        resolve({ type: true, msg: "内存警告" + memper });
    } else {
        resolve({ type: false, msg: "内存警告" + memper });

    }


})

exports.rule = rule;