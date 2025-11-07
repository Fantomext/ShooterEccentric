import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("number")
    speed = 0;

    @type("int8")
    maxHP = 0;

    @type("int8")
    curHP = 0;

    @type("uint8")
    loss = 0;

    @type("number")
    pX = Math.floor(Math.random() * 50) - 25;

    @type("number")
    pY = 0;

    @type("number")
    pZ = Math.floor(Math.random() * 50) - 25;

    @type("number")
    vX = 0;

    @type("number")
    vY = 0;

    @type("number")
    vZ = 0;

    @type("number")
    rX = 0;

    @type("number")
    rY = 0;

}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, data : any) {
        const player = new Player();
        player.speed = data.speed;

        player.maxHP = data.hp;
        player.curHP = data.hp;

        console.log(sessionId);

        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, data: any) {

        const player = this.players.get(sessionId);

        // Position
        player.pX = data.pX;
        player.pY = data.pY;
        player.pZ = data.pZ;

        // Velocity
        player.vX = data.vX;
        player.vY = data.vY;
        player.vZ = data.vZ;

        // Rotation
        player.rX = data.rX;
        player.rY = data.rY;

    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            //console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("shoot", (client, data) =>
        {
            this.broadcast("Shoot", data, { except : client});
        })

        this.onMessage("sit", (client, data) =>
        {
            this.broadcast("Sit", data, { except : client});
        })

        this.onMessage("damage", (client, data) =>
        {
            const clientID = data.id;
            const player = this.state.players.get(clientID);

            let hp = player.curHP -= data.value;

            if (hp > 0){
                player.curHP = hp;
                return;
            }

            player.loss++;
            player.curHP = player.maxHP;

            for (var i = 0; i < this.clients.length; i++)
            {
                if (this.clients[i].id != clientID)
                    continue;

                const x = Math.floor(Math.random() * 50) - 25;
                const z = Math.floor(Math.random() * 50) - 25;
                const killer = data.enId;
                const msg = JSON.stringify({x,z, killer});

                this.clients[i].send("Restart", msg);
                console.log(client.id)
                this.broadcast("Death", this.clients[i].id, { except :  this.clients[i]});
            };

        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data : any) {
        client.send("hello", "world");
        this.state.createPlayer(client.sessionId, data);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}