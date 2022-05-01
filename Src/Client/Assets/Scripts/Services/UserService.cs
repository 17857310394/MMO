using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;

namespace Services
{
    class UserService : Singleton<UserService>, IDisposable
    {
        public UnityEngine.Events.UnityAction<Result, string> OnRegister;
        public UnityEngine.Events.UnityAction<Result, string> OnLogin;
        NetMessage pendingMessage = null;
        bool connected = false;

        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect;  //添加连接监听事件
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;  //添加连接断开事件
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister); //订阅一个注册事件
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);

        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
        }

        public void Init()
        {

        }
        /// <summary>
        /// 连接到服务器
        /// </summary>
        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() Start ");
            //NetClient.Instance.CryptKey = this.SessionId;
            NetClient.Instance.Init("127.0.0.1", 8000);
            NetClient.Instance.Connect();
        }


        void OnGameServerConnect(int result, string reason)
        {
            Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if(this.pendingMessage!=null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason))
                {
                    MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
                }
            }
        }

        public void OnGameServerDisconnect(int result, string reason)
        {
            this.DisconnectNotify(result, reason);
            return;
        }

        bool DisconnectNotify(int result,string reason)
        {
            if (this.pendingMessage != null)
            {
                if (this.pendingMessage.Request.userLogin!=null)
                {
                    if (this.OnLogin != null)
                    {
                        this.OnLogin(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                else if(this.pendingMessage.Request.userRegister!=null)
                {
                    if (this.OnRegister != null)
                    {
                        this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送注册消息到服务端 请求包含用户注册 一个用户名一个密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psw"></param>
        public void SendRegister(string user, string psw)
        {
            //建立一个请求
            Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();
            message.Request.userRegister.User = user;
            message.Request.userRegister.Passward = psw;

            if (this.connected && NetClient.Instance.Connected) //已连接服务器
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else //未连接服务器
            {
                this.pendingMessage = message; //用一个队列将消息记下来 连接上后自动发送
                this.ConnectToServer();
            }
        }

        /// <summary>
        /// 发送登录消息到服务端 请求包含用户注册 一个用户名一个密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psw"></param>
        public void SendLogin(string user, string psw)
        {
            //建立一个请求
            Debug.LogFormat("UserLoginRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userLogin = new UserLoginRequest();
            message.Request.userLogin.User = user;
            message.Request.userLogin.Passward = psw;

            if (this.connected && NetClient.Instance.Connected) //已连接服务器
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else //未连接服务器
            {
                this.pendingMessage = message; //用一个队列将消息记下来 连接上后自动发送
                this.ConnectToServer();
            }
        }

        void OnUserRegister(object sender, UserRegisterResponse response)
        {
            Debug.LogFormat("OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);
            }
        }

        void OnUserLogin(object sender, UserLoginResponse response)
        {
            Debug.LogFormat("OnLogin:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {
                //登陆成功逻辑
                Models.User.Instance.SetupUserInfo(response.Userinfo);
            };
            if (this.OnLogin != null)
            {
                //UI关注用户成功登录的消息
                this.OnLogin(response.Result, response.Errormsg);
            }
        }
    }
}
