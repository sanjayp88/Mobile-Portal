// Excludes console.xxx methods from IE 9 and below except when in IE dev tool
// See
// javascript - Is there a way to log to console without breaking code under IE? - Stack Overflow
//http://stackoverflow.com/questions/7892509/is-there-a-way-to-log-to-console-without-breaking-code-under-ie
//

var ConsoleCheck = ConsoleCheck || {};

ConsoleCheck.run = function () {

    var names = ['log', 'debug', 'info', 'warn', 'error', 'assert', 'dir', 'dirxml', 'group', 'groupEnd', 'time', 'timeEnd', 'count', 'trace', 'profile', 'profileEnd'];

    if (!('console' in window)) {
        window.console = {};
        for (var i = 0; i < names.length; ++i) window.console[names[i]] = function () { };
    } else {
        for (var i = 0; i < names.length; ++i) if (!window.console[names[i]]) window.console[names[i]] = function () { };
    }
}

ConsoleCheck.run();