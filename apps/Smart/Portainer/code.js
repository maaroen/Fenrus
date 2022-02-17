﻿async function authorize(args) {
    let username = args.properties['username'];
    let password = args.properties['password'];
    try{
        console.log('DATA', JSON.stringify({ username: username, password: password }));
        let res = await args.fetch({
            url: 'api/auth',
            method: 'POST',
            headers: { 'Content-Type': 'application/json'},
            body: JSON.stringify({ username: username, password: password })
        });
        if(!res)
            return;
        if(res.jwt)
            return res.jwt;
        if(res.message)
            throw res.message;
    }
    catch(err)
    {
        throw err;
    }
    throw res.body;
}


module.exports = { 
    status: async (args) => {
        let jwt = await authorize(args);
        let data = await args.fetch({
            url: 'api/endpoints?limit=100&start=0',
            headers: { 'Authorization': 'Bearer ' + jwt}
        });
        let running = 0;
        let stopped = 0;

        if (data && typeof (data[Symbol.iterator]) === 'function') {
            for (let instance of data) {
                if (instance?.Snapshots) {
                    for (let snapshot of instance.Snapshots) {
                        running += snapshot.RunningContainerCount;
                        stopped += snapshot.StoppedContainerCount;
                    }
                }
            }
        }
        return args.liveStats([
            ['Running', running],
            ['Stopped', stopped]
        ]);
    },
    
    test: async (args) => {
        return !!(await authorize(args));
    }
}