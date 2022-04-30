﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIRegister : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField passwordConfirmed;

    private void Start()
    {
        UserService.Instance.OnRegister += this.OnRigister;
    }

    public void OnClickRegister()
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
        if (string.IsNullOrEmpty(this.passwordConfirmed.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }
        if (this.password.text!=this.passwordConfirmed.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }

        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }

    void OnRigister(Result result,string message)
    {
        if (result == Result.Success)
        {
            Debug.Log("注册成功");
            MessageBox.Show(message, "注册成功请登录", MessageBoxType.Information);
        }
        else
        {
            MessageBox.Show(message,"错误",MessageBoxType.Error);
        }
    }
}