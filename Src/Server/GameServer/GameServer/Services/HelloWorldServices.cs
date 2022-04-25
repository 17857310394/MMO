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
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest); //用OnFirstTestRequest这个方法来处理这个协议
        }

        //处理器
        void OnFirstTestRequest(NetConnection<NetSession> sender,FirstTestRequest request)
        {
            Log.InfoFormat("OnFirstTestRequest:HelloWorld:{0}", request.helloWorld);
        }

        public void Stop()
        {

        }
    }
}
