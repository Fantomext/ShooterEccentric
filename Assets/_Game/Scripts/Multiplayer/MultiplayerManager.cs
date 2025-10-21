using System.Collections.Generic;
using _Game.Scripts;
using _Game.Scripts.AssetLoader;
using _Game.Scripts.Factory;
using Colyseus;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [Inject] private GameObjectFactory<Character> _playerFactory;
    [Inject] private GameObjectFactory<EnemyController> _enemyFactory;
    [Inject] private LevelAssetLoader _levelAssetLoader;
    
    private Character _playerPrefab;
    private EnemyController _enemy;
    
    private Dictionary<string, EnemyController> _players = new Dictionary<string, EnemyController>();
    
    private ColyseusRoom<State> _room;

    public async UniTask Init()
    {
        InitializeClient();
        await Connect();
    }
    
    private async UniTask Connect()
    {
        _playerPrefab = await _levelAssetLoader.LoadWithoutSpawn<Character>("Player");
        _enemy = await _levelAssetLoader.LoadWithoutSpawn<EnemyController>("Enemy");
        
        _room = await client.JoinOrCreate<State>("state_handler");

        _room.OnStateChange += OnChange;
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

    private void ForEachEnemy(string key, Player player)
    {
        if (key == _room.SessionId)
            return;
        
    }

    private void CreatePlayer(Player player)
    {
        Vector3 position = new Vector3(player.x , 0, player.y);

        _playerFactory.Create(_playerPrefab, position, Quaternion.identity);
    }

    private void CreateEnemy(string key , Player player)
    {
        Vector3 position = new Vector3(player.x , 0, player.y );

        EnemyController enemy = _enemyFactory.Create(_enemy, position, Quaternion.identity);
        player.OnChange += enemy.OnChange;
        _players.TryAdd(key, enemy);

    }

    private void RemoveEnemy(string key, Player value)
    {
        if (!_players.TryGetValue(key, out var player)) 
            return;
        
        Destroy(player.gameObject);
        _players.Remove(key);

    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _room.Leave();
    }
}
