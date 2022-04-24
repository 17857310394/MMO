using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class HelloWorldServices:Singleton<HelloWorldServices>
    {
        public void Init()
        {

        }

        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
        }

        void OnFirstTestRequest(NetConnection<NetSession> sender,FirstTestRequest request)
        {
            Log.InfoFormat("OnFirstTestRequest:User:{0} Pass:{1}", request.helloWorld);
        }

        public void Stop()
        {

        }
    }
}
