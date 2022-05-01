using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;
using UnityEngine.SceneManagement;
public class UILogin : MonoBehaviour
{
    public InputField username;
    public InputField password;

    private void Start()
    {
        UserService.Instance.OnLogin += this.OnLogin;
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
         
        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }

    void OnLogin(Result result, string message)
    {
        if (result == Result.Success)
        {
            //成功登录
            Debug.Log("登录成功");
            UnityEngine.SceneManagement.SceneManager.LoadScene("CharSelect");
        }
        else
        {
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
    }
}
