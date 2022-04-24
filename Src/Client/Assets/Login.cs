using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Network.NetClient.Instance.Init("127.0.0.1", 8000);//初始化 127.0.0.1表示本机ip 一台电脑客户端服务端共用则使用本机地址 两台电脑则写服务端的ip
        Network.NetClient.Instance.Connect();

        //创建主消息
        SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage();
        //创建自己定义的消息
        msg.Request.firstRequest = new SkillBridge.Message.FirstTestRequest();
        //填充数据
        msg.Request.firstRequest.helloWorld = "Hello World";
        Network.NetClient.Instance.SendMessage(msg);
    }

}
