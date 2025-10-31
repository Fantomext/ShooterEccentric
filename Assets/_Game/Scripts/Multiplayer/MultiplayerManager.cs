using System.Collections.Generic;
using _Game.Scripts;
using _Game.Scripts.AssetLoader;
using _Game.Scripts.Factory;
using _Game.Scripts.Providers;
using Colyseus;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [Inject] private PlayerProvider _playerProvider;
    [Inject] private GameObjectFactory<EnemyController> _enemyFactory;
    [Inject] private LevelAssetLoader _levelAssetLoader;
    
    private const string StateHandler = "state_handler";
    private const string MessageShoot = "Shoot";
    private const string MessageCrouch = "Sit";
    
    private EnemyController _enemy;
    private PlayerCharacter _player;
    
    private Dictionary<string, EnemyController> _players = new Dictionary<string, EnemyController>();
    
    private ColyseusRoom<State> _room;

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
            {"speed", _player.Speed}
        };
        
        _room = await client.JoinOrCreate<State>(StateHandler, data);
        

        _room.OnStateChange += OnChange;
        _room.OnMessage<string>(MessageShoot, ShootEvent);
        _room.OnMessage<string>(MessageCrouch, SitEvent);
    }

    private void SitEvent(string jsonSitData)
    {
        SitInfo data = JsonUtility.FromJson<SitInfo>(jsonSitData);

        if (_players.TryGetValue(data.key, out var enemy))
            enemy.Crouch(data.sit);
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
        _player.transform.position = new Vector3(player.pX , player.pY, player.pZ );
    }

    private void CreateEnemy(string key , Player player)
    {
        if (_players.ContainsKey(key))
            return;
        
        Vector3 position = new Vector3(player.pX , player.pY, player.pZ );

        EnemyController enemy = _enemyFactory.Create(_enemy, position, Quaternion.identity);
        enemy.Init(player);
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
