using System;
using System.Collections.Generic;
using _Game.Scripts;
using _Game.Scripts.AssetLoader;
using _Game.Scripts.Configs;
using _Game.Scripts.Info;
using _Game.Scripts.Providers;
using _Game.Scripts.Spawners;
using Colyseus;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [Inject] private PlayerProvider _playerProvider;
    [Inject] private EnemySpawner _enemySpawner;
    [Inject] private LevelAssetLoader _levelAssetLoader;
    [Inject] private PlayerConfig _playerConfig;
    
    private const string StateHandler = "state_handler";
    private const string MessageShoot = "Shoot";
    private const string MessageCrouch = "Sit";
    private const string MessageRestart = "Restart";
    private const string MessageDeath = "Death";
    private const string MessageWin = "Win";
    private const string MessageSwap = "Swap";
    
    private EnemyController _enemy;
    private PlayerCharacter _player;
    
    private Dictionary<string, EnemyController> _players = new Dictionary<string, EnemyController>();
    
    private ColyseusRoom<State> _room;

    public event Action<Player> OnPlayerCreate;
    public event Action<string> OnPlayerRestart;
    public event Action<bool> OnGameEnd;
    public async UniTask Init()
    {
        InitializeClient();
        await Connect();
    }
    
    private async UniTask Connect()
    {
        _player = _playerProvider.GetCharacter();
        _enemy = await _levelAssetLoader.LoadWithoutSpawn<EnemyController>("Enemy");
        
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"speed", _playerConfig.Speed},
            {"hp", _playerConfig.MaxHealth},
        };
        
        _room = await client.JoinOrCreate<State>(StateHandler, data);
        

        _playerProvider.SetSessionID(ReturnSessionId());

        _room.OnStateChange += OnChange;
        
        _room.OnMessage<string>(MessageShoot, ShootEvent);
        _room.OnMessage<string>(MessageCrouch, SitEvent);
        _room.OnMessage<string>(MessageRestart, Restart);
        _room.OnMessage<string>(MessageDeath, Death);
        _room.OnMessage<string>(MessageSwap, SwapWeapon);
        _room.OnMessage<bool>(MessageWin, WinGame);
    }

    private void SwapWeapon(string dataSwap)
    {
        SwapInfo data = JsonUtility.FromJson<SwapInfo>(dataSwap);

        if (_players.TryGetValue(data.id, out var enemyController))
        {
            enemyController.SwapWeapon(data.weapon);
        }
    }

    private void Death(string key)
    {
        if (_players.TryGetValue(key, out var enemy))
            enemy.Restart();
    }

    private void Restart(string jsonData)
    {
        OnPlayerRestart?.Invoke(jsonData);
    }

    private void SitEvent(string jsonSitData)
    {
        SitInfo data = JsonUtility.FromJson<SitInfo>(jsonSitData);

        if (_players.TryGetValue(data.key, out var enemy))
            enemy.Crouch(data.sit);
    }

    public async void WinGame(bool isWin)
    {
        OnGameEnd?.Invoke(isWin);
        await UniTask.WaitForSeconds(3f);
        Application.Quit();
    }

    private void ShootEvent(string jsonShootInfo)
    {
        var data = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

        if (_players.TryGetValue(data.key, out var enemy))
            enemy.Shoot(data);
    }

    private void OnChange(State state, bool isfirststate)
    {
        if (isfirststate == false)
            return;
        
        state.players.ForEach((key, player) =>
        {
            if (key == _room.SessionId)
                CreatePlayer(player);
            else
                CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player)
    {
        _player.transform.position = new Vector3(player.pX , player.pY, player.pZ);
        
        OnPlayerCreate?.Invoke(player);
    }

    private void CreateEnemy(string key , Player player)
    {
        if (_players.ContainsKey(key))
            return;
        
        Vector3 position = new Vector3(player.pX , player.pY, player.pZ );

        EnemyController enemy = _enemySpawner.Spawn(_enemy, position, Quaternion.identity);
        enemy.gameObject.SetActive(true);
        enemy.Init(key, player);
        _players.Add(key, enemy);
    }

    private void RemoveEnemy(string key, Player player)
    {
        if (!_players.TryGetValue(key, out var enemy)) 
            return;
        
        player.OnChange -= enemy.OnChange;
        
        Destroy(enemy.gameObject);
        _players.Remove(key);

    }

    public string ReturnSessionId()
    {
        return _room.SessionId;
    }
    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_room == null)
            return;

        _room.State.players.OnAdd -= CreateEnemy;
        _room.State.players.OnRemove -= RemoveEnemy;
        _room.OnStateChange -= OnChange;
        
        _room.Leave();
    }
}
