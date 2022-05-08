using Common.Data;
using Models;
using Network;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapService : Singleton<MapService>, IDisposable
{
    public MapService()
    {
        MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
        MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
    }

    public int CurrentMapId { get; private set; }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
        MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
    }
    public void Init()
    {
        
    }

    //接收到Response做两件事情：将角色添加到角色管理器 切换地图
    private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
    {
        Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:[{1}]", response.mapId, response.Characters.Count);
        foreach (var cha in response.Characters)
        {
            //刷新一遍本地数据
            if (User.Instance.CurrentCharacter.Id == cha.Id)
            {
                User.Instance.CurrentCharacter = cha;
            }
            CharacterManager.Instance.AddCharacter(cha); //添加到客户端的角色管理器
        }
        if (CurrentMapId != response.mapId) //如果切换了地图
        {
            this.EnterMap(response.mapId);
            this.CurrentMapId = response.mapId;
        }
    }

    

    private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse message)
    {
        
    }

    private void EnterMap(int mapId)
    {
        if (DataManager.Instance.Maps.ContainsKey(mapId))
        {
            MapDefine map = DataManager.Instance.Maps[mapId];
            SceneManager.Instance.LoadScene(map.Resource);
        }
        else
        {
            Debug.LogErrorFormat("EnterMap:Map{0} not existed", mapId);
        }
    }
}
